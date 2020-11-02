using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CIncomeFactory
    {
        private static IList reader公司收入查詢(SqlDataReader reader)
        {
            List<CIncome> lsIncome = new List<CIncome>();
            while (reader.Read())
            {
                lsIncome.Add(new CIncome()
                {
                    fIncomeId = (int)reader[CIncomeKey.fIncomeId],
                    fIncome = (int)reader[CIncomeKey.fIncome],
                    fPaymentDateTime = (DateTime)reader[CIncomeKey.fPaymentDateTime],
                    fIncomeCategory = (string)reader[CIncomeKey.fIncomeCategory],
                    fMemberId = (int)reader[CIncomeKey.fMemberId]
                });
            }
            return lsIncome;
        }
        public static List<CIncome> fn公司收入查詢()
        {
            string sql = $"EXEC 公司收入查詢";
            //List<SqlParameter> paras = new List<SqlParameter>()
            //{
            //    new SqlParameter(CIncomeKey.fMemberId, member.fMemberId)
            //};
            /*List<CIncome> lsIncome =*/ return(List<CIncome>)CDbManager.querySql(sql, null, reader公司收入查詢);
            //return lsIncome;
        }

        public static void fn公司收入新增(CMember member, CIncome Income)
        {
            string sql = $"EXEC 公司收入新增 @{CIncomeKey.fIncome},@{ CIncomeKey.fPaymentDateTime},@{ CIncomeKey.fIncomeCategory},@{CIncomeKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CIncomeKey.fIncome, Income.fIncome),
                new SqlParameter(CIncomeKey.fPaymentDateTime, Income.fPaymentDateTime),
                new SqlParameter(CIncomeKey.fIncomeCategory, Income.fIncomeCategory),                
                new SqlParameter(CIncomeKey.fMemberId, member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }

        public static void fn公司收入更新(CIncome Income)
        {
            string sql = $"EXEC 公司收入更新 @{CIncomeKey.fIncomeId},@{ CIncomeKey.fIncome},@{CIncomeKey.fPaymentDateTime},@{CIncomeKey.fIncomeCategory}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CIncomeKey.fIncomeId, Income.fIncomeId),
                new SqlParameter(CIncomeKey.fIncome, Income.fIncome),
                new SqlParameter(CIncomeKey.fPaymentDateTime, Income.fPaymentDateTime),
                new SqlParameter(CIncomeKey.fIncomeCategory, Income.fIncomeCategory)
            };
            CDbManager.executeSql(sql, paras);
        }

        public static void fn公司獲利新增(CIncome Income)
        {
            string sql = $"EXEC 公司獲利新增 @{CIncomeKey.fIncome},@{ CIncomeKey.fPaymentDateTime},@{ CIncomeKey.fIncomeCategory},@{CIncomeKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CIncomeKey.fIncome, Income.fIncome),
                new SqlParameter(CIncomeKey.fPaymentDateTime, Income.fPaymentDateTime),
                new SqlParameter(CIncomeKey.fIncomeCategory, Income.fIncomeCategory),
                new SqlParameter(CIncomeKey.fMemberId, Income.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}