namespace MyLegacyMaps.MembershipContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDates1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "DateModified", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
