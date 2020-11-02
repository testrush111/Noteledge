using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.NoteModels
{
    public class CNote
    {
        public int fNoteId { get; set; }
        public string fNoteListName { get; set; }
        public DateTime fCreateDateTime { get; set; }
        public DateTime fEditDateTime { get; set; }
        public int fNoteListLevel { get; set; }
        public bool fIsMyFavourite { get; set; }
        public bool fIsTrash { get; set; }
        public string fHTMLContent { get; set; }
        public string fJsonContent { get; set; }
        public string fTheShareLink { get; set; }
        public string fTheContactPerson { get; set; }
        public int fFolderId { get; set; }
    }
}
