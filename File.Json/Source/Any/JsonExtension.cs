namespace App.File
{
    public enum Required
    {
        Default = 0,
        AllowNull = 1,
        Always = 2,
        DisallowNull = 3,
    }

    public static class JsonExtension
    {
        public static JProperty ToWrapper(this Newtonsoft.Json.Linq.JProperty jProperty)
        {
            return new JProperty(jProperty.Name, jProperty.Value);
        }

        public static JToken ToWrapper(this Newtonsoft.Json.Linq.JToken jToken)
        {
            var wrapper = new JToken();

            foreach (Newtonsoft.Json.Linq.JProperty item in jToken)
            {
                if (item is Newtonsoft.Json.Linq.JProperty)
                {
                    var jProperty = item;

                    wrapper.Add(jProperty.ToWrapper());
                }
            }

            return wrapper;
        }
    }
}