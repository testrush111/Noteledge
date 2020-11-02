using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class CDbManager
    {
        
        private static string  _connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;       
        public delegate IList DataReader(SqlDataReader reader);

        /// <summary>
        /// 執行SQL語句
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="paras">變數參數</param>
        public static void executeSql(string sql, List<SqlParameter> paras)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Console.WriteLine("[Info]成功連接資料庫！");

                SqlCommand command = new SqlCommand(sql, connection);
                //Console.WriteLine("[Info]執行的SQL語句為：" + sql);

                if (paras != null)
                {
                    foreach (SqlParameter p in paras)
                    {
                        command.Parameters.Add(p);
                        //Console.WriteLine($"[Info]成功將[{p.ParameterName}]:[{p.Value}]SQL語句參數化！");
                    }
                }
                
                command.ExecuteNonQuery();
               // Console.WriteLine("[Info]執行SQL成功！");
            }
        }

        public static IList querySql(string sql, List<SqlParameter> paras, DataReader dr)
        {
            IList lsResult;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //Console.WriteLine("[Info]成功連接資料庫！");

                SqlCommand command = new SqlCommand(sql, connection);
                //Console.WriteLine("[Info]查詢的SQL語句為：" + sql);

                if (paras != null)
                {
                    foreach (SqlParameter p in paras)
                    {
                        command.Parameters.Add(p);
                        //Console.WriteLine("[Info]成功將SQL語句參數化！");
                    }
                }
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    lsResult = dr(reader);
                    //Console.WriteLine("[Info]查詢SQL成功！"); 
                }
            }
            return lsResult;
        }
    }
}
