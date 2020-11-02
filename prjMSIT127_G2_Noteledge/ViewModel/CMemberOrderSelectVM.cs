using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CMemberOrderSelectVM
    {
        public int fMemberId { get; set; }
        public int fProductId { get; set; }
        public int fOrderId { get; set; }
        public int fDetailOrderIId { get; set; }
        public string fName { get; set; }
        public string fDescription { get; set; }
        public string fPhoto { get; set; }
        public int fPrice { get; set; }
        public DateTime fLaunchDate { get; set; }
    }
}