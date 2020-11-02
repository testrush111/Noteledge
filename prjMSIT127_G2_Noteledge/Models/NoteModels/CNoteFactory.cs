using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.NoteModels
{
    public class CNoteFactory
    {
        public static void fn私人筆記更新(CNote note)
        {
            string sql = $"EXEC 私人筆記更新 ";
            sql += $"@{CNoteKey.fNoteId},";
            sql += $"@{CNoteKey.fNoteListName},";
            sql += $"@{CNoteKey.fCreateDateTime},";
            sql += $"@{CNoteKey.fEditDateTime},";
            sql += $"@{CNoteKey.fNoteListLevel},";
            sql += $"@{CNoteKey.fIsMyFavourite},";
            sql += $"@{CNoteKey.fIsTrash},";
            sql += $"@{CNoteKey.fHTMLContent},";
            sql += $"@{CNoteKey.fJsonContent},";
            sql += $"@{CNoteKey.fTheShareLink},";
            sql += $"@{CNoteKey.fTheContactPerson},";
            sql += $"@{CNoteKey.fFolderId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteKey.fNoteId, note.fNoteId),
                new SqlParameter(CNoteKey.fNoteListName, note.fNoteListName),
                new SqlParameter(CNoteKey.fCreateDateTime, note.fCreateDateTime),
                new SqlParameter(CNoteKey.fEditDateTime, note.fEditDateTime),
                new SqlParameter(CNoteKey.fNoteListLevel, note.fNoteListLevel),
                new SqlParameter(CNoteKey.fIsMyFavourite, note.fIsMyFavourite),
                new SqlParameter(CNoteKey.fIsTrash, note.fIsTrash),
                new SqlParameter(CNoteKey.fHTMLContent, note.fHTMLContent),
                new SqlParameter(CNoteKey.fJsonContent, note.fJsonContent),
                new SqlParameter(CNoteKey.fTheShareLink, (object)note.fTheShareLink ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fTheContactPerson, (object)note.fTheContactPerson ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fFolderId, note.fFolderId)
            };

            CDbManager.executeSql(sql, paras);
        }
        public static int fn私人筆記新增(CNoteFolder noteFolder, CNote note)
        {
            string sql = $"EXEC 私人筆記新增 ";
            sql += $"@{CNoteKey.fNoteListName},";
            sql += $"@{CNoteKey.fCreateDateTime},";
            sql += $"@{CNoteKey.fEditDateTime},";
            sql += $"@{CNoteKey.fNoteListLevel},";
            sql += $"@{CNoteKey.fIsMyFavourite},";
            sql += $"@{CNoteKey.fIsTrash},";
            sql += $"@{CNoteKey.fHTMLContent},";
            sql += $"@{CNoteKey.fJsonContent},";
            sql += $"@{CNoteKey.fTheShareLink},";
            sql += $"@{CNoteKey.fTheContactPerson},";
            sql += $"@{CNoteKey.fFolderId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteKey.fNoteListName, note.fNoteListName),
                new SqlParameter(CNoteKey.fCreateDateTime, note.fCreateDateTime),
                new SqlParameter(CNoteKey.fEditDateTime, note.fEditDateTime),
                new SqlParameter(CNoteKey.fNoteListLevel, note.fNoteListLevel),
                new SqlParameter(CNoteKey.fIsMyFavourite, note.fIsMyFavourite),
                new SqlParameter(CNoteKey.fIsTrash, note.fIsTrash),
                new SqlParameter(CNoteKey.fHTMLContent, note.fHTMLContent),
                new SqlParameter(CNoteKey.fJsonContent, note.fJsonContent),
                new SqlParameter(CNoteKey.fTheShareLink, (object)note.fTheShareLink ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fTheContactPerson, (object)note.fTheContactPerson ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fFolderId, noteFolder.fFolderId)
            };

            CDbManager.executeSql(sql, paras);

            int id = fn私人筆記查詢(noteFolder).LastOrDefault().fNoteId;

            return id;
        } 
        public static List<CNote> fn私人筆記查詢(CNoteFolder noteFolder)
        {
            string sql = $"EXEC 私人筆記查詢 ";
            sql += $"@{CNoteKey.fFolderId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteKey.fFolderId, noteFolder.fFolderId)
            };

            return (List<CNote>)CDbManager.querySql(sql, paras, reader私人筆記查詢);
        }

        public static void fn私人筆記刪除(CNote note)
        {
            string sql = $"EXEC 私人筆記刪除 ";
            sql += $"@{CNoteKey.fNoteId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteKey.fNoteId, note.fNoteId)
            };

            CDbManager.executeSql(sql, paras);
        }

        private static IList reader私人筆記查詢(SqlDataReader reader)
        {
            List<CNote> lsNote = new List<CNote>();
            while (reader.Read())
            {
                lsNote.Add(new CNote() { 
                    fFolderId = (int)reader[CNoteKey.fFolderId],
                    fCreateDateTime = (DateTime)reader[CNoteKey.fCreateDateTime],
                    fEditDateTime  = (DateTime)reader[CNoteKey.fEditDateTime],
                    fHTMLContent = (string)reader[CNoteKey.fHTMLContent],
                    fIsMyFavourite = (bool)reader[CNoteKey.fIsMyFavourite],
                    fIsTrash = (bool)reader[CNoteKey.fIsTrash],
                    fJsonContent = (string)reader[CNoteKey.fJsonContent],
                    fNoteId = (int)reader[CNoteKey.fNoteId],
                    fNoteListLevel = (int)reader[CNoteKey.fNoteListLevel],
                    fNoteListName = (string)reader[CNoteKey.fNoteListName],
                    fTheContactPerson = reader[CNoteKey.fTheContactPerson] as string,
                    fTheShareLink = reader[CNoteKey.fTheShareLink] as string
                });
            }
            return lsNote;
        }
        public static List<CNote> fn私人筆記全部查詢()
        {
            string sql = $"EXEC 私人筆記全部查詢";

            return (List<CNote>)CDbManager.querySql(sql, null, reader私人筆記全部查詢);
        }

        private static IList reader私人筆記全部查詢(SqlDataReader reader)
        {
            List<CNote> lsNote = new List<CNote>();
            while (reader.Read())
            {
                lsNote.Add(new CNote()
                {
                    fFolderId = (int)reader[CNoteKey.fFolderId],
                    fCreateDateTime = (DateTime)reader[CNoteKey.fCreateDateTime],
                    fEditDateTime = (DateTime)reader[CNoteKey.fEditDateTime],
                    fHTMLContent = (string)reader[CNoteKey.fHTMLContent],
                    fIsMyFavourite = (bool)reader[CNoteKey.fIsMyFavourite],
                    fIsTrash = (bool)reader[CNoteKey.fIsTrash],
                    fJsonContent = (string)reader[CNoteKey.fJsonContent],
                    fNoteId = (int)reader[CNoteKey.fNoteId],
                    fNoteListLevel = (int)reader[CNoteKey.fNoteListLevel],
                    fNoteListName = (string)reader[CNoteKey.fNoteListName],
                    fTheContactPerson = reader[CNoteKey.fTheContactPerson] as string,
                    fTheShareLink = reader[CNoteKey.fTheShareLink] as string
                });
            }
            return lsNote;
        }

        public static void fn訂單私人筆記新增( CNote note)
        {
            string sql = $"EXEC 私人筆記新增 ";
            sql += $"@{CNoteKey.fNoteListName},";
            sql += $"@{CNoteKey.fCreateDateTime},";
            sql += $"@{CNoteKey.fEditDateTime},";
            sql += $"@{CNoteKey.fNoteListLevel},";
            sql += $"@{CNoteKey.fIsMyFavourite},";
            sql += $"@{CNoteKey.fIsTrash},";
            sql += $"@{CNoteKey.fHTMLContent},";
            sql += $"@{CNoteKey.fJsonContent},";
            sql += $"@{CNoteKey.fTheShareLink},";
            sql += $"@{CNoteKey.fTheContactPerson},";
            sql += $"@{CNoteKey.fFolderId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteKey.fNoteListName, note.fNoteListName),
                new SqlParameter(CNoteKey.fCreateDateTime, note.fCreateDateTime),
                new SqlParameter(CNoteKey.fEditDateTime, note.fEditDateTime),
                new SqlParameter(CNoteKey.fNoteListLevel, note.fNoteListLevel),
                new SqlParameter(CNoteKey.fIsMyFavourite, note.fIsMyFavourite),
                new SqlParameter(CNoteKey.fIsTrash, note.fIsTrash),
                new SqlParameter(CNoteKey.fHTMLContent, note.fHTMLContent),
                new SqlParameter(CNoteKey.fJsonContent, note.fJsonContent),
                new SqlParameter(CNoteKey.fTheShareLink, (object)note.fTheShareLink ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fTheContactPerson, (object)note.fTheContactPerson ?? DBNull.Value),//可NULL
                new SqlParameter(CNoteKey.fFolderId, note.fFolderId)
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}
