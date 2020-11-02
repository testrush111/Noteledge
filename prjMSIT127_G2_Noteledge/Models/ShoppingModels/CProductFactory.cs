using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Models.ShoppingModels
{
    public class CProductFactory
    {
        public static void fn商品更新(CProduct product)
        {
            string sql = $"EXEC 商品更新 ";
            sql += $"@{CProductKey.fProductId},";
            sql += $"@{CProductKey.fName},";
            sql += $"@{CProductKey.fDescription},";
            sql += $"@{CProductKey.fContent},";
            sql += $"@{CProductKey.fPrice},";
            sql += $"@{CProductKey.fLaunchDate},";
            sql += $"@{CProductKey.fTheRemovedDate},";
            sql += $"@{CProductKey.fDownloadTimes},";
            sql += $"@{CProductKey.fLikeCount},";
            sql += $"@{CProductKey.fMemberSellerId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductKey.fProductId, product.fProductId),
                new SqlParameter(CProductKey.fName, product.fName),
                new SqlParameter(CProductKey.fDescription, product.fDescription),
                new SqlParameter(CProductKey.fContent, product.fContent),
                new SqlParameter(CProductKey.fPrice, product.fPrice),
                new SqlParameter(CProductKey.fLaunchDate, product.fLaunchDate),
                new SqlParameter(CProductKey.fTheRemovedDate, (object)product.fTheRemovedDate ?? DBNull.Value), //可NULL
                new SqlParameter(CProductKey.fDownloadTimes, product.fDownloadTimes),
                new SqlParameter(CProductKey.fLikeCount, product.fLikeCount),
                new SqlParameter(CProductKey.fMemberSellerId, product.fMemberSellerId)
            };

            CDbManager.executeSql(sql, paras);
        }
        public static CProduct fn商品新增(CMember member, CProduct product)
        {
            string sql = $"EXEC 商品新增 ";
            sql += $"@{CProductKey.fName},";
            sql += $"@{CProductKey.fDescription},";
            sql += $"@{CProductKey.fContent},";
            sql += $"@{CProductKey.fPrice},";
            sql += $"@{CProductKey.fLaunchDate},";
            sql += $"@{CProductKey.fTheRemovedDate},";
            sql += $"@{CProductKey.fDownloadTimes},";
            sql += $"@{CProductKey.fLikeCount},";
            sql += $"@{CProductKey.fMemberSellerId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductKey.fName, product.fName),
                new SqlParameter(CProductKey.fDescription, product.fDescription),
                new SqlParameter(CProductKey.fContent, product.fContent),
                new SqlParameter(CProductKey.fPrice, product.fPrice),
                new SqlParameter(CProductKey.fLaunchDate, product.fLaunchDate),
                new SqlParameter(CProductKey.fTheRemovedDate, (object)product.fTheRemovedDate ?? DBNull.Value), //可NULL
                new SqlParameter(CProductKey.fDownloadTimes, product.fDownloadTimes),
                new SqlParameter(CProductKey.fLikeCount, product.fLikeCount),
                new SqlParameter(CProductKey.fMemberSellerId, member.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
            CProduct NewProduct = fn商品查詢().LastOrDefault();
            return NewProduct;
        }
        public static List<CProduct> fn商品查詢()
        {
            string sql = $"EXEC 商品查詢";
            return (List<CProduct>)CDbManager.querySql(sql, null, reader商品查詢);
        }

        private static IList reader商品查詢(SqlDataReader reader)
        {
            List<CProduct> lsProduct = new List<CProduct>(); //商品列表
            while (reader.Read())//讀取每筆商品資料
            {
                lsProduct.Add(new CProduct()
                {
                    fProductId = (int)reader[CProductKey.fProductId],
                    fName = (string)reader[CProductKey.fName],
                    fDescription = (string)reader[CProductKey.fDescription],
                    fContent = (string)reader[CProductKey.fContent],
                    fPrice = (int)reader[CProductKey.fPrice],
                    fLaunchDate = (DateTime)reader[CProductKey.fLaunchDate],
                    fTheRemovedDate = reader[CProductKey.fTheRemovedDate] as DateTime?,//可NULL
                    fDownloadTimes = (int)reader[CProductKey.fDownloadTimes],
                    fLikeCount = (int)reader[CProductKey.fLikeCount],
                    fMemberSellerId = (int)reader[CProductKey.fMemberSellerId]

                });
            }
            return lsProduct;
        }
    }
}