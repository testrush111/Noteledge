using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.ManagementModels
{
    public class CAdminEditor
    {
        public int fAdminId { get; set; }        
        public string fAdminAccount { get; set; }
        public string fAdminPassword { get; set; }
        public string fName { get; set; }
        public string fGender { get; set; }
        public DateTime fBirthDay { get; set; }
        public string fTheAddress { get; set; }
        public string fMobilePhone { get; set; }
        public string fThePhoto { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}