namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flags",
                c => new
                {
                    FlagId = c.Int(nullable: false, identity: true),
                    Name = c.String()
                })
                .PrimaryKey(t => t.FlagId);
        }
        
        public override void Down()
        {
            DropTable("dbo.Flags");
        }
    }
}
