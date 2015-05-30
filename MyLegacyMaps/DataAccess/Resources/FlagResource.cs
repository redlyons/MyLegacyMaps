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
       

        static public async Task<bool> AddFlag(Flag flag)
        {
            if(flag == null)
            {                
                return await Task.FromResult(false);
            }

            var result = false;
            try
            {
                using (var context = new MyLegacyMapsContext())
                {
                    context.Flags.Add(flag);
                    await context.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
               
            }
            return await Task.FromResult(result);

        }

    }
}