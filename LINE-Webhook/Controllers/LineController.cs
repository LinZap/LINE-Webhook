using LINE_Webhook.API.LINE;
using LINE_Webhook.Models;
using RUSE.API.LINE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LINE_Webhook.Controllers
{
    [RoutePrefix("line")]
    public class LINEController : ApiController
    {

        [HttpPost]
        [Route]
        [Signature]
        public IHttpActionResult webhook([FromBody] LineWebhookModels data)
        {
            if (data == null) return BadRequest();
            if (data.events == null) return BadRequest();

            return Ok(data);
        }

        private List<SendMessage> procMessage(ReceiveMessage m)
        {
            List<SendMessage> msgs = new List<SendMessage>();
            SendMessage sm = new SendMessage()
            {
                type = Enum.GetName(typeof(MessageType), m.type)
            };
            switch (m.type)
            {
                case MessageType.sticker:
                    sm.packageId = m.packageId;
                    sm.stickerId = m.stickerId;
                    break;
                case MessageType.text:
                    sm.text = m.text;
                    break;
                default:
                    sm.type = Enum.GetName(typeof(MessageType), MessageType.text);
                    sm.text = "很抱歉，我只是一隻鸚鵡機器人，目前只能回覆基本貼圖與文字訊息喔！";
                    break;
            }
            msgs.Add(sm);
            return msgs;
        }
    }
}
