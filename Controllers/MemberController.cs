using sln_SingleApartment.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using sln_SingleApartment.Models;
using System.Collections.Generic;

namespace sln_SingleApartment.Controllers
{
    public class MemberController : Controller
    {
        SingleApartmentEntities db = new SingleApartmentEntities();
        //後台會員列表   12月2號-修改將參考型別改為CMemberRister
        public ActionResult List()
        {
            var table = from p in db.tMember
                        orderby p.fLeave
                        select p;
            List<CMemberRegister> list = new List<CMemberRegister>();
            foreach (tMember p in table)
            {
                list.Add(new CMemberRegister() { entity = p });
            }
            return View(list);
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
        //  登入
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(CLogIn login)
        { 
            login.txtAccount = Request.Form["txtaccount"];
            login.txtPassword = Request.Form["txtpwd"];
            CMember cm = (new CMember_Factory()).isAuthticated(login.txtAccount, login.txtPassword);
            if (cm != null)
            {
                Session[CDictionary.welcome] = cm;
                CMember member = Session[CDictionary.welcome] as CMember;  

                return RedirectToAction("Home");
            }
            return View();
        }

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
            var member = db.tMember.Where(p => p.fMemberId == input.fMemberId).FirstOrDefault();

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
            CMemberRegister list = new CMemberRegister() { entity = table };
            return View(list);
        }

        [HttpPost]
        public ActionResult Edit(CMember cm)
        {
            var table = db.tMember.Where(p => p.fMemberId == cm.fMemberId).FirstOrDefault();
            if (cm != null)
            {
                int index = cm.myImage.FileName.IndexOf(".");
                string extention = cm.myImage.FileName.Substring(index, cm.myImage.FileName.Length - index);
                string photoName = Guid.NewGuid().ToString() + extention;

                cm.fImage = "../Content/" + photoName;
                cm.myImage.SaveAs(Server.MapPath("~/Content/") + photoName);
                //12月2號新增------------------
                tMember t = new tMember();
                t.fMemberId = cm.fMemberId;
                t.fMemberName = cm.fMemberName;
                t.fAccount = cm.fAccount;
                t.fPassword = cm.fPassword;
                t.fEmail = cm.fEmail;
                t.fRoomId = cm.fRoomId;
                t.fPhone = cm.fPhone;
                t.fAge = cm.fAge;
                t.fSex = cm.fSex;
                t.fBirthDate = cm.fBirthDate;
                t.fSalary = cm.fSalary;
                t.fCareer = cm.fCareer;
                t.fImage = cm.fImage;
                t.fLeave = cm.fLeave;
                t.fRole = cm.fRole;
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
            return View();
        }
    }
}
