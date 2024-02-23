using System;
using System.Xml.Serialization;

namespace App.Core
{
    [Serializable]
    [XmlRoot(ElementName = "Server")]
    public class Server
    {
        [XmlElement(ElementName = "Producao")]
        public string Producao { get; set; }

        [XmlElement(ElementName = "Desenvolvimento")]
        public string Desenvolvimento { get; set; }

        [XmlElement(ElementName = "EhDesenvolvimento")]
        public bool EhDesenvolvimento { get; set; }

        [XmlElement(ElementName = "BaseSistema")]
        public string BaseSistema { get; set; }
    }
}