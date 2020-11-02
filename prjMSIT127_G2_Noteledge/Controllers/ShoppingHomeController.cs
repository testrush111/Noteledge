using Microsoft.Ajax.Utilities;
using Models.MemberModels;
using Models.ShoppingModels;
using Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ViewModel.CShoppingHomeVM;
using ViewModel.CCartVM;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;
using prjMSIT127_G2_Noteledge.ViewModel;
using System.Text.Json;
using prjMSIT127_G2_Noteledge.Controllers.API;
using Models.ManagementModels;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class ShoppingHomeController : Controller
    {

        //GET: ShoppingHome
        public ActionResult Index()
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            //未下架的商品(含商品封面圖片)
            List<CProductPicture> lsproduct = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fTheRemovedDate == null).DistinctBy(p => p.fProductId).OrderBy(p => p.fProductId).ToList();


            //裝多個CShoppingHomeVM的容器
            List<CShoppingHomeVM> lsshoppinghomeVM = new List<CShoppingHomeVM>();

            foreach (var product in lsproduct)
            {
                //撈出該商品的賣家資訊
                CMember memberseller = CMemberFactory.fn會員查詢().FirstOrDefault(m => m.fMemberId == product.fMemberSellerId);
                
                //撈出該商品的類別
                List<CProductCompare> category = CProductCompareFactory.fn商品類別對照查詢().Where(c => c.fProductId == product.fProductId).ToList();

                //CShoppingHomeVM內的變數給值
                CShoppingHomeVM shoppinghomeVM = new CShoppingHomeVM()
                {
                    member = memberseller,
                    lsproductcategory = category,
                    productpicture = product,
                };
                lsshoppinghomeVM.Add(shoppinghomeVM);

            }
            
            return View("Index", lsshoppinghomeVM);
        }
        [HttpPost]
        public int LikeProduct(int ProductId) {
            
            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == ProductId);
            product.fLikeCount +=  1;
            CProductFactory.fn商品更新(product);

            int result = product.fLikeCount;
            return result;
        }

        [HttpPost]
        public int DislikeProduct(int ProductId)
        {

            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == ProductId);
            product.fLikeCount -= 1;
            CProductFactory.fn商品更新(product);

            int result = product.fLikeCount;
            return result;
        }

        //POST:用Productid將商品加入購物車，並回傳購物車頁面
        [HttpPost]
        public ActionResult AddToCart(int ProductId)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            
            //目前購物車Session的項目
            CCartVM currentCart = CCartVM.GetCurrentCart(member);
            
            //商品資訊，給fn購物車商品新增()使用
            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == ProductId);
            //判斷商品是否已在Cart內
            if (currentCart.lscartprooduct.Any(p => p.fProductId == product.fProductId))
            {
                
                return View("Index","ShoppingHome");
            }
            else 
            {
                //不存在購物車內，則新增一筆
                CCartProductFactory.fn購物車商品新增(product, currentCart.mycart);
            }

            return PartialView("_CartPartial");
        }

        //POST:用CartProductId將購物車內的商品移除，並回傳購物車頁面
        [HttpPost]
        public ActionResult RemoveFromCart(int CartProductId)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];

            //目前購物車Session的項目
            CCartVM currentCart = CCartVM.GetCurrentCart(member);

            //商品資訊，給fn購物車商品刪除()使用
            CCartProduct CartProduct =  currentCart.lscartprooduct.FirstOrDefault(p => p.fCartProductId == CartProductId);
            
            CCartProductFactory.fn購物車商品刪除(CartProduct);
           
            return PartialView("_CartPartial");
        }


        //GET:檢視購物車詳細列表
        public ActionResult CartView()
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            //目前購物車Session的項目
            CCartVM currentCart = CCartVM.GetCurrentCart(member);
            
            return View(currentCart);
        }

        //GET:購物車詳細列表移除商品
        [HttpPost]
        public ActionResult RemoveCartProduct(int CartProductId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            CCartVM currentCart = CCartVM.GetCurrentCart(member);
            CCartProduct CartProduct = currentCart.lscartprooduct.FirstOrDefault(p => p.fCartProductId == CartProductId);
            CCartProductFactory.fn購物車商品刪除(CartProduct);
            return RedirectToAction("CartView");
        }

        //GET:購物車詳細列表移除全部商品
        [HttpPost]
        public ActionResult CartClear(int CartId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            CCartVM currentCart = CCartVM.GetCurrentCart(member);
            foreach (var item in currentCart.lscartprooduct.Where(p=>p.fCartId==CartId))
            {
                CCartProductFactory.fn購物車商品刪除(item);
            }
            return RedirectToAction("CartView");
        }

        //GET:下次再買
        public ActionResult PurchaseNextTime(int CartProductId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            CCartVM currentCart = CCartVM.GetCurrentCart(member);
            CCartProduct CartProduct = currentCart.lscartprooduct.FirstOrDefault(p => p.fCartProductId == CartProductId);
            
            //新增購物車
            CCart cart = new CCart()
            {
                fMemberId = member.fMemberId,
                fSubmitTime = null
            };
            CCart mycart = CCartFactory.fn購物車新增(member, cart);
            
            //新增下次再買的商品到新的購物車
            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == CartProduct.fProductId);
            CCartProductFactory.fn購物車商品新增(product, mycart);
            
            //刪除現在購物車裡的商品
            CCartProductFactory.fn購物車商品刪除(CartProduct);
            
            return RedirectToAction("CartView");
        }

        [HttpPost]
        public int ToDetial()
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];

            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);

            return cMember.fMoneyPoint;
        }
        [ValidateInput(false)]
        [HttpPost]
        public void ToOrder(int totalprice,int remain,int cartId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            COrder o = new COrder();
            o.fPurchaseDate = DateTime.UtcNow.AddHours(08);
            o.fTotalPrice = totalprice;
            o.fMemberId = member.fMemberId;
            COrderFactory.fn訂單新增(o);

            CMemberFactory.fn會員更新點數(member, remain);

            CNotice c = new CNotice();
            c.fCategoryType = "系統";
            c.fLink = "超連結";
            c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
            c.fMemberId = member.fMemberId;
            c.fNoticeContent = "您的訂單已完成了";
            CNoticeFactory.fn通知訂單訊息新增(c);

            List<CCartProduct> CP = CCartProductFactory.fn購物車商品個人查詢(cartId).ToList();

            var f = CNoteFolderFactory.fn筆記資料夾查詢(member).Where(q => q.fFolderName == "未分類筆記").ToList();

            int orderid = COrderFactory.fn訂單查詢(member).LastOrDefault().fOrderId;
            foreach (var a in CP)
            {
                CDetailOrderFactory.fn訂單明細新增(orderid, a.fProductId);
                CNotice c1 = new CNotice();
                c1.fCategoryType = "銷售";
                c1.fLink = "超連結";
                c1.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
                c1.fMemberId = a.fMemberSellerId;
                c1.fNoticeContent = "您的"+a.fName+"已被購買";
                CNoticeFactory.fn通知訂單訊息新增(c1);

                CIncome i1 = new CIncome();
                i1.fIncome = a.fPrice;
                i1.fPaymentDateTime = DateTime.UtcNow.AddHours(08);
                i1.fIncomeCategory = "獲利";
                i1.fMemberId = a.fMemberSellerId;
                CIncomeFactory.fn公司獲利新增(i1);

                var point = CMemberFactory.fn會員查詢().Where(z => z.fMemberId == a.fMemberSellerId);
                int point1 = (point.Single().fMoneyPoint) + a.fPrice;
                CMember cm1 = new CMember();
                cm1.fMemberId = point.Single().fMemberId;
                CMemberFactory.fn會員更新點數(cm1, point1);

                var t = CNoteFactory.fn私人筆記全部查詢().Where(q => q.fFolderId == f.Single().fFolderId).ToList();

                int t1 = t.Count();

                CNote n = new CNote();
                n.fNoteListName = a.fName;
                n.fCreateDateTime = DateTime.UtcNow.AddHours(08);
                n.fEditDateTime = DateTime.UtcNow.AddHours(08);
                n.fNoteListLevel = t1;
                n.fIsMyFavourite = false;
                n.fIsTrash = false;
                n.fFolderId = f.Single().fFolderId;
                n.fJsonContent = a.fContent;
                n.fTheShareLink = null;
                n.fTheContactPerson = null;
                n.fHTMLContent = "";
                CNoteFactory.fn訂單私人筆記新增(n);
            }

            CCartFactory.fn購物車個人更新(cartId);
        }

        public ActionResult ToRank(int orderid)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            List<CProducttotal> lsproducttotal = CProductRankFactory.fn評價個人查詢(orderid);

            return View(lsproducttotal);
            
        }

        [HttpPost]
        public ActionResult ToInsert(string datas)
        {
            var scores = JsonSerializer.Deserialize<Score[]>(datas);
            foreach(var i in scores)
            {
                CProductRank rank = new CProductRank();
                rank.fRank = i.score;
                rank.fComment = i.message;
                rank.fSubmitDataTime = DateTime.UtcNow.AddHours(08);
                rank.fDetailOrderIId = i.id;
                CProductRankFactory.fn評價新增(rank);

                var a = CDetailOrderFactory.fn訂單明細查詢().Where(n => n.fDetailOrderIId == i.id).ToList();

                CNotice c = new CNotice();
                c.fCategoryType = "評價留言";
                c.fLink = "超連結";
                c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
                c.fMemberId = a.Single().fMemberSellerId;
                c.fNoticeContent = "您的" + a.Single().fName +"以新增評價";
                CNoticeFactory.fn通知訂單訊息新增(c);
            }
            
            return new ApiResult();
        }

        struct Score
        {
            public int id { get; set; }
            public int score { get; set; }
            public string message { get; set; }
        }
    }
}