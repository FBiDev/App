﻿using Newtonsoft.Json;

namespace App.Json
{
    public static class JsonSettings
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