using LINE_Webhook.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Configuration;

namespace RUSE.API.LINE
{
    /*
        --- LINE Reply Message --- 
    */
    public class Reply
    {
        public const string API_URL = "https://api.line.me/v2/bot/message/reply";
        private WebRequest req;

        /*
            --- Construct create webRequest and set configures ---
        */
        public Reply(ReplyBody body)
        {
            /*
                --- set header and body required infos ---
            */
            req = WebRequest.Create(API_URL);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers["Authorization"] = "Bearer " + WebConfigurationManager.AppSettings["AccessToken"];

            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                string data = JsonConvert.SerializeObject(body);
                streamWriter.Write(data);
                streamWriter.Flush();
            }
        }
        /*
            --- send a message request to LINE ---
            return response data
        */
        public string send()
        {
            string result = null;
            try
            {
                WebResponse response = req.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            return result;
        }
    }
}