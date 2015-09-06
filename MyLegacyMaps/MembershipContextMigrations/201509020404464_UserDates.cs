namespace MyLegacyMaps.MembershipContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime(nullable: true));
            AddColumn("dbo.AspNetUsers", "DateModified", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateModified");
            DropColumn("dbo.AspNetUsers", "DateCreated");
        }
    }
}
