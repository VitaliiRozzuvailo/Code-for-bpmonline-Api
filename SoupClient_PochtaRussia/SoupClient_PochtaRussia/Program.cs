using System;
using System.IO;
using System.Net;
using System.Xml;

namespace SoupClient_PochtaRussia
{
    class Program
    {
        static void Main(string[] args)
        {
            new RussiaPost_SoapClient().GetOperationHistory("RA644000001RU");
            new RussiaPost_SoapClient().PostalOrderEventsForMail("14102192069353");
            //XmlDocument soapEnvelopeXml = new XmlDocument();
            //soapEnvelopeXml.LoadXml(r);

            /*        string soapQuestion = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:oper=""http://russianpost.org/operationhistory"" xmlns:data=""http://russianpost.org/operationhistory/data"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""> <soap:Header/>  <soap:Body><oper:getOperationHistory><data:OperationHistoryRequest><data:Barcode>RA644000001RU</data:Barcode><data:MessageType>0</data:MessageType><data:Language>RUS</data:Language></data:OperationHistoryRequest><data:AuthorizationHeader soapenv:mustUnderstand=""1""><data:login>ihoDnVDnFhtpYU</data:login><data:password>ytQOVwJSsEZA</data:password></data:AuthorizationHeader></oper:getOperationHistory></soap:Body></soap:Envelope>";



                    // prepare outgoing SOAP text
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(soapQuestion);

                    // prepare web request
                    Uri uri = new Uri(@"https://tracking.russianpost.ru/rtm34");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Timeout = 100000000;
                    request.Method = "POST";

                    // you may need these headers
                    request.ContentType = "application/soap+xml;charset=\"utf-16\"";
                    request.Accept = "application/soap+xml";

                    // do request
                    Stream stream = request.GetRequestStream();
                    doc.Save(stream);
                    stream.Close();

                    // get response
                    WebResponse response = request.GetResponse();
                    stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string result = reader.ReadToEnd();

                    // process incoming SOAP text
                    doc = new XmlDocument();
                    doc.LoadXml(result);
                    doc.PreserveWhitespace = true;

                    StringWriter stringwriter = new StringWriter();
                    XmlTextWriter xmlwriter = new XmlTextWriter(stringwriter);
                    xmlwriter.Formatting = Formatting.Indented;
                    doc.WriteTo(xmlwriter);

                    // result
                    string soapAnswer = stringwriter.ToString();
                    Console.WriteLine(soapAnswer);*/
        }
    }
}