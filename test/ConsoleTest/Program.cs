using BigsData.Database;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleTest
{
    class Program
    {
        private const string sourceFilePath = @"../../AnonymousMe.jpeg";
        private const string destinationFilePath = @"../../AnonymousMeSaved.jpeg";
        static void Main(string[] args)
        {
            var db = BigsData.DB.Open("data", trackReferences: true);

            //Task.Run(() => TestStream(db)).Wait();
            //Task.Run(() => TestItems(db)).Wait();
            Task.Run(() => TestDelete(db)).Wait();

            WriteLine("Done!");

            ReadLine();
        }

        private static async Task TestStream(BigsDatabase db)
        {
            WriteLine("Adding stream...");
            var id = Path.GetFileName(sourceFilePath);

            using (var sourceStream = File.OpenRead(sourceFilePath))
                await db.AddStream(id, sourceStream);

            WriteLine("Retrieving stream...");
            using (var targetStream = File.OpenWrite(destinationFilePath))
            using (var savedStream = db.GetSteam(id))
                await savedStream.CopyToAsync(targetStream);

        }

        private static async Task TestItems(BigsDatabase db)
        {
            var item = new Item
            {
                Name = "John Cleese",
                Birthday = new DateTime(1939, 10, 27)
            };

            var result = await db.Add(item);

            WriteLine("Single:");
            var itemRead = await db.Single<Item>(result.ItemId);
            WriteLine($"Item: {itemRead.Name} - {itemRead.Birthday}");

            WriteLine("Query:");
            foreach (var listItem in db.Query<Item>())
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday} - {db.GetId(listItem)}");

            WriteLine("Query with filter: i => i.Name.Contains(\"E\")");
            foreach (var listItem in db.Query<Item>().Where(i => i.Name.Contains("E")))
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday}");
        }

        private static async Task TestDelete(BigsDatabase db)
        {
            var item = new Item
            {
                Name = "Item to be deleted"
            };

            WriteLine("Add item...");
            await db.Add(item);

            WriteLine("Query:");
            foreach (var listItem in db.Query<Item>())
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday} - {db.GetId(listItem)}");

            WriteLine("Delete Item...");
            db.Delete(item);

            WriteLine("Query again:");
            foreach (var listItem in db.Query<Item>())
                WriteLine($"Item: {listItem.Name} - {listItem.Birthday} - {db.GetId(listItem)}");
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}