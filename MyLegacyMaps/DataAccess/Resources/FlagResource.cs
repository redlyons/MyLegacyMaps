using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyLegacyMaps.DataAccess;
using MyLegacyMaps.Models;

namespace MyLegacyMaps.DataAccess.Resources
{
   
    public class FlagResource
    {
       

        static public bool AddFlag(Flag flag)
        {
            if (flag == null) return false;

            try
            {
                using (var context = new MyLegacyMapsContext())
                {
                    context.Flags.Add(flag);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }

    }
}