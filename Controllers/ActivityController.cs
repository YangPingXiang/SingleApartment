using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModel;
using sln_SingleApartment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
//using System.Windows.Forms;



namespace sln_SingleApartment.Controllers
{
    public class ActivityController : Controller
    {
        // GET: List
        public ActionResult List(string newActivity,int page = 1)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            #region 登入者名稱
            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;
            ViewBag.memberID = memberID;
            #endregion


            string search = Request.Form["acName"];
            string searchac = Request.Form["subName"];
            IEnumerable<Activity> table = null;
            List<CActivity> list = new List<CActivity>();
            if (newActivity == "查看最新活動")
            {
                var newaclistt = (from p in db.Activity
                             orderby p.ActivityID descending
                             select p).Take(5).ToList();
                table = newaclistt;
            }
            else
            {

                if (string.IsNullOrEmpty(search) && string.IsNullOrEmpty(searchac))
                {
                    table = from p in db.Activity
                            select p;
                }
                else if (searchac != "")
                {
                    table = from p in db.Activity
                            where p.ActivitySubCategory.ActivitySubCategoryName.Contains(searchac)
                            select p;
                }
                else if (search != "")
                {
                    table = from p in db.Activity
                            where p.ActivityName.Contains(search)
                            select p;
                }
                else
                {
                    table = from p in db.Activity
                            where p.ActivityName.Contains(search) && p.ActivitySubCategory.ActivitySubCategoryName.Contains(searchac)
                            select p;
                }
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
            #region 活動名稱下拉式選單

            var acNamelist = (from p in db.Activity
                    select p.ActivityName).Distinct().ToList();
                       
                SelectList ActivityNamelist = new SelectList(acNamelist, "Name");
                ViewBag.acName = ActivityNamelist;
            #endregion
            #region 活動啟動更新狀態
            List<DateTime?> acEndtime = new List<DateTime?>();
            List<int> lnacid = new List<int>();

          
            var actimenow = from p in db.Activity
                            select new { endtime = p.EndTime, acid = p.ActivityID };

                

            foreach(var r in actimenow)
            {
                acEndtime.Add(r.endtime);
                lnacid.Add(r.acid);
            }
    
            int nowacid = 0;
            for (int i = 0; i < acEndtime.Count; i++)
            {
                                
                if (acEndtime[i] < DateTime.Now)
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);
                    acSt.Status = "活動時間已過";

                   

                }
                else
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);

                    acSt.Status = "可參加";

                }
            
            }
            #endregion
            #region 活動人數確認
            int nIDcount = 0;
            int nIDbuffer = 0;
            List<int> ParticipantID = new List<int>();
            //已參加人員數量
            List<int> ActivityIDCount = new List<int>();
            var palist = from p in db.Participant
                         group p.ActivityID by p.ActivityID into g
                         select new
                         {
                             g.Key,
                             AcIDcount = g.Count()
                            
                         };
           
            foreach (var x in palist)
            {
                ParticipantID.Add(x.Key);
                ActivityIDCount.Add(x.AcIDcount);
                ViewBag.subParticipantID = ParticipantID;
                ViewBag.subActivityIDCount= ActivityIDCount;
            }

            for (int j = 0; j < ActivityIDCount.Count; j++)
            {
                nIDbuffer = ParticipantID[j];
                var AcIDCounttotle = from k in db.Activity
                                     
                                     select new { acidbuffer = k.ActivityID, acpeoplecount = k.PeopleCount, acstates = k.Status };
                foreach (var y in AcIDCounttotle)
                {
                    if (y.acidbuffer == ParticipantID[j])
                    {

                        if (y.acpeoplecount <= ActivityIDCount[j])
                        {
                            nIDcount = ParticipantID[j];
                            Activity aaaa = db.Activity.FirstOrDefault(p => p.ActivityID == nIDcount);
                            aaaa.Status = "人員已滿";

                        }

                        else if (y.acstates != "活動時間已過")
                        {
                            nIDbuffer = ParticipantID[j];
                            Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nIDbuffer);
                            acSt.Status = "可參加";
                        }

                    }
            
                }
            }


            #endregion
           

            db.SaveChanges();//db SaveChange

            #region 人數已滿下架活動
            List<int> MemberIDList = new List<int>();
            List<string> MemberfActivityMessageList = new List<string>();
            tMember tMember = new tMember();
            var membermessage = from mbmsg in db.tMember
                                select new { MbID = mbmsg.fMemberId, MbMessage = mbmsg.fActivityMessage };
            foreach (var m in membermessage)
            {
                MemberIDList.Add(m.MbID);
                MemberfActivityMessageList.Add(m.MbMessage);
            }
            //for (int me = 0; me < MemberIDList.Count; me++)
            //{
            //    if (MemberfActivityMessageList[me] == "TRUE")
            //    {
            //        var statuscount = (from u in db.Activity
            //                           where u.Status == "人員已滿"
            //                           select u.ActivityID).ToList();
            //        for (int sst = 0; sst < statuscount.Count; sst++)

            //        {
            //            bool flag;
            //            CInformationFactory infactory = new CInformationFactory();

            //            int p_source_id = statuscount[sst];   //可能是訂單號碼, 房號 ..
            //            flag = infactory.Add(MemberIDList[me], 200, p_source_id, 20080);
            //        }
                    
            //    }
             
            //}
            #endregion
            #region 活動時間已過下架活動
            //for (int me = 0; me < MemberIDList.Count; me++)
            //{
            //    if (MemberfActivityMessageList[me] == "TRUE")
            //    {
            //        var statusendtime = (from u in db.Activity
            //                             where u.Status == "活動時間已過"
            //                             select u.ActivityID).ToList();

            //        var dbInformationSource = (from dbin in db.Information
            //                            select dbin.InformationSource).ToList();
            //        var dbDocumentID = (from dbin in db.Information
            //                             select dbin.DocumentID).ToList();
            //        //foreach(var informationoverlap in dbinformation)
            //        //{
            //        //    int? aaa=informationoverlap.InformationSource;
            //        //    int bbb = informationoverlap.DocumentID;
            //        //}

            //        int p_source_id = 0;
            //        for (int sst = 0; sst < statusendtime.Count; sst++)
            //        {
            //             p_source_id = statusendtime[sst];   //可能是訂單號碼, 房號 ..
            //            bool flag;
            //            CInformationFactory infactory = new CInformationFactory();

            //            flag = infactory.Add(MemberIDList[me], 200, p_source_id, 20070);


            //        }
                    
                           
                      
            //    }
            //}
            #endregion
           
            
            foreach (Activity p in table)
                list.Add(new CActivity() { entity = p });
            int pageSize = 5;
            int currentpage = page < 1 ? 1 : page;
            var pagelist = list.ToPagedList(currentpage, pageSize);
            return View(pagelist);

           // return View(list);
        }
        //public ActionResult List(string newActivity)
        //{
        //    SingleApartmentEntities db = new SingleApartmentEntities();
        //    List<Activity> list = new List<Activity>();
            
        //    return View(list);
        //}

        //後台
        public ActionResult Back_List()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            #region 登入者名稱
            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;
            ViewBag.memberID = memberID;
            #endregion

            string search = Request.Form["txtKey"];

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
            List<DateTime?> acEndtime = new List<DateTime?>();
            List<int> lnacid = new List<int>();
            var actimenow = from p in db.Activity
                            select new { endtime = p.EndTime, acid = p.ActivityID };
            foreach (var r in actimenow)
            {
                acEndtime.Add(r.endtime);
                lnacid.Add(r.acid);
            }
            int nowacid = 0;
            for (int i = 0; i < acEndtime.Count; i++)
            {

                if (acEndtime[i] < DateTime.Now)
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);
                    acSt.Status = "活動時間已過";
                }
                else
                {
                    nowacid = lnacid[i];
                    Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nowacid);
                    acSt.Status = "可參加";
                }
            }
            #endregion
            #region 活動人數確認
            int nIDcount = 0;
            int nIDbuffer = 0;
            List<int> ParticipantID = new List<int>();
            //已參加人員數量
            List<int> ActivityIDCount = new List<int>();
            var palist = from p in db.Participant
                         group p.ActivityID by p.ActivityID into g
                         select new
                         {
                             g.Key,
                             AcIDcount = g.Count()
                         };

            foreach (var x in palist)
            {
                ParticipantID.Add(x.Key);
                ActivityIDCount.Add(x.AcIDcount);
                ViewBag.subParticipantID = ParticipantID;
                ViewBag.subActivityIDCount = ActivityIDCount;
            }

            for (int j = 0; j < ActivityIDCount.Count; j++)
            {
                nIDbuffer = ParticipantID[j];
                var AcIDCounttotle = from k in db.Activity
                                     select new { acidbuffer = k.ActivityID, acpeoplecount = k.PeopleCount, acstates = k.Status };
                foreach (var y in AcIDCounttotle)
                {
                    if (y.acidbuffer == ParticipantID[j])
                    {

                        if (y.acpeoplecount <= ActivityIDCount[j])
                        {
                            nIDcount = ParticipantID[j];
                            Activity aaaa = db.Activity.FirstOrDefault(p => p.ActivityID == nIDcount);
                            aaaa.Status = "人員已滿";

                        }

                        else if (y.acstates != "活動時間已過")
                        {
                            nIDbuffer = ParticipantID[j];
                            Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nIDbuffer);
                            acSt.Status = "可參加";
                        }

                    }

                }
            }
            #endregion
            db.SaveChanges();//db SaveChange

            #region 人數已滿下架活動
            List<int> MemberIDList = new List<int>();
            List<string> MemberfActivityMessageList = new List<string>();
            tMember tMember = new tMember();
            var membermessage = from mbmsg in db.tMember
                                select new { MbID = mbmsg.fMemberId, MbMessage = mbmsg.fActivityMessage };
            foreach (var m in membermessage)
            {
                MemberIDList.Add(m.MbID);
                MemberfActivityMessageList.Add(m.MbMessage);
            }
          
            #endregion
            #region 活動時間已過下架活動
            for (int me = 0; me < MemberIDList.Count; me++)
            {
                if (MemberfActivityMessageList[me] == "TRUE")
                {
                    var statusendtime = (from u in db.Activity
                                         where u.Status == "活動時間已過"
                                         select u.ActivityID).ToList();
                    for (int sst = 0; sst < statusendtime.Count; sst++)
                    {
                        bool flag;
                        CInformationFactory infactory = new CInformationFactory();

                        int p_source_id = statusendtime[sst];   //可能是訂單號碼, 房號 ..
                        flag = infactory.Add(MemberIDList[me], 200, p_source_id, 20070);
                    }
                }
            }
            #endregion

            List<CActivity> list = new List<CActivity>();
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
            List<CActivityCart> list = Session[CDictionary.Cart_Key] as List<CActivityCart>;
            if (list == null)
            {
                return RedirectToAction("List");
            }
            
            return View(list);

        }

        //暫存到購物車
        public ActionResult AddToCart_Session(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Activity table = db.Activity.FirstOrDefault(p => p.ActivityID == id);
            if (table != null)
            {
                CActivityCart tb = new CActivityCart();
                tb.fJoinedId = table.ActivityID;
               
                tb.fAvtivityName = table.ActivityName;
                tb.fStartTime = table.StartTime;
                tb.fEndTime = table.EndTime;
                tb.fLocation = table.MeetingPoint;
                tb.fPeopleCount = table.PeopleCount;
                tb.fMemberId = table.MemberID;
               
                tb.fNote = table.Note;
                List<CActivityCart> list = Session[CDictionary.Cart_Key] as List<CActivityCart>;
                if (list == null)
                {
                    list = new List<CActivityCart>();
                    Session[CDictionary.Cart_Key] = list;
                }
                list.Add(tb);
            }
            
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
        string subNamebuffer = null;
        [HttpPost]
        
        public ActionResult Create(CActivityNew ac,string subName)
        {
            #region 登入者名稱
            //TODO
            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;
          

            #endregion
            subNamebuffer = subName;
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
            {
                ac.SubCategoryDetailID = sub;
                                              
                int index = ac.myImage.FileName.IndexOf(".");
                string extention = ac.myImage.FileName.Substring(index, ac.myImage.FileName.Length - index);
                string photoName = Guid.NewGuid().ToString() + extention;
                ac.ActivityImage = "../Content/" + photoName;
                ac.myImage.SaveAs(Server.MapPath("../Content/") + photoName);


                Activity t = new Activity();
                t.ActivityID = ac.ActivityID;
                t.ActivityName = ac.ActivityName;
                t.StartTime = ac.StartTime;
                t.EndTime = ac.EndTime;
                t.MeetingPoint = ac.MeetingPoint;
                t.PeopleCount = ac.PeopleCount;
                t.Status = ac.Status;
                t.SubCategoryDetailID = ac.SubCategoryDetailID;
                t.MemberID = 1;
                t.ActivityImage = ac.ActivityImage;
                entity.Activity.Add(t);
            }

            #region 活動建立人加入活動
            Participant p = new Participant();
            p.ActivityID = ac.ActivityID;
            p.MemberID = memberID;
            entity.Participant.Add(p);
            #endregion

         
            entity.SaveChanges();
          
            #region 活動建立完成後發送訊息

            bool flag;
            CInformationFactory infactory = new CInformationFactory();
                        
            int p_source_id=ac.ActivityID;   //可能是訂單號碼, 房號 ..
            flag = infactory.Add(memberID, 200, p_source_id, 20010, subNamebuffer);
            #endregion
            #region 活動建立後媒合訊息發送
            List<int> MemberIDList = new List<int>();
            List<string> MemberfActivityMessageList = new List<string>();

            tMember tMember = new tMember();
            var membermessage = from mbmsg in entity.tMember
                                select new { MbID = mbmsg.fMemberId, MbMessage = mbmsg.fActivityMessage };
            foreach (var m in membermessage)
            {
                MemberIDList.Add(m.MbID);
                MemberfActivityMessageList.Add(m.MbMessage);
            }
            for (int me = 0; me < MemberIDList.Count; me++)
            {
                if (MemberfActivityMessageList[me] == "TRUE")
                {

                    flag = infactory.Add(MemberIDList[me], 200, p_source_id, 20040, subNamebuffer);
                }
            }
            #endregion
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
                //int index = tb.myImage.FileName.IndexOf(".");
                //string extention = tb.myImage.FileName.Substring(index, tb.myImage.FileName.Length - index);
                //string photoName = Guid.NewGuid().ToString() + extention;
                //tb.ActivityImage = "../Content/" + photoName;
                //tb.myImage.SaveAs(Server.MapPath("../Content/") + photoName);
                //Activity activity = new Activity();
                table.ActivityName = tb.ActivityName;
                table.StartTime = tb.StartTime;
                table.EndTime = tb.EndTime;
                table.MeetingPoint = tb.MeetingPoint;
                table.PeopleCount = tb.PeopleCount;
                table.Note = tb.Note;
                //activity.ActivityImage = tb.ActivityImage;

                #region 活動人數確認
                //int nIDcount = 0;
                //int nIDbuffer = 0;
                //List<int> ParticipantID = new List<int>();
                ////已參加人員數量
                //List<int> ActivityIDCount = new List<int>();
                //var palist = from p in db.Participant
                //             group p.ActivityID by p.ActivityID into g
                //             select new
                //             {
                //                 g.Key,
                //                 AcIDcount = g.Count()
                //             };

                //foreach (var x in palist)
                //{
                //    ParticipantID.Add(x.Key);
                //    ActivityIDCount.Add(x.AcIDcount);
                //}

                //for (int j = 0; j < ActivityIDCount.Count; j++)
                //{
                //    nIDbuffer = ParticipantID[j];
                //    var AcIDCounttotle = from k in db.Activity
                //                         select new { acidbuffer = k.ActivityID, acpeoplecount = k.PeopleCount, acstates = k.Status };
                //    foreach (var y in AcIDCounttotle)
                //    {
                //        if (y.acidbuffer == ParticipantID[j] && y.acstates == "可參加")
                //        {

                //            if (y.acpeoplecount <= ActivityIDCount[j])
                //            {
                //                nIDcount = ParticipantID[j];
                //                Activity aaaa = db.Activity.FirstOrDefault(p => p.ActivityID == nIDcount);
                //                aaaa.Status = "人員已滿";
                //            }
                //            else
                //            {
                //                //nIDbuffer = ParticipantID[j];
                //                //Activity acSt = db.Activity.FirstOrDefault(p => p.ActivityID == nIDbuffer);
                //                //acSt.Status = "可參加";
                //            }
                //        }
                //    }
                //}


                #endregion
                //db.Activity.Add(activity);
                db.SaveChanges();
            }
           
           return RedirectToAction("List");
        }

        // GET: Delete
        public ActionResult Deleteacdb(int id)
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
        // GET: Delete 將暫存的單一活動session清除 12/4修改
        public ActionResult Delete(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            var clear = Session["txtfJoinedID"];
            clear = id;
            Activity ac = db.Activity.FirstOrDefault(p => p.ActivityID == id);
            if (ac != null)
            {
                CActivityCart t = new CActivityCart();
                List<CActivityCart> list = Session[CDictionary.Cart_Key] as List<CActivityCart>;
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)     //foreach沒有辦法去修改自己本身的陣列
                    {
                        if (list[i].fJoinedId == id)
                        {
                            list.Remove(list[i]);
                        }
                    }
                }
            }
            return RedirectToAction("CartView");
        }
        // GET: DeleteAll 將活動session全部清除
        public ActionResult DeleteAll(CAcitivitySession input)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            Activity ac = db.Activity.FirstOrDefault(p => p.ActivityID == input.txtActivityID);
            CActivityCart t = new CActivityCart();

            List<CActivityCart> list = Session[CDictionary.Cart_Key] as List<CActivityCart>;
            if (list != null)
            {
                list = new List<CActivityCart>();
                Session[CDictionary.Cart_Key] = list;
            }
            list.Remove(t);
            return RedirectToAction("List");
        }

        //將session資料加進DB 12/4
        public ActionResult AddToDB(CActivityCart input)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;

            List<CActivityCart> list = Session[CDictionary.Cart_Key] as List<CActivityCart>;
            CActivityCart t = new CActivityCart();
            tActivityCart cart = new tActivityCart();
            if (list != null)
            {
                Participant p = new Participant();
                p.ActivityID = input.fJoinedId;
                p.MemberID = memberID;
             

                db.Participant.Add(p);
                db.SaveChanges();
                list = new List<CActivityCart>();
                Session[CDictionary.Cart_Key] = list;

            }
            list.Remove(t);
            return RedirectToAction("List");
        }
        public ActionResult subActivity(int id)
        {
            SingleApartmentEntities entity = new SingleApartmentEntities();
            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;
            SubActivity subActivity = new SubActivity();

            subActivity.ActivityID = id;
            subActivity.MemberID = memberID;

            entity.SubActivity.Add(subActivity);
                   
                
            entity.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult ParticipantStatus()
        {
            return RedirectToAction("ParticipantStatus");
           
        }

    }
}