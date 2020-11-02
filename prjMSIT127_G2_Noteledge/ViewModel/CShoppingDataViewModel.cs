using Models.MemberModels;
using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CShoppingDataViewModel
    {
        public List<CMember> lsMember{get; set;}
        public List<CProductAndProductCompareViewModel> ls產品與產品分類 { get; set; }
    }
}