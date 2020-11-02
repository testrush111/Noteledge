using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Models.MemberModels
{
    public class CMemberEditor
    {
        public string fAccount { get; set; }
        public string fPassword { get; set; }
        public string NewfPassword { get; set; }
        public string ChackNewfPassword { get; set; }
        public string fFirstName { get; set; }
        public string fLastName { get; set; }
        public string fTheNickName { get; set; }
        public string fGender { get; set; }
        public DateTime fBirthDay { get; set; }
        public string fTheAddress { get; set; }
        public string fMobilePhone { get; set; }
        public string fPhoto { get; set; }
        public HttpPostedFileBase MemberImage { get; set; }
    }
}