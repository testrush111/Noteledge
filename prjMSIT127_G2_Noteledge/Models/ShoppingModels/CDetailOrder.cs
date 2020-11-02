using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CDetailOrder
    {
        public int fDetailOrderIId { get; set; }
        public int fProductId { get; set; }
        public int fOrderId { get; set; }
        public int fMemberId { get; set; }
        public int fMemberSellerId { get; set; }
        public string fName { get; set; }
    }
}
