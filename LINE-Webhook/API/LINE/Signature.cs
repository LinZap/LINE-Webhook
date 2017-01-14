using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LINE_Webhook.API.LINE
{
    public class Signature : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage Request = actionContext.Request;

            IEnumerable<string> headerValues;
            if (Request.Headers.TryGetValues("X-Line-Signature", out headerValues))
            {
                string lineSignature = headerValues.FirstOrDefault(),
                       reqBody = Request.Content.ReadAsStringAsync().Result;

                byte[] screct = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["ChannelSecret"]),
                       body = Encoding.UTF8.GetBytes(reqBody),
                       hash = new HMACSHA256(screct).ComputeHash(body);       
                string mySignature = Convert.ToBase64String(hash);
                if (mySignature == lineSignature) return;
            }
            HttpResponseMessage Response = Request.CreateResponse(HttpStatusCode.ExpectationFailed);
            Response.StatusCode = HttpStatusCode.InternalServerError;
            actionContext.Response = Response;
        }
    }
}