using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PostKZ
{
    class Program
    {
        private const string URL_PRICE = @"http://rates.kazpost.kz/postratesws/postratesws.wsdl";
        private const string URL_STATUS = @"http://track.kazpost.kz/api/v2/";
        private const string NUMBER_ORDER = @"RK070333447CN";

        static void Main(string[] args)
        {
            //get status
            var responce = GetStaus(URL_STATUS + NUMBER_ORDER);
            dynamic resObj = JsonConvert.DeserializeObject<dynamic>(responce);
            var status = resObj.status.Value;

            //get price
             GetPrice();

            Console.WriteLine("Статус: " + status);
            //Console.WriteLine("Цена: " + p);

        }

        //GEt order status
        private static string GetStaus(string Url)
        {
            WebRequest req = WebRequest.Create(Url);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        public static void GetPrice()
        {
            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:pos=""http://webservices.kazpost.kz/postratesws"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <pos:GetPostRateRequest>
		                                <pos:MailInfo>
			                                <pos:Product>1</pos:Product>
			                                <pos:MailCat>1</pos:MailCat>
			                                <pos:SendMethod>1</pos:SendMethod>
			                                <pos:Weight>2000</pos:Weight>
			                                <pos:From>2</pos:From>
			                                <pos:To>3</pos:To>
			                                <pos:SpecMarks/>
			                                <pos:InCity/>
			                                <pos:Size/>
			                                <pos:DeclaredValue>1000</pos:DeclaredValue>
		                                </pos:MailInfo>
	                                </pos:GetPostRateRequest>
                                </soapenv:Body>
                                </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    Console.WriteLine(soapResult);
                }
            }
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        private static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL_PRICE);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}