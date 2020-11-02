using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CAdminFactory
    {
        private static IList reader管理員查詢(SqlDataReader reader)
        {
            List<CAdmin> lsAdmin = new List<CAdmin>();//管理員列表
            while (reader.Read())//讀取管理員資料
            {
                lsAdmin.Add(new CAdmin()//加入管理員
                {
                    fAdminId = (int)reader[CAdminKey.fAdminId],
                    fAdminAccount = (string)reader[CAdminKey.fAdminAccount],
                    fAdminPassword = (string)reader[CAdminKey.fAdminPassword] as string,
                    fName = (string)reader[CAdminKey.fName],
                    fGender = (string)reader[CAdminKey.fGender],
                    fBirthDay = (DateTime)reader[CAdminKey.fBirthDay],
                    fTheAddress = reader[CAdminKey.fTheAddress] as string ,//可NULL
                    fMobilePhone = (string)reader[CAdminKey.fMobilePhone],
                    fThePhoto = reader[CAdminKey.fThePhoto]as string,//可NULL
                    fHireDateTime = (DateTime)reader[CAdminKey.fHireDateTime],
                    fLastLoginDateTime = (DateTime)reader[CAdminKey.fLastLoginDateTime]                   
                });
            }
            return lsAdmin;//回傳會員列表
        }
        public static List<CAdmin> fn管理員查詢()
        {
            string sql = $"EXEC 管理員查詢";
            return (List<CAdmin>)CDbManager.querySql(sql, null, reader管理員查詢);
        }
        public static void fn管理員新增(CAdmin admin)
        {
            string sql = $"EXEC 管理員新增 ";
            sql += $"@{CAdminKey.fAdminAccount},";
            sql += $"@{CAdminKey.fAdminPassword},";
            sql += $"@{CAdminKey.fName},";
            sql += $"@{CAdminKey.fGender},";
            sql += $"@{CAdminKey.fBirthDay},";            
            sql += $"@{CAdminKey.fTheAddress},";
            sql += $"@{CAdminKey.fMobilePhone},";
            sql += $"@{CAdminKey.fThePhoto},";
            sql += $"@{CAdminKey.fHireDateTime},";
            sql += $"@{CAdminKey.fLastLoginDateTime}";
            
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CAdminKey.fAdminAccount, admin.fAdminAccount),
                new SqlParameter(CAdminKey.fAdminPassword, admin.fAdminPassword),
                new SqlParameter(CAdminKey.fName, admin.fName),
                new SqlParameter(CAdminKey.fGender, admin.fGender),
                new SqlParameter(CAdminKey.fBirthDay, admin.fBirthDay),
                new SqlParameter(CAdminKey.fTheAddress, (object)admin.fTheAddress ?? DBNull.Value),//可NULL
                new SqlParameter(CAdminKey.fMobilePhone, admin.fMobilePhone),
                new SqlParameter(CAdminKey.fThePhoto, (object)admin.fThePhoto ?? DBNull.Value),//可NULL
                new SqlParameter(CAdminKey.fHireDateTime, admin.fHireDateTime),
                new SqlParameter(CAdminKey.fLastLoginDateTime, admin.fLastLoginDateTime)
                
               
               
            };

            CDbManager.executeSql(sql, paras);
        }

        public static void 管理員更新(CAdmin admin)
        {
            string sql = $"EXEC 管理員更新 ";
            sql += $"@{CAdminKey.fAdminId},";
            sql += $"@{CAdminKey.fAdminAccount},";
            sql += $"@{CAdminKey.fAdminPassword},";
            sql += $"@{CAdminKey.fName},";
            sql += $"@{CAdminKey.fGender},";
            sql += $"@{CAdminKey.fBirthDay},";
            sql += $"@{CAdminKey.fTheAddress},";
            sql += $"@{CAdminKey.fMobilePhone},";
            sql += $"@{CAdminKey.fThePhoto},";
            sql += $"@{CAdminKey.fHireDateTime},";
            sql += $"@{CAdminKey.fLastLoginDateTime}";
            
           
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CAdminKey.fAdminId, admin.fAdminId),
                new SqlParameter(CAdminKey.fAdminAccount, admin.fAdminAccount),
                new SqlParameter(CAdminKey.fAdminPassword, admin.fAdminPassword),
                new SqlParameter(CAdminKey.fName, admin.fName),
                new SqlParameter(CAdminKey.fGender, admin.fGender),                
                new SqlParameter(CAdminKey.fBirthDay,admin.fBirthDay),
                new SqlParameter(CAdminKey.fTheAddress,(object)admin.fTheAddress ?? DBNull.Value),//可NULL
                new SqlParameter(CAdminKey.fMobilePhone, admin.fMobilePhone),                
                new SqlParameter(CAdminKey.fThePhoto, (object)admin.fThePhoto ?? DBNull.Value),//可NULL
                new SqlParameter(CAdminKey.fHireDateTime, admin.fHireDateTime),
                new SqlParameter(CAdminKey.fLastLoginDateTime, admin.fLastLoginDateTime)
                
                
            };

            CDbManager.executeSql(sql, paras);
        }
        public static CAdmin fn管理員登入驗證(string act, string pwd)
        {
            //檢查帳號密碼是否正確
            CAdmin admin = fn管理員查詢().FirstOrDefault(m => act == m.fAdminAccount && pwd == m.fAdminPassword);

            if (admin != null)
            {
                Console.WriteLine("登入成功！");
                Console.WriteLine("客戶ID：" + admin.fAdminId);
                Console.WriteLine("客戶姓名：" + admin.fName);
                Console.WriteLine("帳號：" + admin.fAdminAccount);
                Console.WriteLine("-----------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("帳號或密碼錯誤！");
            }
            return admin;
        }
    }
}
