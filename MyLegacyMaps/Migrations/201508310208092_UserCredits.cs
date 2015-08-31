namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCredits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Credits", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DisplayName");
            DropColumn("dbo.AspNetUsers", "Credits");
        }
    }
}
