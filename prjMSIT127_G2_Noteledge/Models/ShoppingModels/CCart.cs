using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CCart
    {
        public int fCartId { get; set; }
        public DateTime? fSubmitTime { get; set; }
        public int fMemberId { get; set; }
    }
}
