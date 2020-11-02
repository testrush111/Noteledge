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
    public class CSettingFactory
    {
        private static IList reader查詢系統設定(SqlDataReader reader) 
        {
            List<CSetting> IsSetting = new List<CSetting>(); //每個會員-系統設定列表
            while (reader.Read())   //讀取每筆系統設定
            {
                IsSetting.Add(new CSetting()  //把系統設定資料加入陣列
                {
                    fSettingId = (int)reader[CSettingKey.fSettingId],
                    fIsDarkMode = (bool)reader[CSettingKey.fIsDarkMode],
                    fFontSize = (string)reader[CSettingKey.fFontSize],
                    fFontType = (string)reader[CSettingKey.fFontType],
                    fIsFullWidth = (bool)reader[CSettingKey.fIsFullWidth],
                    fIsNotice = (bool)reader[CSettingKey.fIsNotice],
                    fMemberId = (int)reader[CSettingKey.fMemberId]
                }); 
            }
            return IsSetting; //回傳
        }


        public static List<CSetting> fn會員系統設定查詢(CMember member) 
        {
            string sql = $"EXEC 系統設定查詢 ";  
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter(CSettingKey.fMemberId, member.fMemberId));

            return (List<CSetting>)CDbManager.querySql(sql, paras, reader查詢系統設定);
        }


        public static void fn會員系統設定新增(CMember member) 
        {
            string sql = $"EXEC 系統設定新增 ";
            sql += $"@{CSettingKey.fIsDarkMode},";
            sql += $"@{CSettingKey.fFontSize},";
            sql += $"@{CSettingKey.fFontType},";
            sql += $"@{CSettingKey.fIsFullWidth},";
            sql += $"@{CSettingKey.fIsNotice},";
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()  //同一個會員不會有兩種系統樣式，系統設定樣式寫死給預設
            {
                new SqlParameter(CSettingKey.fIsDarkMode , "false"),
                new SqlParameter(CSettingKey.fFontSize , "預設"),
                new SqlParameter(CSettingKey.fFontType , "新細明體"),
                new SqlParameter(CSettingKey.fIsFullWidth , "False"),
                new SqlParameter(CSettingKey.fIsNotice , "True"),
                new SqlParameter(CSettingKey.fMemberId ,member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }

        
        public static void fn會員系統設定更新(CSetting setting, CMember member) 
        {
            string sql = $"EXEC 系統設定更新 ";
            sql += $"@{CSettingKey.fIsDarkMode},";
            sql += $"@{CSettingKey.fFontSize},";
            sql += $"@{CSettingKey.fFontType},";
            sql += $"@{CSettingKey.fIsFullWidth},";
            sql += $"@{CSettingKey.fIsNotice},";
            sql += $"@{CMemberKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CSettingKey.fIsDarkMode,setting.fIsDarkMode),
                new SqlParameter(CSettingKey.fFontSize,setting.fFontSize),
                new SqlParameter(CSettingKey.fFontType,setting.fFontType),
                new SqlParameter(CSettingKey.fIsFullWidth,setting.fIsFullWidth),
                new SqlParameter(CSettingKey.fIsNotice,setting.fIsNotice),
                new SqlParameter(CSettingKey.fMemberId,member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}
