using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace ImportOrderFromApplication
{
    public class CrmManager
    {
        private const string processServiceUri = "https://parnikrb-debug.bpmonline.com/1/rest/SxRunProcessAddingOrderFromSiteService/";
        private const string authServiceUri = "https://parnikrb-debug.bpmonline.com/ServiceModel/AuthService.svc/Login";

        private static CookieContainer AuthCookie = new CookieContainer();

        public static bool IsAuthentificated
        {
            get { return (AuthCookie != null); }
        }

        public static bool TryLogin(string userName, string userPassword)
        {
            var authRequest = HttpWebRequest.Create(authServiceUri) as HttpWebRequest;
            authRequest.Method = "POST";
            authRequest.ContentType = "application/json";
            authRequest.CookieContainer = AuthCookie;

            using (var requesrStream = authRequest.GetRequestStream())
            using (var writer = new StreamWriter(requesrStream))
                writer.Write(@"{
                    ""UserName"":""" + userName + @""",
                    ""UserPassword"":""" + userPassword + @"""
                }");

            using (var response = (HttpWebResponse)authRequest.GetResponse())
                return (AuthCookie.Count > 0);
        }

        public static string Run(string data, string method)
        {
            if (!IsAuthentificated) return "";

            string requestString = string.Format(processServiceUri + method);
            HttpWebRequest request = HttpWebRequest.Create(requestString) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.CookieContainer = AuthCookie;

            using (var requesrStream = request.GetRequestStream())
            using (var writer = new StreamWriter(requesrStream))
                writer.Write(data);

            var result = string.Empty;

            using (var response = request.GetResponse())
            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
            return result;
        }
    }
}
