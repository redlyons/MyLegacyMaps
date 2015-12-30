namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentsDetails : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Payments", "TransactionDetails", c => c.String(maxLength: 1500));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Payments", "TransactionDetails", c => c.String(maxLength: 200));
        }
    }
}
