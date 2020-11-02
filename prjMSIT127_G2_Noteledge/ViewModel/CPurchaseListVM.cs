using Models.MemberModels;
using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CPurchaseListVM
    {
        public List<CMember> lsMemberSeller { get; set; }
        public List<CProductPicture> lsProductPicture { get; set; }
        public List<COrder> lsOrder { get; set; }
        //public List<CDetailOrder> lsOrderDetail { get; set; }
        public List<CMemberOrderSelectVM> lsOrderDetail { get; set; }

    }
}