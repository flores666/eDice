using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.FileService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "file_service");

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
                name: "files",
                schema: "file_service",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    extension = table.Column<string>(type: "character varying", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    link = table.Column<string>(type: "character varying", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_pkey", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files",
                schema: "file_service");
        }
    }
}
