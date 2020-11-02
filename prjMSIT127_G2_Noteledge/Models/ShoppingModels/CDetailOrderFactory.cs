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
    public class CDetailOrderFactory
    {
        private static IList reader訂單明細查詢(SqlDataReader reader)
        {
            List<CDetailOrder> IsCDetailOrder = new List<CDetailOrder>();
            while (reader.Read())
            {
                IsCDetailOrder.Add(new CDetailOrder()
                {
                    fDetailOrderIId = (int)reader[CDetailOrderKey.fDetailOrderIId],
                    fProductId = (int)reader[CDetailOrderKey.fProductId],
                    fOrderId = (int)reader[CDetailOrderKey.fOrderId],
                    fMemberId = (int)reader[CDetailOrderKey.fMemberId],
                    fName = (string)reader[CDetailOrderKey.fName],
                    fMemberSellerId = (int)reader[CDetailOrderKey.fMemberSellerId]
                });
            }
            return IsCDetailOrder;
        }
        public static List<CDetailOrder> fn訂單明細查詢()
        {
            string sql = $"EXEC 訂單明細查詢";
            List<CDetailOrder> IsCDetailOrder = (List<CDetailOrder>)CDbManager.querySql(sql, null, reader訂單明細查詢);
            return IsCDetailOrder;
        }

        public static void fn訂單明細新增(int orderid,int 產品ID)
        {
            string sql = $"EXEC 訂單明細新增 @{CDetailOrderKey.fProductId},";
            sql += $"@{CDetailOrderKey.fOrderId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CDetailOrderKey.fProductId, 產品ID),
                new SqlParameter(CDetailOrderKey.fOrderId, orderid)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}
