namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdoptedMaps",
                c => new
                    {
                        AdoptedMapId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        MapId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 60),
                        ShareStatusTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdoptedMapId)
                .ForeignKey("dbo.Maps", t => t.MapId)
                .ForeignKey("dbo.ShareStatusTypes", t => t.ShareStatusTypeId)
                .Index(t => t.MapId)
                .Index(t => t.ShareStatusTypeId);
            
            CreateTable(
                "dbo.Flags",
                c => new
                    {
                        FlagId = c.Int(nullable: false, identity: true),
                        FlagTypeId = c.Int(nullable: false),
                        AdoptedMapId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Xpos = c.Int(nullable: false),
                        Ypos = c.Int(nullable: false),
                        Description = c.String(),
                        VideoUrl = c.String(),
                        Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.FlagId)
                .ForeignKey("dbo.AdoptedMaps", t => t.AdoptedMapId)
                .ForeignKey("dbo.FlagTypes", t => t.FlagTypeId)
                .Index(t => t.FlagTypeId)
                .Index(t => t.AdoptedMapId);
            
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        MapId = c.Int(nullable: false, identity: true),
                        MapTypeId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 60),
                        Description = c.String(),
                        FileName = c.String(),
                        Orientation = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.MapId)
                .ForeignKey("dbo.MapTypes", t => t.MapTypeId)
                .Index(t => t.MapTypeId);
            
            CreateTable(
                "dbo.MapTypes",
                c => new
                    {
                        MapTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MapTypeId);
            
            CreateTable(
                "dbo.ShareStatusTypes",
                c => new
                    {
                        ShareStatusTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ShareStatusTypeId);
            
            CreateTable(
                "dbo.FlagTypes",
                c => new
                    {
                        FlagTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FlagTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flags", "FlagTypeId", "dbo.FlagTypes");
            DropForeignKey("dbo.AdoptedMaps", "ShareStatusTypeId", "dbo.ShareStatusTypes");
            DropForeignKey("dbo.Maps", "MapTypeId", "dbo.MapTypes");
            DropForeignKey("dbo.AdoptedMaps", "MapId", "dbo.Maps");
            DropForeignKey("dbo.Flags", "AdoptedMapId", "dbo.AdoptedMaps");
            DropIndex("dbo.Maps", new[] { "MapTypeId" });
            DropIndex("dbo.Flags", new[] { "AdoptedMapId" });
            DropIndex("dbo.Flags", new[] { "FlagTypeId" });
            DropIndex("dbo.AdoptedMaps", new[] { "ShareStatusTypeId" });
            DropIndex("dbo.AdoptedMaps", new[] { "MapId" });
            DropTable("dbo.FlagTypes");
            DropTable("dbo.ShareStatusTypes");
            DropTable("dbo.MapTypes");
            DropTable("dbo.Maps");
            DropTable("dbo.Flags");
            DropTable("dbo.AdoptedMaps");
        }
    }
}
