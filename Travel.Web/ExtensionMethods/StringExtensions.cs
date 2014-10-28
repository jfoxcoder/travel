using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Travel.Web.ExtensionMethods
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// Produces are 
        /// 
        /// Turns "United States" into "united-states"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSlug(this string name)
        {
            return name.Replace(' ', '-').ToLower();
        }

        /// <summary>
        /// Turns "united-states" into "United States"
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static string FromSlug(this string slug)
        {            
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(slug.Replace('-', ' '));
        }

        public static string AsEncodedUrl(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// source: http://stackoverflow.com/a/1614090
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TruncateAtWord(this string input, int length=100)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length);
            return string.Format("{0}&hellip;", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }
    }
}