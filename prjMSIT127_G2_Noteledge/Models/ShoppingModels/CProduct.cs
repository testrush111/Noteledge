using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.ShoppingModels
{
    public class CProduct
    {
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

    }
}