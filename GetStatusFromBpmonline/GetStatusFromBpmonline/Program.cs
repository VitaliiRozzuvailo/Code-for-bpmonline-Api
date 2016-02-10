using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace GetStatusFromBpmonline
{
    class Program
    {
        // Строка адреса BPMonline сервиса OData.
        private const string serverUri = "http://localhost:8081/0/ServiceModel/EntityDataService.svc/";
        private const string authServiceUtri = "http://localhost:8081/ServiceModel/AuthService.svc/Login";

        // Ссылки на пространства имен XML.
        private static readonly XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static readonly XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";

        static void Main(string[] args)
        {
            GetOdataCollectionByAuthByHttpExample("Supervisor", "Supervisor");
            //GetOdataObjectByFilterConditionExample();
        }

        // Строка запроса:
        // GET <Адрес приложения BPMonline>/0/ServiceModel/EntityDataService.svc/ContactCollection?$filter=Id eq guid'00000000-0000-0000-0000-000000000000'

        public static void GetOdataObjectByFilterConditionExample()
        {
            // Id искомого объекта.
            string orderNumber = "У-57-573";
            // Формирование строки запроса к сервису.
            string requestUri = serverUri + "OrderCollection?$filter = Number eq '" + orderNumber + "'";
            // Создание объекта запроса к сервису.
            var request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "GET";
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            using (var response = request.GetResponse())
            {
                // Получение ответа от сервиса в xml-формате.
                XDocument xmlDoc = XDocument.Load(response.GetResponseStream());
                // Получение коллекции объектов контактов, соответствующих условию запроса.
                var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                               select new
                               {
                                   Id = new Guid(entry.Element(atom + "content")
                                                     .Element(dsmd + "properties")
                                                     .Element(ds + "Id").Value),
                                   Name = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "Name").Value
                                   // Инициализация свойств объекта, необходимых для дальнейшего использования.
                               };
                foreach (var contact in contacts)
                {
                    // Выполнение действий над контактом.
                }
            }
        }

        public static void GetOdataCollectionByAuthByHttpExample(string userName, string userPassword)
        {
            // Создание запроса на аутентификацию.
            var authRequest = HttpWebRequest.Create(authServiceUtri) as HttpWebRequest;
            authRequest.Method = "POST";
            authRequest.ContentType = "application/json";
            var bpmCookieContainer = new CookieContainer();
            // Включение использования cookie в запросе.
            authRequest.CookieContainer = bpmCookieContainer;
            // Получение потока, ассоциированного с запросом на аутентификацию.
            using (var requesrStream = authRequest.GetRequestStream())
            {
                // Запись в поток запроса учетных данных пользователя BPMonline и дополнительных параметров запроса.
                using (var writer = new StreamWriter(requesrStream))
                {
                    writer.Write(@"{
                                ""UserName"":""" + userName + @""",
                                ""UserPassword"":""" + userPassword + @""",
                                ""SolutionName"":""TSBpm"",
                                ""TimeZoneOffset"":-120,
                                ""Language"":""Ru-ru""
                                }");
                }
            }
            // Получение ответа от сервера. Если аутентификация проходит успешно, в объекте bpmCookieContainer будут 
            // помещены cookie, которые могут быть использованы для последующих запросов.
            using (var response = (HttpWebResponse)authRequest.GetResponse())
            {
                // Создание запроса на получение данных от сервиса OData.
                var dataRequest = HttpWebRequest.Create(serverUri + "ContactCollection?select=Id, Name")
                                        as HttpWebRequest;
                // Для получения данных используется HTTP-метод GET.
                dataRequest.Method = "GET"; 
                // Добавление полученных ранее аутентификационных cookie в запрос на получение данных.
                dataRequest.CookieContainer = bpmCookieContainer;
                // Получение ответа от сервера.
                using (var dataResponse = (HttpWebResponse)dataRequest.GetResponse())
                {
                    // Загрузка ответа сервера в xml-документ для дальнейшей обработки.
                    XDocument xmlDoc = XDocument.Load(dataResponse.GetResponseStream());
                    // Получение коллекции объектов контактов, соответствующих условию запроса.
                    var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                                   select new
                                   {
                                       Id = new Guid(entry.Element(atom + "content")
                                                              .Element(dsmd + "properties")
                                                              .Element(ds + "Id").Value),
                                       Name = entry.Element(atom + "content")
                                                       .Element(dsmd + "properties")
                                                       .Element(ds + "Name").Value
                                   };
                    foreach (var contact in contacts)
                    {
                        // Выполнение действий с контактами.
                    }
                }
            }
        }
    }
}
