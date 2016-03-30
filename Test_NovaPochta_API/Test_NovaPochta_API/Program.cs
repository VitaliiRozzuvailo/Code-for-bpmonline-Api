
using Terrasoft.Configuration.SxNovaPochtaApi;

namespace Test_NovaPochta_API
{
    class Program
    {
        static void Main(string[] args)
        {
            //new SxNovaPochtaApi().GetPriceDeliveryOrderInNP("1440", "3", "1", "WarehouseWarehouse", "Николаев");
            //var res = new SxNovaPochtaApi().GetDepartments("Киев", "в");
            /*string fName, string lName, string city, string price, string date,
            string phoneR, string seatsAmount, string serviceType, string weight, string paymentMethod, string addressRecipient*/
            var s = new SxNovaPochtaApi().CreateOrderInNP(
                "Щин",
                "Кано",
                "Киев",
                "1440",
                "25.03.2016",
                "380953664653",
                "1",
                "WarehouseWarehouse",
                "2",
                "Cash",
                "Отделение №48 (до 30 кг): вул. Киквидзе, 7/11 (м. Дружбы Народов)",
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