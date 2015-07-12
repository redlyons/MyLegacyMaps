namespace MyLegacyMaps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotosUrlAdded2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flags", "PhotosUrl", c => c.String(maxLength: 500, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Flags", "PhotosUrl");
        }
    }
}
