using System;
using System.Xml.Serialization;

namespace App.Core
{
    [Serializable]
    [XmlRoot(ElementName = "Config")]
    public class ConfigXML
    {
        public ConfigXML()
        {
            Servidor = new Server();
            WallPaper = new WallPaper();
        }

        [XmlElement(ElementName = "Server")]
        public Server Servidor { get; set; }

        [XmlElement(ElementName = "WallPaper")]
        public WallPaper WallPaper { get; set; }
    }
}