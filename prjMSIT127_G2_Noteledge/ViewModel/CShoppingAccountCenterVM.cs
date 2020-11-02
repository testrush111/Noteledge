using Models.ManagementModels;
using Models.MemberModels;
using Models.NoteModels;
using Models.ShoppingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CShoppingAccountCenterVM
    {
        public CMember Member { get; set; } //會員資料
        public List<CIncome> lsIncome { get; set; } //升級&儲值資訊(瑪妮幣)
        public List<CDetailOrder> lsOrderDetail { get; set; } //購買清單
        public List<CComment> lsComment { get; set; } //商品評價
        public List<CNotice> lsNotice { get; set; } //通知，VIP會員才會有買家給予的評價通知
        public List<CNoteFolderViewModel> lsNotefolderVM { get; set; } //個人筆記資料夾+筆記
        public CProduct Product { get; set; } //筆記商品
        public List<CProductCategory> lsCategory { get; set; } //筆記類別
        public List<CProductCompare> lsCategoryCompare { get; set; } //筆記類別對照表
        public List<CProductPicture> lsProductPicture { get; set; } //筆記圖片
        public List<CProductPicture> lsDetailPicture { get; set; } //筆記詳細圖片
        public CProductPicture ProductPicture { get; set; } //筆記圖片
        public string Content { get; set; } //存放筆記的JSON資料
        public int NoteId { get; set; } //存放筆記的JSON資料

    }
}