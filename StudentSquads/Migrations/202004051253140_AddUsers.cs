namespace StudentSquads.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"

INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'027a16d1-8330-4fbc-a1eb-9e30c9afa387', N'simplemember@yandex.ru', 0, N'AJgfkEcxirCruSNgkRJgh1Jjfv5/gsHQr8i95lqdu9EICJbiBIU7Vf2degIxVvW56A==', N'0f8cbffc-fdc3-4cef-a82c-0e5ae35ae1e9', NULL, 0, 0, NULL, 1, 0, N'simplemember@yandex.ru')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'5aceb78c-aa5a-42bd-ba07-1bee20b9e8ef', N'squadmanager@yandex.ru', 0, N'AB5CXnvVuq2XOtfIOE7gPGW43x0A5R+UkAmJmDTn1ubYesYNyoOusVBlpJMtFmbdJw==', N'2f498606-aa16-424c-b8a3-50a578d60ea4', NULL, 0, 0, NULL, 1, 0, N'squadmanager@yandex.ru')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'6032ee65-82e2-4f79-9445-9e2ef7dad64a', N'regionalmanager@yandex.ru', 0, N'AEgf8JEAfIUZu6dzmtFtGur8fdezeDvXsGwLLsf7a4w2fUvbMmx+Y3HMV8OdXmM20w==', N'10fc36fe-90cc-4608-b25f-9b7a12bcbfb3', NULL, 0, 0, NULL, 1, 0, N'regionalmanager@yandex.ru')
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'f2c7b26d-9998-4503-a2be-74cb14604784', N'unimanager@yandex.ru', 0, N'ADjcjen7kLzSuweNMTMOA7LZCPByYd6DYP5/XzqVRon0nGtucpOODnUjKFTwhcaCpw==', N'75805ec0-7b7a-45cf-8b76-aea596c8986c', NULL, 0, 0, NULL, 1, 0, N'unimanager@yandex.ru')

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'a6e9b7c5-0d46-47f5-b9a4-e6b31c7326d8', N'RegionalManager')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c925d929-7fdf-4f08-8112-6523d61cff1c', N'SquadManager')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'a47618f9-e0b0-468f-af59-ce044ddac2f3', N'UniManager')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f2c7b26d-9998-4503-a2be-74cb14604784', N'a47618f9-e0b0-468f-af59-ce044ddac2f3')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6032ee65-82e2-4f79-9445-9e2ef7dad64a', N'a6e9b7c5-0d46-47f5-b9a4-e6b31c7326d8')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5aceb78c-aa5a-42bd-ba07-1bee20b9e8ef', N'c925d929-7fdf-4f08-8112-6523d61cff1c')
");
        }

        public override void Down()
        {
        }
    }
}
