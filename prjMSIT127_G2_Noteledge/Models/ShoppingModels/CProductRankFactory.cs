using DbManager;
using Models.MemberModels;
using prjMSIT127_G2_Noteledge.Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CProductRankFactory
    {
        private static IList reader評價查詢(SqlDataReader reader)
        {
            List<CProductRank> IsCProductRank = new List<CProductRank>();
            while (reader.Read())
            {
                IsCProductRank.Add(new CProductRank()
                {
                    fProductRankID = (int)reader[CProductRankKey.fProductRankID],
                    fRank = (int)reader[CProductRankKey.fRank],
                    fComment = (string)reader[CProductRankKey.fComment],
                    fSubmitDataTime = (DateTime)reader[CProductRankKey.fSubmitDataTime],
                    fDetailOrderIId = (int)reader[CProductRankKey.fDetailOrderIId],
                    fMemberId = (int)reader[CProductRankKey.fMemberId]
                });
            }
            return IsCProductRank;
        }
        public static List<CProductRank> fn評價查詢(int productid)
        {
            string sql = $"EXEC 評價查詢 @{CDetailOrderKey.fProductId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CDetailOrderKey.fProductId, productid),
            };
            List<CProductRank> IsCProductRank = (List<CProductRank>)CDbManager.querySql(sql, paras, reader評價查詢);
            return IsCProductRank;
        }

        public static void fn評價新增(CProductRank productRank)
        {
            string sql = $"EXEC 評價新增 @{CProductRankKey.fRank},";
            sql += $"@{CProductRankKey.fComment},";
            sql += $"@{CProductRankKey.fSubmitDataTime},";
            sql += $"@{CProductRankKey.fDetailOrderIId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductRankKey.fRank, productRank.fRank),
                new SqlParameter(CProductRankKey.fComment, productRank.fComment),
                new SqlParameter(CProductRankKey.fSubmitDataTime, productRank.fSubmitDataTime),
                new SqlParameter(CProductRankKey.fDetailOrderIId, productRank.fDetailOrderIId)
            };
            CDbManager.executeSql(sql, paras);
        }

        private static IList reader評價個人查詢(SqlDataReader reader)
        {
            List<CProducttotal> IsProducttotal = new List<CProducttotal>();
            while (reader.Read())
            {
                IsProducttotal.Add(new CProducttotal()
                {
                    fMemberId = (int)reader[CProducttotalKey.fMemberId],
                    fDetailOrderIId = (int)reader[CProducttotalKey.fDetailOrderIId],
                    fProductId = (int)reader[CProducttotalKey.fProductId],
                    fName = (string)reader[CProducttotalKey.fName],
                    fDescription = (string)reader[CProducttotalKey.fDescription],
                    fMemberSellerId = (int)reader[CProducttotalKey.fMemberSellerId],
                    fPicture = (string)reader[CProducttotalKey.fPicture],
                    fRank = (int)reader[CProducttotalKey.fRank],
                    fComment = (string)reader[CProducttotalKey.fComment]
                });
            }
            return IsProducttotal;
        }
        public static List<CProducttotal> fn評價個人查詢(int orderid)
        {
            string sql = $"EXEC 評價個人查詢 @{CProducttotalKey.fRank}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProducttotalKey.fRank, orderid),
            };
            List<CProducttotal> IsProducttotal = (List<CProducttotal>)CDbManager.querySql(sql, paras, reader評價個人查詢);
            return IsProducttotal;
        }
    }
}
