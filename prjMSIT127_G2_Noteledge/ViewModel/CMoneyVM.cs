using Models.ManagementModels;
using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CMoneyVM
    {
        public List<COrder> lsOrder { get; set; }
        public List<CDetailOrder> lsOrderDetail { get; set; }
        public List<CIncome> lsIncome { get; set; }
    }
}