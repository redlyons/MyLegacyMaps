namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealEstate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartnerLogoes",
                c => new
                    {
                        PartnerLogoId = c.Int(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.PartnerLogoId);
            
            AddColumn("dbo.Flags", "PartnerLogoId", c => c.Int(nullable: true));
            AddColumn("dbo.Flags", "Address1", c => c.String(maxLength: 50));
            AddColumn("dbo.Flags", "Address2", c => c.String(maxLength: 50));
            AddColumn("dbo.Flags", "City", c => c.String(maxLength: 50));
            AddColumn("dbo.Flags", "State", c => c.String());
            AddColumn("dbo.Flags", "PostalCode", c => c.String(maxLength: 50));
            CreateIndex("dbo.Flags", "PartnerLogoId");
            AddForeignKey("dbo.Flags", "PartnerLogoId", "dbo.PartnerLogoes", "PartnerLogoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flags", "PartnerLogoId", "dbo.PartnerLogoes");
            DropIndex("dbo.Flags", new[] { "PartnerLogoId" });
            DropColumn("dbo.Flags", "PostalCode");
            DropColumn("dbo.Flags", "State");
            DropColumn("dbo.Flags", "City");
            DropColumn("dbo.Flags", "Address2");
            DropColumn("dbo.Flags", "Address1");
            DropColumn("dbo.Flags", "PartnerLogoId");
            DropTable("dbo.PartnerLogoes");
        }
    }
}
