using Newtonsoft.Json;

namespace SharedServices.Files.V1
{
    public static class JsonUtil
    {
        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            Formatting = Formatting.Indented
        }; 
        
        public static string ToJson<T>(T obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? DefaultSettings);
        }
        
        public static T FromJson<T>(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
        }
    }
}