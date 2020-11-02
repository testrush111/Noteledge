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
    public class CCartFactory
    {
        //=========================== 購物車更新 =========================== //
        public static void fn購物車更新(CCart cart)
        {
            string sql = $"EXEC 購物車更新 ";
            sql += $"@{CCartKey.fCartId},";
            sql += $"@{CCartKey.fSubmitTime},";
            sql += $"@{CCartKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartKey.fCartId,cart.fCartId),
                new SqlParameter(CCartKey.fSubmitTime, (object)cart.fSubmitTime ?? DBNull.Value),//可NULL
                new SqlParameter(CCartKey.fMemberId, cart.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 購物車新增 =========================== //
        public static CCart fn購物車新增(CMember member, CCart cart)
        {
            string sql = $"EXEC 購物車新增 ";
            sql += $"@{CCartKey.fSubmitTime},";
            sql += $"@{CCartKey.fMemberId}";
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartKey.fSubmitTime, (object)cart.fSubmitTime ?? DBNull.Value), //可NULL
                new SqlParameter(CCartKey.fMemberId, member.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
            CCart NewCart = fn購物車查詢(member).LastOrDefault();
            return NewCart;
        }

        //=========================== 購物車查詢(all) =========================== //
        public static List<CCart> fn購物車查詢(CMember member)
        {
            string sql = $"EXEC 購物車查詢 ";
            sql += $"@{CCartKey.fMemberId}";
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartKey.fMemberId, member.fMemberId)
            };
            
            return (List<CCart>)CDbManager.querySql(sql, paras, reader購物車查詢);
        }

        private static IList reader購物車查詢(SqlDataReader reader)
        {
            List<CCart> lsCart = new List<CCart>();
            while (reader.Read())
            {
                lsCart.Add(new CCart()
                {
                    fCartId = (int)reader[CCartKey.fCartId],
                    fSubmitTime = reader[CCartKey.fSubmitTime] as DateTime?,//可NULL,
                    fMemberId = (int)reader[CCartKey.fMemberId]
                    
                });
            }
            return lsCart;
        }

        //=========================== 購物車個人更新 =========================== //
        public static void fn購物車個人更新(int cartid)
        {
            string sql = $"EXEC 購物車個人更新 @{CCartKey.fCartId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCartKey.fCartId,cartid),
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}
