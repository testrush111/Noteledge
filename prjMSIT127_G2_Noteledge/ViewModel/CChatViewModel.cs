using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CChatViewModel
    {
        public string beau { get; set; }
        public int beauId { get; set; }
        public string fromchatname { get; set; }
        public string tochatname { get; set; }
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