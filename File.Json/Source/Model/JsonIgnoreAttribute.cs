using System;

namespace App.File.Json
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonIgnoreAttribute : Attribute
    {
    }
}