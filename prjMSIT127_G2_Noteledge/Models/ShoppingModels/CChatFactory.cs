using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.MemberModels;

namespace Models.ShoppingModels
{
    public class CChatFactory
    {
        private static IList reader聊聊查詢(SqlDataReader reader)
        {
            List<CChat> IsChat = new List<CChat>();
            while (reader.Read())
            {
                IsChat.Add(new CChat()
                {
                    fChatId = (int)reader[CChatKey.fChatId],
                    fMessage = (string)reader[CChatKey.fMessage],
                    fIsRead = (bool)reader[CChatKey.fIsRead],
                    fIsRetract = (bool)reader[CChatKey.fIsRetract],
                    fMemberFrom = (int)reader[CChatKey.fMemberFrom],
                    fMemberTo = (int)reader[CChatKey.fMemberTo],
                    fSubmitDateTime = (DateTime)reader[CChatKey.fSubmitDateTime],
                    fProductId = (int)reader[CChatKey.fProductId]
                });
            }
            return IsChat;
        }
        public static List<CChat> fn聊聊查詢(CMember member)
        {
            string sql = $"EXEC 聊聊查詢 @{CChatKey.fMemberFrom}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fMemberFrom, member.fMemberId)
            };
            List<CChat> lsChat = (List<CChat>)CDbManager.querySql(sql, paras, reader聊聊查詢);
            return lsChat;
        }
        private static IList reader聊聊個人查詢(SqlDataReader reader)
        {
            List<CChat> IsChatBeau = new List<CChat>();
            while (reader.Read())
            {
                IsChatBeau.Add(new CChat()
                {
                    fChatId = (int)reader[CChatKey.fChatId],
                    fMessage = (string)reader[CChatKey.fMessage],
                    fIsRead = (bool)reader[CChatKey.fIsRead],
                    fIsRetract = (bool)reader[CChatKey.fIsRetract],
                    fMemberFrom = (int)reader[CChatKey.fMemberFrom],
                    fMemberTo = (int)reader[CChatKey.fMemberTo],
                    fSubmitDateTime = (DateTime)reader[CChatKey.fSubmitDateTime],
                    fProductId = (int)reader[CChatKey.fProductId]
                });
            }
            return IsChatBeau;
        }
        public static List<CChat> fn聊聊個人查詢(CMember member,int a)
        {
            string sql = $"EXEC 聊聊個人查詢 @{CChatKey.fMemberFrom},@{CChatKey.fMemberTo}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fMemberFrom, member.fMemberId),
                new SqlParameter(CChatKey.fMemberTo, a)
            };
            List<CChat> IsChatBeau = (List<CChat>)CDbManager.querySql(sql, paras, reader聊聊個人查詢);
            return IsChatBeau;
        }
        public static void fn聊聊新增(CChat chat)
        {
            string sql = $"EXEC 聊聊新增 @{CChatKey.fSubmitDateTime},";
            sql += $"@{CChatKey.fMessage},";
            sql += $"@{CChatKey.fIsRead},";
            sql += $"@{CChatKey.fIsRetract},";
            sql += $"@{CChatKey.fMemberFrom},";
            sql += $"@{CChatKey.fMemberTo},";
            sql += $"@{CChatKey.fProductId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fSubmitDateTime, chat.fSubmitDateTime),
                new SqlParameter(CChatKey.fMessage, chat.fMessage),
                new SqlParameter(CChatKey.fIsRead, chat.fIsRead),
                new SqlParameter(CChatKey.fIsRetract, chat.fIsRetract),
                new SqlParameter(CChatKey.fMemberFrom, chat.fMemberFrom),
                new SqlParameter(CChatKey.fMemberTo, chat.fMemberTo),
                new SqlParameter(CChatKey.fProductId, chat.fProductId)
            };
            CDbManager.executeSql(sql, paras);
        }

        public static void fn聊聊更新(int a)
        {
            string sql = $"EXEC 聊聊更新 @{CChatKey.fChatId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fChatId, a)
            };
            CDbManager.executeSql(sql, paras);
        }

        public static void fn聊聊已讀更新(int a, CMember member)
        {
            string sql = $"EXEC 聊聊已讀更新 @{CChatKey.fMemberFrom},@{CChatKey.fMemberTo}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fMemberFrom, a),
                new SqlParameter(CChatKey.fMemberTo, member.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }

        private static IList reader聊聊未讀查詢(SqlDataReader reader)
        {
            List<CChat> IsChat = new List<CChat>();
            while (reader.Read())
            {
                IsChat.Add(new CChat()
                {
                    fProductId = (int)reader[CChatKey.fProductId]
                });
            }
            return IsChat;
        }
        public static List<CChat> fn聊聊未讀查詢(int from,int to)
        {
            string sql = $"EXEC 聊聊未讀查詢 @{CChatKey.fMemberFrom},";
            sql += $"@{CChatKey.fMemberTo}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CChatKey.fMemberFrom, from),
                new SqlParameter(CChatKey.fMemberTo, to)
            };
            List<CChat> lsChat = (List<CChat>)CDbManager.querySql(sql, paras, reader聊聊未讀查詢);
            return lsChat;
        }
    }
}
