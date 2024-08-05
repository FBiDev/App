using System;
using System.Xml.Serialization;

namespace App.Core
{
    [Serializable]
    [XmlRoot(ElementName = "WallPaper")]
    public class WallPaper
    {
        [XmlElement(ElementName = "Directory")]
        public string Directory { get; set; }

        [XmlElement(ElementName = "Splash")]
        public string Splash { get; set; }

        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }

        [XmlElement(ElementName = "Desktop")]
        public string Desktop { get; set; }
    }
}