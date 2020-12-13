using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModels;

namespace sln_SingleApartment.Controllers
{
    public class MailMessageController : Controller
    {


        public ActionResult CKEditorShow()
        {
            return View();
        }

        public string SendMail(string strHtml)
        {
            string htmlBody = strHtml.ToString();
         
            SingleApartmentEntities entity = new SingleApartmentEntities();
            try
            {
                string sMemberEmail = "";
                MailMessage mail = new MailMessage();
                //string email = "dddd";
                mail.From = new MailAddress("singleapart@gmail.com");
                //多收信人, 使用,隔開, 而不是;喔

                #region 活動建立後媒合訊息發送
                List<int> MemberIDList = new List<int>();
                List<string> MemberMemberEmailList = new List<string>();
             
                tMember tMember = new tMember();
                var membermessage = from mbmsg in entity.tMember
                                    select new { MbID = mbmsg.fMemberId, MbEmail = mbmsg.fEmail};
                foreach (var m in membermessage)
                {
                    MemberIDList.Add(m.MbID);
                    MemberMemberEmailList.Add(m.MbEmail);
                }
                for (int me = 0; me < MemberIDList.Count; me++)
                {
                    //if(MemberfActivityMessageList[me] == "TRUE")
                    //{
                        
                        mail.To.Add(MemberMemberEmailList[me]);
                    //}
                  
                                       
                }
                #endregion

                //mail.To.Add("singleapart@gmail.com,apple385827@gmail.com"); 
               mail.To.Add("singleapart@gmail.com");//new MailAddress("singleapart@gmail.com")
                mail.Subject = "窩居公寓:新活動通知!";
                //mail.Date = DateTime.Now;
                
                mail.Body = htmlBody;
                //mail.Body = $@"<h1 style='text-align:center;color:#ff0000'>iTicket 訂購成功</h1>{email}";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
                {
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new NetworkCredential("singleapart@gmail.com", "wojuwoju");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}