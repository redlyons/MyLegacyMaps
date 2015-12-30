namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealEstate1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.PartnerLogoes", "Name", c => c.String());
            //AlterColumn("dbo.Flags", "State", c => c.String(maxLength: 3));
            //AlterColumn("dbo.Flags", "PostalCode", c => c.String(maxLength: 10));
            //AlterColumn("dbo.PartnerLogoes", "ImageUrl", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.PartnerLogoes", "ImageUrl", c => c.String());
            //AlterColumn("dbo.Flags", "PostalCode", c => c.String(maxLength: 50));
            //AlterColumn("dbo.Flags", "State", c => c.String());
            //DropColumn("dbo.PartnerLogoes", "Name");
        }
    }
}
