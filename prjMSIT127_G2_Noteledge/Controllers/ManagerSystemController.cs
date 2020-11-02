using Models.ManagementModels;
using Models.MemberModels;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.Models.ManagementModels;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using ViewModel.CShoppingHomeVM;
using ViewModel.CCartVM;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class ManagerSystemController : Controller
    {

        // GET: ManagerSystem
        public ActionResult HomePage(CMember c)
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
            if (admin == null)
            {
                return RedirectToAction("../Member/AdminLogin");
            }
            //資安 未登入時會跳回登入頁面
            int membercount = CMemberFactory.fn會員查詢().Count();//統計會員數量
            int memberbrowse = CMemberBrowseFactory.fn會員瀏覽紀錄查詢().Count();//統計總瀏覽量
            int totalprice = COrderFactory.fn訂單總金額();//統計金額交易量
            int totalorder = COrderFactory.fn訂單商品累積量();//總訂單數量           
            ViewBag.totalprice = totalprice.ToString("c0");
            ViewBag.browse = memberbrowse.ToString();
            ViewBag.membercount = membercount.ToString();
            ViewBag.totalorder = totalorder;
            
            List<CProduct> lsproduct = CProductFactory.fn商品查詢().ToList();//撈出產品列表
            List<CProductAndProductCompareViewModel> lsProductAndProductCompareVM = new List<CProductAndProductCompareViewModel>();
            //產品與他的產品分類空列表
            List<CProductCompare> lsproductcategory = new List<CProductCompare>();//產品分類空列表
            foreach (var p in lsproduct)
            {
                List<CProductCompare> vlsProductcategory = CProductCompareFactory.fn商品類別對照查詢()
                                                    .Where(m => m.fProductId == p.fProductId)
                                                    .ToList();//抓出某產品所有產品分類
                lsProductAndProductCompareVM.Add(new CProductAndProductCompareViewModel() {
                    product = p,//單一產品
                    lsProductCompare = vlsProductcategory//上述單一產品的多項標籤
                });                
            }
            CShoppingDataViewModel sd = new CShoppingDataViewModel()
            {
                lsMember = CMemberFactory.fn會員查詢().ToList(),
                ls產品與產品分類 = lsProductAndProductCompareVM
            };//將資訊加入列表
            
            return View("HomePage", "_LayoutAdmin", sd);
            
        }
        [HttpPost]
        //顯示管理員照片
        public ActionResult homepagephoto(int adminid)
        {
            var adminphoto = CAdminFactory.fn管理員查詢().FirstOrDefault(n => n.fAdminId == adminid);
            CAdmin a = new CAdmin();
            a.fThePhoto = adminphoto.fThePhoto;
            return View("../ManagerSystem/adminsetupV2", "_LayoutAdmin");
        }


        [HttpPost]
        public string categorycharts(int fProductId, int fOrderId)
        {
            CDetailOrder detailOrder = CDetailOrderFactory.fn訂單明細查詢()
                                    .FirstOrDefault(d => d.fProductId == (int)fProductId);

            return "";
        }
        //黑名單頁面
        public ActionResult BlackList()
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
            if (admin == null)
            {
                return RedirectToAction("../Member/AdminLogin");
            }
            var blacklist = CBlackListFactory.fn黑名單查詢().ToList();
            return View(blacklist);
        }
        [HttpPost]
        //將選取的對象解除黑名單
        public string unlockBlacklist(int bannid,int memberid)
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
            List<CBlackList> blacklist = new List<CBlackList>();
            var unlock = CBlackListFactory.fn黑名單查詢().FirstOrDefault(m => m.fBannedId == bannid);
            var member = CMemberFactory.fn會員查詢().FirstOrDefault(n => n.fMemberId == memberid);
            member.fIsBanned = false;
            CMemberFactory.fn會員更新(member);
            CBlackListFactory.fn黑名單刪除(unlock);

            CNotice c = new CNotice();
            c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
            c.fNoticeContent = "此帳號已從黑名單解鎖";
            c.fCategoryType = "管理員";
            c.fLink = "超連結";
            c.fMemberId = member.fMemberId;
            CNoticeFactory.fn通知訂單訊息新增(c);

            return "解鎖成功!";
        }
        //留言板頁面
        public ActionResult MessageBoardView()
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
            if (admin == null)
            {
                return RedirectToAction("../Member/AdminLogin");
            }

            var board = CCommentFactory.fn留言查詢().ToList();
            return View(board);
        }
        [HttpPost]
        //將選取的對象加入黑名單
        public string commentboard(int commentid, string content,int memberid)
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
           
            var comment = CCommentFactory.fn留言查詢().FirstOrDefault(m => m.fCommentId == commentid);
            comment.fContent = content;
            comment.fIsBanned = true;
            CCommentFactory.fn留言更新(comment);
            var member=CMemberFactory.fn會員查詢().FirstOrDefault(n=>n.fMemberId== memberid);

            CNotice c = new CNotice();
            c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
            c.fNoticeContent = "此留言因涉及違規發言已被遮蔽";
            c.fCategoryType = "管理員";
            c.fLink = "超連結";
            c.fMemberId = member.fMemberId;
            CNoticeFactory.fn通知訂單訊息新增(c);

            return "遮蔽原因更新成功！";
        }      

        [HttpPost]
        //將選取的對象加入黑名單
        public string intoblacklist(int memberid, string blackreason)
        {
           
            var goblacklist = CMemberFactory.fn會員查詢().FirstOrDefault(g => g.fMemberId == memberid);
            
            if (goblacklist.fIsBanned == true)//判斷是否已經是黑名單 避免重複加入黑名單
            {
                ViewBag.blacklist = "此會員已是黑名單會員";

                return "已是黑名單成員";
            }
            else
            {
                
                goblacklist.fIsBanned = true;
                CMemberFactory.fn會員更新(goblacklist);
                CBlackListFactory.fn黑名單新增(new CBlackList()
                {

                    fLockDateTime = DateTime.UtcNow.AddHours(08),
                    fMemberId = memberid,
                    fReason = blackreason
                });

                CNotice c = new CNotice();
                c.fNoticeDatetime = DateTime.Now;
                c.fNoticeContent = "此帳號因發言屢次違規已被設為黑名單，期間內無法針對商品進行留言";
                c.fCategoryType = "管理員";
                c.fLink = "超連結";
                c.fMemberId = goblacklist.fMemberId;
                CNoticeFactory.fn通知訂單訊息新增(c);

                return "黑名單更新成功！";
            }          

        } 


        //管理員資訊頁面
        public ActionResult adminsetupV2()
        {
            if (Session[CAdminSession.Session_Login_User] == null)
            {
                Session[CAdminSession.Session_Login_User] = "憑證消失，請重新登入";
                return RedirectToAction("../Home/Index");
            }
            return View("../ManagerSystem/adminsetupV2");
        }
        [HttpPost]
        //將修改的資料存入資料庫
        public ActionResult adminsetupV2(CAdminEditor a)
        {
            CAdmin admin = Session[CAdminSession.Session_Login_User] as CAdmin;
            if (a.fThePhoto == null)
            {
                List<CAdmin> lsadmin = CAdminFactory.fn管理員查詢();
                CAdmin cadmin = lsadmin.FirstOrDefault(m => m.fAdminId == m.fAdminId);
                if (admin != null)
                {
                    if (a.Image != null)
                    {
                        //string photoName = Guid.NewGuid().ToString();
                        //photoName += Path.GetExtension(a.Image.FileName);
                        //a.Image.SaveAs(Server.MapPath("../Image/ManagerImage/" + photoName));
                        cadmin.fAdminAccount = a.fAdminAccount;
                        cadmin.fAdminPassword = a.fAdminPassword;
                        cadmin.fName = a.fName;
                        cadmin.fGender = a.fGender;
                        cadmin.fBirthDay = a.fBirthDay;
                        cadmin.fTheAddress = a.fTheAddress;
                        cadmin.fMobilePhone = a.fMobilePhone;
                        cadmin.fThePhoto = "../Image/MemberImage/MemberCat.jpg";
                        CAdminFactory.管理員更新(cadmin);
                        Session[CAdminSession.Session_Login_User] = cadmin;
                        return View("../ManagerSystem/adminsetupV2", "_LayoutAdmin");
                    }
                    else
                    {
                        cadmin.fAdminAccount = a.fAdminAccount;
                        cadmin.fAdminPassword = a.fAdminPassword;
                        cadmin.fName = a.fName;
                        cadmin.fGender = a.fGender;
                        cadmin.fBirthDay = a.fBirthDay;
                        cadmin.fTheAddress = a.fTheAddress;
                        cadmin.fMobilePhone = a.fMobilePhone;
                        CAdminFactory.管理員更新(cadmin);
                        Session[CAdminSession.Session_Login_User] = cadmin;
                        return View("../ManagerSystem/adminsetupV2", "_LayoutAdmin");
                    }
                }
            }
            return View("../ManagerSystem/adminsetupV2", "_LayoutAdmin");
        }
    }
}