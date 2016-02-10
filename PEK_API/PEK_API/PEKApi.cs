using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace PEK_API
{
    public class PEKApi
    {
        #region #private fields
        private const string KEY = "D9238AF77DD4D5F652D5EE15C7B3BF4F98ACB196"; // api key - генерится в личном кабинете
        private const string LOGIN = "neos93"; // логин для входа в личный кабинет
        private const string URL = "https://kabinet.pecom.ru/api/v1/";

        #endregion

        #region #private methods

        //send request to api and get json 
        private string SendRequestToPEK(string urlRequest, dynamic jsonData)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create("https://kabinet.pecom.ru/api/v1/" + urlRequest);
            // Кодировка и тип содержимого
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.Method = WebRequestMethods.Http.Post;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            byte[] credentialBuffer = new UTF8Encoding().GetBytes(LOGIN + ":" + KEY);
            httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);

            try
            {
                // Подготовка содержимого запроса
                using (var sw = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(jsonData);
                    sw.Write(json);
                }
                // Выполнение запроса
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //Получение результата запроса
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                return "Error. " + ex.Message;
            }
        }

        //получить дату расчета или оформления в небходимом формате
        private string GetNowDate()
        {
            return DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
        }

        //расчитать максимальные габариты груза
        private double GetMaxDimension(int senderCityId, int receiverCityId, int weight, double volume)
        {
            var jsonData = new
            {
                senderCityId = senderCityId,            // Код города отправителя [Number]
                receiverCityId = receiverCityId,        // Код города получателя [Number]
                weight = weight,                        // Вес, кг [Number]
                volume = volume,                        // Объем груза, м3 [Number]
            };

            var responce = this.SendRequestToPEK("calculator/maxdimension/", jsonData);
            //Deserialize json Object
            var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
            //Get Max Dimension 
            return deserializedProduct.maxdimension.Value;
        }

        private int FindCityByTitle(string city)
        {
            var jsonData = new
            {
                title = city
            };
            //get basic info about cargo
            var responce = this.SendRequestToPEK("branches/findbytitle/", jsonData);
            //Deserialize json Object
            var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
            var success = deserializedProduct.success.Value;
            if (!success) return 0;
            //parse and get status string
            var cityId = deserializedProduct.items[0].cityId.Value;
            if (cityId != null) return int.Parse(cityId);
            var branchId = deserializedProduct.items[0].branchId.Value;
            if (branchId != null) return int.Parse(branchId);
            return 0;
        }

        #endregion

        #region public methods

        //оформление заявки
        public string CreateCargoPickupNetwork(string cityR, string nameR, string phoneR, int weight, double volume, int positionsCount)
        {
            var sId = this.FindCityByTitle("Нефтекамск");
            if (sId == 0) return "Error. Sender city not fount!";
            var rId = this.FindCityByTitle(cityR);
            if (rId == 0) return "Error. Recipent city not fount!";

            var jsonData = new
            {
                common = new
                {
                    applicationDate = this.GetNowDate(),         // Дата выполнение заявки [Date]
                    responsiblePerson = "Роззувайло В.Ю.",//TODO ...      // ФИО ответственного за оформление заявки [String]
                    description = "Парниковое оборудование"// Описание груза [String]
                                                           // поле обязательно для заполнения, если не во всех грузах заявки
                                                           // проставлено описание груза (cargos.items[].cargo.description)
                },
                //sender is const
                sender = new
                {
                    city = "Нефтекамск",
                    title = "Аббасов Д.В.",
                    person = "Аббасов Д.В.",
                    phone = "30-95-89",
                    addressOffice = "ул. Антонова, д. 2",
                    addressStock = "ул. Боровиковского, 17, строение 5"
                },
                cargos = new
                {
                    common = new
                    {
                        cargoTotals = new
                        {
                            volume = volume,            // Общий объём, м3 [Number]
                            weight = weight,           // Общий вес, кг [Number]
                            maxDimension = this.GetMaxDimension(sId, rId, weight, volume),// Максимальный габарит, м [Number]
                            positionsCount = positionsCount      // Общее количество мест, шт [Number]
                        },
                        services = new
                        {
                            pickUp = new
                            {
                                payer = new
                                {
                                    type = 2        //получатель платит
                                }
                            },
                            transporting = new
                            {   //Плательщик за услугу перевозки [Object], поле необязательно
                                payer = new
                                {   //Плательщик [Object] 
                                    type = 2
                                }
                            },
                            delivery = new
                            { // Плательщик за услугу доставки [Object], поле необязательно
                                payer = new
                                { // Плательщик [Object] (детальное описание см. выше)
                                    type = 2
                                }
                            }
                        },
                    },
                    items = new[]
                {
                        new {
                             //info about reciver
                            receiver = new
                            {
                                city = cityR,// Город получателя из списка [City]
                                title = nameR,    // Наименование получателя (организация, физ. лицо)[String]
                                person = nameR,
                                phone = phoneR               // Телефон [String]
                            },
                            cargo = new
                            {
                                transporting = 1,               // Тип перевозки (1 - авто, 2 - авиа) [Number]
                                description = "Парниковое оборудование",
                            },
                            conditions = new
                            {
                                isOpenCar = false,            // Необходима открытая машина [Boolean] 
                                isSideLoad = false,           // Необходима боковая погрузка [Boolean]
                                isDayByDay = false,           // Необходим забор день в день [Boolean]
                                isFast = true,                // Необходима скоростная перевозка [Boolean]
                                isLoading = false             // Необходима погрузка силами «ПЭК» [Boolean]
                            },
                            services = new
                            {
                                delivery = new {
                                    enabled=true,
                                    address = "ул. Садовая 2, кв. 5"
                                },
                                insurance = new
                                {
                                    enabled = true,
                                    cost = 20000,
                                    payer = new
                                    { // Плательщик [Object] (детальное описание см. выше)
                                        type = 2
                                    }
                                },
                                //TODO
                                documentsReturning = new { enabled = false },
                                strapping = new { enabled = false },
                                sealing = new { enabled = false },
                                hardPacking = new { enabled = false },
                                cashOnDelivery  = new { enabled = false }
                            }
                        }
                    }
                },
            };
            //send to API 
            var responce = this.SendRequestToPEK("cargopickupnetwork/submit/", jsonData);

            //Deserialize json Object
            /* var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
             var cargoCode = deserializedProduct.cargos[0].cargoCode.Value;
             var orderNumber = deserializedProduct.cargos[0].orderNumber.Value;
             var barcode = deserializedProduct.cargos[0].barcode.Value;
             var documentId = deserializedProduct.documentId.Value;
             */
            return responce;
        }

        //TODO
        public string CreatePreregistration()
        {
            var jsonData = new
            {
                //sender is const
                sender = new
                {
                    city = "Нефтекамск",
                    title = "Роззувайло Виталий",
                    phone = "30-95-89"
                },
                //TODO - set data from BPM
                cargos = new[] {
                    new {
                        //info about cargo
                        common = new
                        {
                            cashOnDelivery = true,          // Заказана услуга наложенного платежа
                            cashOnDeliverySum = 456.26,     // Общая стоимость заказа
                            actualCost = 789.36,            // Фактическая стоимость товара
                            includeTES = false,             // Включить ТЭУ в сумму НП [Boolean]
                            accompanyingDocuments = false,  // Есть комплект сопроводительных документов [Boolean]
                            positionsCount = 1,             // Количество мест [Number]
                            decription = "Парики",          // Описание груза [String]
                            paymentForm = 2                 // Форма оплаты (1 - Банк, 2 - Касса) [Number], поле необязательно,
                                                            // если значение не указано, равно "Банк" по умолчанию
                        },
                        //info about reciver
                        receiver = new
                        {
                            city = "Санкт-Петербург Обухово",// Город получателя из списка [City]
                            title = "Роззувайло Виталий",    // Наименование получателя (организация, физ. лицо)[String]
                            phone = "88-99-00"               // Телефон [String]
                        },
                        //sender is const
                        services = new
                        {
                            transporting = new                  // Перевозка [Object]
                            {
                                payer = new                     // Плательщик (1 - отправитель, 2 - получатель, 3 - третье лицо) [Number]
                                {
                                    type = 2
                                }
                            },
                            hardPacking = new{ enabled = false },
                            insurance = new { enabled = false },
                            sealing = new { enabled = false },
                            strapping = new { enabled = false },
                            documentsReturning = new { enabled = false },
                            delivery = new { enabled = false }
                        }
                    }
                }
            };
            //send to API 
            var responce = this.SendRequestToPEK("preregistration/submit/", jsonData).Trim();
            //Deserialize json Object
            if (responce.StartsWith("Error"))
            {
                return responce;
            }
            var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
            var cargoCode = deserializedProduct.cargos[0].cargoCode.Value;
            var orderNumber = deserializedProduct.cargos[0].orderNumber.Value;
            var stockTitle = deserializedProduct.cargos[0].stockTitle.Value;

            //TODO
            return "что то!";
        }

        //расчет цены
        public string GetCalculatePrice(string senderCity, string receiverCity, int weight, double volume)
        {
            var senderCityId = this.FindCityByTitle(senderCity);
            if (senderCityId == 0) return "Error. Sender city not fount!";
            var receiverCityId = this.FindCityByTitle(receiverCity);
            if (receiverCityId == 0) return "Error. Recipent city not fount!";
            var maxSize = this.GetMaxDimension(senderCityId, receiverCityId, weight, volume);

            var jsonData = new
            {
                senderCityId = senderCityId,                                // Код города отправителя [Number]
                receiverCityId = receiverCityId,                            // Код города получателя [Number]
                isOpenCarSender = false,                                    // Растентовка отправителя [Boolean]
                senderDistanceType = 0,                                     // Тип доп. услуг отправителя [Number]
                isDayByDay = false,                                         // Необходим забор день в день [Boolean]
                isOpenCarReceiver = false,                                  // Растентовка получателя [Boolean]      
                receiverDistanceType = 0,                                   // Тип доп. услуг отправителя [Number]
                isHyperMarket = false,                                      // признак гипермаркета [Boolean]
                calcDate = this.GetNowDate(),                               // расчетная дата [Date]
                isInsurance = false,                                         // Страхование [Boolean]
                //isInsurancePrice = 234.15,                                  // Оценочная стоимость, руб [Number]
                isPickUp = false,                                           // Нужен забор [Boolean]
                isDelivery = false,                                         // Нужна доставка [Boolean]

                Cargos = new[]                                              // Данные о грузах [Array]
                {
                    new {
                        volume = volume,                                    // Объем груза, м3 [Number]
                        maxSize = maxSize,                                  // Максимальный габарит, м [Number]
                        isHP = false,                                       // Жесткая упаковка [Boolean]
                        sealingPositionsCount = 0,                          // Количество мест для пломбировки [Number]
                        weight = weight,                                    // Вес, кг [Number]
                        overSize = true                                     // Негабаритный груз [Boolean]
                    }
                }
            };

            var responce = this.SendRequestToPEK("calculator/calculateprice/", jsonData);
            //Deserialize json Object
            var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
            //error?
            var hasError = deserializedProduct.hasError.Value;
            if (hasError)
                return "Error. " + deserializedProduct.errorMessage.Value;
            //get price
            var price = deserializedProduct.transfers[0].services[0].cost.Value;
            return price.ToString();
        }

        //get status cargo
        public string GetCargoStatus(string codeCargo)
        {
            var jsonData = new
            {
                cargoCodes = new string[] { codeCargo }
            };
            //get basic info about cargo
            var responce = this.SendRequestToPEK("cargos/basicstatus/", jsonData);
            //Deserialize json Object
            var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
            //parse and get status string
            try
            {
                return deserializedProduct.cargos[0].info.cargoStatus.Value;
            }
            catch// отлов ошибок любых... (((
            {
                return deserializedProduct.error.message.Value;
            }
        }

        #endregion

    }
}