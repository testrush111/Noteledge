using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.Models.NoteModels
{
    public class CTemplateFactory
    {
        public static List<CTemplate> fn模板查詢()
        {
            string sql = $"EXEC 模板查詢";

            return (List<CTemplate>)CDbManager.querySql(sql, null, reader模板查詢);
        }

        private static IList reader模板查詢(SqlDataReader reader)
        {
            List<CTemplate> lsTemplate = new List<CTemplate>();
            while (reader.Read())
            {
                lsTemplate.Add(new CTemplate()
                {
                    fContent = (string)reader[CTemplateKey.fContent],
                    fDownloadCount = (int)reader[CTemplateKey.fDownloadCount],
                    fLaunchDateTime = (DateTime)reader[CTemplateKey.fLaunchDateTime],
                    fName = (string)reader[CTemplateKey.fName],
                    fTemplateId = (int)reader[CTemplateKey.fTemplateId],
                    fTheDescription = reader[CTemplateKey.fTheDescription] as string,
                    fTheRemovedDateTime = reader[CTemplateKey.fTheRemovedDateTime] as DateTime?
                });
            }
            return lsTemplate;
        }
    }
}