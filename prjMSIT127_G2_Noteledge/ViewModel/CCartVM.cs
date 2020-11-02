using Microsoft.Ajax.Utilities;
using Models.MemberModels;
using Models.ShoppingModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ViewModel.CCartVM
{
    public class CCartVM
    {

        public CCart mycart { get; set; }

        public List<CCartProduct> lscartprooduct { get; set; }
        public CProduct product { get; set; }
        public List<CProductPicture> productpic { get; set; }

        public bool isActive { get; set; }

        [WebMethod(EnableSession = true)] //啟用Session
        public static CCartVM GetCurrentCart(CMember member) //取得目前Session中的Cart物件
        {
            //尚未送出的購物車
            var mycart = CCartFactory.fn購物車查詢(member).Where(c => c.fSubmitTime == null).FirstOrDefault();
            //如果為空，新增一個Cart物件
            if (mycart == null)
            {
                CCart cart = new CCart()
                {
                    fMemberId = member.fMemberId,
                    fSubmitTime = null
                };
                mycart = CCartFactory.fn購物車新增(member, cart);
            };

            //購物車內的商品(by cartid)
            var cartitem = CCartProductFactory.fn購物車商品查詢().Where(c => c.fCartId == mycart.fCartId).ToList();
            //所有商品封面
            var lsproduct = CProductPictureFactory.fn商品圖片查詢().Where(p => p.fTheRemovedDate == null).DistinctBy(p => p.fProductId).ToList();
            //CCartVM內的變數給值
            CCartVM Cart = new CCartVM()
            {
                mycart = mycart,
                lscartprooduct = cartitem,
                productpic = lsproduct
            };

            HttpContext.Current.Session["Cart"] = Cart;

            //回傳Session["Cart"]
            return (CCartVM)HttpContext.Current.Session["Cart"];
        }

        //計算購物車內商品的數量
        public int Count
        {
            get
            {
                return lscartprooduct.Count;
            }
        }

        //取得商品總價
        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach (var cartproduct in lscartprooduct)
                {
                    totalAmount += cartproduct.fPrice;
                }
                return totalAmount;
            }
        }
        
        
        internal static CCartVM GetCurrentCart(object member)
        {
            throw new NotImplementedException();
        }
    }
}