using System.IO;
using System.Text;

namespace WcfServiceTemplate
{
    public class ResponseHelper
    {
        public static Stream GetJsonError(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(string.Format("{{\"Error\":\"{0}\"}}", text)));
        }

        public static Stream GetJsonInfo(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(string.Format("{{\"Info\":\"{0}\"}}", text)));
        }
    }
}