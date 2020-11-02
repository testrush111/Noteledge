using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CCartProductFactory
    {
        //=========================== 購物車商品新增 =========================== //
        public static CCartProduct fn購物車商品新增(CProduct product, CCart cart)
        {
            string sql = $"EXEC 購物車商品新增 ";
            sql += $"@{CCartProductKey.fProductId},";
            sql += $"@{CCartProductKey.fCartId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartProductKey.fProductId, product.fProductId),
                new SqlParameter(CCartProductKey.fCartId, cart.fCartId)
            };

            CDbManager.executeSql(sql, paras);
            CCartProduct NewCartProduct = fn購物車商品查詢().LastOrDefault();
            return NewCartProduct;
        }

        //=========================== 購物車商品查詢(all) =========================== //
        public static List<CCartProduct> fn購物車商品查詢()
        {
            string sql = $"EXEC 購物車商品查詢";
            return (List<CCartProduct>)CDbManager.querySql(sql, null, reader購物車商品查詢);
        }

        private static IList reader購物車商品查詢(SqlDataReader reader)
        {
            List<CCartProduct> lsCartProduct = new List<CCartProduct>();
            while (reader.Read())
            {
                lsCartProduct.Add(new CCartProduct()
                {
                    fCartProductId = (int)reader[CCartProductKey.fCartProductId],
                    fProductId = (int)reader[CCartProductKey.fProductId],
                    fName = (string)reader[CCartProductKey.fName],
                    fDescription = (string)reader[CCartProductKey.fDescription],
                    fContent = (string)reader[CCartProductKey.fContent],
                    fPrice = (int)reader[CCartProductKey.fPrice],
                    fLaunchDate = (DateTime)reader[CCartProductKey.fLaunchDate],
                    fTheRemovedDate = reader[CCartProductKey.fTheRemovedDate] as DateTime?,//可NULL
                    fDownloadTimes = (int)reader[CCartProductKey.fDownloadTimes],
                    fLikeCount = (int)reader[CCartProductKey.fLikeCount],
                    fMemberSellerId = (int)reader[CCartProductKey.fMemberSellerId],
                    fCartId = (int)reader[CCartProductKey.fCartId],
                    fSubmitTime = reader[CCartProductKey.fSubmitTime] as DateTime?,//可NULL,
                    fMemberId = (int)reader[CCartProductKey.fMemberId],
                });
            }
            return lsCartProduct;
        }

        //=========================== 購物車商品刪除 =========================== //
        public static void fn購物車商品刪除(CCartProduct cartProduct)
        {
            string sql = $"EXEC 購物車商品刪除 ";
            sql += $"@{CCartProductKey.fCartProductId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartProductKey.fCartProductId, cartProduct.fCartProductId),
            };

            CDbManager.executeSql(sql, paras);
        }

        //==========================購物車個人商品查詢==========================//
        public static List<CCartProduct> fn購物車商品個人查詢(int cartid)
        {
            string sql = $"EXEC 購物車商品個人查詢 @{CCartProductKey.fCartId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartProductKey.fCartId, cartid)
            };
            return (List<CCartProduct>)CDbManager.querySql(sql, paras, reader購物車商品個人查詢);
        }

        private static IList reader購物車商品個人查詢(SqlDataReader reader)
        {
            List<CCartProduct> lsCartProduct = new List<CCartProduct>();
            while (reader.Read())
            {
                lsCartProduct.Add(new CCartProduct()
                {
                    fProductId = (int)reader[CCartProductKey.fProductId],
                    fName = (string)reader[CCartProductKey.fName],
                    fMemberSellerId = (int)reader[CCartProductKey.fMemberSellerId],
                    fContent = (string)reader[CCartProductKey.fContent],
                    fPrice = (int)reader[CCartProductKey.fPrice]
                });
            }
            return lsCartProduct;
        }
    }
}
