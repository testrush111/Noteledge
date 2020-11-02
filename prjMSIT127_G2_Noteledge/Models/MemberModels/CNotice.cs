using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MemberModels
{
    /// <summary>
    /// 訊息資料表
    /// </summary>
    public class CNotice
    {
        /// <summary>
        /// 訊息ID
        /// </summary>
        public int fNoticeId { get; set; }
        /// <summary>
        /// 訊息日期時間
        /// </summary>
        [DisplayName("通知時間")]
        public DateTime fNoticeDatetime { get; set; }
        /// <summary>
        /// 訊息內容
        /// </summary>
        [DisplayName("內容")]
        public string fNoticeContent { get; set; }
        /// <summary>
        /// 訊息分類
        [DisplayName("分類")]
        public string fCategoryType { get; set; }
        /// <summary>
        /// 訊息連結
        /// </summary>
        public string fLink { get; set; }
        /// <summary>
        /// 會員ID
        /// </summary>
        public int fMemberId { get; set; }
    }
}
