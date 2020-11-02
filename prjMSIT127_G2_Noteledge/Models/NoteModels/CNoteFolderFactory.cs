using DbManager;
using Models.MemberModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.NoteModels
{
    public class CNoteFolderFactory
    {
        private static IList reader筆記資料夾查詢(SqlDataReader reader)
        {
            List<CNoteFolder> lsNoteFolder = new List<CNoteFolder>();
            while (reader.Read())
            {
                lsNoteFolder.Add(new CNoteFolder()
                {
                    fFolderId = (int)reader[CNoteFolderKey.fFolderId],
                    fMemberId = (int)reader[CNoteFolderKey.fMemberId],
                    fFolderName = (string)reader[CNoteFolderKey.fFolderName]
                });
            }
            return lsNoteFolder;
        }
        public static List<CNoteFolder> fn筆記資料夾查詢(CMember member)
        {
            string sql = $"EXEC 筆記資料夾查詢 @{CNoteFolderKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteFolderKey.fMemberId,member.fMemberId)
            };
            List<CNoteFolder> lsNoteFolder = (List<CNoteFolder>)CDbManager.querySql(sql, paras, reader筆記資料夾查詢);
            return lsNoteFolder;
        }

        public static int fn筆記資料夾新增(CMember member, CNoteFolder noteFolder)
        {
            string sql = $"EXEC 筆記資料夾新增 @{CNoteFolderKey.fMemberId}, @{CNoteFolderKey.fFolderName}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteFolderKey.fMemberId, member.fMemberId),
                new SqlParameter(CNoteFolderKey.fFolderName, noteFolder.fFolderName)
            };
            CDbManager.executeSql(sql, paras);
            int folderid = fn筆記資料夾查詢(member).LastOrDefault().fFolderId;
            return folderid;
        }

        public static void fn筆記資料夾更新(CNoteFolder noteFolder)
        {
            string sql = $"EXEC 筆記資料夾更新 @{CNoteFolderKey.fFolderId}, @{ CNoteFolderKey.fFolderName}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteFolderKey.fFolderId, noteFolder.fFolderId),
                new SqlParameter(CNoteFolderKey.fFolderName, noteFolder.fFolderName)
            };
            CDbManager.executeSql(sql, paras);
        }
        
        public static void fn建立預設筆記資料夾(CMember member)
        {
            if(fn筆記資料夾查詢(member).FirstOrDefault(f=>f.fFolderName== "未分類筆記") == null)
            {
                fn筆記資料夾新增(member, new CNoteFolder()
                {
                    fFolderName = "未分類筆記",
                    fMemberId = member.fMemberId
                });
            }
        }
        public static void fn筆記資料夾刪除(CNoteFolder folder, CMember member)
        {
            string sql = $"EXEC 筆記資料夾刪除 ";
            sql += $"@{CNoteFolderKey.fFolderId},";
            sql += $"@{CNoteFolderKey.fMemberId}";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CNoteFolderKey.fFolderId, folder.fFolderId),
                new SqlParameter(CNoteFolderKey.fMemberId, member.fMemberId)
            };

            CDbManager.executeSql(sql, paras);
        }
    }
}
