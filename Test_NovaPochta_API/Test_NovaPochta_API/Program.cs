
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
                "Владимир",
                "Нечипоренко",
                "Харьков",
                "628.95",
                "16.01.2016",
                "0999308186",
                "1",
                "WarehouseWarehouse",
                "3.9000000000000004",
                "Cash",
                "Відділення № 109 (до 30 кг) Міні-відділення",
                "false",
                "true",
                "",
                "",
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