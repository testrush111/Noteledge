using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Models.MemberModels;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.ViewModel;

namespace Note_edgeLogin.Controllers
{
    public class HomeController : Controller
    {
        //登入前主畫面-------------------------------------------------------------------------------\\
        public ActionResult Index()
        {
            Session[CMemberSession.Session_Edit_Password] = null;
            Session[CMemberSession.Session_Change_Password] = null;
            Session[CMemberSession.Session_Login_User] = null;
            Session[CMemberSession.Session_Message_Count] = null;
            Session[CMemberSession.Session_sale_Count] = null;
            return View();
        }
        
        [ActionName("MyHome")]
        //登入後主畫面-------------------------------------------------------------------------------\\
        public ActionResult MyHome(string name,string id, CNotice c) 
        {
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;

            //創造一組亂數字串不重複的訂單編號
            var str = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefhijklmnorstuvwxz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }

            //MerchantID(不可變動)，MerchantTradeNo(亂數訂單編號)，MerchantTradeDate(抓取當前日期時間)
            int MerchantID = 2000132;
            var MerchantTradeNo = builder;
            string MerchantTradeDate = DateTime.UtcNow.AddHours(08).ToString("yyyy/MM/dd hh:mm:ss");
            /*tring ReturnUrl = "https://localhost:44300/Home/MyHome?id=" + MerchantTradeNo;*/
            string ReturnUrl = "https://noteledge.azurewebsites.net/Home/MyHome?id=" + MerchantTradeNo;
            string ProductName = "Notedge尊爵鑽石豪華VIP頂級會員";
            int Amount = 99;
            //把需要的資料作串接
            string Url = "HashKey=5294y06JbISpM5x9&ChoosePayment=ALL&ChooseSubPayment=&ClientBackURL=" + ReturnUrl + "&EncryptType=1&ItemName="
                + ProductName
                + "&MerchantID="
                + MerchantID
                + "&MerchantTradeDate="
                + MerchantTradeDate
                + "&MerchantTradeNo="
                + MerchantTradeNo
                + "&PaymentType=aio&ReturnURL=" + ReturnUrl + "&StoreID=&TotalAmount=" + Amount + "&TradeDesc=建立全金流測試訂單&HashIV=v77hoKGq4kWxNNIS";
            //串接好的資料轉成Encoded
            var Encoded = System.Web.HttpUtility.UrlEncode(Url);
            //Encoded 轉成 小寫 encoded
            var encoded = Encoded.ToLower();
            //呼叫sha256_hash(encoded)轉換成SHA256 在轉換大寫
            string SHA256 = sha256_hash(encoded).ToUpper();
            //把資料傳到前端
            ViewBag.MerchantID = MerchantID;
            ViewBag.MerchantTradeNo = MerchantTradeNo;
            ViewBag.MerchantTradeDate = MerchantTradeDate;
            ViewBag.SHA256 = SHA256;
            ViewBag.Url = ReturnUrl;
            ViewBag.ProductName = ProductName;
            ViewBag.Amount = Amount;


            if (id != null) 
            {
                List<CMember> SelecteMember = CMemberFactory.fn會員查詢();
                CMember cMember = SelecteMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);
                cMember.fIsVIP = true;
                cMember.fMoneyPoint =cMember.fMoneyPoint + 500;
                CMemberFactory.fn會員更新(cMember);
                //成為VIP新增通知----------------------------------------------------------------------\\
                c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
                c.fNoticeContent = cMember.fFirstName + cMember.fLastName + "您已成為Notedge尊爵鑽石豪華VIP頂級會員";
                c.fCategoryType = "系統";
                c.fLink = "連結";
                c.fMemberId = member.fMemberId;
                CNoticeFactory.fn通知訊息新增(member, c);
                Session[CMemberSession.Session_Login_User] = cMember;

                if (Session[CMemberSession.Session_Login_User] == null && name == null)
                {
                    Session[CMemberSession.Session_Login_User] = null;
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ViewBag.name = name;
                    return View("../Home/MyHome", "_Layout");
                }
            }
            if (Session[CMemberSession.Session_Login_User] == null && name == null)
            {
                Session[CMemberSession.Session_Login_User] = null;
                return RedirectToAction("../Home/Index");
            }
            else
            {
                if (name == null)
                {
                    var SelectNoticess = CNoticeFactory.fn通知訊息查詢(member).Where(n => n.fCategoryType == "銷售" || n.fCategoryType == "評價留言").OrderByDescending(n => n.fNoticeDatetime);
                    var SelectChats = CChatFactory.fn聊聊查詢(member).Where(n => n.fMemberTo == member.fMemberId).OrderByDescending(n => n.fSubmitDateTime);
                    int MCounts = SelectChats.Count();
                    int SCounts = SelectNoticess.Count();
                    CMemberMessage memberMessage = new CMemberMessage();

                    if (Session[CMemberSession.Session_Message_Count] == null)
                    {
                        memberMessage.MessageBell = MCounts;
                        Session[CMemberSession.Session_Message_Count] = memberMessage;
                    }
                    if (Session[CMemberSession.Session_sale_Count] == null)
                    {
                        memberMessage.SaleBell = SCounts;
                        Session[CMemberSession.Session_sale_Count] = memberMessage;
                    }
                    return View("../Home/MyHome", "_Layout");
                }
                else 
                {
                    ViewBag.name = name;
                    return View("../Home/MyHome", "_Layout");
                }
            }
        }

        //String字串轉成SHA256-------------------------------------------------------------------------\\
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

    }
}