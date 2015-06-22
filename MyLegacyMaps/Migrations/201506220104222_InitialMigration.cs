namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
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
                        Description = c.String(maxLength: 3000),
                        IsActive = c.Boolean(nullable: false),
                        ShareStatusTypeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateModified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
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
                        VideoUrl = c.String(maxLength: 500),
                        Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateModified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(),
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
                        Name = c.String(nullable: false, maxLength: 60),
                        Description = c.String(maxLength: 3000),
                        ImageUrl = c.String(maxLength: 500),
                        ThumbUrl = c.String(maxLength: 500),
                        FileName = c.String(maxLength: 100),
                        OrientationTypeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateModified = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.MapId)
                .ForeignKey("dbo.OrientationTypes", t => t.OrientationTypeId)
                .Index(t => t.OrientationTypeId);
            
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
                "dbo.OrientationTypes",
                c => new
                    {
                        OrientationTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.OrientationTypeId);
            
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
            
            CreateTable(
                "dbo.MapMapTypes",
                c => new
                    {
                        MapId = c.Int(nullable: false),
                        MapTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MapId, t.MapTypeId })
                .ForeignKey("dbo.Maps", t => t.MapId)
                .ForeignKey("dbo.MapTypes", t => t.MapTypeId)
                .Index(t => t.MapId)
                .Index(t => t.MapTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flags", "FlagTypeId", "dbo.FlagTypes");
            DropForeignKey("dbo.AdoptedMaps", "ShareStatusTypeId", "dbo.ShareStatusTypes");
            DropForeignKey("dbo.Maps", "OrientationTypeId", "dbo.OrientationTypes");
            DropForeignKey("dbo.MapMapTypes", "MapTypeId", "dbo.MapTypes");
            DropForeignKey("dbo.MapMapTypes", "MapId", "dbo.Maps");
            DropForeignKey("dbo.AdoptedMaps", "MapId", "dbo.Maps");
            DropForeignKey("dbo.Flags", "AdoptedMapId", "dbo.AdoptedMaps");
            DropIndex("dbo.MapMapTypes", new[] { "MapTypeId" });
            DropIndex("dbo.MapMapTypes", new[] { "MapId" });
            DropIndex("dbo.Maps", new[] { "OrientationTypeId" });
            DropIndex("dbo.Flags", new[] { "AdoptedMapId" });
            DropIndex("dbo.Flags", new[] { "FlagTypeId" });
            DropIndex("dbo.AdoptedMaps", new[] { "ShareStatusTypeId" });
            DropIndex("dbo.AdoptedMaps", new[] { "MapId" });
            DropTable("dbo.MapMapTypes");
            DropTable("dbo.FlagTypes");
            DropTable("dbo.ShareStatusTypes");
            DropTable("dbo.OrientationTypes");
            DropTable("dbo.MapTypes");
            DropTable("dbo.Maps");
            DropTable("dbo.Flags");
            DropTable("dbo.AdoptedMaps");
        }
    }
}
