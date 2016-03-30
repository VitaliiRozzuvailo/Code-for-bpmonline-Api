using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Xml;

namespace PostKZ
{
    class Program
    {
        private const string URL_PRICE = @"http://rates.kazpost.kz/postratesws/postratesws.wsdl";
        private const string URL_ADDRLETTER = @"http://rates.kazpost.kz/postratesprod/postratesws.wsdl";
        private const string URL_STATUS = @"http://track.kazpost.kz/api/v2/";
        private const string NUMBER_ORDER = @"RK070333447CN";

        private const string KEY = @"e73dc2b873ee4fb282ce1d4d46c85c09"; //TODO

        public static void Main(string[] args)
        {
            /*{"trackid":"RK070333557CP","timestamp":"14:48:19 16.02.2016","exectime":"0ms","api":"main","data_name":"prod","error":"Информация о почтовом отправлении \"RK070333557CP\"  не найдена в трекинговой системе АО \"Казпочта\""}*/
            //get status
            // var responce = GetStausKZ(NUMBER_ORDER);

            //get price
            GetPriceKZ(2000, 5, 12300);

            //Console.WriteLine("Статус: " + status);
            //Console.WriteLine("Цена: " + p);

            GetAddrLetterKZ(
                "Айзан Варкулин",
                "7012345678",
                "Казахстан",
                "Караганда",
                "100020",
                "Карагандинская",
                "Сторожевая",
                "3a",
                "4.6",
                "12000"
                );

        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        //GEt order status
        public static string GetStausKZ(string ttn)
        {
            WebRequest req = WebRequest.Create(URL_STATUS + ttn);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();

            dynamic resObj = JsonConvert.DeserializeObject<dynamic>(Out);
            var status = resObj.status.Value;
            return status;
        }

        //TO - Астана
        //SendMethod - Наземный
        //
        public static string GetPriceKZ(int weight, int fromCity, int price)
        {
            HttpWebRequest request = CreateWebRequest(URL_PRICE);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(string.Format(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:pos='http://webservices.kazpost.kz/postratesws'>
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <pos:GetPostRateRequest>
		                                <pos:MailInfo>
			                                <pos:Product>4</pos:Product>
			                                <pos:MailCat>2</pos:MailCat>
			                                <pos:SendMethod>1</pos:SendMethod>
			                                <pos:Weight>{0}</pos:Weight>
			                                <pos:From>1</pos:From>
			                                <pos:To>{1}</pos:To>
			                                <pos:SpecMarks/>
			                                <pos:InCity/>
			                                <pos:Size/>
			                                <pos:DeclaredValue>{2}</pos:DeclaredValue>
		                                </pos:MailInfo>
	                                </pos:GetPostRateRequest>
                                </soapenv:Body>
                                </soapenv:Envelope>", weight, fromCity, price));

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    var result = string.Empty;
                    string soapResult = rd.ReadToEnd();

                    XmlDocument document = new XmlDocument();
                    document.LoadXml(soapResult);  //loading soap message as string 
                    XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
                    manager.AddNamespace("ns2", "http://webservices.kazpost.kz/postratesws");
                    XmlNodeList xNodelst = document.DocumentElement.SelectNodes("//ns2:GetPostRateResponse", manager);
                    foreach (XmlNode xn in xNodelst)
                    {
                        result = xn["ns2:PostRate"].InnerText;
                    }
                    return result;
                }
            }
        }

        public static string GetAddrLetterKZ(string nameR, string phoneR, string countryR, string cityR, string indexR, string districtR, string streetR, string houseR, string weight, string price)
        {
            HttpWebRequest request = CreateWebRequest(URL_ADDRLETTER);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(string.Format(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:pos='http://webservices.kazpost.kz/postratesws'>
                    <soapenv:Header/>
                    <soapenv:Body>
	                    <pos:GetAddrLetterRequest>
		                    <pos:Key>{0}</pos:Key>
		                    <pos:AddrInfo>
			                    <!--Optional: FOR RECIPIENT-->
			                    <pos:RcpnName>{1}</pos:RcpnName>
			                    <pos:RcpnPhone>{2}</pos:RcpnPhone>
			                    <!--Optional:-->
			                    <pos:RcpnCountry>{3}</pos:RcpnCountry>
			                    <pos:RcpnIndex>{4}</pos:RcpnIndex>
			                    <pos:RcpnCity>{5}</pos:RcpnCity>
			                    <pos:RcpnDistrict>{6}</pos:RcpnDistrict>
			                    <pos:RcpnStreet>{7}</pos:RcpnStreet>
			                    <pos:RcpnHouse>{8}</pos:RcpnHouse>

			                    <!--Optional: FOR SENDER-->
                                <pos:SndrBIN>141140024070</pos:SndrBIN>
			                    <pos:SndrName>ТОО 'Грин Хаус'</pos:SndrName>
                                <pos:SndrPhone>7778996404</pos:SndrPhone>
			                    <!--Optional:-->
			                    <pos:SndrEmail>greenhouse@post.kz</pos:SndrEmail>
			                    <pos:SndrCountry>Казахстан</pos:SndrCountry>
			                    <pos:SndrIndex>010000</pos:SndrIndex>
			                    <pos:SndrCity>Астана</pos:SndrCity>
			                    <pos:SndrDistrict>Акмолинская</pos:SndrDistrict>
			                    <pos:SndrStreet>Лермонтова</pos:SndrStreet>
			                    <pos:SndrHouse>26</pos:SndrHouse>

			                    <!--Optional:-->
			                    <pos:Weight>{9}</pos:Weight>
			                    <pos:DeclaredValue>{10}</pos:DeclaredValue>
                                <pos:ProductCode>P103</pos:ProductCode>
		                    </pos:AddrInfo>
	                    </pos:GetAddrLetterRequest>
                    </soapenv:Body>
                    </soapenv:Envelope>", KEY, nameR, phoneR, countryR, indexR, cityR, districtR, streetR, houseR, weight, price));

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();

                    XmlDocument document = new XmlDocument();
                    document.LoadXml(soapResult);  //loading soap message as string 
                    XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
                    manager.AddNamespace("ns2", "http://webservices.kazpost.kz/postratesws");
                    XmlNodeList xNodelst = document.DocumentElement.SelectNodes("//ns2:GetAddrLetterResponse", manager);

                    string pdfBase64 = string.Empty;
                    string ttn = string.Empty;

                    foreach (XmlNode xn in xNodelst)
                    {
                        pdfBase64 = xn["ns2:AddrLetPdf"].InnerText;
                        ttn = xn["ns2:Barcode"].InnerText;
                    }

                    //TODO
                    return ttn + "," + pdfBase64;
                }
            }
        }
    }
}