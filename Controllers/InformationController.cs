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
    public class InformationController : Controller
    {
        public string GetInformationCategory()
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

        public string GetUserCategory()
        {
            //todo:
            int memberID = 1;

            //CMember member = Session[CDictionary.welcome] as CMember;
            //int memberID = member.fMemberId;         

            SingleApartmentEntities db = new SingleApartmentEntities();
            IEnumerable<MemberInformationCategory> info = from p in db.MemberInformationCategory
                                                          where p.MemberID == memberID
                                                          select p;

            string result = "";
            if (info != null)
            {
                result = "[";

                foreach (var c in info)
                {
                    if (result != "[") result += ",";  //務必加 ,

                    //組JSON字串
                    result = result + "{" + $"\"ID\":{c.MemberCategoryID},\"NAME\":\"{c.MemberCategoryName}\"" + "}";
                }
                result += "]";
            }
            return result;
        }

        public void update_read_yn(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Information information = db.Information.FirstOrDefault(p => p.InformationID == id);

            if (information != null)
            {
                information.Read_YN = "Y";
                db.SaveChanges();
            }
        }
        public void update_status(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Information information = db.Information.FirstOrDefault(p => p.InformationID == id);

            if (information != null)
            {
                information.Status = "User_Deleted";
                db.SaveChanges();
            }
            //return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View();
        }

        // GET: Information
        public ActionResult ListPaging(int pageNum = 1, string p_priority = "ALL", string p_read_yn = "N", string p_query_type = "999", string p_data = "N")
        {
            int pageSize = 5;
            int currentPage = pageNum < 1 ? 1 : pageNum;

            ViewBag.Read_YN = p_read_yn;  //將partialview資料傳給 主要view
            ViewBag.Priority = p_priority;

            int memberID = 1;
            //CMember member = Session[CDictionary.welcome] as CMember;
            //int memberID = member.fMemberId;  //(int)Session["MemberID"];
            
            Func<Information, bool> myWhere = null;
            SingleApartmentEntities db = new SingleApartmentEntities();
            
            IEnumerable <Information> table = null;
            //myWhere = p => p.Status != "User_Deleted";   //todo:ok  Linq多重where

            //if (string.IsNullOrEmpty(read_yn))

            //p_priority == "ALL" 顯示全部資料
            if (p_query_type == "999" && p_priority == "ALL") {
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted";
            }
            else if (p_query_type == "999" &&  p_priority == "Null")
            {   //p_priority == "Null" 顯示已讀或未讀資料
                //移除 p.Priority == p_priority
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.Read_YN == p_read_yn;
            }
            else if (p_query_type == "999" && p_read_yn == "Null")
            {   //p_read_yn == "Null" 顯示優先等級資料
                //移除 p.Read_YN == p_read_yn
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.Priority == p_priority;
            }
            //<option value="999">請選擇查詢項目</option>
            //    <option value="100">系統分類</option>
            //    <option value="200">個人分類</option>
            //    <option value="300">關鍵字</option>
            else if (p_query_type == "100")
            {//100 = 系統分類
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.InformationCategoryID == Convert.ToInt32(p_data);
            }
            else if (p_query_type == "200")
            {//200 = 個人分類
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.MemberCategoryID == Convert.ToInt32(p_data);
            }
            else if (p_query_type == "300")
            {//300 = 關鍵字
                myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.InformationContent.Contains(p_data);
            }
            else
            {   //no use
                //myWhere = p => p.MemberID == memberID && p.Status != "User_Deleted" && p.Read_YN == p_read_yn && p.Priority == p_priority;                
            }
            table = db.Information.Where(myWhere);

            //    myWhere = p => p.InformationContent.Contains(keyword) && p.Status != "User_Deleted";
            //    table = db.Information.Where(myWhere);
            //    //table = from p in db.Information
            //    //        where p.InformationContent.Contains(keyword) && p.Status != "User_Deleted"
            //    //        select p;
            //}
                        
            
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

        public ActionResult Edit(int id)
        {
            try
            {
                //todo:
                int memberID = 1;
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                SingleApartmentEntities db = new SingleApartmentEntities();
                var row = db.Information.Where(p => p.InformationID == id).FirstOrDefault();

                //MemberCategoryIDCategory ===========================
                IEnumerable<MemberInformationCategory> ic = from o in db.MemberInformationCategory
                                                            where o.MemberID == memberID
                                                            select o;
                List<SelectListItem> item = new List<SelectListItem>();
                item.Add(new SelectListItem { Value = "999", Text = "請選擇" });
                foreach (MemberInformationCategory m in ic)
                {
                    if (row.MemberCategoryID != null)
                    {
                        SelectListItem a = new SelectListItem()
                        {
                            Value = m.MemberCategoryID.ToString(),
                            Text = m.MemberCategoryName,
                            //selected 值
                            Selected = m.MemberCategoryID.Equals(row.MemberCategoryID)
                        };
                        item.Add(a);
                    }
                    else
                    {
                        SelectListItem a = new SelectListItem()
                        {
                            Value = m.MemberCategoryID.ToString(),
                            Text = m.MemberCategoryName,
                            //selected 值
                        };
                        item.Add(a);
                    }
                }
                ViewBag.MemberCategoryID = item;
                //MemberCategoryIDCategory ===========================

                Information info = db.Information.FirstOrDefault(p => p.InformationID == id);
                if (info == null)
                    return RedirectToAction("List");

                return View(new CInformation(){information_entity = info});
            }
            catch
            {
                return RedirectToAction("List");
            }

        }

        [HttpPost]
        public ActionResult Edit(Information p_info)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                Information info = db.Information.FirstOrDefault(p => p.InformationID == p_info.InformationID);
                if (info != null)
                {
                    //挑選了 "請選擇"項目
                    if (p_info.MemberCategoryID == 999)
                        info.MemberCategoryID = null;
                    else
                        info.MemberCategoryID = p_info.MemberCategoryID;

                    //優先等級
                    info.Priority = p_info.Priority;
                    db.SaveChanges();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("List");
            }

        }

        //no use
        //public ActionResult Delete(int id)
        //{
        //    SingleApartmentEntities db = new SingleApartmentEntities();
        //    Information information = db.Information.FirstOrDefault(p => p.InformationID == id);
        //    if (information != null)
        //    {
        //        //db.Information.Remove(information);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(Information info)
        //{
        //    SingleApartmentEntities db = new SingleApartmentEntities();
        //    db.Information.Add(info);
        //    db.SaveChanges();

        //    return RedirectToAction("List");
        //}
        
        public ActionResult UserCategoryList(int page = 1)
        {
            try
            {
                int memberID = 1;
                //todo:
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                int pageSize = 4;
                SingleApartmentEntities db = new SingleApartmentEntities();
                int currentpage = page < 1 ? 1 : page;

                IEnumerable<MemberInformationCategory> table = from p in db.MemberInformationCategory
                                                               where p.MemberID == memberID
                                                               orderby p.MemberCategoryID
                                                               select p;
                
                List<CMemberInformationCategory> list = new List<CMemberInformationCategory>();
                foreach (MemberInformationCategory d in table)
                {
                    list.Add(new CMemberInformationCategory() { mEntity = d });
                }

                var pagelist = list.ToPagedList(currentpage, pageSize);
                return View(pagelist);
            }
            catch
            {
                return RedirectToAction("UserCategoryList");
            }
        }
        
        //以下為 UserCategoryList View 使用

        [HttpPost]
        public ActionResult CategoryCreate(MemberInformationCategory p_info)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();

                int memberID = 1;
                //todo:
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                p_info.MemberID = memberID;
                db.MemberInformationCategory.Add(p_info);
                db.SaveChanges();

                return RedirectToAction("UserCategoryList");
            }
            catch
            {
                return RedirectToAction("UserCategoryList");
            }
        }

        //// GET: BackStage/Edit/5
        //public ActionResult CategoryEdit(int id)
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult CategoryEdit(MemberInformationCategory p_info)
        { //TODO: FormCollection collection==>不要用
            try
            {
                //todo:
                int memberID = 1;
                //todo:
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                SingleApartmentEntities db = new SingleApartmentEntities();
                MemberInformationCategory info = db.MemberInformationCategory.FirstOrDefault(p => p.MemberCategoryID == p_info.MemberCategoryID && p.MemberID == memberID);
                if (info != null)
                {
                    info.MemberCategoryName = p_info.MemberCategoryName;
                    db.SaveChanges();
                }
                return RedirectToAction("UserCategoryList");
            }
            catch
            {
                //return View();
                return RedirectToAction("UserCategoryList");
            }
        }
        
        //no use
        public void CategoryEdit_JS(int category_id, string category_name)  //MemberInformationCategory p_info
        { //TODO: FormCollection collection==>不要用
            try
            {   //todo:
                int memberID = 1;
                //todo:
                //CMember member = Session[CDictionary.welcome] as CMember;
                //int memberID = member.fMemberId;  //(int)Session["MemberID"];

                SingleApartmentEntities db = new SingleApartmentEntities();
                MemberInformationCategory info = db.MemberInformationCategory.FirstOrDefault(p => p.MemberCategoryID == category_id && p.MemberID == memberID);
                if (info != null)
                {
                    info.MemberCategoryName = category_name;   //p_info.MemberCategoryName;
                    db.SaveChanges();
                }
            }
            catch
            {
                //return View();
            }
        }

        // GET
        public ActionResult CategoryDelete(int member_id, int category_id)
        {
            try
            {
                SingleApartmentEntities db = new SingleApartmentEntities();
                MemberInformationCategory info = db.MemberInformationCategory.FirstOrDefault(p => p.MemberCategoryID == category_id && p.MemberID == member_id);
                if (info != null)
                {
                    db.MemberInformationCategory.Remove(info);
                    db.SaveChanges();
                }
                return RedirectToAction("UserCategoryList");
            }
            catch
            {
                return RedirectToAction("UserCategoryList");
            }
        }

        // POST
        //[HttpPost]
        //public ActionResult CategoryDelete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        SingleApartmentEntities db = new SingleApartmentEntities();
        //        InformationCategory info = db.InformationCategory.FirstOrDefault(p => p.InformationCategoryID == id);
        //        if (info != null)
        //        {
        //            db.InformationCategory.Remove(info);
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("UserCategoryList");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}