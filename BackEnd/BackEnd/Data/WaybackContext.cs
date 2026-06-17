using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class WaybackContext : DbContext
    {
        public WaybackContext(DbContextOptions<WaybackContext> options) : base(options)
        {

        }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Administradores> Administradores { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Estilos> Estilos { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<VarColores> VarColores { get; set; }
        public DbSet<Variantes> Variantes { get; set; }
        public DbSet<Direcciones> Direcciones { get; set; }
        public DbSet<MetodosPago> MetodosPago { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<PedidoDetalles> PedidoDetalles { get; set; }
        public DbSet<Imagenes> Imagenes { get; set; }


        //Fluent API 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tabla Usuarios
            modelBuilder.Entity<Usuarios>(entity =>
            {
                // Tabla y PK
                entity.ToTable("usuarios");
                entity.HasKey(e => e.UsuId)
                      .HasName("pk_usu_id");
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

                entity.Property(e => e.UsuContrasenaHash)
                      .HasColumnName("usu_password_hash")
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.UsuRol)
                      .HasColumnName("usu_rol")
                      .HasMaxLength(20)
                      .IsRequired()
                      .HasDefaultValue("cliente");

                // Columnas nullable
                entity.Property(e => e.UsuFechaRegistro)
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
                entity.ToTable("clientes");
                entity.HasKey(e => e.CliId)
                      .HasName("pk_cli_id");
                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UsuId)
                      .HasColumnName("usu_id")
                      .IsRequired(); // FK no nullable para garantizar la relación 1:1

                // Columnas NOT NULL
                entity.Property(e => e.CliDocumento)
                      .HasColumnName("cli_documento")
                      .HasMaxLength(12)
                      .IsRequired();

                entity.Property(e => e.CliTipoDocumento)
                      .HasColumnName("cli_documento_tipo")
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.CliNombre)
                      .HasColumnName("cli_nombre")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.CliApellido)
                      .HasColumnName("cli_apellido")
                      .HasMaxLength(50)
                      .IsRequired();

                // Columnas nullable
                entity.Property(e => e.CliTelefono)
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
                      .OnDelete(DeleteBehavior.SetNull) //Si borro usuario, en cliente se vera null, pero no se borra cliente
                      .HasConstraintName("fk_cli_usu_id");

                entity.HasIndex(e => e.UsuId)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_usu_id"); 
                //Valida que sea relacion 1:1 por ser unique

                entity.HasIndex(e => e.CliDocumento)
                      .IsUnique()
                      .HasDatabaseName("uq_cli_documento");
            });

            modelBuilder.Entity<Administradores>(entity =>
            {
                entity.ToTable("administradores");
                entity.HasKey(e => e.AdId)
                      .HasName("pk_ad_id");
                entity.Property(e => e.AdId)
                      .HasColumnName("ad_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UsuId)
                      .HasColumnName("usu_id")
                      .IsRequired(); // FK no nullable para garantizar la relación 1:1
                
                entity.Property(e => e.AdNombre)
                      .HasColumnName("ad_nombre")
                      .HasMaxLength(100)    
                      .IsRequired();

                //Constraints
                entity.HasOne(a => a.Usuario)
                      .WithOne(u => u.Administrador)
                      .HasForeignKey<Administradores>(e => e.UsuId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_ad_usu_id");

                entity.HasIndex(e => e.UsuId)
                      .IsUnique()
                      .HasDatabaseName("uq_ad_usu_id");
            });

            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.ToTable("categorias");
                entity.HasKey(e => e.CatId)
                      .HasName("pk_cat_id");
                entity.Property(e => e.CatId)
                      .HasColumnName("cat_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CatNombre)
                      .HasColumnName("cat_nombre")
                      .HasMaxLength(50)
                      .IsRequired();
            });

            modelBuilder.Entity<Estilos>(entity => 
            {
                entity.ToTable("estilos");
                entity.HasKey(e => e.EstId)
                      .HasName("pk_est_id");
                entity.Property(e => e.EstId)
                      .HasColumnName("est_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.EstNombre)
                      .HasColumnName("est_nombre")
                      .HasMaxLength(50)
                      .IsRequired();

                //Constraint
                entity.HasIndex(e => e.EstNombre)
                      .IsUnique()
                      .HasDatabaseName("uq_est_nombre");
            });


            modelBuilder.Entity<Productos>(entity =>
            {
                entity.ToTable("productos");
                entity.HasKey(e => e.ProId)
                      .HasName("pk_pro_id");
                entity.Property(e => e.ProId)
                      .HasColumnName("pro_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CatId)
                      .HasColumnName("cat_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.EstId)
                      .HasColumnName("est_id"); //FK puede ser null

                entity.Property(e => e.ProGenero)
                      .HasColumnName("pro_genero")
                      .HasMaxLength(10)
                      .IsRequired()
                      .HasDefaultValue("Unisex");

                entity.Property(e => e.ProNombre)
                      .HasColumnName("pro_nombre")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.ProDescripcion)
                      .HasColumnName("pro_descripcion")
                      .HasMaxLength(500);

                entity.Property(e => e.ProPrecio)
                      .HasColumnName("pro_precio")
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(e => e.ProDescuento)
                      .HasColumnName("pro_descuento");

                entity.Property(e => e.ProDescuentoInicio)
                      .HasColumnName("pro_desc_fecha_inicio");

                entity.Property(e => e.ProDescuentoFin)
                      .HasColumnName("pro_desc_fecha_fin");

                entity.Property(e => e.ProFechaCreacion)
                      .HasColumnName("pro_fecha_creacion")
                      .HasDefaultValueSql("now()");

                //Constraints
                entity.HasOne(p => p.Categoria) //un producto pertenece a una categoria
                      .WithMany(c => c.Productos) //una categoria tiene muchos productos
                      .HasForeignKey(p => p.CatId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_pro_cat_id");//si intentas borrar categoria
                                                          //No podras porque tiene productos asociados
                entity.HasOne(p => p.Estilo)
                      .WithMany(e => e.Productos)
                      .HasForeignKey(p => p.EstId)
                      .OnDelete(DeleteBehavior.SetNull) //si borro estilo, en productos se vera un null
                      .HasConstraintName("fk_pro_est_id");
            });

            modelBuilder.Entity<VarColores>(entity =>
            {
                entity.ToTable("varcolores");
                entity.HasKey(e => e.ColorId)
                      .HasName("pk_color_id");
                entity.Property(e => e.ColorId)
                      .HasColumnName("color_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.ColorNombre)
                      .HasColumnName("color_nombre")
                      .HasMaxLength(50)
                      .IsRequired();
                
                entity.Property(e => e.ColorHex)
                      .HasColumnName("color_hex")
                      .HasMaxLength(7) //Formato #RRGGBB
                      .IsRequired();
            });

            modelBuilder.Entity<Variantes>(entity =>
            {
                entity.ToTable("variantes");
                entity.HasKey(e => e.VarId)
                      .HasName("pk_var_id");
                entity.Property(e => e.VarId)
                      .HasColumnName("var_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.ProId)
                      .HasColumnName("pro_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.ColorId)
                      .HasColumnName("color_id")
                      .IsRequired(); //FK no nullable

                entity.Property(e => e.VarTalla)
                      .HasColumnName("var_talla")
                      .HasMaxLength(10)
                      .IsRequired()
                      .HasDefaultValue("S");

                entity.Property(e => e.VarStock)
                      .HasColumnName("var_stock")
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(e => e.ImgId)
                      .HasColumnName("img_id"); //FK puede ser null

                //Constraints
                entity.HasOne(v => v.Producto) //una variante pertenece a un producto
                      .WithMany(p => p.Variantes) //un producto tiene muchas variantes
                      .HasForeignKey(v => v.ProId)
                      .OnDelete(DeleteBehavior.Cascade) //borro producto se borran variantes
                      .HasConstraintName("fk_var_pro_id");

                entity.HasOne(v => v.VarColor) //una variante tiene un color
                        .WithMany(c => c.Variantes) //un color puede estar en muchas variantes
                        .HasForeignKey(v => v.ColorId)
                        .OnDelete(DeleteBehavior.Restrict) //si intento borrar color no dejara si tiene variantes asignadas
                        .HasConstraintName("fk_var_color_id");

                entity.HasOne(v => v.Imagen) //una variante tiene una imagen
                      .WithMany(i => i.Variantes) //una imagen puede estar en muchas variantes 
                      .HasForeignKey(v => v.ImgId)
                      .OnDelete(DeleteBehavior.SetNull) //si borro imagen, en variante se vera null, pero no se borra variante
                      .HasConstraintName("fk_var_img_id");
            });

            modelBuilder.Entity<Direcciones>(entity =>
            {
                entity.ToTable("direcciones");
                entity.HasKey(e => e.DirId)
                      .HasName("pk_dir_id");
                entity.Property(e => e.DirId)
                      .HasColumnName("dir_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.DirCalle)
                      .HasColumnName("dir_calle")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.DirDistrito)
                      .HasColumnName("dir_distrito")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.DirProvincia)
                      .HasColumnName("dir_provincia")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.DirDepartamento)
                      .HasColumnName("dir_departamento")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.DirReferencia)
                      .HasColumnName("dir_referencia")
                      .HasMaxLength(200);

                entity.Property(e => e.DirPreferido)
                      .HasColumnName("dir_es_preferido")
                      .IsRequired()
                      .HasDefaultValue(false);

                //Constraints
                entity.HasOne(d => d.Cliente) //una direccion pertenece a un cliente
                      .WithMany(c => c.Direcciones) //un cliente puede tener muchas direcciones
                      .HasForeignKey(d => d.CliId)
                      .OnDelete(DeleteBehavior.Cascade) //si borro cliente se borran todas las direcciones
                      .HasConstraintName("fk_dir_cli_id");
            });

            modelBuilder.Entity<MetodosPago>(entity =>
            {
                entity.ToTable("metodospago");
                entity.HasKey(e => e.MetId)
                      .HasName("pk_met_id");
                entity.Property(e => e.MetId)
                      .HasColumnName("met_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.MetPasarelaCardId)
                      .HasColumnName("met_pasarela_card_id")
                      .HasMaxLength(100); //nullable

                entity.Property(e => e.MetPasarelaCardUltimos4)
                      .HasColumnName("met_pasarela_card_ultimos4")
                      .HasMaxLength(4); //nullable

                entity.Property(e => e.MetTipoPago)
                      .HasColumnName("met_tipo_pago")
                      .HasMaxLength(20)
                      .IsRequired(); //Ejm: "Tarjeta de Crédito", "PayPal", etc.

                entity.Property(e => e.MetPreferido)
                      .HasColumnName("met_es_preferido")
                      .IsRequired()
                      .HasDefaultValue(false);

                //Constraints
                entity.HasOne(p => p.Cliente) //un metodo de pago pertenece a un cliente
                      .WithMany(c => c.MetodosPago) //un cliente puede tener muchos metodos de pago
                      .HasForeignKey(p => p.CliId)
                      .OnDelete(DeleteBehavior.Cascade) //si borro cliente se borran sus metodos de pago
                      .HasConstraintName("fk_met_cli_id");
            });

            modelBuilder.Entity<Pedidos>(entity =>
            {
                entity.ToTable("pedidos");
                entity.HasKey(e => e.PedId)
                      .HasName("pk_ped_id");
                entity.Property(e => e.PedId)
                      .HasColumnName("ped_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.CliId)
                      .HasColumnName("cli_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.MetId)
                      .HasColumnName("met_id"); // FK nullable

                entity.Property(p => p.PedMetTipoPago)
                      .HasColumnName("ped_met_tipo_pago")
                      .HasMaxLength(20);

                entity.Property(p => p.PedMetUltimos4)
                      .HasColumnName("ped_met_ultimos4")
                      .HasMaxLength(4);

                entity.Property(e => e.DirId)
                      .HasColumnName("dir_id"); // FK nullable

                entity.Property(p => p.PedDirCalle)
                      .HasMaxLength(100)
                      .HasColumnName("ped_dir_calle")
                      .IsRequired();

                entity.Property(p => p.PedDirDistrito)
                      .HasMaxLength(50)
                      .HasColumnName("ped_dir_distrito")
                      .IsRequired();

                entity.Property(p => p.PedDirProvincia)
                      .HasMaxLength(50)
                      .HasColumnName("ped_dir_provincia")
                      .IsRequired();

                entity.Property(p => p.PedDirDepartamento)
                      .HasMaxLength(50)
                      .HasColumnName("ped_dir_departamento")
                      .IsRequired();

                entity.Property(p => p.PedDirReferencia)
                      .HasColumnName("ped_dir_referencia")
                      .HasMaxLength(200);

                entity.Property(e => e.PedEstado)
                      .HasColumnName("ped_estado")
                      .HasMaxLength(20)
                      .IsRequired()
                      .HasDefaultValue("pendiente"); //Ejm: "Pendiente", "Enviado", "Entregado", etc.

                entity.Property(e => e.PedTotal)
                      .HasColumnName("ped_total")
                      .HasColumnType("decimal(10,2)")
                      .IsRequired(); 

                entity.Property(e => e.PedPasarelaCargoId)
                      .HasColumnName("ped_pasarela_cargo_id")
                      .HasMaxLength(100); //nullable, se llena si se integra con pasarela de pago

                entity.Property(e => e.PedFechaCompra)
                      .HasColumnName("ped_fecha_compra")
                      .IsRequired()
                      .HasDefaultValueSql("now()");

                entity.Property(e => e.PedFechaEntrega)
                      .HasColumnName("ped_fecha_entrega"); //nullable

                //Constraints
                entity.HasOne(p => p.Cliente) //un pedido pertenece a un cliente
                      .WithMany(c => c.Pedidos) //un cliente puede tener muchos pedidos
                      .HasForeignKey(p => p.CliId)
                      .OnDelete(DeleteBehavior.Restrict) //si borro cliente se borran sus pedidos
                      .HasConstraintName("fk_ped_cli_id");

                entity.HasOne(p => p.MetodoPago) //un pedido puede tener un metodo de pago
                      .WithMany(m => m.Pedidos) //un metodo de pago puede tener muchos pedidos
                      .HasForeignKey(p => p.MetId)
                      .OnDelete(DeleteBehavior.SetNull)//mismo dilema que con direcciones
                      .HasConstraintName("fk_ped_met_id");

                entity.HasOne(p => p.Direccion) //un pedido puede tener una direccion
                      .WithMany(d => d.Pedidos) //una direccion puede estar en muchos pedidos
                      .HasForeignKey(p => p.DirId)
                      .OnDelete(DeleteBehavior.SetNull) //permite eliminar una direccion aunque este siendo usada, porque
                      //ya existe una snapshot de la direccion en ese momento, asi un cliente puede borrar una direccion asociada a su perfil
                      //pero no se pierde esa informacion porque permanece en la base de datos para auditoria o analisis posterior
                      .HasConstraintName("fk_ped_dir_id");
            });

            modelBuilder.Entity<PedidoDetalles>(entity =>
            {
                entity.ToTable("detalle_pedidos");
                entity.HasKey(e => new { e.PedId, e.VarId }) //Clave compuesta por PedId y VarId
                      .HasName("pk_detped_id_com_var_id");
                entity.Property(e => e.PedId)
                      .HasColumnName("ped_id")
                      .IsRequired();
                entity.Property(e => e.VarId)
                      .HasColumnName("var_id")
                      .IsRequired();

                entity.Property(e => e.DetPedCantidad)
                      .HasColumnName("detped_cantidad")
                      .IsRequired()
                      .HasDefaultValue(1);

                entity.Property(e => e.DetPedPrecioUnitario)
                      .HasColumnName("detped_precio_u")
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(e => e.DetPedSubTotal)
                      .HasColumnName("detped_sub_total")
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                //Constraints
                entity.HasOne(d => d.Pedido) //un detalle pertenece a un pedido
                      .WithMany(p => p.Detalles) //un pedido puede tener muchos detalles
                      .HasForeignKey(d => d.PedId)
                      .OnDelete(DeleteBehavior.Cascade) //si borro pedido se borran sus detalles
                      .HasConstraintName("fk_detped_ped_id");

                entity.HasOne(d => d.Variante) //un detalle tiene una variante
                        .WithMany(v => v.Detalles) //una variante puede estar en muchos detalles
                        .HasForeignKey(d => d.VarId)
                        .OnDelete(DeleteBehavior.Restrict) //no permite borrar variante si tiene detalles asociados, para mantener integridad historica de pedidos
                        .HasConstraintName("fk_detped_var_id");
            });

            modelBuilder.Entity<Imagenes>(entity =>
            {
                entity.ToTable("imagenes");
                entity.HasKey(e => e.ImgId)
                      .HasName("pk_img_id");
                entity.Property(e => e.ImgId)
                      .HasColumnName("img_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.ProId)
                      .HasColumnName("pro_id")
                      .IsRequired(); // FK no nullable

                entity.Property(e => e.ImgURL)
                      .HasColumnName("img_url")
                      .HasMaxLength(255)
                      .IsRequired();

                //Constraints
                entity.HasOne(i => i.Producto) //una imagen pertenece a un producto
                      .WithMany(p => p.Imagenes) //un producto puede tener muchas imagenes
                      .HasForeignKey(i => i.ProId)
                      .OnDelete(DeleteBehavior.Cascade) //si borro producto se borran sus imagenes
                      .HasConstraintName("fk_img_pro_id");
            });
        }
    }
}
