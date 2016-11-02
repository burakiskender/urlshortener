using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace UrlShortener.Web.Helpers
{
    public static class UrlHelper
    {

        public const string InvalidUrlMessage="Invalid url.Please enter a valid url.";
        public const string InactiveUrlMessage = "This url is not active. Please enter a valid url.";

        public static bool IsUrlValid(string url)
        {
            
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            var regex = new Regex(@"^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}"); Uri uriResult;
            return regex.IsMatch(url) && (Uri.TryCreate(url, UriKind.Absolute, out uriResult) && 
                  (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));

           
        }

        public static bool ContainsProtocol(this string url)
        {
            var regex = new Regex(@"^(http|https)://");
            return regex.IsMatch(url);
        }

        public static string AddProtocol(this string url)
        {
            url= "http://" + url;
            return url;
        }

        public static string GenerateShortUrl(string url)
        {
            return String.Format("{0:X}", url.GetHashCode());
        }

        public static bool IsUrlActive(string url)
        {
           try
            {
                if (!url.ContainsProtocol())
                {
                    url = url.AddProtocol();
                }
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "HEAD";
                    var response = request.GetResponse() as HttpWebResponse;
                    return response != null && (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}