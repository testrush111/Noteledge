using Microsoft.AspNet.SignalR;
using Models.MemberModels;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;

namespace prjMSIT127_G2_Noteledge.Controllers.API
{
    [RoutePrefix("api/ChatContent")]
    public class ChatContentController : Controller
    {
        [HttpGet, Route("hello/{text}")]
        public ActionResult hello(string text)
        {
            return new ContentResult()
            {
                ContentEncoding = Encoding.UTF8,
                Content = JsonSerializer.Serialize(new { 
                    text = text
                })
            };
        }

        [HttpGet, Route("Search/{fMemberTo:int}")]
        public ActionResult Search(int fMemberTo)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            var mlschat = CChatFactory.fn聊聊個人查詢(member, fMemberTo).Select(n => n).ToList();
            //查詢指定筆記內容

            return new ContentResult()
            {
                ContentEncoding = Encoding.UTF8,
                Content = JsonSerializer.Serialize(mlschat),
                ContentType = "json"
            };
        }

        [ValidateInput(false)]
        [HttpPost, Route("Insert")]
        public ActionResult Insert()
        {
            int i = Convert.ToInt32(HttpContext.Request.Form["fMemberTo"]);
            string m = HttpContext.Request.Form["fMessage"];
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            CChat c = new CChat();
            c.fSubmitDateTime = DateTime.UtcNow.AddHours(08);
            c.fMessage = m;
            c.fIsRead = false;
            c.fIsRetract = false;
            c.fMemberFrom = member.fMemberId;
            c.fMemberTo = i;
            c.fProductId = 0;
            CChatFactory.fn聊聊新增(c);
            GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.Group(i.ToString()).newMessage(member.fMemberId, m, DateTime.UtcNow.AddHours(08), 0);
            return new ApiResult();
        }

        [HttpGet, Route("Read/{fMemberTo:int}")]
        public ActionResult Read(int fMemberTo)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CChatFactory.fn聊聊已讀更新(fMemberTo, member);
            //查詢指定筆記內容
            return new ApiResult();
        }

        [HttpGet, Route("Retract/{fChatId:int}")]
        public ActionResult Retract(int fChatId)
        {
            //查詢筆記資料夾
            CChatFactory.fn聊聊更新(fChatId);
            //查詢指定筆記內容
            return new ApiResult();
        }
    }
}