using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.AuthorizationService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "authorization_service");

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
                name: "users",
                schema: "authorization_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    password_hash = table.Column<string>(type: "character varying", nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    reset_code = table.Column<string>(type: "character varying", nullable: true),
                    code_requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    banned_before = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "authorization_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    token = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_ip = table.Column<string>(type: "text", nullable: true),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    revoked_by_ip = table.Column<string>(type: "text", nullable: true),
                    replaced_by_token = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("refresh_tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "refresh_tokens_replaced_by_token_fkey",
                        column: x => x.replaced_by_token,
                        principalSchema: "authorization_service",
                        principalTable: "refresh_tokens",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "refresh_tokens_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "authorization_service",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_replaced_by_token",
                schema: "authorization_service",
                table: "refresh_tokens",
                column: "replaced_by_token");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                schema: "authorization_service",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_refresh_token",
                schema: "authorization_service",
                table: "refresh_tokens",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "authorization_service");

            migrationBuilder.DropTable(
                name: "users",
                schema: "authorization_service");
        }
    }
}
