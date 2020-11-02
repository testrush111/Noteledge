using DbManager;
using Models.MemberModels;
using Models.ManagementModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT127_G2_Noteledge.Models.ManagementModels
{
    public class CMemberBrowseFactory
    {
        private static IList reader會員瀏覽紀錄查詢(SqlDataReader reader)
        {
            List<CMemberBrowse> lsMemberBrowse = new List<CMemberBrowse>();
            while (reader.Read())
            {
                lsMemberBrowse.Add(new CMemberBrowse()
                {
                    fMemberBrowseId = (int)reader[CMemberBrowseKey.fMemberBrowseId],
                    fMemberId = (int)reader[CMemberBrowseKey.fMemberId],
                    fBrowseDataTime = (DateTime)reader[CMemberBrowseKey.fBrowseDataTime]
                });
            }
            return lsMemberBrowse;
        }
        public static List<CMemberBrowse> fn會員瀏覽紀錄查詢()
        {
            string sql = $"EXEC 會員瀏覽紀錄查詢";
            //List<SqlParameter> paras = new List<SqlParameter>()
            //{
            //    new SqlParameter(CMemberBrows
            //    eKey.fMemberId, member.fMemberId)
            //};
            //List<CMemberBrowse> lsMemberBrowse = (List<CMemberBrowse>)CDbManager.querySql(sql, paras, reader會員瀏覽紀錄查詢);
            return (List<CMemberBrowse>)CDbManager.querySql(sql, null, reader會員瀏覽紀錄查詢); ;
        }

        public static void fn會員瀏覽紀錄新增(CMember member)
        {
            string sql = $"EXEC 會員瀏覽紀錄新增 @{CMemberBrowseKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CMemberBrowseKey.fMemberId, member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}