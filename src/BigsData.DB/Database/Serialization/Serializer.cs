using fastJSON;

namespace BigsData.Database.Serialization
{
    internal static class Serializer
    {
        public static string Serialize(object item)
        {
            
            return JSON.ToJSON(item);
        }

        public static T Deserialize<T>(string json)
        {
            return JSON.ToObject<T>(json);
        }
    }
}
