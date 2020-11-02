using Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjMSIT127_G2_Noteledge.ViewModel
{
    public class CNoteFolderViewModel
    {
        public int fFolderId { get; set; }
        public int fMemberId { get; set; }
        public string fFolderName { get; set; }
        public List<CNote> lsNote { get; set; }
    }
}