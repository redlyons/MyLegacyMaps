using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyLegacyMaps.Classes.Cookies
{
    public interface ICookieHelper
    {
        T GetCookie<T>(string name, HttpContextBase context);
        void SetCookie(string name, string value, HttpContextBase context);
    }
}
