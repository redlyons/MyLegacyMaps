namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealEstate21 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Flags", new[] { "PartnerLogoId" });
            AlterColumn("dbo.Flags", "PartnerLogoId", c => c.Int(nullable: true));
            CreateIndex("dbo.Flags", "PartnerLogoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Flags", new[] { "PartnerLogoId" });
            AlterColumn("dbo.Flags", "PartnerLogoId", c => c.Int());
            CreateIndex("dbo.Flags", "PartnerLogoId");
        }
    }
}
