using sln_SingleApartment.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using sln_SingleApartment.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using CaptchaMvc.HtmlHelpers;
using Newtonsoft.Json.Linq;

namespace sln_SingleApartment.Controllers
{
    public class MemberController : Controller
    {

        SingleApartmentEntities db = new SingleApartmentEntities();
        //後台會員列表   12月2號-修改將參考型別改為CMemberRister
        public ActionResult List()
        {
            var table = from p in db.tMember
                        //orderby p.fLeave
                        select p;
            //List<CMemberRegister> list = new List<CMemberRegister>();
            //foreach (tMember p in table)
            //{
            //    list.Add(new CMemberRegister() { entity = p });
            //}
            return View(table);
        }

        //  最一開始的首頁
        public ActionResult HomePage()
        {
            return View();
        }

        //登入完轉到首頁的畫面
        public ActionResult Home()
        {
            return View();
        }

        public string GoLogin(string returnUrl)
        {
            google ms = JsonConvert.DeserializeObject<google>(returnUrl);

            var member = db.tMember.Where(x => x.fEmail == ms.du).FirstOrDefault();
            if (member == null)
            {
                tMember t = new tMember();
                t.fEmail = ms.du;
                t.fMemberName = ms.Ad;
                t.fAccount = ms.du;
                t.fPassword = ms.OT;
                db.tMember.Add(t);
                db.SaveChanges();
                member = db.tMember.Where(x => x.fEmail == ms.du).FirstOrDefault();



            }
            CMember c = new CMember();
            c.fMemberId = member.fMemberId;
            c.fMemberName = member.fMemberName;
            c.fAccount = member.fAccount;
            c.fPassword = member.fPassword;
            c.fEmail = member.fEmail;
            c.fRoomId = member.fRoomId;
            c.fPhone = member.fPhone;
            c.fAge = member.fAge;
            c.fSex = member.fSex;
            c.fBirthDate = member.fBirthDate;
            c.fSalary = member.fSalary;
            c.fCareer = member.fCareer;
            c.fImage = member.fImage;
            c.fLeave = member.fLeave;
            c.fRole = member.fRole;
            Session[CDictionary.welcome] = c;
            return "驗證成功";
        }



        //  登入
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(CLogIn login, FormCollection form)
        {

            //if (!this.IsCaptchaValid(""))
            //{
            //    ViewBag.ErrorMessage = "會不會算數?";
            //    return View("LogIn", login);
            //}

            login.txtAccount = Request.Form["txtaccount"];
            login.txtPassword = Request.Form["txtpwd"];
            CMember cm = (new CMember_Factory()).isAuthticated(login.txtAccount, login.txtPassword);
            var client = new System.Net.WebClient();
            var s = form["g-recaptcha-response"];
            var googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", "6Lc0YQwaAAAAADHlm1si34fV_VD70AEuEN-4-A8j", s));
            dynamic Json = JObject.Parse(googleReply);
            var isVerify = Json.success;


            if (cm != null)
            {
                if (isVerify=="false")
                {
                    ViewBag.ErrorMessage = "證明你是人類";
                    return View("LogIn", login);

                }

                Session[CDictionary.welcome] = cm;
                CMember member = Session[CDictionary.welcome] as CMember;
                return RedirectToAction("Home");
            }
            else
            {
                ViewBag.msg = "帳號或密碼錯誤，請重新輸入正確帳號密碼!";
            }
            return View();
        }
        //    public ActionResult Robot(FormCollection form, CLogIn login)
        //{
        //    var isVerify = new GoogleReCaptcha().GetCaptchaResponse(form["g-recaptcha-response"]);
        //    if (isVerify)
        //    {
        //        login.txtAccount = Request.Form["txtaccount"];
        //        login.txtPassword = Request.Form["txtpwd"];
        //        CMember cm = (new CMember_Factory()).isAuthticated(login.txtAccount, login.txtPassword);
        //        if (cm != null)
        //        {
        //            Session[CDictionary.welcome] = cm;
        //            CMember member = Session[CDictionary.welcome] as CMember;

        //            return RedirectToAction("Home");
        //        }
        //        else
        //        {
        //            ViewBag.msg = "帳號或密碼錯誤，請重新輸入正確帳號密碼!";

        //        }

        //    }
        //   
        //}


        //註冊
        public ActionResult Register()
        {
            return View();
        }

        //12月2號更新( 將tMember型態轉換為CMember )
        [HttpPost]
        public ActionResult Register(CMember input)//此處更新
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }
            var member = db.tMember.Where(p => p.fAccount == input.fAccount).FirstOrDefault();

            if (member == null)
            {
                int index = input.myImage.FileName.IndexOf(".");
                string extention = input.myImage.FileName.Substring(index, input.myImage.FileName.Length - index);
                string photoName = Guid.NewGuid().ToString() + extention;
                input.fImage = "../Content/" + photoName;
                input.myImage.SaveAs(Server.MapPath("../Content/") + photoName);

                //此處新增---------------------------
                tMember t = new tMember();
                t.fMemberId = input.fMemberId;
                t.fMemberName = input.fMemberName;
                t.fAccount = input.fAccount;
                t.fPassword = input.fPassword;
                t.fEmail = input.fEmail;
                t.fRoomId = input.fRoomId;
                t.fPhone = input.fPhone;
                t.fAge = input.fAge;
                t.fSex = input.fSex;
                t.fBirthDate = input.fBirthDate;
                t.fSalary = input.fSalary;
                t.fCareer = input.fCareer;
                t.fImage = input.fImage;
                t.fLeave = input.fLeave;
                t.fRole = input.fRole;
                //----------------------------------


                db.tMember.Add(t);
                db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            ViewBag.Message = "此帳號已有人使用，請輸入新的帳號";
            return View();
        }

        //public string Check()
        //{
        //    string val1 = Request["val1"].ToString();
        //    string result="0";
        //    var member = db.tMember.Where(p => p.fMemberName == val1).FirstOrDefault();
        //    if (member != null)
        //        result = "1";
        //    return result;
        //}

        //登出
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("HomePage");
        }

        //修改
        public ActionResult Edit(int id)
        {
            var table = db.tMember.Where(p => p.fMemberId == id).FirstOrDefault();
            if (table == null)
            {
                return RedirectToAction("Home");
            }
            //12月2號修改------
          
            CMember c = new CMember();
            c.fMemberId = table.fMemberId;
            c.fMemberName = table.fMemberName;
            c.fAccount = table.fAccount;
            c.fPassword = table.fPassword;
            c.fEmail = table.fEmail;
            c.fRoomId = table.fRoomId;
            c.fPhone = table.fPhone;
            c.fAge = table.fAge;
            //c.fSex = table.fSex;
            //c.fBirthDate = table.fBirthDate;
            c.fSalary = table.fSalary;
            c.fCareer = table.fCareer;
            c.fImage = table.fImage;
            c.fLeave = table.fLeave;
            //c.fRole = table.fRole;
            return View(c);
        }

        [HttpPost]
        public ActionResult Edit(CMember cm)
        {
            var table = db.tMember.Where(p => p.fMemberId == cm.fMemberId).FirstOrDefault();
            if (cm != null)
            {
                if (cm.myImage != null)
                {
                    int index = cm.myImage.FileName.IndexOf(".");
                    string extention = cm.myImage.FileName.Substring(index, cm.myImage.FileName.Length - index);
                    string photoName = Guid.NewGuid().ToString() + extention;

                    cm.fImage = "../Content/" + photoName;
                    cm.myImage.SaveAs(Server.MapPath("~/Content/") + photoName);
                }
                //12月2號新增------------------
                table.fMemberId = cm.fMemberId;
                table.fMemberName = cm.fMemberName;
                table.fAccount = cm.fAccount;
                table.fPassword = cm.fPassword;
                table.fEmail = cm.fEmail;
                table.fRoomId = cm.fRoomId;
                table.fPhone = cm.fPhone;
                table.fAge = cm.fAge;
                //table.fSex = cm.fSex;
                //table.fBirthDate = cm.fBirthDate;
                table.fSalary = cm.fSalary;
                table.fCareer = cm.fCareer;
                if (cm.fImage != null)
                {
                    table.fImage = cm.fImage;
                }
                table.fLeave = cm.fLeave;
                //table.fRole = cm.fRole;
                //-------------------------------------
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }

        //  找房子
        public ActionResult Room()
        {
            return View();
        }

        //  找商品
        public ActionResult Product()
        {
            return View();
        }

        //  會員中心
        public ActionResult MemberCenter()
        {
            CMember me = Session[sln_SingleApartment.Models.CDictionary.welcome] as CMember;
            var tm = db.tMember.Where(m => m.fMemberId == me.fMemberId).FirstOrDefault();

            CMultiple cm = new CMultiple();
            cm.t = tm;

            //Room
            var tr = db.Lease.Where(l => l.MemberID == me.fMemberId).FirstOrDefault();
            if (tr != null)
            {
                cm.r = tr.Room;
                cm.rs = tr.Room.RoomStyle;
            }

            //Activity

            //var z = db.Activity.Where(a => a.MemberID == me.fMemberId).FirstOrDefault();
            var ta = db.Activity.Where(a => a.MemberID == me.fMemberId).ToList();
            //var ta = (from p in db.Activity
            //          from m in db.Participant
            //          where p.ActivityID == m.ActivityID && m.MemberID == me.fMemberId
            //          select p).ToList();

            //var ta1 =db.Activity.Where(p=> p.MemberID == me.fMemberId).ToList();

            var ta2 = (from m in db.Participant
                       where m.MemberID == me.fMemberId
                       select m).ToList();

            //List<Participant> pa = new List<Participant>();
            //foreach(var i in ta)
            //{
            //    var p = db.Participant.Where(s => s.ActivityID == i.ActivityID).ToList();
            //    if (p != null)
            //    {
            //        foreach(var v in p)
            //        {
            //            pa.Add(v);
            //        }
            //    }
            //    //activities.Add(ac);

            //}

            cm.part = ta2;


            List<Product> products = new List<Product>();
            foreach (var i  in ta)
            {
                var p = db.Product.Where(s => s.ActivityID == i.ActivityID).ToList();
                if (p != null)
                {
                    foreach(var v in p)
                    {
                        products.Add(v);
                    }
                } 
            }
            cm.prod = products;
            return View(cm);
        }






        public JsonResult CheckName(string cn)
        {
            System.Threading.Thread.Sleep(800);
            var search = db.tMember.Where(p => p.fMemberName == cn).FirstOrDefault();
            if (search != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        public JsonResult CheckAccount(string ca)
        {
            System.Threading.Thread.Sleep(800);
            var search = db.tMember.Where(p => p.fAccount == ca).FirstOrDefault();
            if (search != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }
        public JsonResult CheckEmail(string ce)
        {
            System.Threading.Thread.Sleep(800);
            var search = db.tMember.Where(p => p.fEmail == ce).FirstOrDefault();
            if (search != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }
        public JsonResult CheckPhone(string cp)
        {
            System.Threading.Thread.Sleep(800);
            var search = db.tMember.Where(p => p.fPhone == cp).FirstOrDefault();
            if (search != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }
        public ActionResult Chat()
        {
            if (Session[CDictionary.welcome] as CMember == null) { return RedirectToAction("Login"); }
            return View();
        }
        public ActionResult ChatRoom()
        {
            if (Session[CDictionary.welcome] as CMember == null) { return RedirectToAction("Login"); }
            CMember cMember = Session[CDictionary.welcome] as CMember;
            var user = db.tMember.Where(m => m.fMemberId == cMember.fMemberId).FirstOrDefault();
            return View(user);
        }
    }
}
