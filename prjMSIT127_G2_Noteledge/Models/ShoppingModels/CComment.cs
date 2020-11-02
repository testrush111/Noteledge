using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CComment
    {
        public int fCommentId { get; set; }
        public DateTime fCommentDateTime { get; set; }
        public string fContent { get; set; }
        public bool fIsRetract { get; set; }
        public int fLikeCount { get; set; }
        public bool fIsBanned { get; set; }
        public int fProductId { get; set; }
        public int fMemberId { get; set; }
    }
}
