using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CWPIO.Data.Migrations
{
    public partial class renameAllToSnakeCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BountyCampaingItemType_BountyCampaing_BountyCampaingId",
                table: "BountyCampaingItemType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaing_BountyCampaing_BountyCampaingId",
                table: "UserBountyCampaing");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaing_users_UserId",
                table: "UserBountyCampaing");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBountyCampaingItem_UserBountyCampaing_UserId_BountyCampaingId",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropForeignKey(
                name: "FK_roleclaims_roles_RoleId",
                schema: "identity",
                table: "roleclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_userclaims_users_UserId",
                schema: "identity",
                table: "userclaims");

            migrationBuilder.DropForeignKey(
                name: "FK_userlogins_users_UserId",
                schema: "identity",
                table: "userlogins");

            migrationBuilder.DropForeignKey(
                name: "FK_userroles_roles_RoleId",
                schema: "identity",
                table: "userroles");

            migrationBuilder.DropForeignKey(
                name: "FK_userroles_users_UserId",
                schema: "identity",
                table: "userroles");

            migrationBuilder.DropForeignKey(
                name: "FK_usertokens_users_UserId",
                schema: "identity",
                table: "usertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usertokens",
                schema: "identity",
                table: "usertokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "identity",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userroles",
                schema: "identity",
                table: "userroles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userlogins",
                schema: "identity",
                table: "userlogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userclaims",
                schema: "identity",
                table: "userclaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                schema: "identity",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roleclaims",
                schema: "identity",
                table: "roleclaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBountyCampaingItem",
                table: "UserBountyCampaingItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBountyCampaing",
                table: "UserBountyCampaing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataProtectionKeys",
                table: "DataProtectionKeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BountyCampaingItemType",
                table: "BountyCampaingItemType");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId_BountyCampaingId",
                table: "BountyCampaingItemType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BountyCampaing",
                table: "BountyCampaing");

            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.RenameTable(
                name: "usertokens",
                schema: "identity",
                newName: "user_tokens");

            migrationBuilder.RenameTable(
                name: "userroles",
                schema: "identity",
                newName: "user_roles");

            migrationBuilder.RenameTable(
                name: "userlogins",
                schema: "identity",
                newName: "user_logins");

            migrationBuilder.RenameTable(
                name: "userclaims",
                schema: "identity",
                newName: "user_claims");

            migrationBuilder.RenameTable(
                name: "roleclaims",
                schema: "identity",
                newName: "role_claims");

            migrationBuilder.RenameTable(
                name: "UserBountyCampaingItem",
                newName: "user_bounty_campaing_item");

            migrationBuilder.RenameTable(
                name: "UserBountyCampaing",
                newName: "user_bounty_campaing");

            migrationBuilder.RenameTable(
                name: "Subscribers",
                newName: "subscribers");

            migrationBuilder.RenameTable(
                name: "DataProtectionKeys",
                newName: "data_protection_keys",
                newSchema: "core");

            migrationBuilder.RenameTable(
                name: "BountyCampaingItemType",
                newName: "bounty_campaing_item_type");

            migrationBuilder.RenameTable(
                name: "BountyCampaing",
                newName: "bounty_campaing");

            migrationBuilder.RenameColumn(
                name: "Value",
                schema: "identity",
                table: "user_tokens",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "identity",
                table: "user_tokens",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                schema: "identity",
                table: "user_tokens",
                newName: "login_provider");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "user_tokens",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "identity",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                schema: "identity",
                table: "users",
                newName: "two_factor_enabled");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                schema: "identity",
                table: "users",
                newName: "security_stamp");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberConfirmed",
                schema: "identity",
                table: "users",
                newName: "phone_number_confirmed");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "identity",
                table: "users",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                schema: "identity",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "NormalizedUserName",
                schema: "identity",
                table: "users",
                newName: "normalized_user_name");

            migrationBuilder.RenameColumn(
                name: "NormalizedEmail",
                schema: "identity",
                table: "users",
                newName: "normalized_email");

            migrationBuilder.RenameColumn(
                name: "LockoutEnd",
                schema: "identity",
                table: "users",
                newName: "lockout_end");

            migrationBuilder.RenameColumn(
                name: "LockoutEnabled",
                schema: "identity",
                table: "users",
                newName: "lockout_enabled");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "identity",
                table: "users",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                schema: "identity",
                table: "users",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "identity",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                schema: "identity",
                table: "users",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                schema: "identity",
                table: "users",
                newName: "access_failed_count");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "identity",
                table: "users",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "users",
                newName: "user_name_index");

            migrationBuilder.RenameIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "users",
                newName: "email_index");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "identity",
                table: "user_roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "user_roles",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_userroles_RoleId",
                schema: "identity",
                table: "user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "user_logins",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ProviderDisplayName",
                schema: "identity",
                table: "user_logins",
                newName: "provider_display_name");

            migrationBuilder.RenameColumn(
                name: "ProviderKey",
                schema: "identity",
                table: "user_logins",
                newName: "provider_key");

            migrationBuilder.RenameColumn(
                name: "LoginProvider",
                schema: "identity",
                table: "user_logins",
                newName: "login_provider");

            migrationBuilder.RenameIndex(
                name: "IX_userlogins_UserId",
                schema: "identity",
                table: "user_logins",
                newName: "ix_user_logins_user_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "identity",
                table: "user_claims",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                schema: "identity",
                table: "user_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                schema: "identity",
                table: "user_claims",
                newName: "claim_type");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "identity",
                table: "user_claims",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_userclaims_UserId",
                schema: "identity",
                table: "user_claims",
                newName: "ix_user_claims_user_id");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                schema: "identity",
                table: "roles",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "identity",
                table: "roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "ConcurrencyStamp",
                schema: "identity",
                table: "roles",
                newName: "concurrency_stamp");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "identity",
                table: "roles",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "roles",
                newName: "role_name_index");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "identity",
                table: "role_claims",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "ClaimValue",
                schema: "identity",
                table: "role_claims",
                newName: "claim_value");

            migrationBuilder.RenameColumn(
                name: "ClaimType",
                schema: "identity",
                table: "role_claims",
                newName: "claim_type");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "identity",
                table: "role_claims",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_roleclaims_RoleId",
                schema: "identity",
                table: "role_claims",
                newName: "ix_role_claims_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_bounty_campaing_item",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "user_bounty_campaing_item",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "user_bounty_campaing_item",
                newName: "item_type");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "user_bounty_campaing_item",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "user_bounty_campaing_item",
                newName: "is_accepted");

            migrationBuilder.RenameColumn(
                name: "BountyCampaingId",
                table: "user_bounty_campaing_item",
                newName: "bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_bounty_campaing_item",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_UserBountyCampaingItem_UserId_BountyCampaingId",
                table: "user_bounty_campaing_item",
                newName: "ix_user_bounty_campaing_item_user_id_bounty_campaing_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserBountyCampaingItem_ItemType_BountyCampaingId",
                table: "user_bounty_campaing_item",
                newName: "ix_user_bounty_campaing_item_item_type_bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "TotalItemCount",
                table: "user_bounty_campaing",
                newName: "total_item_count");

            migrationBuilder.RenameColumn(
                name: "TotalCoinEarned",
                table: "user_bounty_campaing",
                newName: "total_coin_earned");

            migrationBuilder.RenameColumn(
                name: "BountyCampaingId",
                table: "user_bounty_campaing",
                newName: "bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "user_bounty_campaing",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserBountyCampaing_BountyCampaingId",
                table: "user_bounty_campaing",
                newName: "ix_user_bounty_campaing_bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "Unsubscribe",
                table: "subscribers",
                newName: "unsubscribe");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "subscribers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "EmailSend",
                table: "subscribers",
                newName: "email_send");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "subscribers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "subscribers",
                newName: "date_created");

            migrationBuilder.RenameColumn(
                name: "Culture",
                table: "subscribers",
                newName: "culture");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "subscribers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "XmlData",
                schema: "core",
                table: "data_protection_keys",
                newName: "xml_data");

            migrationBuilder.RenameColumn(
                name: "FriendlyName",
                schema: "core",
                table: "data_protection_keys",
                newName: "friendly_name");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "bounty_campaing_item_type",
                newName: "type_id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "bounty_campaing_item_type",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "NeedToApprove",
                table: "bounty_campaing_item_type",
                newName: "need_to_approve");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "bounty_campaing_item_type",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bounty_campaing_item_type",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "BountyCampaingId",
                table: "bounty_campaing_item_type",
                newName: "bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bounty_campaing_item_type",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_BountyCampaingItemType_BountyCampaingId",
                table: "bounty_campaing_item_type",
                newName: "ix_bounty_campaing_item_type_bounty_campaing_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "bounty_campaing",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bounty_campaing",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "FaClass",
                table: "bounty_campaing",
                newName: "fa_class");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bounty_campaing",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_tokens",
                schema: "identity",
                table: "user_tokens",
                columns: new[] { "user_id", "login_provider", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "identity",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                schema: "identity",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_logins",
                schema: "identity",
                table: "user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_claims",
                schema: "identity",
                table: "user_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                schema: "identity",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_claims",
                schema: "identity",
                table: "role_claims",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_bounty_campaing_item",
                table: "user_bounty_campaing_item",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_bounty_campaing",
                table: "user_bounty_campaing",
                columns: new[] { "user_id", "bounty_campaing_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_data_protection_keys",
                schema: "core",
                table: "data_protection_keys",
                column: "friendly_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bounty_campaing_item_type",
                table: "bounty_campaing_item_type",
                column: "id");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_bounty_campaing_item_type_type_id_bounty_campaing_id",
                table: "bounty_campaing_item_type",
                columns: new[] { "type_id", "bounty_campaing_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_bounty_campaing",
                table: "bounty_campaing",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_bounty_campaing_item_type_bounty_campaing_bounty_campaing_id",
                table: "bounty_campaing_item_type",
                column: "bounty_campaing_id",
                principalTable: "bounty_campaing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_bounty_campaing_bounty_campaing_bounty_campaing_id",
                table: "user_bounty_campaing",
                column: "bounty_campaing_id",
                principalTable: "bounty_campaing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_bounty_campaing_users_user_id",
                table: "user_bounty_campaing",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_bounty_campaing_item_bounty_campaing_item_type_item_type_bounty_campaing_id",
                table: "user_bounty_campaing_item",
                columns: new[] { "item_type", "bounty_campaing_id" },
                principalTable: "bounty_campaing_item_type",
                principalColumns: new[] { "type_id", "bounty_campaing_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_bounty_campaing_item_user_bounty_campaing_user_id_bounty_campaing_id",
                table: "user_bounty_campaing_item",
                columns: new[] { "user_id", "bounty_campaing_id" },
                principalTable: "user_bounty_campaing",
                principalColumns: new[] { "user_id", "bounty_campaing_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_claims_roles_role_id",
                schema: "identity",
                table: "role_claims",
                column: "role_id",
                principalSchema: "identity",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_claims_users_user_id",
                schema: "identity",
                table: "user_claims",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_logins_users_user_id",
                schema: "identity",
                table: "user_logins",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "identity",
                table: "user_roles",
                column: "role_id",
                principalSchema: "identity",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "identity",
                table: "user_roles",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_tokens_users_user_id",
                schema: "identity",
                table: "user_tokens",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bounty_campaing_item_type_bounty_campaing_bounty_campaing_id",
                table: "bounty_campaing_item_type");

            migrationBuilder.DropForeignKey(
                name: "fk_user_bounty_campaing_bounty_campaing_bounty_campaing_id",
                table: "user_bounty_campaing");

            migrationBuilder.DropForeignKey(
                name: "fk_user_bounty_campaing_users_user_id",
                table: "user_bounty_campaing");

            migrationBuilder.DropForeignKey(
                name: "fk_user_bounty_campaing_item_bounty_campaing_item_type_item_type_bounty_campaing_id",
                table: "user_bounty_campaing_item");

            migrationBuilder.DropForeignKey(
                name: "fk_user_bounty_campaing_item_user_bounty_campaing_user_id_bounty_campaing_id",
                table: "user_bounty_campaing_item");

            migrationBuilder.DropForeignKey(
                name: "fk_role_claims_roles_role_id",
                schema: "identity",
                table: "role_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_claims_users_user_id",
                schema: "identity",
                table: "user_claims");

            migrationBuilder.DropForeignKey(
                name: "fk_user_logins_users_user_id",
                schema: "identity",
                table: "user_logins");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                schema: "identity",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "identity",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_tokens_users_user_id",
                schema: "identity",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "identity",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_tokens",
                schema: "identity",
                table: "user_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                schema: "identity",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_logins",
                schema: "identity",
                table: "user_logins");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_claims",
                schema: "identity",
                table: "user_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                schema: "identity",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_claims",
                schema: "identity",
                table: "role_claims");

            migrationBuilder.DropPrimaryKey(
                name: "pk_data_protection_keys",
                schema: "core",
                table: "data_protection_keys");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_bounty_campaing_item",
                table: "user_bounty_campaing_item");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_bounty_campaing",
                table: "user_bounty_campaing");

            migrationBuilder.DropPrimaryKey(
                name: "pk_subscribers",
                table: "subscribers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bounty_campaing_item_type",
                table: "bounty_campaing_item_type");

            migrationBuilder.DropUniqueConstraint(
                name: "ak_bounty_campaing_item_type_type_id_bounty_campaing_id",
                table: "bounty_campaing_item_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bounty_campaing",
                table: "bounty_campaing");

            migrationBuilder.RenameTable(
                name: "user_tokens",
                schema: "identity",
                newName: "usertokens");

            migrationBuilder.RenameTable(
                name: "user_roles",
                schema: "identity",
                newName: "userroles");

            migrationBuilder.RenameTable(
                name: "user_logins",
                schema: "identity",
                newName: "userlogins");

            migrationBuilder.RenameTable(
                name: "user_claims",
                schema: "identity",
                newName: "userclaims");

            migrationBuilder.RenameTable(
                name: "role_claims",
                schema: "identity",
                newName: "roleclaims");

            migrationBuilder.RenameTable(
                name: "data_protection_keys",
                schema: "core",
                newName: "DataProtectionKeys");

            migrationBuilder.RenameTable(
                name: "user_bounty_campaing_item",
                newName: "UserBountyCampaingItem");

            migrationBuilder.RenameTable(
                name: "user_bounty_campaing",
                newName: "UserBountyCampaing");

            migrationBuilder.RenameTable(
                name: "subscribers",
                newName: "Subscribers");

            migrationBuilder.RenameTable(
                name: "bounty_campaing_item_type",
                newName: "BountyCampaingItemType");

            migrationBuilder.RenameTable(
                name: "bounty_campaing",
                newName: "BountyCampaing");

            migrationBuilder.RenameColumn(
                name: "user_name",
                schema: "identity",
                table: "users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "two_factor_enabled",
                schema: "identity",
                table: "users",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "security_stamp",
                schema: "identity",
                table: "users",
                newName: "SecurityStamp");

            migrationBuilder.RenameColumn(
                name: "phone_number_confirmed",
                schema: "identity",
                table: "users",
                newName: "PhoneNumberConfirmed");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                schema: "identity",
                table: "users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                schema: "identity",
                table: "users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "normalized_user_name",
                schema: "identity",
                table: "users",
                newName: "NormalizedUserName");

            migrationBuilder.RenameColumn(
                name: "normalized_email",
                schema: "identity",
                table: "users",
                newName: "NormalizedEmail");

            migrationBuilder.RenameColumn(
                name: "lockout_end",
                schema: "identity",
                table: "users",
                newName: "LockoutEnd");

            migrationBuilder.RenameColumn(
                name: "lockout_enabled",
                schema: "identity",
                table: "users",
                newName: "LockoutEnabled");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                schema: "identity",
                table: "users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                schema: "identity",
                table: "users",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "email",
                schema: "identity",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                schema: "identity",
                table: "users",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "access_failed_count",
                schema: "identity",
                table: "users",
                newName: "AccessFailedCount");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "identity",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "user_name_index",
                schema: "identity",
                table: "users",
                newName: "UserNameIndex");

            migrationBuilder.RenameIndex(
                name: "email_index",
                schema: "identity",
                table: "users",
                newName: "EmailIndex");

            migrationBuilder.RenameColumn(
                name: "value",
                schema: "identity",
                table: "usertokens",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "identity",
                table: "usertokens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                schema: "identity",
                table: "usertokens",
                newName: "LoginProvider");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "identity",
                table: "usertokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "identity",
                table: "userroles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "identity",
                table: "userroles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                schema: "identity",
                table: "userroles",
                newName: "IX_userroles_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "identity",
                table: "userlogins",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "provider_display_name",
                schema: "identity",
                table: "userlogins",
                newName: "ProviderDisplayName");

            migrationBuilder.RenameColumn(
                name: "provider_key",
                schema: "identity",
                table: "userlogins",
                newName: "ProviderKey");

            migrationBuilder.RenameColumn(
                name: "login_provider",
                schema: "identity",
                table: "userlogins",
                newName: "LoginProvider");

            migrationBuilder.RenameIndex(
                name: "ix_user_logins_user_id",
                schema: "identity",
                table: "userlogins",
                newName: "IX_userlogins_UserId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                schema: "identity",
                table: "userclaims",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                schema: "identity",
                table: "userclaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                schema: "identity",
                table: "userclaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "identity",
                table: "userclaims",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_user_claims_user_id",
                schema: "identity",
                table: "userclaims",
                newName: "IX_userclaims_UserId");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                schema: "identity",
                table: "roles",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "identity",
                table: "roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "concurrency_stamp",
                schema: "identity",
                table: "roles",
                newName: "ConcurrencyStamp");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "identity",
                table: "roles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "role_name_index",
                schema: "identity",
                table: "roles",
                newName: "RoleNameIndex");

            migrationBuilder.RenameColumn(
                name: "role_id",
                schema: "identity",
                table: "roleclaims",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "claim_value",
                schema: "identity",
                table: "roleclaims",
                newName: "ClaimValue");

            migrationBuilder.RenameColumn(
                name: "claim_type",
                schema: "identity",
                table: "roleclaims",
                newName: "ClaimType");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "identity",
                table: "roleclaims",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_role_claims_role_id",
                schema: "identity",
                table: "roleclaims",
                newName: "IX_roleclaims_RoleId");

            migrationBuilder.RenameColumn(
                name: "xml_data",
                table: "DataProtectionKeys",
                newName: "XmlData");

            migrationBuilder.RenameColumn(
                name: "friendly_name",
                table: "DataProtectionKeys",
                newName: "FriendlyName");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserBountyCampaingItem",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "UserBountyCampaingItem",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "item_type",
                table: "UserBountyCampaingItem",
                newName: "ItemType");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "UserBountyCampaingItem",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_accepted",
                table: "UserBountyCampaingItem",
                newName: "IsAccepted");

            migrationBuilder.RenameColumn(
                name: "bounty_campaing_id",
                table: "UserBountyCampaingItem",
                newName: "BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserBountyCampaingItem",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_user_bounty_campaing_item_user_id_bounty_campaing_id",
                table: "UserBountyCampaingItem",
                newName: "IX_UserBountyCampaingItem_UserId_BountyCampaingId");

            migrationBuilder.RenameIndex(
                name: "ix_user_bounty_campaing_item_item_type_bounty_campaing_id",
                table: "UserBountyCampaingItem",
                newName: "IX_UserBountyCampaingItem_ItemType_BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "total_item_count",
                table: "UserBountyCampaing",
                newName: "TotalItemCount");

            migrationBuilder.RenameColumn(
                name: "total_coin_earned",
                table: "UserBountyCampaing",
                newName: "TotalCoinEarned");

            migrationBuilder.RenameColumn(
                name: "bounty_campaing_id",
                table: "UserBountyCampaing",
                newName: "BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserBountyCampaing",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "ix_user_bounty_campaing_bounty_campaing_id",
                table: "UserBountyCampaing",
                newName: "IX_UserBountyCampaing_BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "unsubscribe",
                table: "Subscribers",
                newName: "Unsubscribe");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Subscribers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email_send",
                table: "Subscribers",
                newName: "EmailSend");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Subscribers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "date_created",
                table: "Subscribers",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "culture",
                table: "Subscribers",
                newName: "Culture");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Subscribers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "BountyCampaingItemType",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "BountyCampaingItemType",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "need_to_approve",
                table: "BountyCampaingItemType",
                newName: "NeedToApprove");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "BountyCampaingItemType",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "BountyCampaingItemType",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "bounty_campaing_id",
                table: "BountyCampaingItemType",
                newName: "BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BountyCampaingItemType",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "ix_bounty_campaing_item_type_bounty_campaing_id",
                table: "BountyCampaingItemType",
                newName: "IX_BountyCampaingItemType_BountyCampaingId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "BountyCampaing",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "BountyCampaing",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "fa_class",
                table: "BountyCampaing",
                newName: "FaClass");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BountyCampaing",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "identity",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usertokens",
                schema: "identity",
                table: "usertokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userroles",
                schema: "identity",
                table: "userroles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userlogins",
                schema: "identity",
                table: "userlogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_userclaims",
                schema: "identity",
                table: "userclaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                schema: "identity",
                table: "roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roleclaims",
                schema: "identity",
                table: "roleclaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataProtectionKeys",
                table: "DataProtectionKeys",
                column: "FriendlyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBountyCampaingItem",
                table: "UserBountyCampaingItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBountyCampaing",
                table: "UserBountyCampaing",
                columns: new[] { "UserId", "BountyCampaingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BountyCampaingItemType",
                table: "BountyCampaingItemType",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BountyCampaingItemType_TypeId_BountyCampaingId",
                table: "BountyCampaingItemType",
                columns: new[] { "TypeId", "BountyCampaingId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BountyCampaing",
                table: "BountyCampaing",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BountyCampaingItemType_BountyCampaing_BountyCampaingId",
                table: "BountyCampaingItemType",
                column: "BountyCampaingId",
                principalTable: "BountyCampaing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaing_BountyCampaing_BountyCampaingId",
                table: "UserBountyCampaing",
                column: "BountyCampaingId",
                principalTable: "BountyCampaing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaing_users_UserId",
                table: "UserBountyCampaing",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaingItem_BountyCampaingItemType_ItemType_BountyCampaingId",
                table: "UserBountyCampaingItem",
                columns: new[] { "ItemType", "BountyCampaingId" },
                principalTable: "BountyCampaingItemType",
                principalColumns: new[] { "TypeId", "BountyCampaingId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBountyCampaingItem_UserBountyCampaing_UserId_BountyCampaingId",
                table: "UserBountyCampaingItem",
                columns: new[] { "UserId", "BountyCampaingId" },
                principalTable: "UserBountyCampaing",
                principalColumns: new[] { "UserId", "BountyCampaingId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_roleclaims_roles_RoleId",
                schema: "identity",
                table: "roleclaims",
                column: "RoleId",
                principalSchema: "identity",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userclaims_users_UserId",
                schema: "identity",
                table: "userclaims",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userlogins_users_UserId",
                schema: "identity",
                table: "userlogins",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userroles_roles_RoleId",
                schema: "identity",
                table: "userroles",
                column: "RoleId",
                principalSchema: "identity",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userroles_users_UserId",
                schema: "identity",
                table: "userroles",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_usertokens_users_UserId",
                schema: "identity",
                table: "usertokens",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
