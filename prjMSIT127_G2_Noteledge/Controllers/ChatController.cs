using Microsoft.AspNet.SignalR;
using Models.MemberModels;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.Hubs;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class ChatController : Controller
    {
        [HttpPost]
        public ActionResult Productblock(int ProductId)
        {
            if (Session[CMemberSession.Session_Login_User] == null)
                return RedirectToAction("../Member/Login");
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            CProductPicture myProductPicture = CProductPictureFactory.fn商品圖片查詢().FirstOrDefault(n => n.fProductId == ProductId);
            //CProduct myProduct = CProductFactory.fn商品查詢().FirstOrDefault(n => n.fProductId == ProductId);
            return PartialView("_Productblock", myProductPicture);
        }

        public ActionResult ProductIndex(int ProductId)
        {
            if (Session[CMemberSession.Session_Login_User] == null)
                return RedirectToAction("../Member/Login");
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            var lsChatBeau = CChatFactory.fn聊聊查詢(member).Select(n => new { from = n.fMemberFrom, to = n.fMemberTo, time = n.fSubmitDateTime }).OrderByDescending(n => n.time).Distinct().ToList();
            List<CChatBeauViewModel> lsChatBeauVM = new List<CChatBeauViewModel>();
            ViewBag.ProductId = ProductId;
            foreach (var q in lsChatBeau)
            {
                if (q.from == member.fMemberId)
                {
                    var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.to).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                    if (lsChatBeauVM.Any(n => n.BeauId == q.to) == false)
                    {
                        lsChatBeauVM.Add(new CChatBeauViewModel()
                        {
                            Beau = w.Single().surname + w.Single().name,
                            BeauId = q.to,
                            MyID = q.from
                        });
                    }
                }
                else
                {
                    var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.from).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                    if (lsChatBeauVM.Any(n => n.BeauId == q.from) == false)
                    {
                        lsChatBeauVM.Add(new CChatBeauViewModel()
                        {
                            Beau = w.Single().surname + w.Single().name,
                            BeauId = q.from,
                            MyID = q.to
                        });
                    }
                }
            }
            return View(lsChatBeauVM);
        }


        // GET: Chat
        public ActionResult Index(int? productid,int? sellerid,string message)
        {
            if (Session[CMemberSession.Session_Login_User] == null)
                return RedirectToAction("../Member/Login");
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            if(productid == null || productid ==0)
            {
                var lsChatBeau = CChatFactory.fn聊聊查詢(member).Select(n => new { from = n.fMemberFrom, to = n.fMemberTo, time = n.fSubmitDateTime }).OrderByDescending(n => n.time).Distinct().ToList();
                var lsChat = CChatFactory.fn聊聊查詢(member).Select(n => n).ToList();
                List<CChatViewModel> lsChatVM = new List<CChatViewModel>();
                List<CChatBeauViewModel> lsChatBeauVM = new List<CChatBeauViewModel>();

                foreach (var q in lsChatBeau)
                {
                    if (q.from == member.fMemberId)
                    {
                        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.to).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                        var e = CChatFactory.fn聊聊未讀查詢(q.from, q.to).ToList();
                        if (lsChatBeauVM.Any(n => n.BeauId == q.to) == false)
                        {
                            lsChatBeauVM.Add(new CChatBeauViewModel()
                            {
                                Beau = w.Single().surname + w.Single().name,
                                BeauId = q.to,
                                MyID = q.from,
                                nRead = e.Single().fProductId
                            });
                        }
                    }
                    else
                    {
                        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.from).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                        var e = CChatFactory.fn聊聊未讀查詢(q.from, q.to).ToList();
                        if (lsChatBeauVM.Any(n => n.BeauId == q.from) == false)
                        {
                            lsChatBeauVM.Add(new CChatBeauViewModel()
                            {
                                Beau = w.Single().surname + w.Single().name,
                                BeauId = q.from,
                                MyID = q.to,
                                nRead = e.Single().fProductId
                            });
                        }
                    }
                }
                return View(lsChatBeauVM);
            }
            else
            {
                CChat c = new CChat();
                c.fSubmitDateTime = DateTime.UtcNow.AddHours(08);
                c.fMessage = message;
                c.fIsRead = false;
                c.fIsRetract = false;
                c.fMemberFrom = member.fMemberId;
                c.fMemberTo = Convert.ToInt32(sellerid);
                c.fProductId = Convert.ToInt32(productid);
                CChatFactory.fn聊聊新增(c);
                GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.Group(sellerid.ToString()).newMessage(member.fMemberId, message, DateTime.UtcNow.AddHours(08), Convert.ToInt32(productid));
                var lsChatBeau = CChatFactory.fn聊聊查詢(member).Select(n => new { from = n.fMemberFrom, to = n.fMemberTo, time = n.fSubmitDateTime }).OrderByDescending(n => n.time).Distinct().ToList();
                var lsChat = CChatFactory.fn聊聊查詢(member).Select(n => n).ToList();
                List<CChatViewModel> lsChatVM = new List<CChatViewModel>();
                List<CChatBeauViewModel> lsChatBeauVM = new List<CChatBeauViewModel>();

                foreach (var q in lsChatBeau)
                {
                    if (q.from == member.fMemberId)
                    {
                        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.to).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                        if (lsChatBeauVM.Any(n => n.BeauId == q.to) == false)
                        {
                            lsChatBeauVM.Add(new CChatBeauViewModel()
                            {
                                Beau = w.Single().surname + w.Single().name,
                                BeauId = q.to,
                                MyID = q.from
                            });
                        }
                    }
                    else
                    {
                        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.from).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
                        if (lsChatBeauVM.Any(n => n.BeauId == q.from) == false)
                        {
                            lsChatBeauVM.Add(new CChatBeauViewModel()
                            {
                                Beau = w.Single().surname + w.Single().name,
                                BeauId = q.from,
                                MyID = q.to
                            });
                        }
                    }
                }
                return View(lsChatBeauVM);
            }
            

            //foreach (var q in lsChat)
            //{
            //    var p = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.fMemberFrom ).Select(n => new { surname = n.fFirstName,name = n.fLastName}).ToList();
            //    var o = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.fMemberTo).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
            //    if(q.fMemberFrom == member.fMemberId)
            //    {
            //        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.fMemberTo).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
            //        lsChatVM.Add(new CChatViewModel()
            //        {
            //            beau = w.Single().surname+w.Single().name,
            //            beauId = q.fMemberTo,
            //            fromchatname = p.Single().surname + p.Single().name,
            //            tochatname = o.Single().surname + o.Single().name,
            //            fChatId = q.fChatId,
            //            fSubmitDateTime = q.fSubmitDateTime,
            //            fMessage = q.fMessage,
            //            fIsRead = q.fIsRead,
            //            fIsRetract = q.fIsRetract,
            //            fMemberFrom = q.fMemberFrom,
            //            fMemberTo = q.fMemberTo,
            //            fProductId = q.fProductId
            //        });
            //    }
            //    else
            //    {
            //        var w = CMemberFactory.fn會員查詢().Where(t => t.fMemberId == q.fMemberFrom).Select(n => new { surname = n.fFirstName, name = n.fLastName }).ToList();
            //        lsChatVM.Add(new CChatViewModel()
            //        {
            //            beau = w.Single().surname + w.Single().name,
            //            beauId = q.fMemberFrom,
            //            fromchatname = p.Single().surname + p.Single().name,
            //            tochatname = o.Single().surname + o.Single().name,
            //            fChatId = q.fChatId,
            //            fSubmitDateTime = q.fSubmitDateTime,
            //            fMessage = q.fMessage,
            //            fIsRead = q.fIsRead,
            //            fIsRetract = q.fIsRetract,
            //            fMemberFrom = q.fMemberFrom,
            //            fMemberTo = q.fMemberTo,
            //            fProductId = q.fProductId
            //        });
            //    }

            //}
        }

        //public class Student
        //{
        //    public int fChatId { get; set; }
        //    public DateTime fSubmitDateTime { get; set; }
        //    public string fMessage { get; set; }
        //    public bool fIsRead { get; set; }
        //    public bool fIsRetract { get; set; }
        //    public int fMemberFrom { get; set; }
        //    public int fMemberTo { get; set; }
        //    public int fProductId { get; set; }
        //    public Student(string _id, string _name, int _score)
        //    {
        //        fChatId = _id;
        //        fSubmitDateTime = _name;
        //        fMessage = _score;
        //    }
        //    public override string ToString()
        //    {
        //        return $"學號:{id}, 姓名:{name}, 分數:{score}.";
        //    }
        //}

        
    }  
}
