
using Terrasoft.Configuration.SxNovaPochtaApi;

namespace Test_NovaPochta_API
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string fName, string lName, string city, string price, string date,
            string phoneR, string seatsAmount, string serviceType, string weight, string paymentMethod, string addressRecipient*/
            var s = new SxNovaPochtaApi().CreateOrderInNP(
                "Дмитрий",
                "Счастливый",
                "Киев",
                "2571.42",
                "23.12.2015",
                "0356294616",
                "1",
                "WarehouseWarehouse",
                "24",
                "Cash",
                "Відділення № 134(до 30 кг), Міні - відділення: вул.Новопирогівська, 31(маг.\"Фора\")",
                "true",
                "true",
                "героев днепра",
                "50",
            "");

            //var st = new SxNovaPochtaApi().GetStatusOrderInNP();
            /* XPathNavigator nav = xdoc.CreateNavigator();

             string c = null;

             foreach (XPathNavigator n in nav.Select("/root/data/item"))
             {
                 var ct = n.SelectSingleNode("DescriptionRu").Value;

                 Console.WriteLine("City: {0}", ct);
                 if (ct == "Харьков")
                     c = n.SelectSingleNode("Ref").Value;
             }

             var sd = new ClassApi().GetWarehousesByCity(c);
             XDocument xdoc1 = XDocument.Parse(sd);
             XPathNavigator nav1 = xdoc1.CreateNavigator();


             foreach (XPathNavigator n in nav1.Select("/root/data/item"))
             {
                 var ct = n.SelectSingleNode("DescriptionRu").Value;
                 Console.WriteLine("Warehouse: {0}", ct);
             }*/
        }
    }
}