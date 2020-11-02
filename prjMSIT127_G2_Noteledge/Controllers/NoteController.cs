using Models.MemberModels;
using Models.NoteModels;
using prjMSIT127_G2_Noteledge.Models.NoteModels;
using prjMSIT127_G2_Noteledge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMSIT127_G2_Noteledge.Controllers
{
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        {
            if (Session[CMemberSession.Session_Login_User] == null)//防止未登入者進入筆記系統
                return RedirectToAction("../Member/Login");
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢所有的筆記資料夾
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                           .ToList();
            //筆記資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();
            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder).OrderBy(n=>n.fNoteListLevel)
                                                   .ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            }
            
            return View("Index", "_Layout", lsNotefolderVM);
        }
        public ActionResult MyNote(int? FolderId)
        {
            if (Session[CMemberSession.Session_Login_User] == null)//防止未登入者進入筆記系統
                return RedirectToAction("../Member/Login");

            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢未分類筆記
            CNoteFolder defaultFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderName == "未分類筆記");
            List<CNote> lsDefaultNote = CNoteFactory.fn私人筆記查詢(defaultFolder)
                                                     .ToList();
            //網址未輸入資料夾ID，轉到未分類筆記
            if (FolderId == null)
                return View("MyNote", "_Layout", lsDefaultNote);
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == (int)FolderId);
            //網址輸入不存在的資料夾ID，轉到未分類筆記
            if (myFolder == null)
                return View("MyNote", "_Layout", lsDefaultNote);
            //查詢所有筆記
            List<CNote> lsNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                             .ToList();
            //轉到筆記頁面
            return View("MyNote", "_Layout", lsNote);
        }
        [HttpPost]
        public string Search(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記全部查詢()
                                       .FirstOrDefault(n => n.fNoteId == NoteId && n.fIsTrash == false);

            return myNote.fJsonContent;
        }
        [HttpPost]
        public string SearchFavor(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == (int)FolderId);
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId && n.fIsTrash == false && n.fIsMyFavourite == true);

            return myNote.fJsonContent;
        }
        [ValidateInput(false)]
        [HttpPost]
        public string Update(int FolderId, int NoteId, string JsonContent)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            myNote.fJsonContent = JsonContent;
            myNote.fEditDateTime = DateTime.UtcNow.AddHours(08);
            CNoteFactory.fn私人筆記更新(myNote);
            return "筆記更新成功";
        }
        [HttpPost]
        public string NoteRename(int FolderId, int NoteId, string NoteName)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            myNote.fNoteListName = NoteName;
            myNote.fEditDateTime = DateTime.UtcNow.AddHours(08);
            CNoteFactory.fn私人筆記更新(myNote);
            return "筆記已重新命名！";
        }
        [HttpPost]
        public string UpdateFavor(int FolderId, int NoteId, bool IsFavor)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            myNote.fIsMyFavourite = IsFavor;
            myNote.fEditDateTime = DateTime.UtcNow.AddHours(08);
            CNoteFactory.fn私人筆記更新(myNote);

            return "我的最愛筆記已更新！";
        }
        [HttpPost]
        public int AddNote(int Folderid, string TxtNewNoteName)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == Folderid);
            int level = CNoteFactory.fn私人筆記查詢(myFolder).Where(n => n.fIsTrash == false).Count();
            CNote myNote = new CNote()
            {
                fCreateDateTime = DateTime.UtcNow.AddHours(08),
                fEditDateTime = DateTime.UtcNow.AddHours(08),
                fFolderId = Folderid,
                fIsMyFavourite = false,
                fIsTrash = false,
                fHTMLContent = "",
                fJsonContent = "{\"ops\":[{\"insert\":\"\\n\"}]}",
                fNoteListLevel = level,
                fNoteListName = TxtNewNoteName,
                fTheContactPerson = null,
                fTheShareLink = null
            };
            int noteId = CNoteFactory.fn私人筆記新增(myFolder, myNote);

            return noteId;
        }
        [HttpPost]
        public int AddNoteFolder(string TxtNewFolderName)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            CNoteFolder myFolder = new CNoteFolder()
            {
                fMemberId = member.fMemberId,
                fFolderName = TxtNewFolderName
            };
            int folderid = CNoteFolderFactory.fn筆記資料夾新增(member, myFolder);
            return folderid;
        }
        [HttpPost]
        public ActionResult MoveToTrash(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            CNoteFolder m原筆記資料夾 = CNoteFolderFactory.fn筆記資料夾查詢(member)
                .FirstOrDefault(f => f.fFolderId == FolderId);
            CNoteFolder m未分類筆記 = CNoteFolderFactory.fn筆記資料夾查詢(member)
                .FirstOrDefault(f => f.fFolderName == "未分類筆記");

            CNote myNote = CNoteFactory.fn私人筆記查詢(m原筆記資料夾)
                .FirstOrDefault(n => n.fNoteId == NoteId);

            myNote.fIsTrash = true;
            myNote.fFolderId = m未分類筆記.fFolderId;

            CNoteFactory.fn私人筆記更新(myNote);

            //查詢所有的筆記資料夾
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                           .ToList();
            //筆記資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();
            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder)
                                                   .ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            }

            return PartialView("_TrashView", lsNotefolderVM);
            //return "筆記移到垃圾桶！";
        }
        [HttpPost]
        public string MoveOutTrash(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            CNoteFolder m原筆記資料夾 = CNoteFolderFactory.fn筆記資料夾查詢(member)
                .FirstOrDefault(f => f.fFolderId == FolderId);

            CNote myNote = CNoteFactory.fn私人筆記查詢(m原筆記資料夾)
                .FirstOrDefault(n => n.fNoteId == NoteId);

            myNote.fIsTrash = false;

            CNoteFactory.fn私人筆記更新(myNote);

            return "筆記還原！";
        }
        [HttpPost]
        public ActionResult DeleteNote(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);

            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            CNoteFactory.fn私人筆記刪除(myNote);
            //查詢所有的筆記資料夾
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                           .ToList();
            //筆記資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();
            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder)
                                                   .ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            }

            return PartialView("_TrashView", lsNotefolderVM);
            //return "筆記永久刪除成功！";
        }
        [HttpPost]
        public string FolderRename(int FolderId, string FolderName)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            myFolder.fFolderName = FolderName;
            CNoteFolderFactory.fn筆記資料夾更新(myFolder);
            return "資料夾重新命名成功！";
        }
        [HttpPost]
        public ActionResult DeleteFolder(int FolderId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            CNoteFolderFactory.fn筆記資料夾刪除(myFolder, member);

            //查詢所有的筆記資料夾
            List<CNoteFolder> lsFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                           .ToList();
            //筆記資料夾包含筆記的列表
            List<CNoteFolderViewModel> lsNotefolderVM = new List<CNoteFolderViewModel>();
            //讀取筆記資料夾內的筆記
            foreach (var folder in lsFolder)
            {
                List<CNote> myLsNote = CNoteFactory.fn私人筆記查詢(folder)
                                                   .ToList();
                lsNotefolderVM.Add(new CNoteFolderViewModel()
                {
                    fFolderId = folder.fFolderId,
                    fFolderName = folder.fFolderName,
                    fMemberId = folder.fMemberId,
                    lsNote = myLsNote
                });
            }

            return PartialView("_TrashView", lsNotefolderVM);
            //return "資料夾刪除成功！";
        }
        [HttpPost]
        public ActionResult SearchVersion(int FolderId, int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == (int)FolderId);
            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記查詢(myFolder)
                                       .FirstOrDefault(n => n.fNoteId == NoteId && n.fIsTrash == false);
            List<CVersion> lsVersion = CVersionFactory.fn筆記版本控制查詢(myNote);

            return PartialView("_SearchVersionView", lsVersion);
        }
        [HttpPost]
        public string SearchVersionJsonContent(int Versionid)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢指定筆記內容
            CVersion myVersion = CVersionFactory.fn筆記版本控制查詢用VersionID查(Versionid).FirstOrDefault();
            return myVersion.fJsonContent;
        }
        [ValidateInput(false)]
        [HttpPost]
        public string UpdateVersion(int NoteId, string JsonContent)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記全部查詢()
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            myNote.fJsonContent = JsonContent;
            myNote.fEditDateTime = DateTime.UtcNow.AddHours(08);
            CNoteFactory.fn私人筆記更新(myNote);
            return "筆記更新成功";
        }
        [HttpPost]
        public string AddVersion(int NoteId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            //查詢指定筆記內容
            CNote myNote = CNoteFactory.fn私人筆記全部查詢()
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            myNote.fEditDateTime = DateTime.UtcNow.AddHours(08);
            CVersionFactory.fn筆記版本控制新增(myNote);

            return "版本紀錄新增成功！";
        }
        [HttpPost]
        public ActionResult SearchTemplate()
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            List<CTemplate> lsTemplate = CTemplateFactory.fn模板查詢();
            return PartialView("_TemplateBlock", lsTemplate);
        }
        [HttpPost]
        public string SearchTemplateContent(int TemplateId)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            CTemplate myTemplate = CTemplateFactory.fn模板查詢()
                                                   .FirstOrDefault(v=>v.fTemplateId == TemplateId);
            return myTemplate.fContent;
        }
        [HttpPost]
        public string UpdateNoteLevel(int FolderId, int NoteId, int Level)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員

            CNote myNote = CNoteFactory.fn私人筆記全部查詢()
                                       .FirstOrDefault(n => n.fNoteId == NoteId);
            //if(myNote.fFolderId == FolderId && myNote.fNoteId == NoteId && myNote.fNoteListLevel == Level)
            //    return myNote.fNoteListName + " 筆記順序更新成功！";
            myNote.fFolderId = FolderId;
            myNote.fNoteListLevel = Level;
            CNoteFactory.fn私人筆記更新(myNote);
            return myNote.fNoteListName + " 筆記順序更新成功！";
        }
        [HttpPost]
        public int AddTemplate(int FolderId, string TxtNewNoteName, string Content)
        {
            CMember member = (CMember)Session[CMemberSession.Session_Login_User];//會員
            //查詢筆記資料夾
            CNoteFolder myFolder = CNoteFolderFactory.fn筆記資料夾查詢(member)
                                                     .FirstOrDefault(f => f.fFolderId == FolderId);
            int level = CNoteFactory.fn私人筆記查詢(myFolder).Where(n=>n.fIsTrash == false).Count();
            CNote myNote = new CNote()
            {
                fCreateDateTime = DateTime.UtcNow.AddHours(08),
                fEditDateTime = DateTime.UtcNow.AddHours(08),
                fFolderId = FolderId,
                fIsMyFavourite = false,
                fIsTrash = false,
                fHTMLContent = "",
                fJsonContent = Content,
                fNoteListLevel = level,
                fNoteListName = TxtNewNoteName,
                fTheContactPerson = null,
                fTheShareLink = null
            };

            int noteid = CNoteFactory.fn私人筆記新增(myFolder, myNote);

            return noteid;
        }
    }
}