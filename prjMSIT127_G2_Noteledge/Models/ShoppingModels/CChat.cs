using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CChat
    {
        public int fChatId { get; set; }
        public DateTime fSubmitDateTime { get; set; }
        public string fMessage { get; set; }
        public bool fIsRead { get; set; }
        public bool fIsRetract { get; set; }
        public int fMemberFrom { get; set; }
        public int fMemberTo { get; set; }
        public int fProductId { get; set; }
    }
}
