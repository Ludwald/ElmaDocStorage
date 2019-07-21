using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Elma.Models.utils
{
    public static class Utils
    {
        public static string GetLimitedString(this string str, int charallowed)
        {
            if (str.Length > charallowed)
                return str.Substring(0, charallowed) + "...";
            return str;
        }
    }
}