using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WcfServiceTemplate.Models;

namespace WcfServiceTemplate
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract]
    public class MyServiceClass : BaseService
    {
        /// <summary>
        /// <![CDATA[http://localhost/MyService/Info/GetAnswer?question=will it rain tommorow]]
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetAnswer?question={question}", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetAnswer(string question)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebOperationContext ctx = WebOperationContext.Current;
            MemoryStream ms;

            HttpContext context = HttpContext.Current;
            MyModel responseModel;

            try
            {
                if (string.IsNullOrEmpty(question))
                {
                    return ResponseHelper.GetJsonError("Sorry question is empty");
                }

                responseModel = new MyModel();
                responseModel.Question = question;
                responseModel.Answer = DateTime.Now.Second % 2 == 0 ? "yes" : "no";
            }
            catch (Exception ex)
            {
                WriteInLog("InfoService", ErrorLevel.Fatal, "Unknown", string.Format("Unhandled exception while trying to give a answer: [{0}]", ex.Message));
                ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return ResponseHelper.GetJsonError("Fatal error using service methode GetAnswer");
            }

            // return the detail of publication layer info in BMD as json
            string json = new JavaScriptSerializer().Serialize(responseModel);
            ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            WriteInLog("InfoService", ErrorLevel.Debug, "Unknown", string.Format("Gave anwser [{0}] to the question [{1}]", responseModel.Answer,responseModel.Question));

            return ms;
        }
    }
}