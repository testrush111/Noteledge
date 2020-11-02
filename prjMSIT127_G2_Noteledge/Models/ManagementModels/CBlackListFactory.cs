using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CBlackListFactory
    {
        private static IList reader黑名單查詢(SqlDataReader reader)
        {
            List<CBlackList> lsBlackList = new List<CBlackList>();//黑名單列表
            while (reader.Read())//讀取每筆黑名單
            {
                lsBlackList.Add(new CBlackList()//加入黑名單
                {
                    fBannedId = (int)reader[CBlackListKey.fBannedId],
                    fReason = (string)reader[CBlackListKey.fReason],
                    fLockDateTime = (DateTime)reader[CBlackListKey.fLockDateTime],
                    fMemberId = (int)reader[CBlackListKey.fMemberId],
                    
                });
            }
            return lsBlackList;//回傳黑名單列表
        }
        public static List<CBlackList> fn黑名單查詢()
        {
            string sql = $"EXEC 黑名單查詢";
            //List<SqlParameter> paras = new List<SqlParameter>()
            //{
            //    new SqlParameter(CBlackListKey.fMemberId, member.fMemberId)
            //};
            List<CBlackList> lsBlackList = (List<CBlackList>)CDbManager.querySql(sql, null, reader黑名單查詢);
            return lsBlackList;
        }
        public static void fn黑名單新增(CBlackList BlackList)
        {
            string sql = $"EXEC 黑名單新增 ";
            sql += $"@{CBlackListKey.fReason},";
            sql += $"@{CBlackListKey.fLockDateTime},";
            sql += $"@{CBlackListKey.fMemberId}";




            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CBlackListKey.fReason, BlackList.fReason),
                new SqlParameter(CBlackListKey.fLockDateTime, BlackList.fLockDateTime),
                new SqlParameter(CBlackListKey.fMemberId, BlackList.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
        }

        public static void fn黑名單更新(CBlackList BlackList)
        {
            string sql = $"EXEC 黑名單更新 ";
            sql += $"@{CBlackListKey.fBannedId},";
            sql += $"@{CBlackListKey.fReason},";
            sql += $"@{CBlackListKey.fLockDateTime}";
            
            
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CBlackListKey.fBannedId, BlackList.fMemberId),
                new SqlParameter(CBlackListKey.fReason, BlackList.fReason),
                new SqlParameter(CBlackListKey.fLockDateTime, BlackList.fLockDateTime)               
               
            };

            CDbManager.executeSql(sql, paras);
        }
        public static void fn黑名單刪除(CBlackList blackList)
        {
            string sql = $"EXEC 黑名單刪除 ";
            sql += $"@{CBlackListKey.fBannedId}";
            //sql += $"@{CBlackListKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CBlackListKey.fBannedId, blackList.fBannedId)
                //new SqlParameter(CBlackListKey.fMemberId, member.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}
