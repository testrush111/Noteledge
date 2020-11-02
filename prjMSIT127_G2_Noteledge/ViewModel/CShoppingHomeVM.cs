using Microsoft.Ajax.Utilities;
using Models.MemberModels;
using Models.ShoppingModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ViewModel.CShoppingHomeVM
{
    public class CShoppingHomeVM
    {
        public CMember member { get; set; }

        public List<CProductCompare> lsproductcategory { get; set; }

        public CProductPicture productpicture { get; set; }

        public IPagedList<CShoppingHomeVM> pagedlist { get; set; }

    }
}