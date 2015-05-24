using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace MyLegacyMaps.DataAccess
{
    public class Initializer : DropCreateDatabaseIfModelChanges<MyLegacyMapsContext>
    {
    }
}