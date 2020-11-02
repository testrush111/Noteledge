using Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Google.Apis.Auth;
using System.Web.Configuration;
using System.Collections;
using System.Security.Cryptography;
using Models.ManagementModels;
using prjMSIT127_G2_Noteledge.Models.ManagementModels;
using Models.ShoppingModels;
using Models.NoteModels;
using prjMSIT127_G2_Noteledge.ViewModel;
using Imgur.API.Models;
using Imgur.API.Authentication;
using System.Net.Http;
using Imgur.API.Endpoints;

namespace Note_edgeLogin.Controllers
{
    public class MemberController : Controller
    {
        //會員登入----------------------------------------------------------------------------------------\\
        public ActionResult Login(string name)
        {
                Session[CMemberSession.Session_Edit_Password] = null;
                Session[CMemberSession.Session_Change_Password] = null;
                Session[CMemberSession.Session_Login_User] = null;
                Session[CMemberSession.Session_Message_Count] = null;
                ViewBag.name = name;
                return View();
        }
        [HttpPost]
        public ActionResult Login(CMember m ,CMemberBrowse browse)
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember member = SELECTMember.FirstOrDefault(n => n.fAccount == m.fAccount);
            string code = Request.Form["code"].ToString();

            if (member == null)
            {
                ViewBag.LoginMessage = "!沒有該使用者帳戶";
                return View();
            }
            if (!member.fPassword.Equals(m.fPassword))
            {
                ViewBag.LoginMessage = "!密碼不符";

                return View();
            }
            if (code != TempData["code"].ToString())
            {
                ViewBag.code = code;
                ViewBag.LoginMessage = "!驗證碼錯誤";
                return View();
            }
            if (m.Remember_Check != null)
            {
                CMemberBrowseFactory.fn會員瀏覽紀錄新增(member);
                member.fLastLoginDateTime = DateTime.UtcNow.AddHours(08);
                CMemberFactory.fn會員更新(member);
                ViewBag.code = code;
                Session[CMemberSession.Session_Login_User] = member;
                Session[CMemberSession.Session_Login_Remember] = member;
                return View("../Home/MyHome");
            }
            else
            {
                CMemberBrowseFactory.fn會員瀏覽紀錄新增(member);
                member.fLastLoginDateTime = DateTime.UtcNow.AddHours(08);
                CMemberFactory.fn會員更新(member);
                ViewBag.code = code;
                ViewBag.Ans = TempData["code"];
                ViewBag.Result = "驗證正確";
                Session[CMemberSession.Session_Login_User] = member;
                Session[CMemberSession.Session_Login_Remember] = null;
                return RedirectToAction("../Home/MyHome");
            }
        }

        //管理員登入----------------------------------------------------------------------------------------\\
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(CAdmin m)
        {
            List<CAdmin> SelectAdmin = CAdminFactory.fn管理員查詢();
            CAdmin Admin = SelectAdmin.FirstOrDefault(n => n.fAdminAccount == m.fAdminAccount);

            if (Admin == null)
            {
                ViewBag.LoginMessage = "！沒有該使用者帳戶";
                return View();
            }
            if (!Admin.fAdminPassword.Equals(m.fAdminPassword))
            {
                ViewBag.LoginMessage = "！" +
                    "密碼不符";

                return View();
            }
            else
            {
                Admin.fLastLoginDateTime = DateTime.UtcNow.AddHours(08);
                CAdminFactory.管理員更新(Admin);
                Session[CAdminSession.Session_Login_User] = Admin;
                return RedirectToAction("../ManagerSystem/HomePage", "_LayoutAdmin");
            }
        }

        //第三方GOOGLE登入------------------------------------------------------------------------------------\\
        public ActionResult APITest()
        {
            return View();
        }
        [HttpPost]
        public string APITest(string restr)
        {
            string[] words = restr.Split('"');
            string GoogleName = words[43];
            string GoogleEmail = words[65];

            return GoogleName;
        }


        //註冊-----------------------------------------------------------------------------------------\\
        public ActionResult SignIn()
        {
            Session[CMemberSession.Session_Edit_Password] = null;
            Session[CMemberSession.Session_Change_Password] = null;
            Session[CMemberSession.Session_Login_User] = null;
            Session[CMemberSession.Session_Message_Count] = null;
            DateTime day = DateTime.UtcNow.AddHours(08);
            string Today = day.ToString("yyyy-MM-dd");
            ViewBag.Today = Today;
            return View();
        }
        [HttpPost]
        public string SignIn(CMember m, CNotice c)
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember member = SELECTMember.FirstOrDefault(n => n.fAccount == m.fAccount);
            var data = "";
            if (member == null)
            {
                string str = m.fPassword + "QAQ";
                var md5 = CMemberFactory.MD5驗證碼新增(str);
                m.fMoneyPoint = 100;
                m.fPhoto = "../Image/MemberImage/Member.jpg";
                m.fRegisterDateTime = DateTime.UtcNow.AddHours(08);
                m.fLastLoginDateTime = DateTime.UtcNow.AddHours(08);
                m.fIsVIP = false;
                m.fIsBanned = false;
                m.fThePasswordURL = md5;
                m.fTheAddress = m.fCity + m.fTown + m.fTheAddress;
                CMemberFactory.fn會員新增(m);
                //會員註冊訊息新增---------------------------------------------------------------\\
                List<CMember> SELECTMember2 = CMemberFactory.fn會員查詢();
                CMember member2 = SELECTMember2.FirstOrDefault(n => n.fAccount == m.fAccount);
                c.fNoticeDatetime = DateTime.UtcNow.AddHours(08);
                c.fNoticeContent = m.fFirstName + m.fLastName + "歡迎加入Notedge!";
                c.fCategoryType = "系統";
                c.fLink = "我是超連結";
                c.fMemberId = member2.fMemberId;
                CNoticeFactory.fn通知訊息新增(member2, c);
                //會員註冊新增未分類筆記----------------------------------------------------------\\
                CNoteFolderFactory.fn建立預設筆記資料夾(member2);
                data = "新增成功";
                return data;
            }
            else
            {
                data = "新增失敗";
                ViewBag.Message = "信箱與人重複，請重新輸入";
                return data;
            }
        }

        //驗證碼-----------------------------------------------------------------------------------------\\
        private string RandomCode(int length)
        {
            string s = "0123456789";
            //string s = "0000000000000";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }

        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }

        public ActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = RandomCode(4);
            TempData["code"] = code;
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 35))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 20.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 7, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }

        //會員設定-個人資料------------------------------------------------------------------------------------------------

        public ActionResult MemberSetup()
        {
            CMemberEditPassword editPassword = Session[CMemberSession.Session_Edit_Password] as CMemberEditPassword;
            CMemberEditPassword ChangePassword = Session[CMemberSession.Session_Change_Password] as CMemberEditPassword;

            if (Session[CMemberSession.Session_Login_User] == null)
            {
                Session[CMemberSession.Session_Login_User] = "憑證消失，請重新登入";
                return RedirectToAction("../Home/Index");
            }
            if (Session[CMemberSession.Session_Edit_Password] != null)
            {
                if (editPassword.PasswordMassage == "密碼相符")
                {
                    ViewBag.PasswordTrue = "密碼相符";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else if (editPassword.PasswordMassage == "!密碼不符") 
                {
                    ViewBag.Passwordfalse = "密碼不符";
                    ViewBag.Error = "!密碼不符";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else if (editPassword.PasswordMassage == "!密碼請符合，5字元-15字元之間")
                {
                    ViewBag.Passwordfalse = "密碼不符";
                    ViewBag.Error = "!密碼請符合，5字元-15字元之間";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else 
                {
                    ViewBag.Passwordfalse = "密碼不符";
                    ViewBag.Error = "!不可為空，請輸入密碼";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
            }
            if (Session[CMemberSession.Session_Change_Password] != null) 
            {
                if (ChangePassword.PasswordMassage == "!不可為空，請輸入密碼")
                {
                    ViewBag.PasswordTrue = "密碼相符";
                    ViewBag.Error = "!不可為空，請輸入密碼";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else if (ChangePassword.PasswordMassage == "!密碼請符合，5字元-15字元之間")
                {
                    ViewBag.PasswordTrue = "密碼相符";
                    ViewBag.Error = "!密碼請符合，5字元-15字元之間";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else if (ChangePassword.PasswordMassage == "!與上方密碼不相符")
                {
                    ViewBag.PasswordTrue = "密碼相符";
                    ViewBag.Error = "!與上方密碼不相符";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else if (ChangePassword.PasswordMassage == "!新密碼與舊密碼相符")
                {
                    ViewBag.PasswordTrue = "密碼相符";
                    ViewBag.Error = "!新密碼與舊密碼相符";
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
                else 
                {
                    Session[CMemberSession.Session_Edit_Password] = null;
                    Session[CMemberSession.Session_Change_Password] = null;
                    return View("../Member/MemberSetup", "_Layout");
                }
            }
            else
            {
                Session[CMemberSession.Session_Edit_Password] = null;
                Session[CMemberSession.Session_Change_Password] = null;
                return View("../Member/MemberSetup", "_Layout");
            }
            
        }
        [HttpPost]
        public string MemberSetup(CMemberEditor m )
        {
            var data = "";
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            if (m.fPhoto == null && m.NewfPassword == null)
            {
                List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
                CMember cMember = SELECTMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);
                if (cMember != null)
                {
                    if (m.MemberImage != null)
                    {
                        string photoName = Guid.NewGuid().ToString();
                        photoName += Path.GetExtension(m.MemberImage.FileName);
                        var A = Server.MapPath(@"~/Image/MemberImage/" + photoName);
                        m.MemberImage.SaveAs(Server.MapPath("../Image/MemberImage/" + photoName));


                        var CLIENT_ID = "b6ff140d7b00eef";
                        var CLIENT_SECRET = "5008867fee0b01b1a3e9ccbc0d82ccee76290f7d";

                        //建立ImgurClient(其中的"CLIENT_ID", "CLIENT_SECRET"要換成你自己的)
                        var client = new ApiClient(CLIENT_ID, CLIENT_SECRET);
                        var httpClient = new HttpClient();
                        var endpoint = new ImageEndpoint(client, httpClient);
                        var ImgPath = Server.MapPath(@"~/Image/MemberImage/" + photoName);
                        IImage image;
                        //取得圖片檔案FileStream
                        using (var fs = new FileStream(ImgPath, FileMode.Open))
                        {
                            image = endpoint.UploadImageAsync(fs).GetAwaiter().GetResult();
                        }
                        cMember.fPhoto = image.Link;
                        System.IO.File.Delete(ImgPath);
                        //顯示圖檔位置
                        Response.Write("Image uploaded. Image Url: " + image.Link);

                        m.MemberImage.SaveAs(A);
                        cMember.fAccount = m.fAccount;
                        cMember.fPassword = m.fPassword;
                        cMember.fFirstName = m.fFirstName;
                        cMember.fLastName = m.fLastName;
                        cMember.fTheNickName = m.fTheNickName;
                        cMember.fGender = m.fGender;
                        cMember.fBirthDay = m.fBirthDay;
                        cMember.fTheAddress = m.fTheAddress;
                        cMember.fMobilePhone = m.fMobilePhone;
                        CMemberFactory.fn會員更新(cMember);
                        Session[CMemberSession.Session_Login_User] = cMember;
                        data = "修改成功";
                        return data;
                    }
                    else
                    {
                        cMember.fAccount = m.fAccount;
                        cMember.fPassword = m.fPassword;
                        cMember.fFirstName = m.fFirstName;
                        cMember.fLastName = m.fLastName;
                        cMember.fTheNickName = m.fTheNickName;
                        cMember.fGender = m.fGender;
                        cMember.fBirthDay = m.fBirthDay;
                        cMember.fTheAddress = m.fTheAddress;
                        cMember.fMobilePhone = m.fMobilePhone;
                        CMemberFactory.fn會員更新(cMember);
                        Session[CMemberSession.Session_Login_User] = cMember;
                        data = "修改成功";
                        return data;
                    }
                }
                else
                {
                    data = "修改失敗";
                    return data;
                }
            }
            data = "修改失敗";
            return data;
        }
        [HttpPost]
        public ActionResult MemberImage(CMemberEditor m)
        {
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);
            if (m.MemberImage != null)
            {
                //string photoName = Guid.NewGuid().ToString();
                //photoName += Path.GetExtension(m.MemberImage.FileName);
                ////m.MemberImage.SaveAs(Environment.GetEnvironmentVariable("HOME") +
                ////           "\\site\\wwwroot\\Image\\MemberImage" + photoName);
                ////var A = Environment.GetEnvironmentVariable("HOME") +
                ////           "\\site\\wwwroot\\Image\\MemberImage"+ photoName;
                //var A = Server.MapPath(@"~/Image/MemberImage/" + photoName);
                //m.MemberImage.SaveAs(Server.MapPath("~/Image/MemberImage/" + photoName));
                //var CLIENT_ID = "b6ff140d7b00eef";
                //var CLIENT_SECRET = "5008867fee0b01b1a3e9ccbc0d82ccee76290f7d";


                ////建立ImgurClient(其中的"CLIENT_ID", "CLIENT_SECRET"要換成你自己的)
                //var client = new ApiClient(CLIENT_ID, CLIENT_SECRET);
                //var httpClient = new HttpClient();
                //var endpoint = new ImageEndpoint(client, httpClient);
                ////var ImgPath = Environment.GetEnvironmentVariable("HOME") +
                ////           "\\site\\wwwroot\\Image\\MemberImage"+ photoName;
                //var ImgPath = Server.MapPath(@"~/Image/MemberImage/" + photoName);
                //IImage image;
                ////取得圖片檔案FileStream
                //using (var fs = new FileStream(ImgPath, FileMode.Open))
                //{
                //    image = endpoint.UploadImageAsync(fs).GetAwaiter().GetResult();
                //}
                cMember.fPhoto ="../Image/MemberImage/MemberPreset.jpg";
                //System.IO.File.Delete(ImgPath);



                //cMember.fPhoto = "../Image/MemberImage/" + photoName;
                CMemberFactory.fn會員更新(cMember);
                Session[CMemberSession.Session_Login_User] = cMember;
                return View("../Member/MemberSetup", "_Layout");
            }
            else
            {
                return View("../Member/MemberSetup", "_Layout");
            }
        }

        public string PasswordEdit(CMemberEditor m) 
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);

            
             if (m.NewfPassword == null)
            {
                ViewBag.Error = "!不可為空，請輸入密碼";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Edit_Password] = editPassword;
                return ViewBag.Error;
            }
            else if (m.NewfPassword.Length < 5 || m.NewfPassword.Length > 15)
            {
                ViewBag.Error = "!密碼請符合，5字元-15字元之間";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Edit_Password] = editPassword;
                return ViewBag.Error;
            }
            else if (cMember.fPassword != m.NewfPassword)
            {
                ViewBag.Error = "!密碼不符";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Edit_Password] = editPassword;
                return ViewBag.Error;
            }
            else 
            {
                ViewBag.PasswordTrue = "密碼相符";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.PasswordTrue;
                Session[CMemberSession.Session_Edit_Password] = editPassword;
                return ViewBag.PasswordTrue;
            }
        }


        public string PasswordChange(CMemberEditor m)
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fMemberId == member.fMemberId);


            if (m.NewfPassword == null || m.ChackNewfPassword == null)
            {
                ViewBag.Error = "!不可為空，請輸入密碼";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Change_Password] = editPassword;
                return ViewBag.Error;
            }
            else if (m.NewfPassword != m.ChackNewfPassword)
            {
                ViewBag.Error = "!與上方密碼不相符";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Change_Password] = editPassword;
                return ViewBag.Error;
            }
            else if (m.NewfPassword.Length < 5 || m.NewfPassword.Length > 15 )
            {
                ViewBag.Error = "!密碼請符合，5字元-15字元之間";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Change_Password] = editPassword;
                return ViewBag.Error;
            }
            else if (cMember.fPassword == m.NewfPassword)
            {
                ViewBag.Error = "!新密碼與舊密碼相符";
                CMemberEditPassword editPassword = new CMemberEditPassword();
                editPassword.PasswordMassage = ViewBag.Error;
                Session[CMemberSession.Session_Change_Password] = editPassword;
                return ViewBag.Error;
            }
            else
            {
                ViewBag.PasswordTrue = "修改成功";
                cMember.fPassword = m.ChackNewfPassword;
                CMemberFactory.fn會員更新(cMember);
                Session[CMemberSession.Session_Login_User] = cMember;
                return ViewBag.PasswordTrue;
                
            }
        }

        //忘記密碼+寄信Email------------------------------------------------------------------------------
        public ActionResult ForgetPassword()
        {
            Session[CMemberSession.Session_Edit_Password] = null;
            Session[CMemberSession.Session_Change_Password] = null;
            Session[CMemberSession.Session_Login_User] = null;
            Session[CMemberSession.Session_Message_Count] = null;
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(CMember m)
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fAccount == m.fAccount);
            if (cMember != null)
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.To.Add(m.fAccount);
                msg.From = new MailAddress("simplefree7077@gmail.com", "Not$edge官方網站", System.Text.Encoding.UTF8);
                msg.Subject = "Not$edge官方網站-忘記密碼連結網址";//郵件標題
                msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼
                msg.Body = "請點擊此連結-> https://noteledge.azurewebsites.net/Member/PasswordSetup/" + cMember.fThePasswordURL; //郵件內容
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                msg.IsBodyHtml = true;//是否是HTML郵件 
                                      //msg.Priority = MailPriority.High;//郵件優先級 

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("simplefree7077@gmail.com", "inno2100"); //這裡要填正確的帳號跟密碼
                client.Host = "smtp.gmail.com"; //設定smtp Server
                client.Port = 25; //設定Port
                client.EnableSsl = true; //gmail預設開啟驗證
                client.Send(msg); //寄出信件
                client.Dispose();
                msg.Dispose();
                ViewBag.Message = m.fAccount + "已發送驗證成功，請查看您的Email";
                return View("../Member/ForgetPassword", "_Layout");
            }
            else
            {
                ViewBag.Message = "!查無此信箱填寫不正確，請重新輸入";
                return View("../Member/ForgetPassword", "_Layout");
            }
        }

        //更換密碼-------------------------------------------------------------------------------\\
        public ActionResult PasswordSetup(string id)
        {
            Session[CMemberSession.Session_Edit_Password] = null;
            Session[CMemberSession.Session_Change_Password] = null;
            Session[CMemberSession.Session_Login_User] = null;
            Session[CMemberSession.Session_Message_Count] = null;
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fThePasswordURL == id);
            if (cMember != null)
            {
                ViewBag.MD5 = id;
                return View("../Member/PasswordSetup", "_Layout");
            }
            else
            {
                ViewBag.Message = "!再重新操作一次";
                return View("../Member/ForgetPassword", "_Layout");
            }
        }
        [HttpPost]
        public ActionResult PasswordSetup(CMemberEditor m, string id)
        {
            List<CMember> SELECTMember = CMemberFactory.fn會員查詢();
            CMember cMember = SELECTMember.FirstOrDefault(n => n.fThePasswordURL == id);
            if (cMember != null && m.ChackNewfPassword == m.NewfPassword)
            {
                cMember.fPassword = m.ChackNewfPassword;
                CMemberFactory.fn會員更新(cMember);
                id = "";
                return RedirectToAction("../Member/Login");
            }
            else
            {
                ViewBag.Error = "!與上方密碼不相符";
                return View("../Member/PasswordSetup", "_Layout");
            }

        }

        //會員訊息查詢-----------------------------------------------------\\
        public ActionResult MemberNotice() 
        {
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            if (member != null) 
            {
                var SelectNotices = CNoticeFactory.fn通知訊息查詢(member).ToList().OrderByDescending(n =>n.fNoticeDatetime);
                var Count銷售 = CNoticeFactory.fn通知訊息查詢(member).Where(n =>n.fCategoryType == "銷售").Count();
                var Count留言 = CNoticeFactory.fn通知訊息查詢(member).Where(n => n.fCategoryType == "評價留言").Count();
                ViewBag.Count銷售 = Count銷售;
                ViewBag.Count留言 = Count留言;
                return View("../Member/MemberNotice", SelectNotices); 
            }
            return RedirectToAction("../Home/Index"); 
        }


        //會員購賣紀錄---------------------------------------------------------\\\
        public ActionResult MemberOrder()
        {
            CMember member = Session[CMemberSession.Session_Login_User] as CMember;
            if (member != null) 
            {
                var SelectMemberOrder = CMemberFactory.fn會員訂單個人查詢(member).OrderByDescending(n => n.fLaunchDate);
                var Count訂單 = SelectMemberOrder.Count();
                ViewBag.Count訂單 = Count訂單;
                return View("../Member/MemberOrder", SelectMemberOrder);
            }
            return RedirectToAction("../Home/Index");
        }

        //成為VIP---------------------------------------------------------------------\\
        public ActionResult LvVIP()
        {
            //創造一組亂數字串不重複的訂單編號
            var str = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefhijklmnorstuvwxz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }

            //MerchantID(不可變動)，MerchantTradeNo(亂數訂單編號)，MerchantTradeDate(抓取當前日期時間)
            int MerchantID = 2000132;
            var MerchantTradeNo = builder;
            string MerchantTradeDate = DateTime.UtcNow.AddHours(08).ToString("yyyy/MM/dd hh:mm:ss");
            string ReturnUrl = "https://localhost:44300/Home/MyHome/" + MerchantTradeNo;
            string ProductName = "Notedge尊爵鑽石豪華VIP頂級會員";
            int Amount = 99;
            //把需要的資料作串接
            string Url = "HashKey=5294y06JbISpM5x9&ChoosePayment=ALL&ChooseSubPayment=&ClientBackURL="+ ReturnUrl + "&EncryptType=1&ItemName="
                + ProductName
                + "&MerchantID="
                + MerchantID
                + "&MerchantTradeDate="
                + MerchantTradeDate
                + "&MerchantTradeNo="
                + MerchantTradeNo
                + "&PaymentType=aio&ReturnURL=" + ReturnUrl + "&StoreID=&TotalAmount=" + Amount + "&TradeDesc=建立全金流測試訂單&HashIV=v77hoKGq4kWxNNIS";
            //串接好的資料轉成Encoded
            var Encoded = System.Web.HttpUtility.UrlEncode(Url);
            //Encoded 轉成 小寫 encoded
            var encoded = Encoded.ToLower();
            //呼叫sha256_hash(encoded)轉換成SHA256 在轉換大寫
            string SHA256 = sha256_hash(encoded).ToUpper();
            //把資料傳到前端
            ViewBag.MerchantID = MerchantID;
            ViewBag.MerchantTradeNo = MerchantTradeNo;
            ViewBag.MerchantTradeDate = MerchantTradeDate;
            ViewBag.SHA256 = SHA256;
            ViewBag.Url = ReturnUrl;
            ViewBag.ProductName = ProductName;
            ViewBag.Amount = Amount;
            return View("../Member/LvVIP", "_LayoutMember");
        }


        //金流交易API---------------------------------------------------------------------\\
        public ActionResult FlowAPI() 
        {
            //創造一組亂數字串不重複的訂單編號
            var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var next = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                builder.Append(str[next.Next(0, str.Length)]);
            }

            //MerchantID(不可變動)，MerchantTradeNo(亂數訂單編號)，MerchantTradeDate(抓取當前日期時間)
            int MerchantID = 2000132;
            var MerchantTradeNo = builder;
            string MerchantTradeDate = DateTime.UtcNow.AddHours(08).ToString("yyyy/MM/dd hh:mm:ss");
            string ReturnUrl = "https://localhost:44300/Member/Login";
            string ProductName = "Notedge尊爵鑽石豪華VIP頂級會員";
            int Amount = 150;
            //把需要的資料作串接
            string Url = "HashKey=5294y06JbISpM5x9&ChoosePayment=ALL&ChooseSubPayment=&ClientBackURL=https://localhost:44300/Member/Login&EncryptType=1&ItemName="
                + ProductName 
                + "&MerchantID="
                + MerchantID
                + "&MerchantTradeDate="
                + MerchantTradeDate
                + "&MerchantTradeNo="
                + MerchantTradeNo
                + "&PaymentType=aio&ReturnURL=https://localhost:44300/Member/Login&StoreID=&TotalAmount="+ Amount + "&TradeDesc=建立全金流測試訂單&HashIV=v77hoKGq4kWxNNIS";
            //串接好的資料轉成Encoded
            var Encoded = System.Web.HttpUtility.UrlEncode(Url);
            //Encoded 轉成 小寫 encoded
            var encoded = Encoded.ToLower();
            //呼叫sha256_hash(encoded)轉換成SHA256 在轉換大寫
            string SHA256 = sha256_hash(encoded).ToUpper();
            //把資料傳到前端
            ViewBag.MerchantID = MerchantID;
            ViewBag.MerchantTradeNo = MerchantTradeNo;
            ViewBag.MerchantTradeDate = MerchantTradeDate;
            ViewBag.SHA256 = SHA256;
            ViewBag.Url = ReturnUrl;
            ViewBag.ProductName = ProductName;
            ViewBag.Amount = Amount;
            return View();
        }
        //String字串轉成SHA256-------------------------------------------------------------------------\\
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


        //會員個人聊天商城通知-------------------------------------------------------------\\
        public ActionResult MemberMessage() 
        {
            CMember member123 = Session[CMemberSession.Session_Login_User] as CMember;
            var SelectChat = CChatFactory.fn聊聊查詢(member123);
            List<CMemberMessageViewModel> MemberMessageVM = new List<CMemberMessageViewModel>();

            foreach (var i in SelectChat)
            {
                if (i.fMemberTo == member123.fMemberId)
                {
                    MemberMessageVM.Add(new CMemberMessageViewModel()
                    {
                        fMemberFrom = i.fMemberFrom,
                        fMessage = i.fMessage,
                        fSubmitDateTime = i.fSubmitDateTime.ToString()

                    });
                }
            }
            int Count = MemberMessageVM.Count();
            return PartialView(MemberMessageVM);
        }
    }
}