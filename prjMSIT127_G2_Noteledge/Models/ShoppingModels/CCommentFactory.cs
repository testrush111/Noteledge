using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.MemberModels;
using Models.ShoppingModels;

namespace Models.ShoppingModels
{
    public class CCommentFactory
    {
        private static IList reader留言查詢(SqlDataReader reader)
        {
            List<CComment> IsComment = new List<CComment>();
            while (reader.Read())
            {
                IsComment.Add(new CComment()
                {
                    fCommentId = (int)reader[CCommentKey.fCommentId],
                    fCommentDateTime = (DateTime)reader[CCommentKey.fCommentDateTime],
                    fContent = (string)reader[CCommentKey.fContent],
                    fIsRetract = (bool)reader[CCommentKey.fIsRetract],
                    fLikeCount = (int)reader[CCommentKey.fLikeCount],
                    fIsBanned = (bool)reader[CCommentKey.fIsBanned],
                    fProductId = (int)reader[CCommentKey.fProductId],
                    fMemberId = (int)reader[CCommentKey.fMemberId]
                });
            }
            return IsComment;
        }
        public static List<CComment> fn留言查詢()
        {
            string sql = $"EXEC 留言查詢";
            List<CComment> IsComment = (List<CComment>)CDbManager.querySql(sql, null, reader留言查詢);
            return IsComment;
        }

        public static void fn留言新增(CComment comment)
        {
            string sql = $"EXEC 留言新增 @{CCommentKey.fCommentDateTime},";
            sql += $"@{CCommentKey.fContent},";
            sql += $"@{CCommentKey.fIsRetract},";
            sql += $"@{CCommentKey.fLikeCount},";
            sql += $"@{CCommentKey.fIsBanned},";
            sql += $"@{CCommentKey.fProductId},";
            sql += $"@{CCommentKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCommentKey.fCommentDateTime, comment.fCommentDateTime),
                new SqlParameter(CCommentKey.fContent, comment.fContent),
                new SqlParameter(CCommentKey.fIsRetract, comment.fIsRetract),
                new SqlParameter(CCommentKey.fLikeCount, comment.fLikeCount),
                new SqlParameter(CCommentKey.fIsBanned, comment.fIsBanned),
                new SqlParameter(CCommentKey.fProductId, comment.fProductId),
                new SqlParameter(CCommentKey.fMemberId, comment.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }

        public static void fn留言更新(CComment comment)
        {
            string sql = $"EXEC 留言更新 @{CCommentKey.fCommentId},";
            sql += $"@{CCommentKey.fCommentDateTime},";
            sql += $"@{CCommentKey.fContent},";
            sql += $"@{CCommentKey.fIsRetract},";
            sql += $"@{CCommentKey.fLikeCount},";
            sql += $"@{CCommentKey.fIsBanned},";
            sql += $"@{CCommentKey.fProductId},";
            sql += $"@{CCommentKey.fMemberId}";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(CCommentKey.fCommentId, comment.fCommentId),
                new SqlParameter(CCommentKey.fCommentDateTime, comment.fCommentDateTime),
                new SqlParameter(CCommentKey.fContent, comment.fContent),
                new SqlParameter(CCommentKey.fIsRetract, comment.fIsRetract),
                new SqlParameter(CCommentKey.fLikeCount, comment.fLikeCount),
                new SqlParameter(CCommentKey.fIsBanned, comment.fIsBanned),
                new SqlParameter(CCommentKey.fProductId, comment.fProductId),
                new SqlParameter(CCommentKey.fMemberId, comment.fMemberId)
            };
            CDbManager.executeSql(sql, paras);
        }
    }
}
