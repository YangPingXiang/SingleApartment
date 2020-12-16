using sln_SingleApartment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;      //使用PagedList套件必須引用此命名空間
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModels;

namespace sln_SingleApartment.Controllers
{
    public class BackInformationController : Controller
    {
        public ActionResult JobList()
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                IEnumerable<JobService> job = from m in db.JobService select m;

                List<CJobService> list = new List<CJobService>();
                foreach (JobService d in job)
                {
                    list.Add(new CJobService() { job_service_entity = d });
                }
                return View(list);
            }
            catch
            {
                return View();
            }            
        }

        // GET: Create
        public ActionResult JobCreate()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public ActionResult JobCreate(JobService p_job)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();

                db.JobService.Add(p_job);
                db.SaveChanges();
                return RedirectToAction("JobList");
            }
            catch
            {
                return RedirectToAction("JobList");
            }
        }

        // GET: Edit/5
        public ActionResult JobEdit(int id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                JobService job = db.JobService.Where(p => p.JobID == id).FirstOrDefault();

                if (job == null)
                    return RedirectToAction("JobList");

                return View(new CJobService() {  job_service_entity = job });
            }
            catch
            {
                return RedirectToAction("JobList");
            }            
        }

        [HttpPost]
        public ActionResult JobEdit(JobService p_job)
        { //TODO: FormCollection collection==>不要用 
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                JobService job = db.JobService.FirstOrDefault(p => p.JobID == p_job.JobID);
                if (job != null)
                {
                    job.JobName = p_job.JobName;
                    job.JobDescription = p_job.JobDescription;
                    job.JobCycle = p_job.JobCycle;
                    job.BeforeDays = p_job.BeforeDays;

                    db.SaveChanges();
                }
                return RedirectToAction("JobList");
            }
            catch
            {
                //return View();
                return RedirectToAction("JobList");
            }
        }

        // GET
        public ActionResult JobDelete(int id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                JobService info = db.JobService.FirstOrDefault(p => p.JobID == id);
                if (info != null)
                {
                    db.JobService.Remove(info);
                    db.SaveChanges();
                }
                return RedirectToAction("JobList");
            }
            catch
            {
                return RedirectToAction("JobList");
            }
        }



        public string GetInformationCategory()
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                IEnumerable<InformationCategory> info = from p in db.InformationCategory select p;

                string result = "";
                if (info != null)
                {
                    result = "[";

                    foreach (var c in info)
                    {
                        if (result != "[") result += ",";  //務必加 ,

                        //組JSON字串
                        result = result + "{" + $"\"ID\":{c.InformationCategoryID},\"NAME\":\"{c.InformationCategoryName}\"" + "}";
                    }
                    result += "]";
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        public void update_status(int id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                Information information = db.Information.FirstOrDefault(p => p.InformationID == id);

                if (information != null)
                {
                    information.Status = "Admin_Deleted";
                    db.SaveChanges();
                }
            }
            catch
            {

            }
        }
        //return RedirectToAction("List");

        public string GetMemberID(string account)
        {
            string result = "0";
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                tMember m = db.tMember.Where(p => p.fAccount.Contains(account)).FirstOrDefault();

                if (m != null)
                {
                    result = m.fMemberId.ToString();
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }
        
        public ActionResult BackList()
        {
            //modify by Jony 109-12-15
            CMember member = Session[CDictionary.welcome] as CMember;
            if (member == null) { return RedirectToAction("Login", "Member"); }

            return View();
        }

        // GET: Information
        public ActionResult BackListPaging(int pageNum = 1, string p_priority = "ALL", string p_read_yn = "N", string p_query_type = "999", string p_data = "N")
        {
            try
            {
                int pageSize = 5;
                int currentPage = pageNum < 1 ? 1 : pageNum;

                ViewBag.Read_YN = p_read_yn;  //將partialview資料傳給 主要view
                ViewBag.Priority = p_priority;
                ViewBag.Query_Type = p_query_type;
                ViewBag.Query_Data = p_data;

                //int memberID = 1;
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                Func<Information, bool> myWhere = null;
                SingleApartmentEntities db = new SingleApartmentEntities();

                IEnumerable<Information> table = null;
                //myWhere = p => p.Status != "User_Deleted";   //todo:ok  Linq多重where

                //if (string.IsNullOrEmpty(read_yn))

                //modify by Jony 1091205 後台程式, 移除 p.MemberID == memberID 這個 where 條件
                //p_priority == "ALL" 顯示全部資料
                if (p_query_type == "999" && p_priority == "ALL")
                {
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted";
                }
                else if (p_query_type == "999" && p_priority == "Null")
                {   //p_priority == "Null" 顯示已讀或未讀資料
                    //移除 p.Priority == p_priority
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.Read_YN == p_read_yn;
                }
                else if (p_query_type == "999" && p_read_yn == "Null")
                {   //p_read_yn == "Null" 顯示優先等級資料
                    //移除 p.Read_YN == p_read_yn
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.Priority == p_priority;
                }
                //<option value="999">請選擇查詢項目</option>
                //    <option value="100">系統分類</option>
                //    <option value="200">個人分類</option>
                //    <option value="300">關鍵字</option>
                else if (p_query_type == "100")
                {//100 = 系統分類
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.InformationCategoryID == Convert.ToInt32(p_data);
                }
                else if (p_query_type == "200")
                {//200 = 使用者帳號
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.MemberID == Convert.ToInt32(p_data);
                }
                else if (p_query_type == "300")
                {//300 = 關鍵字
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.InformationContent.Contains(p_data);
                }
                else if (p_query_type == "400")
                {//全部資料
                    myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted";
                }
                else
                {   //no use
                    //myWhere = p => p.Status != "User_Deleted" && p.Status != "Admin_Deleted" && p.Read_YN == p_read_yn && p.Priority == p_priority;                
                }
                table = db.Information.Where(myWhere);
                
                List<CInformation> list = new List<CInformation>();
                foreach (Information item in table)
                {
                    list.Add(new CInformation()
                    {
                        information_entity = item,
                        InformationCategoryName = item.InformationCategory.InformationCategoryName,
                        //三元運算子
                        UserCategoryName = item.MemberCategoryID == null ? "未分類" : item.MemberInformationCategory.MemberCategoryName,
                        //UserCategoryName = item.MemberInformationCategory.MemberCategoryName == null ? "未分類": item.MemberInformationCategory.MemberCategoryName
                    });

                    //CInformation x = new CInformation();
                    //x.information_entity = item;
                    //x.InformationCategoryName = item.InformationCategory.InformationCategoryName;
                    //if (x.MemberCategoryID != null)
                    //{
                    //    MemberInformationCategory c = db.MemberInformationCategory.Where(p => p.MemberCategoryID == item.MemberCategoryID).FirstOrDefault();

                    //    //沒設關聯, 為了取得 UserCategoryName = MemberInformationCategory.MemberCategoryName
                    //    if (c != null)
                    //        x.UserCategoryName = c.MemberCategoryName;
                    //}
                    //list.Add(x);
                }

                //return View(list);
                var pagedlist = list.ToPagedList(currentPage, pageSize);

                //return View(pagedlist);  //Page_原始ok 此cshtml使用
                return PartialView(pagedlist);//使用部分顯示                
            }
            catch
            {
                return View();
            }
        }

        //BackList =========================================



        //InfoCategoryList ==================================
        public ActionResult InfoCategoryList(int page = 1)
        {
            int pageSize = 4;
            SingleApartmentEntities db = new SingleApartmentEntities();
            try
            {
                int currentpage = page < 1 ? 1 : page;

                IEnumerable<InformationCategory> table = from p in db.InformationCategory
                                                         select p;

                //List<CInformationCategory> list = new List<CInformationCategory>().ToPagedList(currentpage,pageSize).ToList();
                List<CInformationCategory> list = new List<CInformationCategory>();
                foreach (InformationCategory d in table)
                {
                    list.Add(new CInformationCategory() { infoCategory_entity = d });
                }

                var pagelist = list.ToPagedList(currentpage, pageSize);
                return View(pagelist);
            }
            catch
            {
                return View();
            }
        }
        
        // GET: Create
        public ActionResult InfoCategoryCreate()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public ActionResult InfoCategoryCreate(InformationCategory p_info)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();

                db.InformationCategory.Add(p_info);
                db.SaveChanges();
                return RedirectToAction("InfoCategoryList");
            }
            catch
            {
                return RedirectToAction("InfoCategoryList");
            }
        }

        // GET: Edit/5
        public ActionResult InfoCategoryEdit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult InfoCategoryEdit(InformationCategory p_info)
        { //TODO: FormCollection collection==>不要用 
            try
            {                
                SingleApartmentEntities db = new SingleApartmentEntities();
                InformationCategory info = db.InformationCategory.FirstOrDefault(p => p.InformationCategoryID == p_info.InformationCategoryID);
                if (info != null)
                {
                    //info.InformationSource = p_info.InformationSource;
                    info.InformationCategoryName = p_info.InformationCategoryName;
                    db.SaveChanges();
                }
                return RedirectToAction("InfoCategoryList");
            }
            catch
            {
                //return View();
                return RedirectToAction("InfoCategoryList");
            }
        }

        // GET
        public ActionResult InfoCategoryDelete(int id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                InformationCategory info = db.InformationCategory.FirstOrDefault(p => p.InformationCategoryID == id);
                if (info != null)
                {
                    db.InformationCategory.Remove(info);
                    db.SaveChanges();
                }
                return RedirectToAction("InfoCategoryList");
            }
            catch
            {
                return RedirectToAction("InfoCategoryList");
            }
        }
    }
}
