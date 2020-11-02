using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CProducttotal
    {
        public int fMemberId { get; set; }
        public int fDetailOrderIId { get; set; }
        public int fProductId { get; set; }
        public string fName { get; set; }
        public string fDescription { get; set; }
        public int fMemberSellerId { get; set; }
        public string fPicture { get; set; }
        public int fRank { get; set; }
        public string fComment { get; set; }
    }
}