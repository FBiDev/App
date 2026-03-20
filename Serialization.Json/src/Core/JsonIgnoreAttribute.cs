using System;

namespace App.Serialization
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class JsonIgnoreAttribute : Attribute
    {
    }
}