using Newtonsoft.Json;

namespace App.File
{
    internal static class JsonSettings
    {
        public static JsonSerializerSettings Get()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new JsonContractResolver()
            };
            return settings;
        }
    }
}