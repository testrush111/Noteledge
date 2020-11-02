using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.MemberModels
{
    public class CMemberOrder
    {
        public int fMemberId { get; set; }
        public int fProductId { get; set; }
        public int fOrderId { get; set; }
        public int fDetailOrderIId { get; set; }
        public string fName { get; set; }
        public int fPrice { get; set; }
        public DateTime fLaunchDate { get; set; }
    }
}