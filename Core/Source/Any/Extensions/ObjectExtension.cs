using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace App.Core
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object T)
        {
            return T == null;
        }

        public static bool NotNull(this object T)
        {
            return T != null;
        }

        public static TypeCode TypeCode(this Type type)
        {
            var code = Type.GetTypeCode(type);
            return code;
        }

        public static Type GetNullableType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type.IsValueType)
                return typeof(Nullable<>).MakeGenericType(type);
            return type;
        }

        public static Type GetNotNullableType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type.IsValueType)
                return type;
            return typeof(Nullable<>).MakeGenericType(type);
        }

        public static bool IsEqual<T>(this T objA, T objB)
        {
            foreach (var item in objA.GetType().GetProperties())
            {
                var valA = item.GetValue(objA);
                var valB = item.GetValue(objB);

                if (Equals(valA, valB) == false)
                    return false;
            }

            return true;
        }

        public static void Clone(this object origin, object from)
        {
            if (from.IsNull()) return;

            foreach (PropertyInfo property in from.GetType().GetProperties())
            {
                if (property.CanWrite && !property.Name.Equals("Cloned"))
                {
                    var value = property.GetValue(from, null);

                    property.SetValue(origin, value, null);
                }
            }
        }

        public static object Clone(this object source)
        {
            var clone = FormatterServices.GetUninitializedObject(source.GetType());
            for (var type = source.GetType(); type != null; type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var field in fields)
                    field.SetValue(clone, field.GetValue(source));
            }
            return clone;
        }
    }
}