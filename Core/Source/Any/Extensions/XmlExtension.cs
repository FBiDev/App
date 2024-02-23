using System;
using System.Xml;

namespace App.Core
{
    public static class XmlExtension
    {
        public static XmlNode Child(this XmlNode father, string childName)
        {
            XmlNode child;

            try
            {
                child = father.SelectSingleNode(childName);
            }
            catch (Exception)
            {
                throw;
            }

            return child;
        }

        public static string ChildValue(this XmlNode father, string childName)
        {
            try
            {
                return father.SelectSingleNode(childName).InnerText;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
