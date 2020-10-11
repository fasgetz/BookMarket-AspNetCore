using Microsoft.AspNetCore.Html;
using System.Linq;

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

        public static string ToTranslit(this string value)
        {
            return Transliteration.Front(value);
        }

        public static string GetCountWords(this string value, int countWords)
        {
            return string.Join(' ', value.Split(' ').Take(countWords));
        }
    }
}
