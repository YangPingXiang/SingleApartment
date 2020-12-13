using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using PagedList;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModels;

namespace sln_SingelApartment.Controllers
{
    public class RoomController : Controller
    {
        SingleApartmentEntities dbSA = new SingleApartmentEntities();

        public string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        
        public ActionResult GetSearchData(string case1, string case2, int case3, int case4)
        {

            List<CRoomViewModel> room_list = new List<CRoomViewModel>();           

            if (case3 == 1)
            {

                var query = from r in dbSA.Room
                            join b in dbSA.BuildCase
                            on r.BuildCaseID equals b.ID
                            join rs in dbSA.RoomStyle
                            on r.RoomStyleID equals rs.ID
                            where r.BuildCaseID == case1 && r.RoomType == case2 && r.RoomStyleID == case4
                            && r.Rent <= 10000
                            select new { r };

                foreach (var item in query)
                {
                    CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                    room_list.Add(room_VM);
                }


            }

            if (case3 == 2)
            {

                var query = from r in dbSA.Room
                            join b in dbSA.BuildCase
                            on r.BuildCaseID equals b.ID
                            join rs in dbSA.RoomStyle
                            on r.RoomStyleID equals rs.ID
                            where r.BuildCaseID == case1 && r.RoomType == case2 && r.RoomStyleID == case4
                            && r.Rent > 10000 && r.Rent <= 20000
                            select new { r };

                foreach (var item in query)
                {
                    CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                    room_list.Add(room_VM);
                }


            }

            if (case3 == 3)
            {

                var query = from r in dbSA.Room
                            join b in dbSA.BuildCase
                            on r.BuildCaseID equals b.ID
                            join rs in dbSA.RoomStyle
                            on r.RoomStyleID equals rs.ID
                            where r.BuildCaseID == case1 && r.RoomType == case2 && r.RoomStyleID == case4
                            && r.Rent > 20000 && r.Rent <= 30000
                            select new { r };

                foreach (var item in query)
                {
                    CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                    room_list.Add(room_VM);
                }


            }

            if (case3 == 4)
            {

                var query = from r in dbSA.Room
                            join b in dbSA.BuildCase
                            on r.BuildCaseID equals b.ID
                            join rs in dbSA.RoomStyle
                            on r.RoomStyleID equals rs.ID
                            where r.BuildCaseID == case1 && r.RoomType == case2 && r.RoomStyleID == case4
                            && r.Rent > 30000 && r.Rent<= 40000
                            select new { r };

                foreach (var item in query)
                {
                    CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                    room_list.Add(room_VM);
                }


            }

            if (case3 == 5)
            {

                var query = from r in dbSA.Room
                            join b in dbSA.BuildCase
                            on r.BuildCaseID equals b.ID
                            join rs in dbSA.RoomStyle
                            on r.RoomStyleID equals rs.ID
                            where r.BuildCaseID == case1 && r.RoomType == case2 && r.RoomStyleID == case4
                            && r.Rent > 40000
                            select new { r };

                foreach (var item in query)
                {
                    CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                    room_list.Add(room_VM);
                }


            }



            var html = RenderPartialToString("_box", room_list, ControllerContext);

            

            return Json(new { html = html }); ;


        }

        //new search Page
        public ActionResult SearchPage()
        {
            CAboutRoomViewModel abt_VM = new CAboutRoomViewModel();

            //buildcase
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            var b = dbSA.BuildCase;
            foreach (var item in b)
            {
                buildcase_VM_lt.Add(new CBuildCaseViewModel() { entity_buildcase = item });
            }
            abt_VM.buildcaseViewModels = buildcase_VM_lt;

            //roomtype
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
            var r = dbSA.Room;
            foreach (var item in r)
            {
                room_VM_lt.Add(new CRoomViewModel() { entity_room = item });
            }
            abt_VM.roomViewModels = room_VM_lt;

            //RoomStyle
            List<CRoomStyleViewModel> roomStyle_VM_lt = new List<CRoomStyleViewModel>();
            var rs = dbSA.RoomStyle;
            foreach(var item in rs)
            {
                roomStyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abt_VM.roomStyleViewModels = roomStyle_VM_lt;


            return View(abt_VM);
        }

        public JsonResult AddRoomToFavorite(string RoomID)
        {
            var user = Session[CDictionary.welcome] as CMember;
            SingleApartmentEntities db = new SingleApartmentEntities();
            var result = "發生錯誤，請重新登入再試一次！";
            if (user != null && int.TryParse(RoomID, out int roomID))
            {
                CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
                result = theUser.AddRoomToFavorite(roomID);
            }
            return Json(JsonConvert.SerializeObject(new { ans = result }));
        }

        public ActionResult RoomFavoritelist(int page = 1, int pageSize = 6)
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }
            int memberID = user.fMemberId;
            var list = dbSA.RoomFavorite.Where(r => r.MemberID == memberID);
            int currentPage = page < 1 ? 1 : page;
            List<CRoomFavorite> lt = new List<CRoomFavorite>();
            foreach (var item in list)
            {
                lt.Add(new CRoomFavorite { entity_RoomFavorite = item });
            }
            //var result = lt.ToPagedList(currentPage, pageSize);
            ViewData.Model = lt;
            ViewBag.MemberID = memberID;
            return View(lt);
        }

        public ActionResult DeleteRoomFavorite(int id)
        {
            var user = Session[CDictionary.welcome] as CMember;
            int memberID = user.fMemberId;
            RoomFavorite roomfa = dbSA.RoomFavorite.FirstOrDefault(r => r.RoomID == id);
            if (roomfa != null)
            {
                dbSA.RoomFavorite.Remove(roomfa);
                dbSA.SaveChanges();
                
            }
            return RedirectToAction("RoomFavoritelist");
        }

        //PartialRoomFavorite
        //public ActionResult PartialRoomFavorite(string MemberID, int page = 1, int pageSize = 6)
        //{
        //    SingleApartmentEntities db = new SingleApartmentEntities();
        //    var list = db.RoomFavorite.Where(r => r.MemberID.ToString() == MemberID);
        //    int currentPage = page < 1 ? 1 : page;
        //    List<CRoomFavorite> lt = new List<CRoomFavorite>();
        //    foreach (var item in list)
        //    {
        //        lt.Add(new CRoomFavorite { entity_RoomFavorite = item });
        //    }
        //    var result = lt.ToPagedList(currentPage, pageSize);
        //    ViewData.Model = result;
        //    ViewBag.MemberID = MemberID;
        //    return PartialView("_PartialRoomFavorite");
        //}

        

        //public ActionResult PartialResult(string buildcaseID, string roomstyleID, string peoplecount, string amountrent)
        //{
        //    CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

        //    var result = from t in dbSA.BuildCase
        //                 join r in dbSA.Room
        //                 on t.ID equals r.BuildCaseID
        //                 join rms in dbSA.RoomStyle
        //                 on r.RoomStyleID equals rms.ID
        //                 where t.ID == buildcaseID
        //                    && r.RoomStyleID.ToString() == roomstyleID
        //                 && rms.MaxNumberOfPeople.ToString() == peoplecount
        //                 //rms.Rent.ToString() == amountrent
        //                 select new { b = t, r = r, rms = rms };

        //    List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();
        //    List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
        //    List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
        //    var test = result.ToList();
        //    foreach (var item in result)
        //    {
        //        CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel() { entity_roomstyle = item.rms };
        //        roomstyle_VM_lt.Add(roomstyle_VM);

        //        CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel() { entity_buildcase = item.b };
        //        buildcase_VM_lt.Add(buildcase_VM);

        //        CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
        //        room_VM_lt.Add(room_VM);

        //    }
        //    abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
        //    abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
        //    abtRoom_VM.roomViewModels = room_VM_lt;

        //    ViewData.Model = abtRoom_VM;

        //    return PartialView("_PartialSearchResult");
        //}


        public ActionResult ListAllRooms(
                string buildcaseID, string roomname, string roomstyleID, string area)
        {

            
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();
            var result = from b in dbSA.BuildCase
                         join r in dbSA.Room
                         on b.ID equals r.BuildCaseID
                         join rms in dbSA.RoomStyle
                         on r.RoomStyleID equals rms.ID
                         where b.ID == buildcaseID
                         && rms.ID.ToString() == roomstyleID
                         select new { b = b, r = r, rms = rms };
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            List<CRoomViewModel> rooom_VM_lt = new List<CRoomViewModel>();
            List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();

            var test = result.ToList();

            foreach (var item in result)
            {
                CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel() { entity_buildcase = item.b };
                buildcase_VM_lt.Add(buildcase_VM);

                CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel() { entity_roomstyle = item.rms };
                roomstyle_VM_lt.Add(roomstyle_VM);

                CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                rooom_VM_lt.Add(room_VM);
            }
            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            abtRoom_VM.roomViewModels = rooom_VM_lt;

            ViewData.Model = abtRoom_VM;

            return View(abtRoom_VM);
        }
        
        //GET
        public ActionResult ListRoomDetail(int id)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            var result = from r in dbSA.Room
                         join rs in dbSA.RoomStyle
                         on r.RoomStyleID equals rs.ID 
                         join b in dbSA.BuildCase
                         on r.BuildCaseID equals b.ID
                         where r.ID == id
                         select new { r = r, b = b, rs = rs, };

            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            List<CRoomViewModel> rooom_VM_lt = new List<CRoomViewModel>();
            List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();
            List<CRoomFacilityViewModel> roomfacility_VM_lt = new List<CRoomFacilityViewModel>();
            List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();
            
            var test = result.ToList();   //有4個
            foreach (var item in result)
            {
                CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel() { entity_buildcase = item.b };
                buildcase_VM_lt.Add(buildcase_VM);
                CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel() { entity_roomstyle = item.rs };
                roomstyle_VM_lt.Add(roomstyle_VM);
                CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                rooom_VM_lt.Add(room_VM);
            }

            List<CLeaseViewModel> lease_VM_lt = new List<CLeaseViewModel>();
            var l_result = from l in dbSA.Lease
                           select l;
            foreach(var litem in l_result)
            {
                CLeaseViewModel lease_VM = new CLeaseViewModel() { entity_lease = litem };
                lease_VM_lt.Add(lease_VM);
            }

            CMember member = Session[CDictionary.welcome] as CMember;           

            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
            abtRoom_VM.roomfacilityViewModel = roomfacility_VM_lt;
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            abtRoom_VM.roomViewModels = rooom_VM_lt;
            abtRoom_VM.facilityViewModels = facility_VM_lt;
            abtRoom_VM.leaseViewModels = lease_VM_lt;

            ViewData.Model = abtRoom_VM;

            return View(abtRoom_VM);

        }


        public ActionResult BuildcaseInfo()
        {
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            var getabuildcase = from b in dbSA.BuildCase
                                
                                select   b ;
            var test = getabuildcase.ToList();
            foreach (var item in getabuildcase)
            {
                buildcase_VM_lt.Add(new CBuildCaseViewModel() { entity_buildcase = item });
            }
            return View(buildcase_VM_lt);
        }

        // RoomBooking
        public ActionResult BookingInfo(int id)
        {

            CMember member = Session[CDictionary.welcome] as CMember;

            var model = new CRoomBooking();

            model.RoomId = id;
            model.MemberId = member.fMemberId;
            model.MemberName = member.fAccount;

            return View(model);

        }

        [HttpPost]
        public ActionResult BookingInfo(CRoomBooking objBookingInfo)
        {
            Lease roomBooking = new Lease()
            {
                MemberID = objBookingInfo.MemberId,
                RoomID = objBookingInfo.RoomId,
                StartDate = objBookingInfo.StartTime,
                ExpiryDate = objBookingInfo.EndTime,

            };

            dbSA.Lease.Add(roomBooking);
            dbSA.SaveChanges();

            //訂房成功
            CInformationFactory x = new CInformationFactory();
            x.Add(objBookingInfo.MemberId, 400, objBookingInfo.RoomId, 40020);

            return Json(data: new { message = "Booking is successfully", success = true }, behavior: JsonRequestBehavior.AllowGet);

            //return Json(new { data = model });

            //return Content(model.MemberId.ToString());
        }

        public ActionResult MyRoom()
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            CMember member = Session[CDictionary.welcome] as CMember;
            int memberID = member.fMemberId;

            var result = from L in dbSA.Lease
                         join R in dbSA.Room
                         on L.RoomID equals R.ID
                         where L.MemberID == memberID
                         select new { L, R };

            List<CRoomViewModel> List_Room = new List<CRoomViewModel>();
            List<CLeaseViewModel> List_Lease = new List<CLeaseViewModel>();

            var test = result.ToList();
            foreach (var item in result)
            {
                CRoomViewModel ViewM_Room = new CRoomViewModel() { entity_room = item.R };
                List_Room.Add(ViewM_Room);

                CLeaseViewModel ViewM_Lease = new CLeaseViewModel() { entity_lease = item.L };
                List_Lease.Add(ViewM_Lease);
            }

            abtRoom_VM.roomViewModels = List_Room;
            abtRoom_VM.leaseViewModels = List_Lease;

            ViewData.Model = abtRoom_VM;

            return View(abtRoom_VM);
        }


        public ActionResult PayInfo()
        {
            return View();

        }

        //public ActionResult BookingDone(int id)
        //{
        //    CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

        //    //For room
        //    List<CRoomViewModel> r_VM_lt = new List<CRoomViewModel>();
        //    var a = dbSA.Room.Where(r => r.RoomStyleID == id);
        //    foreach (var item in a)
        //    {
        //        r_VM_lt.Add(new CRoomViewModel() { entity_room = item });
        //    }
        //    abtRoom_VM.roomViewModels = r_VM_lt;


        //    // For roomstyle
        //    CRoomStyleViewModel roomsty_VM = new CRoomStyleViewModel();
        //    List<CRoomStyleViewModel> rsty_VM_lt = new List<CRoomStyleViewModel>();
        //    var b = dbSA.RoomStyle.Where(r => r.ID == id);
        //    foreach (var item in b)
        //    {
        //        rsty_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
        //    }
        //    abtRoom_VM.roomStyleViewModels = rsty_VM_lt;


        //    //for pic.
        //    CPictureViewModel roompic_VM = new CPictureViewModel();
        //    List<CPictureViewModel> rpic_VM_lt = new List<CPictureViewModel>();
        //    var c = dbSA.Picture.Where(r => r.ID == id);
        //    foreach (var item in c)
        //    {
        //        rpic_VM_lt.Add(new CPictureViewModel() { entity_picture = item });
        //    }
        //    abtRoom_VM.roomPicViewModels = rpic_VM_lt;


        //    //for facility
        //    CFacilityViewModel facility_VM = new CFacilityViewModel();
        //    List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();

        //    var i = dbSA.RoomStyle.Where(r => r.ID == id).FirstOrDefault();
        //    foreach (var item in i.RoomFacilities)
        //    {
        //        facility_VM_lt.Add(new CFacilityViewModel() { entity_Facility = item.Facility });
        //    }

        //    abtRoom_VM.facilityViewModels = facility_VM_lt;

        //    return View(abtRoom_VM);
        //}




    }
}