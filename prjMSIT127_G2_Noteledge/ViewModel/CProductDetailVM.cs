using Models.MemberModels;
using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CProductDetailVM
    {
        public List<CMember> lsMember { get; set; }
        public CMember MemberSeller { get; set; }
        public CProduct Product { get; set; }
        public List<CProductPicture> lsProductPicture { get; set; }
        public List<CProductCompare> lsProductCategory { get; set; }
        public List<CProductRank> lsProductRank { get; set; }
        public List<CComment> lsProductComment { get; set; }
        public List<CMember> Isbanned { get; set; }
    }
}