namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealEstate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerLogoes", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.PartnerLogoes", "Height", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartnerLogoes", "Height");
            DropColumn("dbo.PartnerLogoes", "Width");
        }
    }
}
