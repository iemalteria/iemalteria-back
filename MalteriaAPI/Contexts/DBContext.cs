using System;
using System.Collections.Generic;
using MalteriaAPI.Models;
using MalteriaAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace MalteriaAPI.Contexts;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<TextosWeb> TextosWeb { get; set; }
    public virtual DbSet<EmpleadoDto> Empleado { get; set; }
    public virtual DbSet<ProductosDto> Productos { get; set; }
    public virtual DbSet<CreseDto> Crese { get; set; }
    public virtual DbSet<CreseImagenesDto> CreseImagenes { get; set; }
    public virtual DbSet<CategoriaBlog> CategoriasBlog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3213E83FAE5D1C59");

            entity.ToTable("Blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contenido)
                .IsUnicode(false)
                .HasColumnName("contenido");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaPublicacion");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Blog_Usuario");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaId");
        });

        modelBuilder.Entity<TextosWeb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TextosWe__3214EC07425D49D6");

            entity.ToTable("TextosWeb");

            entity.Property(e => e.AltText).HasMaxLength(255);
            entity.Property(e => e.ImagenUrl).HasMaxLength(255);
            entity.Property(e => e.Seccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titulo).HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3213E83F535931BF");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });
        modelBuilder.Entity<EmpleadoDto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empleado__3214EC07425D49D6");

            entity.ToTable("Empleado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("videoUrl");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Sede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sede");
        });
        modelBuilder.Entity<ProductosDto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC0732DA493B");

            entity.ToTable("Productos");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Descripcion");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio");
            entity.Property(e => e.Precio2)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio2");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImagenUrl");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Categoria");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Tipo");
            entity.Property(e => e.Activo)
                .HasColumnName("Activo")
                .HasDefaultValueSql("1");
        });
        modelBuilder.Entity<CreseDto>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Crese");

            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Texto)
                .IsUnicode(false);

            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            
        });

        modelBuilder.Entity<CreseImagenesDto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CreseImagenes");

            entity.Property(e => e.ImagenUrl)
                .IsUnicode(false);

            entity.Property(e => e.CreseId).HasColumnName("CreseId");

            // Configurar relación con Crese
         
        });
        modelBuilder.Entity<CategoriaBlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CategoriasBlog");

            entity.ToTable("CategoriasBlog");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("FechaCreacion")
                .HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Activo)
                .HasColumnName("Activo")
                .HasDefaultValueSql("1");
        });


        OnModelCreatingPartial(modelBuilder);
    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
