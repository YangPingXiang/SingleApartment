using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModels;
using PagedList;

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
                //string sMemberEmail = "";
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
        public ActionResult MessageList(int pageNum = 1, string orderByString = "ByDateAsc")
        {            
            try
            {
                int pageSize = 5;
                int currentPage = pageNum < 1 ? 1 : pageNum;

                ViewBag.OrderBy = orderByString;  //將資料傳給view

                SingleApartmentEntities db = new SingleApartmentEntities();
                IEnumerable<Message> message = null;

                if (orderByString == "ByDateAsc")
                {
                    message = db.Message.OrderBy(p => p.MessageDate);
                }
                else
                {
                    message = db.Message.OrderByDescending(p => p.MessageDate);
                }

                List<CMessage> list = new List<CMessage>();
                foreach (Message d in message)
                {
                    list.Add(new CMessage() { message_entity = d });
                }
                var pagelist = list.ToPagedList(currentPage, pageSize);
                return View(pagelist);
                //return View(list);
            }
            catch
            {
                return View();
            }
        }

        // GET: Create
        public ActionResult MessageCreate()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public JsonResult MessageCreate(Message p_ms)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();

                p_ms.MessageDate = DateTime.Now;
                db.Message.Add(p_ms);
                db.SaveChanges();

                return new JsonResult { Data = new { status = true } };
            }
            catch
            {
                return new JsonResult { Data = new { status = false } };
            }
        }

        // POST: Create
        [HttpPost]
        public JsonResult MessageEdit(Message p_ms)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                Message mg = db.Message.FirstOrDefault(p => p.MessageID == p_ms.MessageID);
                if (mg != null)
                {
                    //info.InformationSource = p_info.InformationSource;
                    mg.AdminContent = p_ms.AdminContent;
                    db.SaveChanges();
                }

                return new JsonResult { Data = new { status = true } };
            }
            catch
            {
                return new JsonResult { Data = new { status = false } };
            }
        }

    }
}