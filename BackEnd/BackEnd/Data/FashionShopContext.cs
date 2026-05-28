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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>(entity=>
            {
                // Tabla y PK
                entity.ToTable("clientes");
                entity.HasKey(e => e.CliId);
                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .ValueGeneratedOnAdd();

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

                entity.Property(e => e.CliEmail)
                      .HasColumnName("cli_email")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.CliUsername)
                      .HasColumnName("cli_usuario")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.CliPasswordHash)
                      .HasColumnName("cli_password_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                // Columnas nullable
                entity.Property(e => e.CliPhone)
                      .HasColumnName("cli_telefono")
                      .HasMaxLength(15)
                      .IsRequired(false);

                entity.Property(e => e.CliStripeId)
                      .HasColumnName("cli_stripe_id")
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(e => e.CliRegisterDate)
                      .HasColumnName("cli_fecha_registro")
                      .HasDefaultValueSql("now()")
                      .IsRequired(false);

                // Unique constraints
                entity.HasIndex(e => e.CliDocument)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_documento");

                entity.HasIndex(e => e.CliEmail)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_email");

                entity.HasIndex(e => e.CliUsername)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_usuario");
            });
        }
    }
}
