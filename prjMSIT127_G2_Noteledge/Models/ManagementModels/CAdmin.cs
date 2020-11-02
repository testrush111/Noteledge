using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CAdmin
    {
        public int fAdminId { get; set; }
        [Required(ErrorMessage = "!請輸入Email(信箱)")]        
        [EmailAddress(ErrorMessage = "!不符合Email(信箱)格式")]
        public string fAdminAccount { get; set; }
        [Required(ErrorMessage = "!請輸入密碼")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "!此密碼長度必須在5~15字元之間")]
        public string fAdminPassword { get; set; }
        [Required(ErrorMessage = "!請輸入姓名")]
        public string fName { get; set; }
        [Required(ErrorMessage = "!請點選性別")]
        public string fGender { get; set; }
        [Required(ErrorMessage = "!請選擇出生日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fBirthDay { get; set; }
        [StringLength(100, ErrorMessage = "!不可超過100字元")]
        public string fTheAddress { get; set; }
        [Required(ErrorMessage = "!請輸入電話(手機)")]
        [StringLength(20, ErrorMessage = "!不可超過20字元")]
        public string fMobilePhone { get; set; }
        public string fThePhoto { get; set; }
        public DateTime fHireDateTime { get; set; }
        public DateTime fLastLoginDateTime { get; set; }       
    }
}
