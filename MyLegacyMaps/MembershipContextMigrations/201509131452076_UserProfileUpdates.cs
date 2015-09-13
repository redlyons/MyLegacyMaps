namespace MyLegacyMaps.MembershipContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmailPrevious", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "ProfileImageUrl", c => c.String(maxLength: 500));
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.AspNetUsers", "HomeTown");
            DropColumn("dbo.AspNetUsers", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "HomeTown", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "DisplayName", c => c.String());
            DropColumn("dbo.AspNetUsers", "ProfileImageUrl");
            DropColumn("dbo.AspNetUsers", "EmailPrevious");
        }
    }
}
