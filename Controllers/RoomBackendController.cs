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
            return View();
        }

        public ActionResult BackPartialReturnRoomManage(int page = 1, int pageSize = 10, string keyword = null, string Cname = null)
        {
            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
            IQueryable<Room> r;
            if (String.IsNullOrEmpty(keyword))
                r = dbSA.Room;
            else
                r = dbSA.Room.Where(o => o.RoomName.Contains(keyword));
            if (!String.IsNullOrEmpty(Cname))
                r = r.Where(o => (o.Lease.Where(k => k.tMember.fMemberName.Contains(Cname))).Count() != 0);
            //int total_rent = 0;
            foreach (Room item in r)
            {
                room_VM_lt.Add((new CRoomViewModel() { entity_room = item }));
            }

            IPagedList<CRoomViewModel> query = room_VM_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;
            ViewBag.keyword = keyword;
            ViewBag.Cname = Cname;
            return PartialView("_BackPartialReturnRoomManage");
        }
      

        public ActionResult BackRoomDetail(string id)
        {

            var table = dbSA.Room.Where(x => x.ID.ToString() == id);


            List<CRoomViewModel> room_VM_lt = new List<CRoomViewModel>();
           
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

        public ActionResult BackEditTheRoomDetail(int id)
        {

            Room r = dbSA.Room.Where(t => t.ID == id).FirstOrDefault(); //(new SingleApartmentEntities()).Room.FirstOrDefault(t => t.ID.ToString() == id);
                                                                        // RoomFacilities f = dbSA.RoomFacilities.Where(t => t.RoomID == id).FirstOrDefault();
            CImage image_VM = new CImage();
            image_VM.roomID = id;
            image_VM.roomname = r.RoomName;
            image_VM.roomtype = r.RoomType;
            image_VM.floor = r.Floor;
            image_VM.rent = r.Rent;
            image_VM.squarefootage = r.SquareFootage;
            //f.Facility.FacilityName = r.RoomFacilities.FirstOrDefault().Facility.FacilityName; //  f.Facility.FacilityName ;

            if (r == null)
                RedirectToAction("BackRoomManage");
            return View(image_VM);
        }

        [HttpPost]
        public ActionResult BackEditTheRoomDetail(CImage r)
        {

            Room room = dbSA.Room.Where(t => t.ID == r.roomID).FirstOrDefault();

           

            if (room != null)
            {

                int index = r.mypic.FileName.IndexOf(".");
                string extention = r.mypic.FileName.Substring(index, r.mypic.FileName.Length - index);
                string photoName = Guid.NewGuid().ToString() + extention;
                r.entity_room = room;
                r.entity_room.Picture.FirstOrDefault().RoomStylePicture = "/Content/" + photoName;
                r.mypic.SaveAs(Server.MapPath("/Content/") + photoName);

                room.RoomName = r.roomname;
                room.RoomType = r.roomtype;

                //room.BuildCase.BuildCaseName = r.BuildCase.BuildCaseName;
                room.Description = r.description;
                room.SquareFootage = r.squarefootage;
                room.Rent = r.rent;

                Picture picture = new Picture();
                picture.RoomStylePicture = r.entity_room.Picture.FirstOrDefault().RoomStylePicture;
                picture.RoomID = r.roomID;

                dbSA.SaveChanges();
            }
            return RedirectToAction("BackRoomManage");
        }


        public ActionResult BackPartialGotoLease(int page = 1, int pageSize = 10, string keyword = null, string Cname = null, string Order = null)
        {
            IQueryable<Lease> table;
            if (String.IsNullOrEmpty(keyword))
                table = from t in (new SingleApartmentEntities()).Lease select t;
            else
                table = from t in (new SingleApartmentEntities()).Lease where t.Room.RoomName.Contains(keyword) select t;

            if (!String.IsNullOrEmpty(Cname))
                table = table.Where(r => r.MemberID != null && r.tMember.fMemberName.Contains(Cname));
            if (!String.IsNullOrEmpty(Order)&& Order== "ExpiryDateD")
                table = table.OrderByDescending(x => x.ExpiryDate);
            else if (!String.IsNullOrEmpty(Order) && Order == "ExpiryDateA")
                table = table.OrderBy(x => x.ExpiryDate);
            List < CLeaseViewModel > lt_lease_lt = new List<CLeaseViewModel>();
            
            foreach (Lease item in table)
            {
                lt_lease_lt.Add((new CLeaseViewModel() { entity_lease = item }));
                
            }
            IPagedList<CLeaseViewModel> query = lt_lease_lt.ToPagedList(page, pageSize);
            ViewData.Model = query;
            ViewBag.keyword = keyword;
            ViewBag.Cname = Cname;
            ViewBag.Order = Order;
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