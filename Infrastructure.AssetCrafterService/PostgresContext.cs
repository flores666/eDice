using Infrastructure.AssetCrafterService.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AssetCrafterService;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pack> Packs { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenType> TokenTypes { get; set; }

    public virtual DbSet<UserPack> UserPacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Pack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("packs_pkey");

            entity.ToTable("packs", "asset_crafter_service", tb => tb.HasComment("Паки токенов"));

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IsConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("is_confirmed");
            entity.Property(e => e.IsOfficial)
                .HasDefaultValue(false)
                .HasColumnName("is_official");
            entity.Property(e => e.IsPublic)
                .HasDefaultValue(true)
                .HasColumnName("is_public");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasMany(d => d.Tokens).WithMany(p => p.Packs)
                .UsingEntity<Dictionary<string, object>>(
                    "PackToken",
                    r => r.HasOne<Token>().WithMany()
                        .HasForeignKey("TokenId")
                        .HasConstraintName("pack_tokens_token_id_fkey"),
                    l => l.HasOne<Pack>().WithMany()
                        .HasForeignKey("PackId")
                        .HasConstraintName("pack_tokens_pack_id_fkey"),
                    j =>
                    {
                        j.HasKey("PackId", "TokenId").HasName("pack_tokens_pkey");
                        j.ToTable("pack_tokens", "asset_crafter_service", tb => tb.HasComment("Связи между паками и токенами"));
                        j.IndexerProperty<Guid>("PackId").HasColumnName("pack_id");
                        j.IndexerProperty<Guid>("TokenId").HasColumnName("token_id");
                    });
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tokens_pkey");

            entity.ToTable("tokens", "asset_crafter_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("character varying")
                .HasColumnName("image_url");
            entity.Property(e => e.IsConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("is_confirmed");
            entity.Property(e => e.IsOfficial)
                .HasDefaultValue(false)
                .HasColumnName("is_official");
            entity.Property(e => e.IsPublic)
                .HasDefaultValue(true)
                .HasColumnName("is_public");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tokens_type_fkey");
        });

        modelBuilder.Entity<TokenType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_types_pkey");

            entity.ToTable("token_types", "asset_crafter_service");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Caption)
                .HasColumnType("character varying")
                .HasColumnName("caption");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<TokenType>().HasData(new List<TokenType>(3)
        {
            new TokenType { Caption = "Персонаж", Name = "character", Id = Guid.NewGuid() },
            new TokenType { Caption = "Предмет", Name = "item", Id = Guid.NewGuid() },
            new TokenType { Caption = "Игровое поле", Name = "map", Id = Guid.NewGuid() }
        });

        modelBuilder.Entity<UserPack>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_packs_pkey");

            entity.ToTable("user_packs", "asset_crafter_service", tb => tb.HasComment("Связи пользователей с паками"));

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.GrantedAt).HasColumnName("granted_at");
            entity.Property(e => e.PackId).HasColumnName("pack_id");

            entity.HasOne(d => d.Pack).WithMany(p => p.UserPacks)
                .HasForeignKey(d => d.PackId)
                .HasConstraintName("user_packs_pack_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
