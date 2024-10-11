using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace MalteriaAPI.Models.Services
{
    public class WhatsAppService
    {
        private readonly string accountSid;
        private readonly string authToken;

        public WhatsAppService(string accountSid, string authToken)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
            TwilioClient.Init(accountSid, authToken);
        }

        public void EnviarMensajeWhatsApp(string to, string from, string message)
        {
            var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{to}"))
            {
                From = new PhoneNumber($"whatsapp:{from}"),
                Body = message
            };

            var messageResource = MessageResource.Create(messageOptions);
            Console.WriteLine($"Mensaje enviado con SID: {messageResource.Sid}");
        }
    }
}
