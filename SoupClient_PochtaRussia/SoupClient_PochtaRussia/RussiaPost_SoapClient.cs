using System;
using System.IO;
using System.Net;
using System.Xml;

namespace SoupClient_PochtaRussia
{
    public class RussiaPost_SoapClient
    {
        private const string LOGIN = "nef@parnikrb.ru";
        private const string PASSWORD = "dima1708";

        public void SendRequestToRussianPost()
        {
            string soapXml = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""
                                              xmlns:oper=""http://russianpost.org/operationhistory""
                                              xmlns:data=""http://russianpost.org/operationhistory/data""
                                              xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope"">
                                <soap:Header/>
                                    <soap:Body>
                                        <oper:getOperationHistory>
                                            <data:OperationHistoryRequest>
                                                <data:Barcode>RA644000001RU</data:Barcode>
                                                <data:MessageType>0</data:MessageType>
                                                <data:Language>RUS</data:Language>
                                            </data:OperationHistoryRequest>
                                            <data:AuthorizationHeader soapenv:mustUnderstand=""1"">
                                                <data:login>nef@parnikrb.ru</data:login>
                                                <data:password>dima1708</data:password>
                                            </data:AuthorizationHeader>
                                        </oper:getOperationHistory>
                                    </soap:Body>
                                </soap:Envelope>";

            var res = this.ServiceCall(soapXml);
        }

        /// <summary>
        /// Execute a Soap WebService call
        /// </summary>
        private string ServiceCall(string xmlSoap)
        {
            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xmlSoap);

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            try
            {
                IAsyncResult asyncResult = request.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                string soapResult;
                using (WebResponse webResponse = request.EndGetResponse(asyncResult))
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                    return soapResult;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        private HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://tracking.russianpost.ru/rtm34");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "applicatoin/soap+xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}