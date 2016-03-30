using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace JsonParserConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute();
            Console.ReadKey();
        }

        static void Execute()
        {
            // Выполнение аутентификации пользователя с имененем UserName. 
            if (BPMonlineRunService.TryLogin("Supervisor", "12345"))
            {
                try
                {
                    string data = "{\"data\":\""
                        + "{'Order':"
                            + "{"
                                + "'Name':'Ironman',"
                                + "'Phone':'9379992',"
                                + "'Email':'',"
                                + "'City':'Харьков',"
                                + "'Country':'Россия',"
                                + "'Notes':'',"
                                + "'Site':'http://test2.shopikk.ru', "
                                + "'Currency': '5FB76920-53E6-DF11-971B-001D60E938C6'},"
                        + "'OrderProducts':"
                            + "[{"
                                + "'ProductId':'0708D0BC-985B-4379-8CC5-07DA294A0C93',"
                                + "'Quantity':'1',"
                                + "'Price':'1399'"
                            + "}]}\"}";

                    // Добавление нового заказа через запуск бизнес-процесса в системе.
                    var order = BPMonlineRunService.Run(data, "Execute");
                    //var order = "66E9E644-E223-4F85-AB91-41E220A1BD6C";

                    Thread.Sleep(10000);

                    var email = "ironman@gmail.com";
                    var emailStr = "{\"OrderId\":\"" + order.ToUpper() + "\",\"Email\":\"" + email + "\"}";

                    // Добавление нового заказа через запуск бизнес-процесса в системе.
                    BPMonlineRunService.Run(emailStr, "AddEmailToContact");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    public class BPMonlineRunService
    {
        private const string processServiceUri =
                "https://parnikrb-debug.bpmonline.com/1/rest/SxRunProcessAddingOrderFromSiteService/";
        private const string authServiceUri =
                "https://parnikrb-debug.bpmonline.com/ServiceModel/AuthService.svc/Login";

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
            using (var writer = new StreamWriter(requesrStream, Encoding.ASCII))
                writer.Write(data);

            var result = string.Empty;

            using (var response = request.GetResponse())
            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                Console.WriteLine(responseText);
                dynamic parameters = JsonConvert.DeserializeObject(responseText);
                result = parameters.ExecuteResult.ToString();
            }
            return result;
        }
    }
}
