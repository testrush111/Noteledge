using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class COrderFactory
    {
        private static IList reader訂單查詢(SqlDataReader reader)
        {
            List<COrder> IsCOrder = new List<COrder>();
            while (reader.Read())
            {
                IsCOrder.Add(new COrder()
                {
                    fOrderId = (int)reader[COrderKey.fOrderId],
                    fPurchaseDate = (DateTime)reader[COrderKey.fPurchaseDate],
                    fTotalPrice = (int)reader[COrderKey.fTotalPrice],
                    fMemberId = (int)reader[COrderKey.fMemberId]
                });
            }
            return IsCOrder;
        }
        public static List<COrder> fn訂單查詢(CMember member)
        {
            string sql = $"EXEC 訂單查詢 @{COrderKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(COrderKey.fMemberId, member.fMemberId)
            };
            List<COrder> IsCOrder = (List<COrder>)CDbManager.querySql(sql, paras, reader訂單查詢);
            return IsCOrder;
        }

        public static void fn訂單新增(COrder order)
        {
            string sql = $"EXEC 訂單新增 @{COrderKey.fPurchaseDate},";
            sql += $"@{COrderKey.fTotalPrice},";
            sql += $"@{COrderKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(COrderKey.fPurchaseDate, order.fPurchaseDate),
                new SqlParameter(COrderKey.fTotalPrice, order.fTotalPrice),
                new SqlParameter(COrderKey.fMemberId, order.fMemberId),
            };
            CDbManager.executeSql(sql, paras);
        }
       public static int fn訂單總金額()
        {
            List<CMember> lsmember = CMemberFactory.fn會員查詢().ToList();
            List<int> lsprice = new List<int>();
            foreach(var m in lsmember)
            {
                int price = COrderFactory.fn訂單查詢(m).Sum(p=>p.fTotalPrice);
                lsprice.Add(price);
            }
            int sum = lsprice.Sum();
            return sum;
        }
        public static int fn訂單商品累積量()
        {
            List<CMember> lsmember = CMemberFactory.fn會員查詢().ToList();
            List<int> lsordertotal = new List<int>();

            foreach (var m in lsmember)
            {
                int ordertotal = COrderFactory.fn訂單查詢(m).Count();
                lsordertotal.Add(ordertotal);
            }
            int total = lsordertotal.Sum();
            return total;

        }

        
    }
}
