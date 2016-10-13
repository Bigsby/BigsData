using System;
using System.Linq;
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
                Name = "Eric Idle",
                Birthday = new DateTime(1943, 3, 29)
            };

            var result = db.Add(item).Result;

            WriteLine("Single:");
            var itemRead = db.Single<Item>(result.ItemId).Result;
            WriteLine($"Item: {itemRead.Name} - {itemRead.Birthday}");

            WriteLine("Query:");
            foreach (var listItem in db.Query<Item>())
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday} - {db.GetId(listItem)}");

            WriteLine("Query with filter:");
            foreach (var listItem in db.Query<Item>().Where(i => i.Name.Contains("E")))
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday}");

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