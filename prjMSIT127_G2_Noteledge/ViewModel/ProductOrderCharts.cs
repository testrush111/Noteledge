using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.ShoppingModels;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class ProductOrderCharts
    {
        public CProduct product { get; set; }
        public CDetailOrder detailOrder { get; set; }
        public List<CDetailOrder> IsDetailOrder{get; set;}
        public List<COrder> lsOrder { get; set; }
        public List<CDetailOrder> lsOrderDetail { get; set; }
    }
}