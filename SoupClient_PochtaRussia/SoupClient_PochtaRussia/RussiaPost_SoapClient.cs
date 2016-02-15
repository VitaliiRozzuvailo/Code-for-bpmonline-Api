using System;
using System.IO;
using System.Net;
using System.Xml;

namespace SoupClient_PochtaRussia
{
    public class RussiaPost_SoapClient
    {
        private const string LOGIN = "ihoDnVDnFhtpYU";
        private const string PASSWORD = "ytQOVwJSsEZA";

        //получение подробной информации обо всех операциях, совершенных над отправлением 
        public string GetOperationHistory(string ttn)
        {
            string result = string.Empty;
            string request = string.Format(
                @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope""
                                xmlns:oper=""http://russianpost.org/operationhistory""
                                xmlns:data=""http://russianpost.org/operationhistory/data""
                                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope"">
                <soap:Header/>
                    <soap:Body>
                        <oper:getOperationHistory>
                            <data:OperationHistoryRequest>
                                <data:Barcode>{0}</data:Barcode>
                                <data:MessageType>0</data:MessageType>
                                <data:Language>RUS</data:Language>
                            </data:OperationHistoryRequest>
                            <data:AuthorizationHeader soapenv:mustUnderstand=""1"">
                                <data:login>{1}</data:login>
                                <data:password>{2}</data:password>
                            </data:AuthorizationHeader>
                        </oper:getOperationHistory>
                    </soap:Body>
                </soap:Envelope>", ttn, LOGIN, PASSWORD);

            //var response = SendRequestToRussianPost(request);

            var response = "<?xml version='1.0' encoding='UTF-8'?><S:Envelope xmlns:S=\"http://www.w3.org/2003/05/soap-envelope\"><S:Body><ns7:getOperationHistoryResponse xmlns:ns2=\"http://russianpost.org/sms-info/data\" xmlns:ns3=\"http://russianpost.org/operationhistory/data\" xmlns:ns4=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns5=\"http://www.russianpost.org/custom-duty-info/data\" xmlns:ns6=\"http://www.russianpost.org/RTM/DataExchangeESPP/Data\" xmlns:ns7=\"http://russianpost.org/operationhistory\"><ns3:OperationHistoryData><ns3:historyRecord><ns3:AddressParameters><ns3:DestinationAddress><ns3:Index>644008</ns3:Index><ns3:Description>Омск 8</ns3:Description></ns3:DestinationAddress><ns3:OperationAddress><ns3:Index>644001</ns3:Index><ns3:Description>Омск 1</ns3:Description></ns3:OperationAddress><ns3:MailDirect><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:MailDirect><ns3:CountryOper><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryOper></ns3:AddressParameters><ns3:FinanceParameters><ns3:Payment>741000</ns3:Payment><ns3:Value>4900000</ns3:Value><ns3:MassRate>17200</ns3:MassRate><ns3:InsrRate>49000</ns3:InsrRate><ns3:AirRate>1200</ns3:AirRate><ns3:Rate>0</ns3:Rate><ns3:CustomDuty>0</ns3:CustomDuty></ns3:FinanceParameters><ns3:ItemParameters><ns3:Barcode>RA644000001RU</ns3:Barcode><ns3:ValidRuType>false</ns3:ValidRuType><ns3:ValidEnType>false</ns3:ValidEnType><ns3:ComplexItemName>Отправление EMS с объявл. ценностью и налож. платежом</ns3:ComplexItemName><ns3:MailRank><ns3:Id>0</ns3:Id><ns3:Name>Без разряда</ns3:Name></ns3:MailRank><ns3:PostMark><ns3:Id>0</ns3:Id><ns3:Name>Без отметки</ns3:Name></ns3:PostMark><ns3:MailType><ns3:Id>7</ns3:Id><ns3:Name>Отправление EMS</ns3:Name></ns3:MailType><ns3:MailCtg><ns3:Id>4</ns3:Id><ns3:Name>С объявленной ценностью и наложенным платежом</ns3:Name></ns3:MailCtg><ns3:Mass>101</ns3:Mass></ns3:ItemParameters><ns3:OperationParameters><ns3:OperType><ns3:Id>1</ns3:Id><ns3:Name>Прием</ns3:Name></ns3:OperType><ns3:OperAttr><ns3:Id>1</ns3:Id><ns3:Name>Единичный</ns3:Name></ns3:OperAttr><ns3:OperDate>2015-03-23T10:01:00.000+03:00</ns3:OperDate></ns3:OperationParameters><ns3:UserParameters><ns3:SendCtg><ns3:Id>1</ns3:Id><ns3:Name>Население</ns3:Name></ns3:SendCtg><ns3:Sndr>РТМ0046-Иванов-01</ns3:Sndr><ns3:Rcpn>Петров Пётр Петрович</ns3:Rcpn></ns3:UserParameters></ns3:historyRecord><ns3:historyRecord><ns3:AddressParameters><ns3:DestinationAddress><ns3:Index>644008</ns3:Index><ns3:Description>Омск 8</ns3:Description></ns3:DestinationAddress><ns3:OperationAddress><ns3:Index>644001</ns3:Index><ns3:Description>Омск 1</ns3:Description></ns3:OperationAddress><ns3:MailDirect><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:MailDirect><ns3:CountryOper><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryOper></ns3:AddressParameters><ns3:FinanceParameters><ns3:Payment>741000</ns3:Payment><ns3:Value>4900000</ns3:Value><ns3:MassRate>17200</ns3:MassRate><ns3:InsrRate>49000</ns3:InsrRate><ns3:AirRate>1200</ns3:AirRate><ns3:Rate>0</ns3:Rate><ns3:CustomDuty>0</ns3:CustomDuty></ns3:FinanceParameters><ns3:ItemParameters><ns3:Barcode>RA644000001RU</ns3:Barcode><ns3:ValidRuType>false</ns3:ValidRuType><ns3:ValidEnType>false</ns3:ValidEnType><ns3:ComplexItemName>Отправление EMS с объявл. ценностью и налож. платежом</ns3:ComplexItemName><ns3:MailRank><ns3:Id>0</ns3:Id><ns3:Name>Без разряда</ns3:Name></ns3:MailRank><ns3:PostMark><ns3:Id>0</ns3:Id><ns3:Name>Без отметки</ns3:Name></ns3:PostMark><ns3:MailType><ns3:Id>7</ns3:Id><ns3:Name>Отправление EMS</ns3:Name></ns3:MailType><ns3:MailCtg><ns3:Id>4</ns3:Id><ns3:Name>С объявленной ценностью и наложенным платежом</ns3:Name></ns3:MailCtg><ns3:Mass>101</ns3:Mass></ns3:ItemParameters><ns3:OperationParameters><ns3:OperType><ns3:Id>8</ns3:Id><ns3:Name>Обработка</ns3:Name></ns3:OperType><ns3:OperAttr><ns3:Id>3</ns3:Id><ns3:Name>Прибыло в сортировочный центр</ns3:Name></ns3:OperAttr><ns3:OperDate>2015-03-24T10:01:00.000+03:00</ns3:OperDate></ns3:OperationParameters><ns3:UserParameters><ns3:SendCtg><ns3:Id>1</ns3:Id><ns3:Name>Население</ns3:Name></ns3:SendCtg><ns3:Sndr>РТМ0046-Иванов-01</ns3:Sndr><ns3:Rcpn>Петров Пётр Петрович</ns3:Rcpn></ns3:UserParameters></ns3:historyRecord><ns3:historyRecord><ns3:AddressParameters><ns3:DestinationAddress><ns3:Index>644008</ns3:Index><ns3:Description>Омск 8</ns3:Description></ns3:DestinationAddress><ns3:OperationAddress><ns3:Index>644001</ns3:Index><ns3:Description>Омск 1</ns3:Description></ns3:OperationAddress><ns3:MailDirect><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:MailDirect><ns3:CountryOper><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryOper></ns3:AddressParameters><ns3:FinanceParameters><ns3:Payment>741000</ns3:Payment><ns3:Value>4900000</ns3:Value><ns3:MassRate>17200</ns3:MassRate><ns3:InsrRate>49000</ns3:InsrRate><ns3:AirRate>1200</ns3:AirRate><ns3:Rate>0</ns3:Rate><ns3:CustomDuty>0</ns3:CustomDuty></ns3:FinanceParameters><ns3:ItemParameters><ns3:Barcode>RA644000001RU</ns3:Barcode><ns3:ValidRuType>false</ns3:ValidRuType><ns3:ValidEnType>false</ns3:ValidEnType><ns3:ComplexItemName>Отправление EMS с объявл. ценностью и налож. платежом</ns3:ComplexItemName><ns3:MailRank><ns3:Id>0</ns3:Id><ns3:Name>Без разряда</ns3:Name></ns3:MailRank><ns3:PostMark><ns3:Id>0</ns3:Id><ns3:Name>Без отметки</ns3:Name></ns3:PostMark><ns3:MailType><ns3:Id>7</ns3:Id><ns3:Name>Отправление EMS</ns3:Name></ns3:MailType><ns3:MailCtg><ns3:Id>4</ns3:Id><ns3:Name>С объявленной ценностью и наложенным платежом</ns3:Name></ns3:MailCtg><ns3:Mass>101</ns3:Mass></ns3:ItemParameters><ns3:OperationParameters><ns3:OperType><ns3:Id>8</ns3:Id><ns3:Name>Обработка</ns3:Name></ns3:OperType><ns3:OperAttr><ns3:Id>4</ns3:Id><ns3:Name>Покинуло сортировочный центр</ns3:Name></ns3:OperAttr><ns3:OperDate>2015-03-24T10:01:00.000+03:00</ns3:OperDate></ns3:OperationParameters><ns3:UserParameters><ns3:SendCtg><ns3:Id>1</ns3:Id><ns3:Name>Население</ns3:Name></ns3:SendCtg><ns3:Sndr>РТМ0046-Иванов-01</ns3:Sndr><ns3:Rcpn>Петров Пётр Петрович</ns3:Rcpn></ns3:UserParameters></ns3:historyRecord><ns3:historyRecord><ns3:AddressParameters><ns3:DestinationAddress><ns3:Index>0</ns3:Index><ns3:Description></ns3:Description></ns3:DestinationAddress><ns3:OperationAddress><ns3:Index>644008</ns3:Index><ns3:Description>Омск 8</ns3:Description></ns3:OperationAddress><ns3:MailDirect><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:MailDirect><ns3:CountryFrom><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryFrom><ns3:CountryOper><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryOper></ns3:AddressParameters><ns3:FinanceParameters><ns3:Payment>0</ns3:Payment><ns3:Value>0</ns3:Value><ns3:MassRate>0</ns3:MassRate><ns3:InsrRate>0</ns3:InsrRate><ns3:AirRate>0</ns3:AirRate><ns3:Rate>0</ns3:Rate><ns3:CustomDuty>0</ns3:CustomDuty></ns3:FinanceParameters><ns3:ItemParameters><ns3:Barcode>RA644000001RU</ns3:Barcode><ns3:ValidRuType>false</ns3:ValidRuType><ns3:ValidEnType>false</ns3:ValidEnType><ns3:ComplexItemName>Отправление EMS с объявл. ценностью и налож. платежом</ns3:ComplexItemName><ns3:MailRank><ns3:Id>0</ns3:Id><ns3:Name>Без разряда</ns3:Name></ns3:MailRank><ns3:PostMark><ns3:Id>0</ns3:Id><ns3:Name>Без отметки</ns3:Name></ns3:PostMark><ns3:MailType><ns3:Id>7</ns3:Id><ns3:Name>Отправление EMS</ns3:Name></ns3:MailType><ns3:MailCtg><ns3:Id>4</ns3:Id><ns3:Name>С объявленной ценностью и наложенным платежом</ns3:Name></ns3:MailCtg><ns3:Mass>0</ns3:Mass></ns3:ItemParameters><ns3:OperationParameters><ns3:OperType><ns3:Id>8</ns3:Id><ns3:Name>Обработка</ns3:Name></ns3:OperType><ns3:OperAttr><ns3:Id>2</ns3:Id><ns3:Name>Прибыло в место вручения</ns3:Name></ns3:OperAttr><ns3:OperDate>2015-03-26T10:08:00.000+03:00</ns3:OperDate></ns3:OperationParameters><ns3:UserParameters><ns3:SendCtg><ns3:Id>0</ns3:Id></ns3:SendCtg><ns3:Sndr></ns3:Sndr><ns3:Rcpn></ns3:Rcpn></ns3:UserParameters></ns3:historyRecord><ns3:historyRecord><ns3:AddressParameters><ns3:DestinationAddress><ns3:Index>644008</ns3:Index><ns3:Description>Омск 8</ns3:Description></ns3:DestinationAddress><ns3:OperationAddress><ns3:Index>644001</ns3:Index><ns3:Description>Омск 1</ns3:Description></ns3:OperationAddress><ns3:MailDirect><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:MailDirect><ns3:CountryOper><ns3:Id>643</ns3:Id><ns3:Code2A>RU</ns3:Code2A><ns3:Code3A>RUS</ns3:Code3A><ns3:NameRU>Российская Федерация</ns3:NameRU><ns3:NameEN>Russian Federation</ns3:NameEN></ns3:CountryOper></ns3:AddressParameters><ns3:FinanceParameters><ns3:Payment>741000</ns3:Payment><ns3:Value>4900000</ns3:Value><ns3:MassRate>17200</ns3:MassRate><ns3:InsrRate>49000</ns3:InsrRate><ns3:AirRate>1200</ns3:AirRate><ns3:Rate>0</ns3:Rate><ns3:CustomDuty>0</ns3:CustomDuty></ns3:FinanceParameters><ns3:ItemParameters><ns3:Barcode>RA644000001RU</ns3:Barcode><ns3:ValidRuType>false</ns3:ValidRuType><ns3:ValidEnType>false</ns3:ValidEnType><ns3:ComplexItemName>Отправление EMS с объявл. ценностью и налож. платежом</ns3:ComplexItemName><ns3:MailRank><ns3:Id>0</ns3:Id><ns3:Name>Без разряда</ns3:Name></ns3:MailRank><ns3:PostMark><ns3:Id>0</ns3:Id><ns3:Name>Без отметки</ns3:Name></ns3:PostMark><ns3:MailType><ns3:Id>7</ns3:Id><ns3:Name>Отправление EMS</ns3:Name></ns3:MailType><ns3:MailCtg><ns3:Id>4</ns3:Id><ns3:Name>С объявленной ценностью и наложенным платежом</ns3:Name></ns3:MailCtg><ns3:Mass>101</ns3:Mass></ns3:ItemParameters><ns3:OperationParameters><ns3:OperType><ns3:Id>1</ns3:Id><ns3:Name>Прием</ns3:Name></ns3:OperType><ns3:OperAttr><ns3:Id>1</ns3:Id><ns3:Name>Единичный</ns3:Name></ns3:OperAttr><ns3:OperDate>2015-08-28T03:00:00.000+03:00</ns3:OperDate></ns3:OperationParameters><ns3:UserParameters><ns3:SendCtg><ns3:Id>1</ns3:Id><ns3:Name>Население</ns3:Name></ns3:SendCtg><ns3:Sndr>РТМ0046-Иванов-01</ns3:Sndr><ns3:Rcpn>Петров Пётр Петрович</ns3:Rcpn></ns3:UserParameters></ns3:historyRecord></ns3:OperationHistoryData></ns7:getOperationHistoryResponse></S:Body></S:Envelope>";

            XmlDocument document = new XmlDocument();
            document.LoadXml(response);  //loading soap message as string 
            XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("ns3", "http://russianpost.org/operationhistory/data");
            XmlNodeList xNodelst = document.DocumentElement.SelectNodes("//ns3:historyRecord", manager);
            foreach (XmlNode xn in xNodelst)
            {
                result = xn["ns3:OperationParameters"].FirstChild.LastChild.InnerText + " ("+ xn["ns3:OperationParameters"].LastChild.LastChild.InnerText + ")";
            }
            return result;
        }

        //получение информации об операциях с наложенным платежом, который связан с отправлением 
        public string PostalOrderEventsForMail(string ttn)
        {
            string result = string.Empty;

            string request = string.Format(
                @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" 
                                xmlns:oper=""http://russianpost.org/operationhistory"" 
                                xmlns:data=""http://russianpost.org/operationhistory/data"" 
                                xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                                xmlns:data1=""http://www.russianpost.org/RTM/DataExchangeESPP/Data"">
               <soap:Header/>
               <soap:Body>
                  <oper:PostalOrderEventsForMail>
                     <data:AuthorizationHeader soapenv:mustUnderstand=""1"">
                        <data:login>{1}</data:login>
                        <data:password>{2}</data:password>
                     </data:AuthorizationHeader>
                     <data1:PostalOrderEventsForMailInput Barcode=""{0}"" Language= ""RUS"" />
                  </oper:PostalOrderEventsForMail>
               </soap:Body>
            </soap:Envelope>", ttn, LOGIN, PASSWORD);

            var response = SendRequestToRussianPost(request);

            return result;
        }

        private string SendRequestToRussianPost(string xmlSoap)
        {
            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xmlSoap);

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    return soapResult;
                }
            }
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        private static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://tracking.russianpost.ru/rtm34");
            webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}