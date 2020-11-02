using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MemberModels
{
    public class CNoticeFactory
    {
        private static IList reader查詢通知訊息(SqlDataReader reader) 
        {
            List<CNotice> IsNotice = new List<CNotice>();
            while (reader.Read()) 
            {
                IsNotice.Add(new CNotice()
                {
                    fNoticeId = (int)reader[CNoticeKey.fNoticeId],
                    fNoticeDatetime = (DateTime)reader[CNoticeKey.fNoticeDatetime],
                    fNoticeContent = (string)reader[CNoticeKey.fNoticeContent],
                    fCategoryType = (string)reader[CNoticeKey.fCategoryType],
                    fLink = (string)reader[CNoticeKey.fLink],
                    fMemberId = (int)reader[CNoticeKey.fMemberId]
                });
            }
            return IsNotice;
        }


        public static List<CNotice> fn通知訊息查詢(CMember member) 
        {
            string sql = $"EXEC 通知訊息查詢 ";
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter(CNoticeKey.fMemberId, member.fMemberId));

            return (List<CNotice>)CDbManager.querySql(sql, paras, reader查詢通知訊息);
        }

        /// <summary>
        /// 此會員新增一筆訊息通知(小鈴鐺)
        /// </summary>
        /// <param name="member">會員</param>
        /// <param name="notice">訊息</param>
        public static void fn通知訊息新增(CMember member  , CNotice notice) 
        {
            string sql = $"EXEC 通知訊息新增 ";
            sql += $"@{CNoticeKey.fNoticeDatetime},";
            sql += $"@{CNoticeKey.fNoticeContent},";
            sql += $"@{CNoticeKey.fCategoryType},";
            sql += $"@{CNoticeKey.fLink},";
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>() 
            {
                new SqlParameter(CNoticeKey.fNoticeDatetime , notice.fNoticeDatetime),
                new SqlParameter(CNoticeKey.fNoticeContent ,notice.fNoticeContent),
                new SqlParameter(CNoticeKey.fCategoryType , notice.fCategoryType),
                new SqlParameter(CNoticeKey.fLink , notice.fLink),
                new SqlParameter(CNoticeKey.fMemberId ,member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }

        //public static void fn通知訊息更新(CMember member, CNotice notice)
        //{
        //    string sql = $"EXEC 通知訊息更新 ";
        //    sql += $"@{CNoticeKey.fNoticeDatetime},";
        //    sql += $"@{CNoticeKey.fNoticeContent},";
        //    sql += $"@{CNoticeKey.fCategoryType},";
        //    sql += $"@{CNoticeKey.fLink},";
        //    sql += $"@{CMemberKey.fMemberId}";

        //    List<SqlParameter> paras = new List<SqlParameter>()
        //    {
        //        new SqlParameter(CNoticeKey.fNoticeDatetime,notice.fNoticeDatetime),
        //        new SqlParameter(CNoticeKey.fNoticeContent,notice.fNoticeContent),
        //        new SqlParameter(CNoticeKey.fCategoryType,notice.fCategoryType),
        //        new SqlParameter(CNoticeKey.fLink,notice.fLink),
        //        new SqlParameter(CNoticeKey.fMemberId,member.fMemberId)
        //    };
        //    CDbManager.executeSql(sql, paras);
        //}

        public static void fn通知訂單訊息新增(CNotice notice)
        {
            string sql = $"EXEC 通知訊息新增 ";
            sql += $"@{CNoticeKey.fNoticeDatetime},";
            sql += $"@{CNoticeKey.fNoticeContent},";
            sql += $"@{CNoticeKey.fCategoryType},";
            sql += $"@{CNoticeKey.fLink},";
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoticeKey.fNoticeDatetime , notice.fNoticeDatetime),
                new SqlParameter(CNoticeKey.fNoticeContent ,notice.fNoticeContent),
                new SqlParameter(CNoticeKey.fCategoryType , notice.fCategoryType),
                new SqlParameter(CNoticeKey.fLink , notice.fLink),
                new SqlParameter(CNoticeKey.fMemberId ,notice.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}
