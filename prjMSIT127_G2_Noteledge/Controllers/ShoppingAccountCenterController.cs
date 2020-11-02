using Microsoft.Ajax.Utilities;
using Models.ManagementModels;
using Models.MemberModels;
using Models.NoteModels;
using Models.ShoppingModels;
using Newtonsoft.Json;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class ShoppingAccountCenterController : Controller
    {

        // GET: ShoppingAccountCenter
        public ActionResult Index()
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入~
            if (member == null)
                return RedirectToAction("../Member/Login");

            return View();
        }
        public ActionResult MyProduct()
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            //會員販售的商品
            List<CProductPicture> MyProduct = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fTheRemovedDate == null && p.fMemberSellerId == member.fMemberId).DistinctBy(p => p.fProductId).OrderBy(p => p.fProductId).ToList();

            //查詢會員所有的筆記資料夾
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member).ToList();

            //筆記會員資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();

            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder).ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            };

            //商品類別給後面下拉式選單用
            List<CProductCategory> lsProductCategory = CProductCategoryFactory.fn商品類別查詢().ToList();
            ViewBag.Categories = new MultiSelectList(lsProductCategory, "fCategoryId", "fCategoryName");

            List<CProductCompare> lsproductcategory = CProductCompareFactory.fn商品類別對照查詢().Where(m=>m.fMemberSellerId== member.fMemberId).ToList();
            
            CShoppingAccountCenterVM ShoppingAccountCenter = new CShoppingAccountCenterVM()
            {
                lsNotefolderVM = lsNotefolderVM,
                lsCategory = lsProductCategory,
                lsProductPicture = MyProduct,
                lsCategoryCompare= lsproductcategory
            };

            return PartialView("_MyProduct", ShoppingAccountCenter);
        }
        
        [HttpPost]
        public String ProductPicture(int ProductId)
        {
            //該商品的圖片(多個)
            List<CProductPicture> lsProductPicture = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fProductId == ProductId).ToList();
            string result = JsonConvert.SerializeObject(lsProductPicture);
            return result;
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateProduct(CShoppingAccountCenterVM NewProduct, int?[] fCategoryId, string[] ProductPicture)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            //商品新增
            CProduct product = new CProduct();
            product.fName = NewProduct.Product.fName;
            product.fDescription = NewProduct.Product.fDescription;
            product.fContent = NewProduct.Content; //抓JSON資料
            product.fPrice = NewProduct.Product.fPrice;
            product.fLaunchDate = DateTime.UtcNow.AddHours(08);
            product.fTheRemovedDate = null;
            product.fDownloadTimes = 0;
            product.fLikeCount = 0;
            product.fMemberSellerId = member.fMemberId;
            product = CProductFactory.fn商品新增(member, product);

            //商品圖片新增
            CProductPicture productPicture = new CProductPicture();

            //如果沒有選就給預設圖片(封面)
            if (ProductPicture[0] == "")
            {
                productPicture.fPicture = "https://creazilla-store.fra1.digitaloceanspaces.com/emojis/44574/notebook-emoji-clipart-md.png";//預設的圖片
                productPicture.fProductId = product.fProductId;
                CProductPictureFactory.fn商品圖片新增(product, productPicture);
            }
            else
            {
                foreach (var item in ProductPicture)
                {
                    if (item != "")
                    {
                        productPicture.fPicture = item;
                        productPicture.fProductId = product.fProductId;
                        CProductPictureFactory.fn商品圖片新增(product, productPicture);
                    }
                    
                }
            }

            //商品類別新增
            CProductCompare productCompare = new CProductCompare();
            if (fCategoryId == null)
            {
                
            }
            else
            {
                foreach (var item in fCategoryId)
                {
                    CProductCategory productCategory = CProductCategoryFactory.fn商品類別查詢().Where(c => c.fCategoryId == item).FirstOrDefault();
                    productCompare.fProductId = product.fProductId;

                    productCompare.fCategoryId = item.Value;
                    CProductCompareFactory.fn商品類別對照新增(product, productCategory);
                }
            }

            return Redirect("../ShoppingAccountCenter?goto=seller");
        }

        //修改GET抓取JSON資料
        //string myProductContent;
        public JsonResult EditProduct(int? fProductId)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入

            CProduct myProduct = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == fProductId);

            List<CProductPicture> lsMyProductPicture = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fProductId == fProductId).ToList();

            List<CProductCompare> lsProductCategory = CProductCompareFactory.fn商品類別對照查詢().Where(c => c.fProductId == fProductId).ToList();
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member).ToList();
            
            //筆記資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();
            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder).OrderBy(n => n.fNoteListLevel).Where(n=>n.fJsonContent == myProduct.fContent).ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            }
            CNote mynote = CNoteFactory.fn私人筆記全部查詢().FirstOrDefault(n => n.fJsonContent == myProduct.fContent);

            CShoppingAccountCenterVM ShoppingAccountCenter = new CShoppingAccountCenterVM()
            {
                lsCategoryCompare = lsProductCategory,
                lsProductPicture = lsMyProductPicture,
                Product = myProduct,
                lsNotefolderVM= lsNotefolderVM,
                NoteId = mynote.fNoteId
                //Content = myProductContent,
            };

            string value = string.Empty;
            value = JsonConvert.SerializeObject(ShoppingAccountCenter, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditProduct(CShoppingAccountCenterVM NewProduct, int[] fCategoryId, int? fProductId)
        {
            //登入的會員資訊
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (member == null)
                return RedirectToAction("../Member/Login");

            CProduct product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == fProductId);
            product.fName = NewProduct.Product.fName;
            product.fDescription = NewProduct.Product.fDescription;
            product.fContent = NewProduct.Content;
            product.fPrice = NewProduct.Product.fPrice;
            product.fLaunchDate = DateTime.UtcNow.AddHours(08);
            product.fTheRemovedDate = null;
            product.fDownloadTimes = 0;
            product.fLikeCount = 0;
            product.fMemberSellerId = member.fMemberId;
            CProductFactory.fn商品更新(product);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteProduct(int? fProductId)
        {
            //更新下架時間不再出現於商品展示區
            CProduct myProduct = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fProductId == fProductId);
            myProduct.fTheRemovedDate = DateTime.UtcNow.AddHours(08);
            CProductFactory.fn商品更新(myProduct);

            return RedirectToAction("Index");
        }

        //我的帳戶個人檔案
        [HttpPost]
        public ActionResult MyProfile(int fMemberId)
        {
            //登入的會員資訊
            CMember Member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (Member == null)
                return RedirectToAction("../Member/Login");
            
            return PartialView("_MyProfile", Member);
        }

        //我的瑪妮幣
        [HttpPost]
        public ActionResult MyMoney(int fMemberId)
        {
            //登入的會員資訊
            CMember Member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (Member == null)
                return RedirectToAction("../Member/Login");
            List<CIncome> lsincome = CIncomeFactory.fn公司收入查詢().Where(i => (i.fMemberId == Member.fMemberId && i.fIncomeCategory == "儲值") || (i.fMemberId == Member.fMemberId && i.fIncomeCategory == "獲利")).ToList();
            List<COrder> lsorder = COrderFactory.fn訂單查詢(Member).ToList();
            List<CDetailOrder> lsdetailorder = CDetailOrderFactory.fn訂單明細查詢().ToList();            
             
            CMoneyVM MoneyVM = new CMoneyVM() 
            {
            lsIncome= lsincome,
            lsOrder = lsorder,
            lsOrderDetail = lsdetailorder
             };

            return PartialView("_MyMoney", MoneyVM);
        }

        //我的購買清單
        [HttpPost]
        public ActionResult PurchaseList(int fMemberId)
        {
            //登入的會員資訊
            CMember Member = (CMember)Session[CMemberSession.Session_Login_User];
            //防止未登入者進入
            if (Member == null)
                return RedirectToAction("../Member/Login");
            
            List<COrder> lsorder = COrderFactory.fn訂單查詢(Member).ToList();
            List<CMemberOrderSelectVM> lsdetailorder = CMemberFactory.fn會員訂單個人查詢(Member).ToList();

            
            List<CMember> lsmemberseller = new List<CMember>();
            foreach (var item in lsdetailorder)
            {
                CProduct product = new CProduct();
                product = CProductFactory.fn商品查詢().FirstOrDefault(p => p.fTheRemovedDate == null && p.fProductId== item.fProductId);
                if (product == null)
                    break;
                CMember memberseller = new CMember();
                memberseller = CMemberFactory.fn會員查詢().FirstOrDefault(m => m.fMemberId == product.fMemberSellerId);
                
                lsmemberseller.Add(memberseller);
            }

            //未下架的商品(含商品封面圖片)
            List<CProductPicture> lsproductpic = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fTheRemovedDate == null).DistinctBy(p => p.fProductId).OrderBy(p => p.fProductId).ToList();

            CPurchaseListVM PurchaseListVM = new CPurchaseListVM()
            {
                lsMemberSeller = lsmemberseller,
                lsProductPicture = lsproductpic,
                lsOrder = lsorder,
                lsOrderDetail = lsdetailorder
            };

            return PartialView("_PurchaseList", PurchaseListVM);
        }
    }
}