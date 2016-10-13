using System;

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

            var id = db.Add(item);

            db.
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}
