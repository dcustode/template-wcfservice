using log4net;
using log4net.Config;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Configuration;

namespace WcfServiceTemplate
{
    public enum ErrorLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public class BaseService
    {
        protected readonly ILog log;

        public BaseService()
        {
            XmlConfigurator.Configure();

            log = LogManager.GetLogger(typeof(BaseService));
        }

        public MyConfig LoadConfig()
        {
            MyConfig config = new MyConfig();
            config.MyConfigEntry = WebConfigurationManager.AppSettings["MyConfigKey"];
            return config;
        }


        public void WriteInLog(string serviceName, ErrorLevel level, string user, string message)
        {
            if (user == string.Empty)
            {
                user = "Unknown";
            }
            var remEndPnt = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string addr = remEndPnt.Address;
            string serviceNameNote = string.IsNullOrEmpty(serviceName) ? string.Empty : string.Format("[{0}]", serviceName);
            string UserAndIpAddr = string.Format("User [{0}], IP-Address [{1}]", user, addr);
            string strFullMsg = string.Format("{0}{1}, {2}", serviceNameNote, message, UserAndIpAddr);

            if (level == ErrorLevel.Debug)
            {
                log.Debug(strFullMsg);
            }
            else if (level == ErrorLevel.Info)
            {
                log.Info(strFullMsg);
            }
            else if (level == ErrorLevel.Error)
            {
                log.Error(strFullMsg);
            }
            else if (level == ErrorLevel.Fatal)
            {
                log.Fatal(strFullMsg);
            }
        }
    }
}