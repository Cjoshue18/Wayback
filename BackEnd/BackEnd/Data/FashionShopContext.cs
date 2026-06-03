using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class FashionShopContext : DbContext
    {
        public FashionShopContext(DbContextOptions<FashionShopContext> options) : base(options)
        {

        }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Administradores> Administradores { get; set; }

        //Fluent API 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tabla Usuarios
            modelBuilder.Entity<Usuarios>(entity =>
            {
                // Tabla y PK
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.UsuId);
                entity.Property(e => e.UsuId)
                      .HasColumnName("usu_id")
                      .ValueGeneratedOnAdd();

                // Columnas NOT NULL
                entity.Property(e => e.UsuEmail)
                      .HasColumnName("usu_email")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.UsuUsername)
                      .HasColumnName("usu_username")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.UsuPasswordHash)
                      .HasColumnName("usu_password_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.UsuRole)
                      .HasColumnName("usu_rol")
                      .HasMaxLength(20)
                      .IsRequired()
                      .HasDefaultValue("cliente");

                // Columnas nullable
                entity.Property(e => e.CliRegisterDate)
                      .HasColumnName("usu_fecha_registro")
                      .IsRequired(false)
                      .HasDefaultValueSql("now()");

                //Constraints
                entity.HasIndex(e => e.UsuEmail)
                      .IsUnique()
                      .HasDatabaseName("uq_usu_email");

                entity.HasIndex(e => e.UsuUsername)
                      .IsUnique()
                      .HasDatabaseName("uq_usu_username");
            });

            //Tabla Clientes
            modelBuilder.Entity<Clientes>(entity=>
            {
                // Tabla y PK
                entity.ToTable("Clientes");
                entity.HasKey(e => e.CliId);
                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UsuId)
                      .HasColumnName("usu_id")
                      .IsRequired(); // FK no nullable para garantizar la relación 1:1

                // Columnas NOT NULL
                entity.Property(e => e.CliDocument)
                      .HasColumnName("cli_documento")
                      .HasMaxLength(12)
                      .IsRequired();

                entity.Property(e => e.CliDocumentType)
                      .HasColumnName("cli_documento_tipo")
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.CliName)
                      .HasColumnName("cli_nombre")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.CliLastName)
                      .HasColumnName("cli_apellido")
                      .HasMaxLength(50)
                      .IsRequired();

                // Columnas nullable
                entity.Property(e => e.CliPhone)
                      .HasColumnName("cli_telefono")
                      .HasMaxLength(15)
                      .IsRequired(false);

                entity.Property(e => e.CliPasarelaId)
                      .HasColumnName("cli_pago_pasarela_id")
                      .HasMaxLength(100)
                      .IsRequired(false);

                //constraints
                // Relación 1:1 con Usuarios (FK)
                entity.HasOne(c => c.Usuario) //Cliente tiene un Usuario (tabla actual)
                      .WithOne(u => u.Cliente)//Usuario tiene un Cliente (tabla relacionada)
                      .HasForeignKey<Clientes>(e => e.UsuId) //para relaciones 1:1
                       //Le dice al modelo que la clave foránea en Clientes es UsuId,
                       //que se refiere a la PK de Usuarios
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_cli_usu_id");

                entity.HasIndex(e => e.UsuId)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_usu_id"); //Valida que sea relacion 1:1 por ser unique

                entity.HasIndex(e => e.CliDocument)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_documento");
            });

            modelBuilder.Entity<Administradores>(entity =>
            {
                entity.ToTable("Administradores");
                entity.HasKey(e => e.AdId);
                entity.Property(e => e.AdId)
                      .HasColumnName("ad_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UsuId)
                      .HasColumnName("usu_id")
                      .IsRequired(); // FK no nullable para garantizar la relación 1:1
                
                entity.Property(e => e.AdName)
                      .HasColumnName("ad_nombre")
                      .HasMaxLength(100)
                      .IsRequired();

                //Constraints
                entity.HasOne(a => a.Usuario)
                      .WithOne(u => u.Administrador)
                      .HasForeignKey<Administradores>(e => e.UsuId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_ad_usu_id");

                entity.HasIndex(e => e.UsuId)
                      .IsUnique()
                      .HasDatabaseName("uq_ad_usu_id");
            });
        }
    }
}
