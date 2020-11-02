using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CProductAndProductCompareViewModel
    {
        public CProduct product { get; set; }
        public List<CProductCompare> lsProductCompare { get; set; }
    }
}