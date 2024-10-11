using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using MalteriaAPI.Models.DTO;

namespace MalteriaAPI.Models.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        // Inyectar las configuraciones desde appsettings
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void EnviarCorreoConAdjunto(string destinatario, string asunto, string mensaje, string rutaArchivoPdf)
        {
            // Crear el mensaje de correo
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
            message.To.Add(new MailboxAddress("", destinatario));
            message.Subject = asunto;

            // Crear el cuerpo del mensaje (en texto y HTML)
            var bodyBuilder = new BodyBuilder
            {
                TextBody = mensaje,
                HtmlBody = "<p>" + mensaje + "</p>"
            };

            // Adjuntar el archivo PDF
            if (!string.IsNullOrEmpty(rutaArchivoPdf) && File.Exists(rutaArchivoPdf))
            {
                var archivoAdjunto = bodyBuilder.Attachments.Add(rutaArchivoPdf);
                archivoAdjunto.ContentDisposition.FileName = Path.GetFileName(rutaArchivoPdf);
            }

            message.Body = bodyBuilder.ToMessageBody();

            // Configurar el cliente SMTP
            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, false);
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
