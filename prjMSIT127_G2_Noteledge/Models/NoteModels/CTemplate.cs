using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.Models.NoteModels
{
    public class CTemplate
    {
        public int fTemplateId { get; set; }
        public string fName { get; set; }
        public string fTheDescription { get; set; }
        public string fContent { get; set; }
        public DateTime fLaunchDateTime { get; set; }
        public DateTime? fTheRemovedDateTime { get; set; }
        public int fDownloadCount { get; set; }
    }
}