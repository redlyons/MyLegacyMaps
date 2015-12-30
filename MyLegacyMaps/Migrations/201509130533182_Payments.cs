namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payments : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Payments",
            //    c => new
            //        {
            //            TransactionId = c.String(nullable: false, maxLength: 128),
            //            UserId = c.String(nullable: false),
            //            GrossTotal = c.Double(nullable: false),
            //            Currency = c.String(maxLength: 15),
            //            PayerFirstName = c.String(maxLength: 100),
            //            PayerLastName = c.String(maxLength: 100),
            //            PayerEmail = c.String(maxLength: 100),
            //            Tokens = c.Int(nullable: false),
            //            TransactionDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
            //            TransactionStatus = c.String(maxLength: 50),
            //            TransactionDetails = c.String(maxLength: 1500),
            //        })
            //    .PrimaryKey(t => t.TransactionId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Payments");
        }
    }
}
