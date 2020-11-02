using DbManager;
using Models.ShoppingModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CProductBrowseFactory
    {
        //=========================== 商品瀏覽紀錄新增 =========================== //
        public static void fn商品瀏覽紀錄新增(CProduct product, CProductBrowse productBorwse)
        {
            string sql = $"EXEC 商品瀏覽紀錄新增 ";
            sql += $"@{CProductBrowseKey.fBrowseDataTime},";
            sql += $"@{CProductBrowseKey.fProductId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductBrowseKey.fBrowseDataTime, productBorwse.fBrowseDataTime),
                new SqlParameter(CProductBrowseKey.fProductId, product.fProductId)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 商品瀏覽紀錄查詢(all) =========================== //
        public static List<CProductBrowse> fn商品瀏覽紀錄查詢()
        {
            string sql = $"EXEC 商品瀏覽紀錄查詢";
            return (List<CProductBrowse>)CDbManager.querySql(sql, null, reader商品瀏覽紀錄查詢);
        }

        private static IList reader商品瀏覽紀錄查詢(SqlDataReader reader)
        {
            List<CProductBrowse> lsProductBorwse = new List<CProductBrowse>();
            while (reader.Read())
            {
                lsProductBorwse.Add(new CProductBrowse()
                {
                    fProductBrowseId = (int)reader[CProductBrowseKey.fProductBrowseId],
                    fBrowseDataTime = (DateTime)reader[CProductBrowseKey.fBrowseDataTime],
                    fProductId = (int)reader[CProductBrowseKey.fProductId],
                    fName = (string)reader[CProductBrowseKey.fName],
                    fDescription = (string)reader[CProductBrowseKey.fDescription],
                    fContent = (string)reader[CProductBrowseKey.fContent],
                    fPrice = (int)reader[CProductBrowseKey.fPrice],
                    fLaunchDate = (DateTime)reader[CProductBrowseKey.fLaunchDate],
                    fTheRemovedDate = reader[CProductBrowseKey.fTheRemovedDate] as DateTime?,//可NULL
                    fDownloadTimes = (int)reader[CProductBrowseKey.fDownloadTimes],
                    fLikeCount = (int)reader[CProductBrowseKey.fLikeCount],
                    fMemberSellerId = (int)reader[CProductBrowseKey.fMemberSellerId]
                });
            }
            return lsProductBorwse;
        }
    }
}
