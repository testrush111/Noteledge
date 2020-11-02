using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.NoteModels
{
    public class CVersion
    {
        public int fVersionId { get; set; }
        public DateTime fEditDateTime { get; set; }
        public string fHTMLContent { get; set; }
        public string fJsonContent { get; set; }
        public int fNoteId { get; set; }
    }
}
