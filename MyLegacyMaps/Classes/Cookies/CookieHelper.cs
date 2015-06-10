using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLegacyMaps.Classes.Cookies
{
    public class CookieHelper : ICookieHelper
    {
        public T GetCookie<T>(string name, HttpContextBase context)
        {
            var retVal = default(T);
            if (context == null)
                return retVal;

            var cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                Type type = typeof(T);
                switch(type.Name)
                {
                    case "Int32":
                        int intValue = 0;
                        if(Int32.TryParse(cookie.Value, out intValue))
                        {
                            retVal = (T)Convert.ChangeType(cookie.Value, typeof(T));
                        }

                        break;

                }
            }

            return retVal;
        }

        public void SetCookie(string name, string value, HttpContextBase context)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.HttpOnly = true;
            context.Response.Cookies.Add(cookie);
        }
    }
}