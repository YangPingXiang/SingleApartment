using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;
using PagedList;
using PagedList.Mvc;

namespace tryTemplete_Room.Controllers
{
    public class RoomBackendController : Controller
    {
        SingleApartmentEntities dbSA = new SingleApartmentEntities();
        // GET: RoomBackend
        //public ActionResult BackListBuildCase()
        //{

        //    var table = from t in (new SingleApartmentEntities()).BuildCase select t;

        //    List<CBuildCaseViewModel> lt_buildcase = new List<CBuildCaseViewModel>();
        //    foreach( BuildCase item in table)
        //    {
        //        lt_buildcase.Add((new CBuildCaseViewModel() { entity_buildcase = item }));
        //    }
        //    return View(lt_buildcase);
        //}

        //public ActionResult BackListRooms()
        //{
        //    var table = from t in (new SingleApartmentEntities()).Room select t;

        //    List<CRoomViewModel> lt_room = new List<CRoomViewModel>();
        //    foreach(Room item in table)
        //    {
        //        lt_room.Add((new CRoomViewModel() { entity_room = item }));
        //    }
        //    return View(lt_room);
        //}

        //public ActionResult BackListAllLease()
        //{
        //    var table = from t in (new SingleApartmentEntities()).Lease select t;
        //    List<CLeaseViewModel> lt_lease = new List<CLeaseViewModel>();
        //    foreach (Lease item in table)
        //    {
        //        lt_lease.Add((new CLeaseViewModel() { entity_lease = item }));
        //    }
        //    return View(lt_lease);
        //}

        //public ActionResult BackListFacility()
        //{
        //    var table = from t in (new SingleApartmentEntities()).Facility select t;
        //    List<CFacilityViewModel> lt_facility = new List<CFacilityViewModel>();
        //    foreach(Facility item in table)
        //    {
        //        lt_facility.Add((new CFacilityViewModel() { entity_Facility = item }));
        //    }

        //    return View(lt_facility);
        //}

        //public ActionResult BackHome()
        //{
        //    return View();
        //}


        ////BackLeaseCreate Lease 
        ////GET
        //public ActionResult BackLeaseCreate()
        //{
        //    return View();
        //}
        //save to the lease table database 
        //[HttpPost]
        //public ActionResult BackLeaseCreate
        //    (int roomID, DateTime startdate, DateTime expirydate, int memberID)
        //{
        //    Lease tlease = new Lease();
        //    tlease.RoomID = roomID;
        //    tlease.StartDate = startdate;
        //    tlease.ExpiryDate = expirydate;
        //    tlease.MemberID = memberID;
        //    dbSA.Lease.Add(tlease);
        //    dbSA.SaveChanges();

        //    return RedirectToAction("BackListAllLease");
        //}

        //[HttpPost]
        //public ActionResult Edit(tProduct p)
        //{
        //  dbDemoEntities db = new dbDemoEntities();
        //  tProduct prod = db.tProducts.FirstOrDefault(t => t.fId == p.fId);

        //    if (prod != null)
        //    {
        //        prod.fName = p.fName;
        //        prod.fCost = p.fCost;
        //        prod.fPrice = p.fPrice;
        //        prod.fQty = p.fQty;
        //        prod.fImagePath = p.fImagePath;

        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("List");
        //}


        //public ActionResult BackEditLease(int id)
        //{
        //    Lease lease = (new SingleApartmentEntities()).Lease.FirstOrDefault(l => l.ID == id);
        //    if(lease == null)
        //    {
        //        RedirectToAction("BackListAllLease");
        //    }

        //    return View(lease);
        //}
        //[HttpPost]
        //public ActionResult BackEditLease(Lease l)
        //{
        //    Lease lease = dbSA.Lease.FirstOrDefault(t => t.ID == l.ID);

        //    if (lease != null)
        //    {
        //        lease.ID = l.ID;
        //        lease.RoomID = l.RoomID;
        //        lease.StartDate = l.StartDate;
        //        lease.ExpiryDate = l.ExpiryDate;
        //        lease.MemberID = l.MemberID;

        //        dbSA.SaveChanges();
        //    }
        //    return RedirectToAction("BackListAllLease");
        //}


        ////GET
        //public ActionResult BackCreateFacility()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult BackCreateFacility
        //    (string facilityName)
        //{
        //    Facility tfacility = new Facility();
        //    tfacility.FacilityName = facilityName;

        //    dbSA.Facility.Add(tfacility);
        //    dbSA.SaveChanges();

        //    return RedirectToAction("BackListFacility");
        //}

        //public ActionResult BackListAllRoomstyle()
        //{
        //    var table = from t in (new SingleApartmentEntities()).RoomStyle select t;

        //    List<CRoomStyleViewModel> lt_roomstyle = new List<CRoomStyleViewModel>();

        //    foreach (RoomStyle item in table)
        //    {
        //        lt_roomstyle.Add(( new CRoomStyleViewModel() {  entity_roomstyle = item }));
        //    }
        //    return View(lt_roomstyle);

        //}


        public ActionResult BackRoomManage(int page = 1, int pageSize = 10)
        {
           

            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();


            var  r = from t in dbSA.Room
                                            select t;

            foreach(Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));
            }
            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);
            return View(query);
        }

        public ActionResult BackCreateRoom()
        {
            return View();
        }


        public ActionResult BackRoomDetail(/*int page = 1, int pageSize = 10 ,*/ string id)
        {
            //CAboutRoomViewModel abt_VM = new CAboutRoomViewModel();



            var table = dbSA.Room.Where(x => x.ID.ToString() == id);


            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
            //List<CRoomFacilityViewModel> roomfacility_VM_lt = new List<CRoomFacilityViewModel>();
            //List<CFacilityViewModel> facility_VM_lt = new List<CFacilityViewModel>();

            foreach (var item in table)
            {
                room_VM_lt.Add(new CRoomViewModel() { entity_room = item });

            }
            //abt_VM.roomViewModels = room_VM_lt;

            //foreach(var item in table)
            //{
            //    roomfacility_VM_lt.Add(new CRoomFacilityViewModel() { entity_roomfacilities = item.rf });
            //}
            //abt_VM.roomfacilityViewModel = roomfacility_VM_lt;

            //foreach(var item in table)
            //{
            //    facility_VM_lt.Add(new CFacilityViewModel() { entity_Facility = item.f });
            //}
            //abt_VM.facilityViewModels = facility_VM_lt;

            //List<CAboutRoomViewModel> abt_VM_lt = new List<CAboutRoomViewModel>();

            //abt_VM_lt.Add(abt_VM);

            //var query = room_VM_lt.ToPagedList(page, pageSize);

            return View(room_VM_lt);
        }


        public ActionResult BackEditTheRoomDetail(string id)
        {

            var table = dbSA.Room.Where(x => x.ID.ToString() == id);


            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();

            foreach (var item in table)
            {
                room_VM_lt.Add(new CRoomViewModel() { entity_room = item });

            }

            return View(room_VM_lt);
        }

    }
}