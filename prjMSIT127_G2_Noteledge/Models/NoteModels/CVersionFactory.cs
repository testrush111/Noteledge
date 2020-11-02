using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.NoteModels
{
    public class CVersionFactory
    {
        public static void fn筆記版本控制新增(CNote note)
        {
            string sql = $"EXEC 筆記版本控制新增 ";
            sql += $"@{CVersionKey.fEditDateTime},";
            sql += $"@{CVersionKey.fHTMLContent},";
            sql += $"@{CVersionKey.fJsonContent},";
            sql += $"@{CVersionKey.fNoteId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CVersionKey.fEditDateTime, note.fEditDateTime),
                new SqlParameter(CVersionKey.fHTMLContent, note.fHTMLContent),
                new SqlParameter(CVersionKey.fJsonContent, note.fJsonContent),
                new SqlParameter(CVersionKey.fNoteId, note.fNoteId)
            };

            CDbManager.executeSql(sql, paras);
        }
        public static List<CVersion> fn筆記版本控制查詢用VersionID查(int versionId)
        {
            string sql = $"EXEC 筆記版本控制查詢用VersionID查 ";
            sql += $"@{CVersionKey.fVersionId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CVersionKey.fVersionId, versionId)
            };
            return (List<CVersion>)CDbManager.querySql(sql, paras, reader筆記版本控制查詢用VersionID查);
        }

        private static IList reader筆記版本控制查詢用VersionID查(SqlDataReader reader)
        {
            List<CVersion> lsVersion = new List<CVersion>();

            while (reader.Read())
            {
                lsVersion.Add(
                    new CVersion()
                    {
                        fEditDateTime = (DateTime)reader[CVersionKey.fEditDateTime],
                        fHTMLContent = (string)reader[CVersionKey.fHTMLContent],
                        fJsonContent = (string)reader[CVersionKey.fJsonContent],
                        fNoteId = (int)reader[CVersionKey.fNoteId],
                        fVersionId = (int)reader[CVersionKey.fVersionId]
                    });
            }

            return lsVersion;
        }

        public static List<CVersion> fn筆記版本控制查詢(CNote note)
        {
            string sql = $"EXEC 筆記版本控制查詢 ";
            sql += $"@{CVersionKey.fNoteId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CVersionKey.fNoteId, note.fNoteId)
            };
            return (List<CVersion>)CDbManager.querySql(sql, paras, reader筆記版本控制查詢);
        }

        private static IList reader筆記版本控制查詢(SqlDataReader reader)
        {
            List<CVersion> lsVersion = new List<CVersion>();

            while (reader.Read())
            {
                lsVersion.Add(
                    new CVersion(){ 
                        fEditDateTime = (DateTime)reader[CVersionKey.fEditDateTime],
                        fHTMLContent = (string)reader[CVersionKey.fHTMLContent],
                        fJsonContent = (string)reader[CVersionKey.fJsonContent],
                        fNoteId = (int)reader[CVersionKey.fNoteId],
                        fVersionId = (int)reader[CVersionKey.fVersionId]
                    });
            }

            return lsVersion;
        }
    }
}
