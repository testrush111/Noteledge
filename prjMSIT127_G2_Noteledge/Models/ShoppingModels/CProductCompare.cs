using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShoppingModels
{
    public class CProductCompare
    {
        public int fProductComparedId { get; set; }
        public int fProductId { get; set; }
        public string fName { get; set; }
        public string fDescription { get; set; }
        public string fContent { get; set; }
        public int fPrice { get; set; }
        public DateTime fLaunchDate { get; set; }
        public DateTime? fTheRemovedDate { get; set; }
        public int fDownloadTimes { get; set; }
        public int fLikeCount { get; set; }
        public int fMemberSellerId { get; set; }
        public int fCategoryId { get; set; }
        public string fCategoryName { get; set; }
    }
}
