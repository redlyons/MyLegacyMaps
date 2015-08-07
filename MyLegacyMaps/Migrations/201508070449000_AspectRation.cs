namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspectRation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspectRatios",
                c => new
                    {
                        AspectRatioId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.AspectRatioId);
            
            AddColumn("dbo.Maps", "AspectRatioId", c => c.Int(nullable:true));
            CreateIndex("dbo.Maps", "AspectRatioId");
            AddForeignKey("dbo.Maps", "AspectRatioId", "dbo.AspectRatios", "AspectRatioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Maps", "AspectRatioId", "dbo.AspectRatios");
            DropIndex("dbo.Maps", new[] { "AspectRatioId" });
            DropColumn("dbo.Maps", "AspectRatioId");
            DropTable("dbo.AspectRatios");
        }
    }
}
