using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CProductCategoryFactory
    {
        //=========================== 商品類別新增 =========================== //
        public static void fn商品類別新增(CProductCategory productCategory)
        {
            string sql = $"EXEC 商品類別新增 ";
            sql += $"@{CProductCategoryKey.fCategoryName}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CProductCategoryKey.fCategoryId, productCategory.fCategoryId),
                new SqlParameter(CProductCategoryKey.fCategoryName, productCategory.fCategoryName)
            };

            CDbManager.executeSql(sql, paras);
        }

        //=========================== 商品類別查詢(all) =========================== //
        public static List<CProductCategory> fn商品類別查詢()
        {
            string sql = $"EXEC 商品類別查詢";
            return (List<CProductCategory>)CDbManager.querySql(sql, null, reader商品類別查詢);
        }
        private static IList reader商品類別查詢(SqlDataReader reader)
        {
            List<CProductCategory> lsProductCategory = new List<CProductCategory>();
            while (reader.Read())
            {
                lsProductCategory.Add(new CProductCategory()
                {
                    fCategoryId = (int)reader[CProductCategoryKey.fCategoryId],
                    fCategoryName = (string)reader[CProductCategoryKey.fCategoryName]
                });
            }
            return lsProductCategory;
        }
    }
}
