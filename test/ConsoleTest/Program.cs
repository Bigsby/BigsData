using System;
using static System.Console;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = BigsData.DB.Open("data");
            var item = new Item
            {
                Name = "John Cleese",
                Birthday = new DateTime(1939, 10, 27)
            };

            var result = db.Add(item).Result;

            var itemRead = db.Single<Item>(result.ItemId).Result;
            WriteLine($"Item: {itemRead.Name} - {item.Birthday}");

            WriteLine("Done!");
            ReadLine();
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}
