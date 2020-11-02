using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MemberModels
{
    public class CMember
    {
        public int fMemberId { get; set; }
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
        public string fCity { get; set; }
        public string fTown { get; set; }
        public string fMobilePhone { get; set; }
        public int fMoneyPoint { get; set; }
        public string fPhoto { get; set; }
        public DateTime fRegisterDateTime { get; set; }
        public DateTime fLastLoginDateTime { get; set; }
        public bool fIsVIP { get; set; }
        public bool fIsBanned { get; set; }
        public string fThePasswordURL { get; set; }
        public string Remember_Check { get; set; }
        public string GoogleApiId { get; set; }
    }
}
