using Newtonsoft.Json;

namespace BigsData.Database.Serialization
{
    internal static class Serializer
    {
        public static string Serialize(object item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
