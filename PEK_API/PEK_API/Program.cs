using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace PEK_API
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            /*
            var responce = new PEKApi().CreateCargoPickupNetwork("Санкт-Петербург", "Иванов Иван", "88-60-35", 8, 0.2, 1);

            //new PEKApi().CreatePreregistration();

            if (responce.StartsWith("Error"))
            {
                Console.WriteLine("Error." + responce);
            }
            else
            {
                var deserializedProduct = JsonConvert.DeserializeObject<dynamic>(responce);
                var cargoCode = deserializedProduct.cargos[0].cargoCode.Value;
                var orderNumber = deserializedProduct.cargos[0].orderNumber.Value;
                var barcode = deserializedProduct.cargos[0].barcode.Value;
                var documentId = deserializedProduct.documentId.Value;

                Console.WriteLine(new PEKApi().GetCargoStatus(cargoCode));
            }
            */
            Console.WriteLine("Цена перевозки: " + new PEKApi().GetCalculatePrice("Нефтекамск", "Сочи", 80, 2));

            stopWatch.Stop();

            Console.WriteLine("Time execute: " + stopWatch.ElapsedMilliseconds / 1000.0 + " sec");
        }
    }
}
