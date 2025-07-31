using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.AssetCrafterService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset_crafter_service");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:auth.aal_level", "aal1,aal2,aal3")
                .Annotation("Npgsql:Enum:auth.code_challenge_method", "s256,plain")
                .Annotation("Npgsql:Enum:auth.factor_status", "unverified,verified")
                .Annotation("Npgsql:Enum:auth.factor_type", "totp,webauthn,phone")
                .Annotation("Npgsql:Enum:auth.one_time_token_type", "confirmation_token,reauthentication_token,recovery_token,email_change_token_new,email_change_token_current,phone_change_token")
                .Annotation("Npgsql:Enum:realtime.action", "INSERT,UPDATE,DELETE,TRUNCATE,ERROR")
                .Annotation("Npgsql:Enum:realtime.equality_op", "eq,neq,lt,lte,gt,gte,in")
                .Annotation("Npgsql:PostgresExtension:extensions.pg_stat_statements", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:extensions.uuid-ossp", ",,")
                .Annotation("Npgsql:PostgresExtension:graphql.pg_graphql", ",,")
                .Annotation("Npgsql:PostgresExtension:vault.supabase_vault", ",,");

            migrationBuilder.CreateTable(
                name: "packs",
                schema: "asset_crafter_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_official = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("packs_pkey", x => x.id);
                },
                comment: "Паки токенов");

            migrationBuilder.CreateTable(
                name: "token_types",
                schema: "asset_crafter_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    caption = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_types_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_packs",
                schema: "asset_crafter_service",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pack_id = table.Column<Guid>(type: "uuid", nullable: false),
                    granted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_packs_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "user_packs_pack_id_fkey",
                        column: x => x.pack_id,
                        principalSchema: "asset_crafter_service",
                        principalTable: "packs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Связи пользователей с паками");

            migrationBuilder.CreateTable(
                name: "tokens",
                schema: "asset_crafter_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    type = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_official = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "tokens_type_fkey",
                        column: x => x.type,
                        principalSchema: "asset_crafter_service",
                        principalTable: "token_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pack_tokens",
                schema: "asset_crafter_service",
                columns: table => new
                {
                    pack_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pack_tokens_pkey", x => new { x.pack_id, x.token_id });
                    table.ForeignKey(
                        name: "pack_tokens_pack_id_fkey",
                        column: x => x.pack_id,
                        principalSchema: "asset_crafter_service",
                        principalTable: "packs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "pack_tokens_token_id_fkey",
                        column: x => x.token_id,
                        principalSchema: "asset_crafter_service",
                        principalTable: "tokens",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Связи между паками и токенами");

            migrationBuilder.InsertData(
                schema: "asset_crafter_service",
                table: "token_types",
                columns: new[] { "id", "caption", "name" },
                values: new object[,]
                {
                    { new Guid("1370cf71-5d4c-4fed-a1a1-d96899a4725c"), "Персонаж", "character" },
                    { new Guid("b91d04e3-bfe2-467c-b894-086d4e5812f5"), "Предмет", "item" },
                    { new Guid("e180fd1f-4dc3-4797-8af9-27274ce3fc6e"), "Игровое поле", "map" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_pack_tokens_token_id",
                schema: "asset_crafter_service",
                table: "pack_tokens",
                column: "token_id");

            migrationBuilder.CreateIndex(
                name: "IX_tokens_type",
                schema: "asset_crafter_service",
                table: "tokens",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_user_packs_pack_id",
                schema: "asset_crafter_service",
                table: "user_packs",
                column: "pack_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pack_tokens",
                schema: "asset_crafter_service");

            migrationBuilder.DropTable(
                name: "user_packs",
                schema: "asset_crafter_service");

            migrationBuilder.DropTable(
                name: "tokens",
                schema: "asset_crafter_service");

            migrationBuilder.DropTable(
                name: "packs",
                schema: "asset_crafter_service");

            migrationBuilder.DropTable(
                name: "token_types",
                schema: "asset_crafter_service");
        }
    }
}
