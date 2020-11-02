using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CBlackList
    {
        public int fBannedId { get; set; }
        public string fReason { get; set; }
        public DateTime fLockDateTime { get; set; }
        public int fMemberId { get; set; }        
    }
}
