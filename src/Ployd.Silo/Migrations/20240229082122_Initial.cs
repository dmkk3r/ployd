using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ployd.Silo.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeploymentSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DeploymentSource = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeploymentTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DeploymentTarget = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentTargets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DeploymentSourceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deployments_DeploymentSources_DeploymentSourceId",
                        column: x => x.DeploymentSourceId,
                        principalTable: "DeploymentSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeploymentEntityDeploymentTargetEntity",
                columns: table => new
                {
                    DeploymentTargetsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeploymentsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentEntityDeploymentTargetEntity", x => new { x.DeploymentTargetsId, x.DeploymentsId });
                    table.ForeignKey(
                        name: "FK_DeploymentEntityDeploymentTargetEntity_DeploymentTargets_DeploymentTargetsId",
                        column: x => x.DeploymentTargetsId,
                        principalTable: "DeploymentTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeploymentEntityDeploymentTargetEntity_Deployments_DeploymentsId",
                        column: x => x.DeploymentsId,
                        principalTable: "Deployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentEntityDeploymentTargetEntity_DeploymentsId",
                table: "DeploymentEntityDeploymentTargetEntity",
                column: "DeploymentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_DeploymentSourceId",
                table: "Deployments",
                column: "DeploymentSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeploymentEntityDeploymentTargetEntity");

            migrationBuilder.DropTable(
                name: "DeploymentTargets");

            migrationBuilder.DropTable(
                name: "Deployments");

            migrationBuilder.DropTable(
                name: "DeploymentSources");
        }
    }
}
