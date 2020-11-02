using DbManager;
using Models.ShoppingModels;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MemberModels
{
    public class CMemberFactory
    {
        private static IList reader查詢會員(SqlDataReader reader)
        {
            List<CMember> lsMember = new List<CMember>();//會員列表
            while (reader.Read())//讀取每筆會員
            {
                lsMember.Add(new CMember()//加入會員
                {
                    fMemberId = (int)reader[CMemberKey.fMemberId],
                    fAccount = (string)reader[CMemberKey.fAccount],
                    fTheAddress = reader[CMemberKey.fTheAddress] as string,//可NULL
                    fBirthDay = (DateTime)reader[CMemberKey.fBirthDay],
                    fFirstName = (string)reader[CMemberKey.fFirstName],
                    fGender = (string)reader[CMemberKey.fGender],
                    fIsBanned = (bool)reader[CMemberKey.fIsBanned],
                    fIsVIP = (bool)reader[CMemberKey.fIsVIP],
                    fLastLoginDateTime = (DateTime)reader[CMemberKey.fLastLoginDateTime],
                    fLastName = (string)reader[CMemberKey.fLastName],
                    fMobilePhone = (string)reader[CMemberKey.fMobilePhone],
                    fMoneyPoint = (int)reader[CMemberKey.fMoneyPoint],
                    fTheNickName = reader[CMemberKey.fTheNickName] as string,//可NULL
                    fPassword = (string)reader[CMemberKey.fPassword],
                    fThePasswordURL = reader[CMemberKey.fThePasswordURL] as string,//可NULL
                    fPhoto = (string)reader[CMemberKey.fPhoto],
                    fRegisterDateTime = (DateTime)reader[CMemberKey.fRegisterDateTime]
                });
            }
            return lsMember;//回傳會員列表
        }
        public static List<CMember> fn會員查詢()
        {
            string sql = $"EXEC 會員查詢";
            return (List<CMember>)CDbManager.querySql(sql, null, reader查詢會員);
        }
        public static void fn會員新增(CMember member)
        {
            string sql = $"EXEC 會員新增 ";
            sql += $"@{CMemberKey.fAccount},";
            sql += $"@{CMemberKey.fPassword},";
            sql += $"@{CMemberKey.fFirstName},";
            sql += $"@{CMemberKey.fLastName},";
            sql += $"@{CMemberKey.fTheNickName},";
            sql += $"@{CMemberKey.fGender},";
            sql += $"@{CMemberKey.fBirthDay},";
            sql += $"@{CMemberKey.fTheAddress},";
            sql += $"@{CMemberKey.fMobilePhone},";
            sql += $"@{CMemberKey.fMoneyPoint},";
            sql += $"@{CMemberKey.fPhoto},";
            sql += $"@{CMemberKey.fRegisterDateTime},";
            sql += $"@{CMemberKey.fLastLoginDateTime},";
            sql += $"@{CMemberKey.fIsVIP},";
            sql += $"@{CMemberKey.fIsBanned},";
            sql += $"@{CMemberKey.fThePasswordURL}";
            //https://stackoverflow.com/questions/863374/sqlparameter-with-nullable-value-give-error-while-executenonquery
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CMemberKey.fAccount, member.fAccount),
                new SqlParameter(CMemberKey.fPassword, member.fPassword),
                new SqlParameter(CMemberKey.fFirstName, member.fFirstName),
                new SqlParameter(CMemberKey.fLastName, member.fLastName),
                new SqlParameter(CMemberKey.fTheNickName, (object)member.fTheNickName ?? DBNull.Value),//可NULL
                new SqlParameter(CMemberKey.fGender, member.fGender),
                new SqlParameter(CMemberKey.fBirthDay, member.fBirthDay),
                new SqlParameter(CMemberKey.fTheAddress, (object)member.fTheAddress ?? DBNull.Value),//可NULL
                new SqlParameter(CMemberKey.fMobilePhone, member.fMobilePhone),
                new SqlParameter(CMemberKey.fMoneyPoint, member.fMoneyPoint),
                new SqlParameter(CMemberKey.fPhoto, member.fPhoto),
                new SqlParameter(CMemberKey.fRegisterDateTime, member.fRegisterDateTime),
                new SqlParameter(CMemberKey.fLastLoginDateTime, member.fLastLoginDateTime),
                new SqlParameter(CMemberKey.fIsVIP, member.fIsVIP),
                new SqlParameter(CMemberKey.fIsBanned, member.fIsBanned),
                new SqlParameter(CMemberKey.fThePasswordURL, (object)member.fThePasswordURL ?? DBNull.Value)//可NULL
            };

            CDbManager.executeSql(sql, paras);
        }

        public static void fn會員更新(CMember member)
        {
            string sql = $"EXEC 會員更新 ";
            sql += $"@{CMemberKey.fMemberId},";
            sql += $"@{CMemberKey.fAccount},";
            sql += $"@{CMemberKey.fPassword},";
            sql += $"@{CMemberKey.fFirstName},";
            sql += $"@{CMemberKey.fLastName},";
            sql += $"@{CMemberKey.fTheNickName},";
            sql += $"@{CMemberKey.fGender},";
            sql += $"@{CMemberKey.fBirthDay},";
            sql += $"@{CMemberKey.fTheAddress},";
            sql += $"@{CMemberKey.fMobilePhone},";
            sql += $"@{CMemberKey.fMoneyPoint},";
            sql += $"@{CMemberKey.fPhoto},";
            sql += $"@{CMemberKey.fRegisterDateTime},";
            sql += $"@{CMemberKey.fLastLoginDateTime},";
            sql += $"@{CMemberKey.fIsVIP},";
            sql += $"@{CMemberKey.fIsBanned},";
            sql += $"@{CMemberKey.fThePasswordURL}";
            //https://stackoverflow.com/questions/863374/sqlparameter-with-nullable-value-give-error-while-executenonquery
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CMemberKey.fMemberId, member.fMemberId),
                new SqlParameter(CMemberKey.fAccount, member.fAccount),
                new SqlParameter(CMemberKey.fPassword, member.fPassword),
                new SqlParameter(CMemberKey.fFirstName, member.fFirstName),
                new SqlParameter(CMemberKey.fLastName, member.fLastName),
                //fTheNickName可為NULL
                new SqlParameter(CMemberKey.fTheNickName, (object)member.fTheNickName ?? DBNull.Value),
                new SqlParameter(CMemberKey.fGender, member.fGender),
                new SqlParameter(CMemberKey.fBirthDay, member.fBirthDay),
                //fTheAddress可為NULL
                new SqlParameter(CMemberKey.fTheAddress, (object)member.fTheAddress ?? DBNull.Value),
                new SqlParameter(CMemberKey.fMobilePhone, member.fMobilePhone),
                new SqlParameter(CMemberKey.fMoneyPoint, member.fMoneyPoint),
                new SqlParameter(CMemberKey.fPhoto, member.fPhoto),
                new SqlParameter(CMemberKey.fRegisterDateTime, member.fRegisterDateTime),
                new SqlParameter(CMemberKey.fLastLoginDateTime, member.fLastLoginDateTime),
                new SqlParameter(CMemberKey.fIsVIP, member.fIsVIP),
                new SqlParameter(CMemberKey.fIsBanned, member.fIsBanned),
                //fThePasswordURL可為NULL
                new SqlParameter(CMemberKey.fThePasswordURL, (object)member.fThePasswordURL ?? DBNull.Value)
            };

            CDbManager.executeSql(sql, paras);
        }

        public static CMember fn會員登入驗證(string act, string pwd)
        {
            //檢查帳號密碼是否正確
            CMember member = fn會員查詢().FirstOrDefault(m => act == m.fAccount && pwd == m.fPassword);

            if (member != null)
            {
                Console.WriteLine("登入成功！");
                Console.WriteLine("客戶ID：" + member.fMemberId);
                Console.WriteLine("客戶姓名：" + member.fFirstName + member.fLastName);
                Console.WriteLine("帳號：" + member.fAccount);
                Console.WriteLine("-----------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("帳號或密碼錯誤！");
            }
            return member;
        }
        public static void fn普通會員升級VIP會員(CMember member)
        {
            Console.WriteLine(member.fLastName + "升級VIP......................");

            member.fIsVIP = true;
            CMemberFactory.fn會員更新(member);
        }
        public static void fnVIP會員降為普通會員(CMember member)
        {
            Console.WriteLine(member.fLastName + "取消VIP......................");

            member.fIsVIP = false;
            CMemberFactory.fn會員更新(member);
        }

        public static string MD5驗證碼新增(string str)
        {
            using (var cryptoMD5 = System.Security.Cryptography.MD5.Create())
            {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(str);

                //取得雜湊值位元組陣列
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得 MD5
                var md5 = BitConverter.ToString(hash)
                  .Replace("-", String.Empty)
                  .ToUpper();

                return md5;
            }
        }

        private static IList reader會員訂單個人查詢(SqlDataReader reader)
        {
            List<CMemberOrderSelectVM> IsMemberOrders = new List<CMemberOrderSelectVM>();
            while (reader.Read())
            {
                IsMemberOrders.Add(new CMemberOrderSelectVM()
                {
                    fMemberId = (int)reader[CMemberOrderSelectVMKey.fMemberId],
                    fOrderId = (int)reader[CMemberOrderSelectVMKey.fOrderId],
                    fProductId = (int)reader[CMemberOrderSelectVMKey.fProductId],
                    fDetailOrderIId = (int)reader[CMemberOrderSelectVMKey.fDetailOrderIId],
                    fLaunchDate = (DateTime)reader[CMemberOrderSelectVMKey.fLaunchDate],
                    fName = (string)reader[CMemberOrderSelectVMKey.fName],
                    fPrice = (int)reader[CMemberOrderSelectVMKey.fPrice],
                    fDescription = (string)reader[CMemberOrderSelectVMKey.fDescription],
                    fPhoto = (string)reader[CMemberOrderSelectVMKey.fPhoto]
                });
            }
            return IsMemberOrders;
        }

        public static List<CMemberOrderSelectVM> fn會員訂單個人查詢(CMember m)
        {
            string sql = $"EXEC 訂單明細個人查詢 ";
            sql += $"@{CMemberOrderSelectVMKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter(CMemberOrderSelectVMKey.fMemberId, m.fMemberId));
            return (List<CMemberOrderSelectVM>)CDbManager.querySql(sql, paras, reader會員訂單個人查詢);
        }

        public static void fn會員更新點數(CMember member,int point)
        {
            string sql = $"EXEC 會員更新點數 @{CMemberKey.fMemberId},@{CMemberKey.fMoneyPoint}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CMemberKey.fMemberId, member.fMemberId),
                new SqlParameter(CMemberKey.fMoneyPoint, point)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}
