using Microsoft.AspNet.SignalR;
using Google.Apis.Util;
using Microsoft.Ajax.Utilities;
using Models.MemberModels;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.Hubs;
using PagedList;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel.CShoppingHomeVM;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class ProductController : Controller
    {
        // GET: AllProduct
        public ActionResult Index(string sortOrder, string currentFilter, string searchString,int? page)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            //排序條件(日期/價格/下載量/讚數)
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            //記錄每頁商品數及目前頁數
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            //未下架的商品(含商品封面圖片)
            List<CProductPicture> lsproduct = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fTheRemovedDate == null).DistinctBy(p => p.fProductId).OrderBy(p => p.fProductId).ToList();

            //篩選商品(類別/名稱/描述)
            switch (searchString)
            {
                case "Education":
                    List<CProductCompare> lscategory = CProductCompareFactory.fn商品類別對照查詢().Where(c => c.fCategoryName=="程式").ToList();
                    List<CProductPicture> lsproducttag1 = new List<CProductPicture>();
                    foreach (var item in lscategory)
                    {
                        CProductPicture producttag1 = new CProductPicture();
                        producttag1 = lsproduct.Where(p => p.fProductId == item.fProductId && p.fTheRemovedDate == null).FirstOrDefault();
                        if (producttag1 != null)
                        lsproducttag1.Add(producttag1);
                    }
                    lsproduct = lsproducttag1;
                    break;
                case null:
                    searchString = currentFilter;
                    break;
                default:
                    lsproduct = lsproduct.Where(p => p.fName.Contains(searchString)
                                             || p.fDescription.Contains(searchString)).ToList();
                    break;
            }

            switch (sortOrder)
            {
                case "New":
                    lsproduct = lsproduct.Where(p => DateTime.UtcNow.AddHours(08).AddMonths(-1).Month < p.fLaunchDate.Month && p.fLaunchDate.Month <= DateTime.UtcNow.AddHours(08).Month).OrderByDescending(p => p.fLaunchDate).ToList();
                    break;
                case "Date":
                    lsproduct = lsproduct.OrderBy(p => p.fLaunchDate).ToList();
                    break;
                case "Price":
                    lsproduct = lsproduct.OrderBy(p=>p.fPrice).ToList();
                    break;
                case "Price_desc":
                    lsproduct = lsproduct.OrderByDescending(p => p.fPrice).ToList();
                    break;
                case "DownLoad_desc":
                    lsproduct = lsproduct.OrderByDescending(p => p.fDownloadTimes).ToList();
                    break;
                case "Like_desc":
                    lsproduct = lsproduct.OrderByDescending(p => p.fLikeCount).ToList();
                    break;
                case "Free":
                    lsproduct = lsproduct.Where(p => p.fPrice == 0).ToList();
                    break;
                default:
                    lsproduct = lsproduct.OrderByDescending(p => p.fLaunchDate).ToList(); 
                    break;
            }

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

            IPagedList<CShoppingHomeVM> pagedlist = lsshoppinghomeVM.ToPagedList(pageNumber, pageSize);
            return View("Index",pagedlist);
        }


        // GET: ProductDetail
        public ActionResult ProductDetail(int ProductId)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");
            //所有會員資訊
            List<CMember> lsMember = CMemberFactory.fn會員查詢().ToList();

            List<CMember> Isbanned = CMemberFactory.fn會員查詢().Where(n => n.fMemberId == member.fMemberId).ToList();
            //所選到的商品
            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == ProductId);
            
            //該商品的賣家資訊
            CMember memberseller = CMemberFactory.fn會員查詢().FirstOrDefault(m => m.fMemberId == product.fMemberSellerId);

            //該商品的圖片(多個)
            List<CProductPicture> lsProductPicture = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fProductId == ProductId).ToList();

            //該商品的類別(多個)
            List<CProductCompare> lsCategory = CProductCompareFactory.fn商品類別對照查詢().Where(c => c.fProductId == ProductId).ToList();

            //該商品的留言(多個)
            List<CComment> lsProductComment = CCommentFactory.fn留言查詢().Where(c => c.fProductId == ProductId).ToList();

            List<CProductRank> lsProductRank = CProductRankFactory.fn評價查詢(ProductId).ToList();

            //新增瀏覽紀錄
            CProductBrowse productBrowse = new CProductBrowse() {
                fBrowseDataTime = DateTime.UtcNow.AddHours(08),
                fProductId = ProductId
            };
            CProductBrowseFactory.fn商品瀏覽紀錄新增(product, productBrowse);
            
            List<CProductDetailVM> lsProductDetail = new List<CProductDetailVM>();

            CProductDetailVM productDetailVM = new CProductDetailVM()
            {
                lsMember = lsMember,
                MemberSeller = memberseller,
                Product = product,
                lsProductPicture = lsProductPicture,
                lsProductCategory = lsCategory,
                lsProductComment = lsProductComment,
                lsProductRank = lsProductRank,
                Isbanned = Isbanned
            };
            
            return View(productDetailVM);
        }

        [HttpPost]
        public void ToComment(string content,int pid)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];

            CComment c = new CComment();
            c.fCommentDateTime = DateTime.UtcNow.AddHours(08);
            c.fContent = content;
            c.fIsBanned = false;
            c.fIsRetract = false;
            c.fLikeCount = 0;
            c.fMemberId = member.fMemberId;
            c.fProductId = pid;

            CCommentFactory.fn留言新增(c);

            var m = CMemberFactory.fn會員查詢().Where(z => z.fMemberId == member.fMemberId);

            GlobalHost.ConnectionManager.GetHubContext<ProductHub>().Clients.Group(ProductHub.getGroupIdString(pid)).newMessage(m.Single().fPhoto, m.Single().fTheNickName, DateTime.UtcNow.AddHours(08).ToString(), content);
        }

        [HttpPost]
        public int LikeProduct(int ProductId)
        {

            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == ProductId);
            product.fLikeCount += 1;
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

        [HttpPost]
        public int LikeProductDetial(int fCommentId)
        {

            CComment comment = CCommentFactory.fn留言查詢().FirstOrDefault(p => p.fCommentId == fCommentId);
            comment.fLikeCount += 1;
            CCommentFactory.fn留言更新(comment);

            int result = comment.fLikeCount;
            return result;
        }

        [HttpPost]
        public int DislikeProductDetial(int fCommentId)
        {

            CComment comment = CCommentFactory.fn留言查詢().FirstOrDefault(p => p.fCommentId == fCommentId);
            comment.fLikeCount -= 1;
            CCommentFactory.fn留言更新(comment);

            int result = comment.fLikeCount;
            return result;
        }
    }
}