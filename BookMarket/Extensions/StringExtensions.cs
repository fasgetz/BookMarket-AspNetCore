using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a standard string into a htmlstring for rendering in a view
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HtmlString ToHtmlString(this string value)
        {
            return new HtmlString(value);
        }
    }
}
