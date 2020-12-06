using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModel;
using sln_SingleApartment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace sln_SingleApartment.Controllers
{
    public class ActivityController : Controller
    {
        // GET: List
        public ActionResult List()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            
            string search = Request.Form["txtKey"];
            //string SubCategoryName = Response.Write(Namelist);
            IEnumerable<Activity> table = null;
            if (string.IsNullOrEmpty(search))
            {
                table = from p in db.Activity
                     select p;
            }
            else
            {
                table = from p in db.Activity
                        where p.ActivityName.Contains(search)
                        select p;
            }
            //下拉式選單
            #region SubCategoryName
            List<string> cNamelist = new List<string>();
            var q = from p in db.ActivitySubCategory
                    select p.ActivitySubCategoryName;
                     
            foreach (var g in q)
            {
                cNamelist.Add(g);
                SelectList Namelist = new SelectList(cNamelist, "Name");
                ViewBag.subName = Namelist;
            }
            #endregion

            #region 活動啟動更新狀態
            List<DateTime> acEndtime = new List<DateTime>();
            List<int> lnacid = new List<int>();

            List<int> ParticipantID = new List<int>();
            List<int> ActivityID = new List<int>();
            List<int> MemberID = new List<int>();
            var actimenow = from p in db.Activity
                            select new { endtime = p.EndTime, acid = p.ActivityID };

            var peoplemax = from par in db.Participant
                            select new { partID = par.ParticipantID, paracID = par.ActivityID, parMemberID = par.MemberID };
                           

            foreach(var r in actimenow)
            {
                acEndtime.Add(r.endtime);
                lnacid.Add(r.acid);
            }
            foreach (var part in peoplemax)
            {
                ParticipantID.Add(part.partID);
                ActivityID.Add(part.paracID);
                MemberID.Add(part.parMemberID);
            }
            int nowacid = 0;
            for (int i = 0; i < acEndtime.Count; i++)
            {
                                
                if (acEndtime[i] < DateTime.Now)
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);
                    acSt.Status = "活動時間已過";
                    db.SaveChanges();
                                     
                }
                else
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);
                    
                    acSt.Status = "可參加";
                    db.SaveChanges();
                }
                //else if()
            }
            int nIDcount = 0;
            int nIDbuffer = 0;
            List<int> ParticipantIDList = new List<int>();
            for (int j = 0; j < ParticipantID.Count; j++)
            {
                if (ActivityID[j] != nIDbuffer)
                {
                    nIDbuffer = ActivityID[j];
                    nIDcount++;
                    ParticipantIDList.Add(nIDbuffer);


                }


            }
          
           
           // MessageBox.Show(nIDcount.ToString());
            #endregion
            List< CActivity > list = new List<CActivity>();
            foreach (Activity p in table)
                list.Add(new CActivity() { entity = p });
            return View(list);
        }

        // GET: Joined_List
        public ActionResult Joined_List()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            IEnumerable<Activity> table = from p in db.Activity
                                          select p;

            List<CActivity> list = new List<CActivity>();
            foreach (Activity a in table)
                list.Add(new CActivity() { entity = a });
            return View(list);
        }

        //CartView
        public ActionResult CartView()
        {
            List<tActivityCart> list = Session[CDictionary.Cart_Key] as List<tActivityCart>;
            if (list == null)
            {
                return RedirectToAction("List");
            }
            List<CActivityCart> lst = new List<CActivityCart>();
            foreach (tActivityCart b in list)
                lst.Add(new CActivityCart() { entity = b });
            return View(lst);

        }


        //暫存到購物車
        public ActionResult AddToCart_Session( int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Activity table = db.Activity.FirstOrDefault(p => p.ActivityID == id);
            if(table == null)
            {
                return RedirectToAction("List");
            }
            CActivity CA = new CActivity() { entity = table };
            return View(CA);
        }

        [HttpPost]
        public ActionResult AddToCart_Session(CShoppingCart ac)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Activity table = db.Activity.FirstOrDefault(p => p.ActivityID == ac.txtfId);
            if (table != null)
            {
                tActivityCart tb = new tActivityCart();
                tb.fJoinedId = table.ActivityID;
                //tb.SubCategoryDetailID = table.SubCategoryDetailID;
                tb.fAvtivityName = table.ActivityName;
                tb.fStartTime = table.StartTime;
                tb.fEndTime = table.EndTime;
                tb.fLocation = table.MeetingPoint;
                tb.fPeopleCount = table.PeopleCount;
                //tb.Note = table.Note;
                tb.fNote = table.Status;
                List<tActivityCart> list = Session[CDictionary.Cart_Key] as List<tActivityCart>;
                if (list == null)
                {
                    list = new List<tActivityCart>();
                    Session[CDictionary.Cart_Key] = list;
                }
                list.Add(tb);
            }
            //CActivity CA = new CActivity() { entity = table };
            //return View(CA);
            return RedirectToAction("List");
        }


        // GET: Create
        public ActionResult Create()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            //下拉式選單
            List<string> cNamelist = new List<string>();
            var q = from p in db.ActivitySubCategory
                    select p.ActivitySubCategoryName;

            foreach (var g in q)
            {
                cNamelist.Add(g);
                SelectList Namelist = new SelectList(cNamelist, "Name");
                ViewBag.subName = Namelist;
            }
            return View();
        }

        // GET: Create
        [HttpPost]
        public ActionResult Create(Activity ac,string subName)
        {
           
            SingleApartmentEntities entity = new SingleApartmentEntities();
          
            int sub = 0;
            var subID = from SUBID in entity.ActivitySubCategory
                        where SUBID.ActivitySubCategoryName == subName
                        select SUBID.ActivitySubCategoryID;
            foreach (var g in subID)
            {
                sub = g;
            }
            if (subName != null)
                ac.SubCategoryDetailID = sub;
            entity.Activity.Add(ac);
            entity.SaveChanges();
            return RedirectToAction("List");
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            Activity table = (new SingleApartmentEntities()).Activity.FirstOrDefault(p => p.ActivityID == id);
            if (table == null)
                return RedirectToAction("List"); 
            CActivity tb = new CActivity() { entity = table };
            return View(tb);
        }

        // GET: [HTTPPOST] Edit
        [HttpPost]
        public ActionResult Edit(Activity tb)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Activity table = db.Activity.FirstOrDefault(p => p.ActivityID ==tb.ActivityID );
            if (table != null)
            {
                table.ActivityName = tb.ActivityName;
                table.StartTime = tb.StartTime;
                table.EndTime = tb.EndTime;
                table.MeetingPoint = tb.MeetingPoint;
                table.PeopleCount = tb.PeopleCount;
                table.Note = tb.Note;
                db.SaveChanges();
            }
           
           return RedirectToAction("List");
        }

        // GET: Delete
        public ActionResult Delete(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Activity table = db.Activity.FirstOrDefault(p => p.ActivityID == id);
            if (table != null)
            {
                db.Activity.Remove(table);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        //public ActionResult SubCategoryDetailName()
        //{
        //    SingleApartmentEntities db = new SingleApartmentEntities();
        //    int search = Request.Form["txtKey"];
        //    if (string.IsNullOrEmpty(search))
        //    {
        //        var q = from p in db.ActivitySubCategoryDetail
        //                where p.SubCategoryDetailID.Contains(search)
        //                select p;
        //    }
                
           
        //    return;
        //}
    }
}