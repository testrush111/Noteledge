using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Models.ShoppingModels
{
    public class CProductPictureFactory
    {
        //=========================== 商品圖片更新 =========================== //
        public static void fn商品圖片更新(CProductPicture productPicture)
        {
            string sql = $"EXEC 商品圖片更新 ";
            sql += $"@{CProductPictureKey.fProductPictureId},";
            sql += $"@{CProductPictureKey.fPicture},";
            sql += $"@{CProductPictureKey.fProductId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductPictureKey.fProductPictureId,productPicture.fProductPictureId),
                new SqlParameter(CProductPictureKey.fPicture, productPicture.fPicture),
                new SqlParameter(CProductPictureKey.fProductId, productPicture.fProductId)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 商品圖片新增 =========================== //
        public static void fn商品圖片新增(CProduct product, CProductPicture productPicture)
        {
            string sql = $"EXEC 商品圖片新增 ";
            sql += $"@{CProductPictureKey.fPicture},";
            sql += $"@{CProductPictureKey.fProductId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductPictureKey.fPicture, productPicture.fPicture),
                new SqlParameter(CProductPictureKey.fProductId, product.fProductId)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 商品圖片查詢(all) =========================== //
        public static List<CProductPicture> fn商品圖片查詢()
        {
            string sql = $"EXEC 商品圖片查詢";
            return (List<CProductPicture>)CDbManager.querySql(sql, null, reader商品圖片查詢);
        }

        private static IList reader商品圖片查詢(SqlDataReader reader)
        {
            List<CProductPicture> lsProductPicture = new List<CProductPicture>();
            while (reader.Read())
            {
                lsProductPicture.Add(new CProductPicture()
                {
                    fProductPictureId = (int)reader[CProductPictureKey.fProductPictureId],
                    fPicture = (string)reader[CProductPictureKey.fPicture],
                    fProductId = (int)reader[CProductPictureKey.fProductId],
                    fName = (string)reader[CProductPictureKey.fName],
                    fDescription = (string)reader[CProductPictureKey.fDescription],
                    fContent = (string)reader[CProductPictureKey.fContent],
                    fPrice = (int)reader[CProductPictureKey.fPrice],
                    fLaunchDate = (DateTime)reader[CProductPictureKey.fLaunchDate],
                    fTheRemovedDate = reader[CProductPictureKey.fTheRemovedDate] as DateTime?,//可NULL
                    fDownloadTimes = (int)reader[CProductPictureKey.fDownloadTimes],
                    fLikeCount = (int)reader[CProductPictureKey.fLikeCount],
                    fMemberSellerId = (int)reader[CProductPictureKey.fMemberSellerId]
                });
            }
            return lsProductPicture;
        }

        //=========================== 商品圖片刪除 =========================== //
        public static void fn商品圖片刪除(CProductPicture productPicture)
        {
            string sql = $"EXEC 商品圖片刪除 ";
            sql += $"@{CProductPictureKey.fProductPictureId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductPictureKey.fProductPictureId, productPicture.fProductPictureId),
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}