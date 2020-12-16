using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.ViewModels;
using sln_SingleApartment.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace tryTemplete_Room.Controllers
{
    public class RoomBackendController : Controller
    {
        SingleApartmentEntities dbSA = new SingleApartmentEntities();
        // GET: RoomBackend

        #region Wait
        //public ActionResult BackListBuildCase()
        //{
        //    //int pageSize = 10;


        //    var table = from t in (new SingleApartmentEntities()).BuildCase select t;


        //    List<CBuildCaseViewModel> lt_buildcase = new List<CBuildCaseViewModel>();
        //    foreach( BuildCase item in table)
        //    {
        //        lt_buildcase.Add((new CBuildCaseViewModel() { entity_buildcase = item }));
        //    }
        //    //分頁

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
        ////save to the lease table database 
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

        ////[HttpPost]
        ////public ActionResult Edit(tProduct p)
        ////{
        ////  dbDemoEntities db = new dbDemoEntities();
        ////  tProduct prod = db.tProducts.FirstOrDefault(t => t.fId == p.fId);

        ////    if (prod != null)
        ////    {
        ////        prod.fName = p.fName;
        ////        prod.fCost = p.fCost;
        ////        prod.fPrice = p.fPrice;
        ////        prod.fQty = p.fQty;
        ////        prod.fImagePath = p.fImagePath;

        ////        db.SaveChanges();
        ////    }

        ////    return RedirectToAction("List");
        ////}


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



        //public ActionResult BackCreateRoom()
        //{
        //    return View();
        //}





        #endregion


        public ActionResult BackRoomManage(int page = 1, int pageSize = 10)
        {
          
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
           

            var r = dbSA.Room.OrderBy(x => x.ID);
            
            //int total_rent = 0;
            foreach (Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));
               
            }
            
            #region 關於totalrent
            //List<CLeaseViewModel> lease_VM_lt = new List<CLeaseViewModel>();
            //var l = dbSA.Lease.Where(x => x.ExpiryDate > DateTime.Now);
            //foreach (Lease item in l)
            //{
            //    lease_VM_lt.Add(new CLeaseViewModel() { entity_lease = item });
            //    total_rent += item.PersonalRent == null ? 0 : (int)item.PersonalRent;
            //}

            //ViewBag.TotalRent = total_rent;
            #endregion

            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);

            return View(query);
        }

        public ActionResult BackPartialReturnRoomManage(int page = 1, int pageSize = 10)
        {
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();


            var r = dbSA.Room.OrderBy(x => x.ID);

            //int total_rent = 0;
            foreach (Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));

            }

            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;
            return PartialView("_BackPartialReturnRoomManage");
        }
      
        public ActionResult BackPartialKeyWordResult(string keyword ,int page = 1, int pageSize = 10)
        {
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();


            var r = dbSA.Room.Where(x => x.RoomName.Contains(keyword)|| x.Lease.FirstOrDefault().tMember.fMemberName.Contains(keyword)).OrderBy(x=> x.RoomName);
            var test = r.ToList();
            foreach (Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));
            }

            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;
            return PartialView("_BackPartialKeyWordResult");
        }


        public ActionResult BackRoomDetail(string id)
        {

            var table = dbSA.Room.Where(x => x.ID.ToString() == id);


            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
            ;

            foreach (var item in table)
            {
                room_VM_lt.Add(new CRoomViewModel() { entity_room = item });

            }


            return View(room_VM_lt);
        }


        public ActionResult BackPartialaAbtRoom(int page = 1, int pageSize = 10)
        {
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();


            var r = dbSA.Room.OrderBy(x => x.ID);

            foreach (Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));
            }

            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);

            ViewData.Model = query;

            return PartialView("_BackPartialaAbtRoom");
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
        [HttpPost]
        public ActionResult BackEditTheRoomDetail ( Room r)
        {

            Room room = dbSA.Room.FirstOrDefault(t => t.ID == r.ID);
            
            if(room != null)
            {
                room.RoomName = r.RoomName;
                room.RoomType = r.RoomType;
                //room.RoomStyleID = r.RoomStyleID;
                //room.BuildCase.BuildCaseName = r.BuildCase.BuildCaseName;
                room.Description = r.Description;
                room.SquareFootage = r.SquareFootage;
                room.Rent = r.Rent;

                dbSA.SaveChanges();
            }
            return RedirectToAction("BackRoomManage");
        }

        //修改房間
        public ActionResult BackRoomEdit( string id )
        {
            Room r = (new SingleApartmentEntities()).Room.FirstOrDefault(t=> t.ID.ToString() == id);
            if (r == null)
                RedirectToAction("BackRoomManage");
            return View(r);
        }

        [HttpPost]
        public ActionResult BackRoomEdit(Room r)
        {
            SingleApartmentEntities dbSA = new SingleApartmentEntities();

            #region 不確定
            //var m = from tr in dbSA.Room
            //        join tm in dbSA.tMember
            //        on tr.Lease.FirstOrDefault().tMember.fMemberId equals tm.fMemberId
            //        select new { member = tm.fMemberName };
            //List<string> list = new List<string>();

            //foreach (var item in m)
            //{
            //    list.Add(new CRoomViewModel() { entity_room = item.member });

            //}
            #endregion 

            Room room = dbSA.Room.FirstOrDefault(t => t.ID == r.ID);
            if (room != null  )
            {
                room.RoomName = r.RoomName;
                room.RoomType = r.RoomType;
                room.Floor = r.Floor;
                room.Rent = r.Rent;
                room.SquareFootage = r.SquareFootage;

                dbSA.SaveChanges();
            }
            

            return RedirectToAction("BackRoomManage");
        }


        public ActionResult BackPartialGotoLease(int page = 1, int pageSize = 10)
        {
            var table = from t in (new SingleApartmentEntities()).Lease select t;

            List<CLeaseViewModel> lt_lease_lt = new List<CLeaseViewModel>();
            
            foreach (Lease item in table)
            {
                lt_lease_lt.Add((new CLeaseViewModel() { entity_lease = item }));
                
            }
            
            IPagedList<CLeaseViewModel> query = lt_lease_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;

            return PartialView("_BackPartialGotoLease");

            #region total rent 

            //CRoomViewModel room = new CRoomViewModel();

            //var rent = new CRoomRent();
            //rent.RoomRent = room.rent;

            //var rent = from r in dbSA.Room
            //           select r.Rent;
            //var totalofpeople = from l in dbSA.Lease
            //                    where l.ExpiryDate > DateTime.Now 
            //                    select l.RoomID;

            //int total = totalofpeople.Count();
            
            //int personalrent = (int)(rent.FirstOrDefault()) * total;
            #endregion
            
        }

        public ActionResult BackPartialLeaseKeyword(string keyword, int page = 1, int pageSize = 10)
        {
            var table = from t in (new SingleApartmentEntities()).Lease.Where(x => x.Room.RoomName.Contains(keyword) || x.tMember.fMemberName.Contains(keyword) ) select t;

            List<CLeaseViewModel> lt_lease_lt = new List<CLeaseViewModel>();

            foreach (Lease item in table)
            {
                lt_lease_lt.Add((new CLeaseViewModel() { entity_lease = item }));
            }

            IPagedList<CLeaseViewModel> query = lt_lease_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;

            return PartialView("_BackPartialLeaseKeyword");
        }


        public ActionResult BackLeaseDelete(string id)
        {
            Lease l = dbSA.Lease.FirstOrDefault(t => t.ID.ToString() == id);
            if (l != null)
            {
                dbSA.Lease.Remove(l);
                dbSA.SaveChanges();

                return RedirectToAction("BackRoomManage");
            }
            return View(l);
        }

        public ActionResult BackPartialGOtoRoomstyle()
        {
            var table = from t in (new SingleApartmentEntities()).RoomStyle select t;
            List<CRoomStyleViewModel> roomstyle_lt = new List<CRoomStyleViewModel>();

            foreach(RoomStyle item in table)
            {
                roomstyle_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });
            }
            ViewData.Model = roomstyle_lt;
            

            return PartialView("_BackPartialGOtoRoomstyle");
        }

        //public ActionResult BackListRoomStylePic(string id)
        //{
        //    List<CRoomStyleViewModel> roomstyle_VM_lt = new List<CRoomStyleViewModel>();

        //    var table = dbSA.RoomStyle.Where(x => x.ID.ToString() == id);

        //    foreach(var item in table)
        //    {
        //        roomstyle_VM_lt.Add(new CRoomStyleViewModel() { entity_roomstyle = item });

        //    }

        //    return View(roomstyle_VM_lt);
        //}


        //image gallery 
        public ActionResult BackImageGalleryforRoomStyle(/*string id*/)
        {

            //var style = dbSA.RoomStyle.Where(t => t.ID.ToString() == id);

            var imageModel = new CImageGallery();
            var imageFiles = Directory.GetFiles(Server.MapPath("~/Content/Room/images/roomstyle/"));


            foreach (var item in imageFiles)
            {
                imageModel.ImageList.Add(Path.GetFileName(item));
            }
            return View(imageModel);

        }


    }
}