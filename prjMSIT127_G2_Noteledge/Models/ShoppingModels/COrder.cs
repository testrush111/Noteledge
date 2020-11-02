using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class COrder
    {
        public int fOrderId { get; set; }
        public DateTime fPurchaseDate { get; set; }
        public int fTotalPrice { get; set; }
        public int fMemberId { get; set; }
    }
}
