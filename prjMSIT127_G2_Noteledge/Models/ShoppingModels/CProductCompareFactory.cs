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
    public class CProductCompareFactory
    {
        //=========================== 商品類別對照新增 =========================== //
        public static void fn商品類別對照新增(CProduct product, CProductCategory productCategory)
        {
            string sql = $"EXEC 商品類別對照新增 ";
            sql += $"@{CProductCompareKey.fProductId},";
            sql += $"@{CProductCompareKey.fCategoryId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductCompareKey.fProductId, product.fProductId),
                new SqlParameter(CProductCompareKey.fCategoryId, productCategory.fCategoryId)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 商品類別對照查詢(all) =========================== //
        public static List<CProductCompare> fn商品類別對照查詢()
        {
            string sql = $"EXEC 商品類別對照查詢";
            return (List<CProductCompare>)CDbManager.querySql(sql, null, reader商品類別對照查詢);
        }

        private static IList reader商品類別對照查詢(SqlDataReader reader)
        {
            List<CProductCompare> lsProductCompare = new List<CProductCompare>();
            while (reader.Read())
            {
                lsProductCompare.Add(new CProductCompare()
                {
                    fProductComparedId = (int)reader[CProductCompareKey.fProductComparedId],
                    fProductId = (int)reader[CProductCompareKey.fProductId],
                    fName = (string)reader[CProductCompareKey.fName],
                    fDescription = (string)reader[CProductCompareKey.fDescription],
                    fContent = (string)reader[CProductCompareKey.fContent],
                    fPrice = (int)reader[CProductCompareKey.fPrice],
                    fLaunchDate = (DateTime)reader[CProductCompareKey.fLaunchDate],
                    fTheRemovedDate = reader[CProductCompareKey.fTheRemovedDate] as DateTime?,//可NULL
                    fDownloadTimes = (int)reader[CProductCompareKey.fDownloadTimes],
                    fLikeCount = (int)reader[CProductCompareKey.fLikeCount],
                    fMemberSellerId = (int)reader[CProductCompareKey.fMemberSellerId],
                    fCategoryId = (int)reader[CProductCompareKey.fCategoryId],
                    fCategoryName = (string)reader[CProductCompareKey.fCategoryName]
                });
            }
            return lsProductCompare;
        }

        //=========================== 商品類別對照刪除 =========================== //
        public static void fn商品類別對照刪除(CProductCompare productCompare)
        {
            string sql = $"EXEC 商品類別對照刪除 ";
            sql += $"@{CProductCompareKey.fProductComparedId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductCompareKey.fProductComparedId, productCompare.fProductComparedId),
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}
