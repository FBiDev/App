using System.IO;
using System.Text;

namespace App.Core
{
    public class StringWriterUTF8 : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}