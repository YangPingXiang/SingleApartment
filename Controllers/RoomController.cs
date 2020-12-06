using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModels;

namespace sln_SingelApartment.Controllers
{
    public class RoomController : Controller
    {
        SingleApartmentEntities dbSA = new SingleApartmentEntities();
        public ActionResult Searching()
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            //建案
            CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel();
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            var b = dbSA.BuildCase;
            foreach (var item in b)
            {
                buildcase_VM_lt.Add(new CBuildCaseViewModel() { entity_buildcase = item });
            }
            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;

            //Roomstyle
            CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel();
            List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();
            var rs = dbSA.RoomStyle;
            foreach (var item in rs)
            {
                roomstyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;

            return View(abtRoom_VM);
        }

        [HttpPost]
        public ActionResult Searching(
           string buildcaseID, string roomstyleID, string peoplecount, string amountrent)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();


            #region Find room by buildcase 
            CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel();
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            var b = dbSA.BuildCase.Where(r => r.ID == buildcaseID);
            foreach (var item in b)
            {
                buildcase_VM_lt.Add(new CBuildCaseViewModel() { entity_buildcase = item });
            }
            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
            #endregion


            #region Find room by roomstyle
            CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel();
            List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();
            var rs = dbSA.RoomStyle.Where(i => i.ID.ToString() == roomstyleID);
            foreach (var item in rs)
            {
                roomstyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            #endregion


            #region Find room by peoplecount
            var p = dbSA.RoomStyle.Where(t => t.MaxNumberOfPeople.ToString() == peoplecount);
            foreach (var item in p)
            {
                roomstyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });

            }
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            #endregion



            #region Find room by rent
            var a = dbSA.RoomStyle.Where(f => f.Rent.ToString() == amountrent);
            foreach (var item in a)
            {
                roomstyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            #endregion

            ViewData.Model = abtRoom_VM;


            ViewBag.amountrent = amountrent;
            ViewBag.peoplecount = peoplecount;
            ViewBag.buildcaseID = buildcaseID;
            ViewBag.roomstyleID = roomstyleID;


            return RedirectToAction("ListAllRooms",
                new
                {
                    buildcaseID = buildcaseID,
                    roomstyleID = roomstyleID,
                    peoplecount = peoplecount,
                    amountrent = amountrent
                });

        }

        public ActionResult PartialResult(string buildcaseID, string roomstyleID, string peoplecount, string amountrent)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            var result = from t in dbSA.BuildCase
                         join r in dbSA.Room
                         on t.ID equals r.BuildCaseID
                         join rms in dbSA.RoomStyle
                         on r.RoomStyleID equals rms.ID
                         where t.ID == buildcaseID
                            && r.RoomStyleID.ToString() == roomstyleID
                         && rms.MaxNumberOfPeople.ToString() == peoplecount
                         //rms.Rent.ToString() == amountrent
                         select new { b = t, r = r, rms = rms };

            List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();
            List<CBuildCaseViewModel> buildcase_VM_lt = new List<CBuildCaseViewModel>();
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
            var test = result.ToList();
            foreach (var item in result)
            {
                CRoomStyleViewModel roomstyle_VM = new CRoomStyleViewModel() { entity_roomstyle = item.rms };
                roomstyle_VM_lt.Add(roomstyle_VM);

                CBuildCaseViewModel buildcase_VM = new CBuildCaseViewModel() { entity_buildcase = item.b };
                buildcase_VM_lt.Add(buildcase_VM);

                CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item.r };
                room_VM_lt.Add(room_VM);

            }
            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            abtRoom_VM.roomViewModels = room_VM_lt;

            ViewData.Model = abtRoom_VM;

            return PartialView("_PartialSearchResult");
        }


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
                         join b in dbSA.BuildCase
                          on r.BuildCaseID equals b.ID
                         join rs in dbSA.RoomStyle
                         on r.RoomStyleID equals rs.ID
                         join rf in dbSA.RoomFacilities
                         on rs.ID equals rf.RoomStyleID
                         join f in dbSA.Facility
                         on rf.FacilityID equals f.ID
                         where r.ID == id
                         select new { r = r, b = b, rs = rs, rf = rf, f = f };

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
                CRoomFacilityViewModel roomfacility_VM = new CRoomFacilityViewModel() { entity_roomfacilities = item.rf };
                roomfacility_VM_lt.Add(roomfacility_VM);
                CFacilityViewModel facility_VM = new CFacilityViewModel() { entity_Facility = item.f };
                facility_VM_lt.Add(facility_VM);
            }
            abtRoom_VM.buildcaseViewModels = buildcase_VM_lt;
            abtRoom_VM.roomfacilityViewModel = roomfacility_VM_lt;
            abtRoom_VM.roomStyleViewModels = roomstyle_VM_lt;
            abtRoom_VM.roomViewModels = rooom_VM_lt;
            abtRoom_VM.facilityViewModels = facility_VM_lt;

            ViewData.Model = abtRoom_VM;

            return View(abtRoom_VM);

        }

        [HttpPost]
        public ActionResult ListRoomDetail(string roomstyleID)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();


            var result = dbSA.Room.Where(r => r.RoomStyleID.ToString() == roomstyleID);

            List<CRoomViewModel> rooom_VM_lt = new List<CRoomViewModel>();

            var test = result.ToList();

            foreach (var item in result)
            {
                CRoomViewModel room_VM = new CRoomViewModel() { entity_room = item };
                rooom_VM_lt.Add(room_VM);

            }
      
            abtRoom_VM.roomViewModels = rooom_VM_lt;

            ViewData.Model = abtRoom_VM;
            ViewBag.roomstyleid = roomstyleID;

            return View(abtRoom_VM);
        }

        public ActionResult PartialCheckRoom(string roomstyleID, string buildcaseID, string peoplecount, string rent)
        {
            //todo: 【FOR ROOM】 寫if判斷式 Room 關聯到Lease table的Lease的DateTime.Now 是否在startdat & Expiry Date之間, if false 才可以show

            var available = dbSA.Room.Where(r =>
                                                r.RoomStyleID.ToString() == roomstyleID
                                                && r.BuildCaseID == buildcaseID
                                                && r.RoomStyle.MaxNumberOfPeople.ToString() == peoplecount
                                                && r.RoomStyle.Rent.ToString() == rent
                                                );
            var test = available.ToList();

            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();

            foreach (var item in available)
            {
                room_VM_lt.Add(new CRoomViewModel() { entity_room = item });

            }

            ViewData.Model = room_VM_lt;
            return PartialView("_PartialCheckRoom");
        }


        // RoomBooking

        public ActionResult BookingInfo(int id)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            //For room
            List<CRoomViewModel> r_VM_lt = new List<CRoomViewModel>();
            var a = dbSA.Room.Where(r => r.RoomStyleID == id);
            foreach (var item in a)
            {
                r_VM_lt.Add(new CRoomViewModel() { entity_room = item });
            }
            abtRoom_VM.roomViewModels = r_VM_lt;


            // For roomstyle
            CRoomStyleViewModel roomsty_VM = new CRoomStyleViewModel();
            List<CRoomStyleViewModel> rsty_VM_lt = new List<CRoomStyleViewModel>();
            var b = dbSA.RoomStyle.Where(r => r.ID == id);
            foreach (var item in b)
            {
                rsty_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = rsty_VM_lt;


            //for pic.
            CPictureViewModel roompic_VM = new CPictureViewModel();
            List<CPictureViewModel> rpic_VM_lt = new List<CPictureViewModel>();
            var c = dbSA.Picture.Where(r => r.ID == id);
            foreach (var item in c)
            {
                rpic_VM_lt.Add(new CPictureViewModel() { entity_picture = item });
            }
            abtRoom_VM.roomPicViewModels = rpic_VM_lt;


            //for facility
            CFacilityViewModel facility_VM = new CFacilityViewModel();
            List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();

            var i = dbSA.RoomStyle.Where(r => r.ID == id).FirstOrDefault();
            foreach (var item in i.RoomFacilities)
            {
                facility_VM_lt.Add(new CFacilityViewModel() { entity_Facility = item.Facility });
            }

            abtRoom_VM.facilityViewModels = facility_VM_lt;

            if (Session[CDictionary.welcome] == null)
            {
                return Content("請先登入");
            }
            else
            {
                return View(abtRoom_VM);
            }

        }

        public ActionResult MemberInfo()
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            List<CMemberViewModel> mem_VM_lt = new List<CMemberViewModel>();
            var a = dbSA.tMember.Where(m => m.fMemberId == 1);
            foreach (var item in a)
            {
                mem_VM_lt.Add(new CMemberViewModel() { entity_Member = item });
            }
            abtRoom_VM.memberViewModels = mem_VM_lt;

            return View(abtRoom_VM);

        }

        public ActionResult PayInfo(int id)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            //For room
            List<CRoomViewModel> r_VM_lt = new List<CRoomViewModel>();
            var a = dbSA.Room.Where(r => r.RoomStyleID == id);
            foreach (var item in a)
            {
                r_VM_lt.Add(new CRoomViewModel() { entity_room = item });
            }
            abtRoom_VM.roomViewModels = r_VM_lt;

            // For roomstyle
            CRoomStyleViewModel roomsty_VM = new CRoomStyleViewModel();
            List<CRoomStyleViewModel> rsty_VM_lt = new List<CRoomStyleViewModel>();
            var b = dbSA.RoomStyle.Where(r => r.ID == id);
            foreach (var item in b)
            {
                rsty_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = rsty_VM_lt;


            //for pic.
            CPictureViewModel roompic_VM = new CPictureViewModel();
            List<CPictureViewModel> rpic_VM_lt = new List<CPictureViewModel>();
            var c = dbSA.Picture.Where(r => r.ID == id);
            foreach (var item in c)
            {
                rpic_VM_lt.Add(new CPictureViewModel() { entity_picture = item });
            }
            abtRoom_VM.roomPicViewModels = rpic_VM_lt;


            //for facility
            CFacilityViewModel facility_VM = new CFacilityViewModel();
            List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();

            var i = dbSA.RoomStyle.Where(r => r.ID == id).FirstOrDefault();
            foreach (var item in i.RoomFacilities)
            {
                facility_VM_lt.Add(new CFacilityViewModel() { entity_Facility = item.Facility });
            }

            abtRoom_VM.facilityViewModels = facility_VM_lt;

            return View(abtRoom_VM);

        }

        public ActionResult BookingDone(int id)
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            //For room
            List<CRoomViewModel> r_VM_lt = new List<CRoomViewModel>();
            var a = dbSA.Room.Where(r => r.RoomStyleID == id);
            foreach (var item in a)
            {
                r_VM_lt.Add(new CRoomViewModel() { entity_room = item });
            }
            abtRoom_VM.roomViewModels = r_VM_lt;


            // For roomstyle
            CRoomStyleViewModel roomsty_VM = new CRoomStyleViewModel();
            List<CRoomStyleViewModel> rsty_VM_lt = new List<CRoomStyleViewModel>();
            var b = dbSA.RoomStyle.Where(r => r.ID == id);
            foreach (var item in b)
            {
                rsty_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            abtRoom_VM.roomStyleViewModels = rsty_VM_lt;


            //for pic.
            CPictureViewModel roompic_VM = new CPictureViewModel();
            List<CPictureViewModel> rpic_VM_lt = new List<CPictureViewModel>();
            var c = dbSA.Picture.Where(r => r.ID == id);
            foreach (var item in c)
            {
                rpic_VM_lt.Add(new CPictureViewModel() { entity_picture = item });
            }
            abtRoom_VM.roomPicViewModels = rpic_VM_lt;


            //for facility
            CFacilityViewModel facility_VM = new CFacilityViewModel();
            List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();

            var i = dbSA.RoomStyle.Where(r => r.ID == id).FirstOrDefault();
            foreach (var item in i.RoomFacilities)
            {
                facility_VM_lt.Add(new CFacilityViewModel() { entity_Facility = item.Facility });
            }

            abtRoom_VM.facilityViewModels = facility_VM_lt;

            return View(abtRoom_VM);
        }

        public ActionResult Logintest(int memberID)
        {
            //Session["UserName"] = Name;

            CMember member = Session[CDictionary.welcome] as CMember;
            memberID = member.fMemberId;

            return Content("登入成功");
        }

        public ActionResult LogintestResult()
        {
            var userName = Session["UserName"] != null ? (string)Session["UserName"] : "未登入";

            return Content(userName);
        }




    }
}