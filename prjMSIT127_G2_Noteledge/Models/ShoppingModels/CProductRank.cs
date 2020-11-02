using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CProductRank
    {
        public int fProductRankID { get; set; }
        public int fRank { get; set; }
        public string fComment { get; set; }
        public DateTime fSubmitDataTime { get; set; }
        public int fDetailOrderIId { get; set; }
        public int fMemberId { get; set; }
    }
}
