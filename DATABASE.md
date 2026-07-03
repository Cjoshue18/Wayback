# Wayback Database Architecture

The PostgreSQL database for the Wayback platform is managed via the **Entity Framework Core Code-First** approach. The schema is highly normalized to ensure data integrity and is logically divided into four distinct domains.

## Entity-Relationship Schema

The following diagram illustrates the complete relational structure, including primary keys (PK), foreign keys (FK), and essential columns.

```mermaid
erDiagram
    usuarios ||--o| clientes : "extends"
    usuarios ||--o| administradores : "extends"
    
    clientes ||--o{ direcciones : "has"
    clientes ||--o{ pedidos : "places"
    clientes ||--o{ metodospago : "saves"
    
    pedidos ||--|{ detalle_pedidos : "contains"
    metodospago ||--o{ pedidos : "processes"
    direcciones ||--o{ pedidos : "delivers to"
    
    productos ||--o{ variantes : "has skus"
    productos ||--o{ imagenes : "displays"
    categorias ||--o{ productos : "categorizes"
    estilos ||--o{ productos : "styles"
    
    varcolores ||--o{ variantes : "defines color"
    variantes ||--o{ detalle_pedidos : "fulfills"

    usuarios {
        integer usu_id PK
        varchar usu_email
        varchar usu_username
        varchar usu_password_hash
        varchar usu_rol "cliente / admin"
        timestamp usu_fecha_registro
    }

    clientes {
        integer cli_id PK
        integer usu_id FK
        varchar cli_documento
        varchar cli_documento_tipo
        varchar cli_nombre
        varchar cli_apellido
        varchar cli_telefono
    }

    administradores {
        integer ad_id PK
        integer usu_id FK
        varchar ad_nombre
    }

    categorias {
        integer cat_id PK
        varchar cat_nombre
    }

    estilos {
        integer est_id PK
        varchar est_nombre
    }

    varcolores {
        integer color_id PK
        varchar color_nombre
        varchar color_hex
    }

    productos {
        integer pro_id PK
        integer cat_id FK
        integer est_id FK
        varchar pro_genero
        varchar pro_nombre
        varchar pro_descripcion
        numeric pro_precio
        smallint pro_descuento
    }

    variantes {
        integer var_id PK
        integer pro_id FK
        integer color_id FK
        varchar var_talla
        integer var_stock
    }

    imagenes {
        integer img_id PK
        integer pro_id FK
        varchar img_url
    }

    direcciones {
        integer dir_id PK
        integer cli_id FK
        varchar dir_calle
        varchar dir_distrito
        boolean dir_es_preferido
    }

    metodospago {
        integer met_id PK
        integer cli_id FK
        varchar met_pasarela_card_ultimos4
        varchar met_tipo_pago
        boolean met_es_preferido
    }

    pedidos {
        integer ped_id PK
        integer cli_id FK
        integer met_id FK
        integer dir_id FK
        varchar ped_estado
        numeric ped_total
        timestamp ped_fecha_compra
    }

    detalle_pedidos {
        integer ped_id PK,FK
        integer var_id PK,FK
        integer detped_cantidad
        numeric detped_precio_u
        numeric detped_sub_total
    }
```

## Domain Breakdown

### 1. Identity & Access Domain
Handles authentication and role-based profiles.
- **usuarios:** Base table containing credentials and role logic.
- **clientes / administradores:** Extension tables containing role-specific profile data linked one-to-one with the base user table.

### 2. Catalog Domain
Organizes the core product offerings.
- **productos:** Core product definitions. Links to categories and styles.
- **categorias & estilos:** Lookup tables defining the taxonomy of the store for dynamic filtering.

### 3. Inventory Domain (SKUs)
Manages the physical stock variations.
- **variantes:** Represents the actual physical stock units (SKUs). Combines a specific product with a specific size and color, tracking exact inventory levels.
- **varcolores:** Global color palette definitions for consistency.
- **imagenes:** Product photography URLs stored externally linked directly to parent products.

### 4. Sales & Checkout Domain
Tracks the lifecycle of customer purchases.
- **pedidos (Orders):** Central record for financial transactions, linking clients to payment states, totals, and delivery destinations.
- **detalle_pedidos:** Line items mapping specific Variants, quantities, and historical prices to an Order.
- **direcciones:** Geolocation and physical address strings managed by clients.
- **metodospago:** Payment provider mappings.
