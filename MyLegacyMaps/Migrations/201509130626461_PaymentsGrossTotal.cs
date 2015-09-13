namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentsGrossTotal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "GrossTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "GrossTotal", c => c.Double(nullable: false));
        }
    }
}
