using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.MemberModels
{
    public class CSetting
    {
        public int fSettingId { get; set; }
        public bool fIsDarkMode { get; set; }
        public string fFontSize { get; set; }
        public string fFontType { get; set; }
        public bool fIsFullWidth { get; set; }
        public bool fIsNotice { get; set; }
        public int fMemberId { get; set; }
    }
}
