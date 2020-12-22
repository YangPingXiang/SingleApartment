using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AllPay.Payment.Integration;
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
            if (user == null) { return RedirectToAction("Login", "Member"); }
            
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
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

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

        public ActionResult DeleteMyLease(int id)
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

            var memberId = user.fMemberId;

            Lease l = dbSA.Lease.FirstOrDefault(t => t.ID == id);
            string roomID = null;
            
            if (l != null)
            {
                roomID = l.RoomID.ToString();
                dbSA.Lease.Remove(l);
                dbSA.SaveChanges();

                //退租成功通知訊息
                CInformationFactory x = new CInformationFactory();
                x.Add(memberId, 400, id, 40030);

                var rf = from r in dbSA.RoomFavorite
                         where r.RoomID.ToString() == roomID
                         select r.MemberID;

                if (rf != null)
                {
                    foreach (var item in rf)
                    {
                        int reID = item.Value;
                        //通知空房訊息
                        CInformationFactory y = new CInformationFactory();
                        y.Add(reID, 400, 0 , 40040);

                    }
                }

                return RedirectToAction("SearchPage");

            }

            return View(l);

        }

        public ActionResult MyRoom()
        {
            CAboutRoomViewModel abtRoom_VM = new CAboutRoomViewModel();

            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

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

        public ActionResult RoomCheckOut()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

            ViewBag.MemberID = user.fMemberId;

            var memberRoom = from l in dbSA.Lease
                             where l.MemberID == user.fMemberId
                             select l;
            List<CLeaseViewModel> lease_VM_lt = new List<CLeaseViewModel>();
            foreach(var item in memberRoom)
            {
                lease_VM_lt.Add(new CLeaseViewModel() { entity_lease = item });
            }

            //CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            //List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            
            if (lease_VM_lt == null || lease_VM_lt.Count == 0)
            {
                return RedirectToAction("MyRoom");
            }

            //List<COrderDetailsViewModel> orderlist = theUser.SearchProductInCart(list);
            
            return View(lease_VM_lt);
        }
        [HttpPost]
        public string RoomCheckOut(string payment_method)
        {
            var user = Session[CDictionary.welcome] as CMember;
            ViewBag.MemberID = user.fMemberId;

            //CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            //List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;

            var memberlease = from l in dbSA.Lease
                              where l.MemberID == user.fMemberId
                              select l;
            
            List<CLeaseViewModel> list = new List<CLeaseViewModel>();
            
            foreach(var item in memberlease)
            {
                list.Add(new CLeaseViewModel() { entity_lease = item });
            }

            var leaseid = list.FirstOrDefault().leaseID;
                
           
            //int leaseID = 0;
            //if (cLease != null)
            //{
            //    leaseID = cLease.leaseID == null ? 0 : (int)cLease.leaseID;
            //    //if (orderID != "發生錯誤，請稍後再試！")
            //    //{
            //    //    Session[CDictionary.PRODUCTS_IN_CART] = null;
            //    //}
            //}


            //List<COrderDetailsViewModel> orderlist = theUser.SearchProductInCart(list);

            int TotalPrice = 0;

            foreach (var item in list)
            {
                TotalPrice += (item.rent == null ? 0 : (int)item.rent);
            }
            if (payment_method == "歐付寶")
            {
                List<string> enErrors = new List<string>();
                try
                {
                    using (AllInOne oPayment = new AllInOne())
                    {

                        //var order1 = new Order();
                        //var orderdetails = orderlist.Where(p => p.OrderID == order1.OrderID);
                        int? total = TotalPrice;

                        /* 服務參數 */
                        oPayment.ServiceMethod = AllPay.Payment.Integration.HttpMethod.HttpPOST;
                        oPayment.ServiceURL = "https://payment-stage.opay.tw/Cashier/AioCheckOut/V5";
                        oPayment.HashKey = "5294y06JbISpM5x9";
                        oPayment.HashIV = "v77hoKGq4kWxNNIS";
                        oPayment.MerchantID = "2000132";
                        /* 基本參數 */
                        oPayment.Send.ReturnURL = "http://localhost:44332/Member/Home";
                        oPayment.Send.ClientBackURL = "http://localhost:44332/Product/OrderList";
                        //oPayment.Send.OrderResultURL = "<<您要收到付款完成通知的瀏覽器端網址>>";
                        oPayment.Send.MerchantTradeNo = string.Format("{0:00000}", (new Random()).Next(100000));
                        oPayment.Send.MerchantTradeDate = DateTime.Now;
                        oPayment.Send.TotalAmount = (decimal)total;
                        oPayment.Send.TradeDesc = "感謝您的購買";
                        //oPayment.Send.ChoosePayment = PaymentMethod.ALL;
                        //oPayment.Send.Remark = "<<您要填寫的其他備註>>";
                        oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;
                        oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;
                        oPayment.Send.HoldTrade = HoldTradeType.No;
                        oPayment.Send.DeviceSource = DeviceType.PC;
                        //oPayment.Send.UseRedeem = UseRedeemFlag.Yes; //購物金/紅包折抵
                        //oPayment.Send.IgnorePayment = "<<您不要顯示的付款方式>>"; // 例如財付通:Tenpay
                        //                                                      // 加入選購商品資料。

                        foreach (var item in list)
                        {
                            oPayment.Send.Items.Add(new Item()
                            {
                                Name = item.entity_lease.Room.RoomName,

                                Price = (decimal)item.rent,

                                Currency = "元",

                                //Quantity = item.Quantity
                            });


                        }
                        // 當付款方式為 ALL 時，建議增加的參數。
                        //oPayment.SendExtend.PaymentInfoURL = "<<您要接收回傳自動櫃員機/超商/條碼付款相關資訊的網址。>> ";

                        /* 產生訂單 */
                        enErrors.AddRange(oPayment.CheckOut());
                        /* 產生產生訂單 Html Code 的方法 */
                        string szHtml = String.Empty;
                        enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                        return szHtml;
                    }
                }
                catch (Exception ex)
                {
                    // 例外錯誤處理。
                    enErrors.Add(ex.Message);
                    return ex.Message;

                }
                finally
                {
                    // 顯示錯誤訊息。
                    if (enErrors.Count() > 0)
                    {
                        string szErrorMessage = String.Join("\\r\\n", enErrors);
                    }
                }
            }
            else if (payment_method == "綠界科技")
            {
                List<string> enErrors = new List<string>();
                try
                {
                    using (ECPay.Payment.Integration.AllInOne oPayment = new ECPay.Payment.Integration.AllInOne())
                    {
                        /* 服務參數 */
                        oPayment.ServiceMethod = ECPay.Payment.Integration.HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
                        oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
                        oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
                        oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
                        oPayment.MerchantID = "2000132";//ECPay提供的特店編號

                        /* 基本參數 */
                        oPayment.Send.ReturnURL = "http://localhost:44332/Product/MakeOrderIntoDB";
                        oPayment.Send.ClientBackURL = "http://localhost:44332/Product/OrderList";
                        //oPayment.Send.ReturnURL = "http://example.com";//付款完成通知回傳的網址
                        //oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
                        oPayment.Send.OrderResultURL = "http://localhost:44332/Product/MakeOrderIntoDB";//瀏覽器端回傳付款結果網址
                        oPayment.Send.MerchantTradeNo = "WoJuApartment" + leaseid.ToString();//廠商的交易編號
                        oPayment.Send.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); ;//廠商的交易時間
                        oPayment.Send.TotalAmount = (decimal)TotalPrice;//交易總金額
                        oPayment.Send.TradeDesc = "感謝您的購買^^";//交易描述
                        //oPayment.Send.ChoosePayment = PaymentMethod.ALL;//使用的付款方式
                        oPayment.Send.Remark = "窩居公寓－測試訂單";//備註欄位
                        //oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
                        //oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;//是否需要額外的付款資訊
                        //oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
                        oPayment.Send.CustomField1 = user.fMemberId.ToString(); ;
                        //oPayment.Send.CustomField2 = leaseid;
                        //訂單的商品資料
                        foreach (var item in list)
                        {
                            oPayment.Send.Items.Add(new ECPay.Payment.Integration.Item()
                            {
                                Name = item.entity_lease.Room.RoomName,
                                Price = (decimal)item.rent,
                                Currency = "元",
                                //Quantity = item.Quantity
                            });
                        }

                        /*************************非即時性付款:ATM、CVS 額外功能參數**************/

                        #region ATM 額外功能參數

                        //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
                        //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                        //oPayment.SendExtend.ClientRedirectURL = "";//Client 端回傳付款相關資訊

                        #endregion
                        #region CVS 額外功能參數

                        //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                        //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                        //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                        //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                        //oPayment.SendExtend.Desc_4 = "";//交易描述 4
                        //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                        //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                        #endregion

                        /***************************信用卡額外功能參數***************************/

                        #region Credit 功能參數

                        //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
                        //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
                        //oPayment.SendExtend.Language = ""; //語系設定

                        #endregion Credit 功能參數
                        #region 一次付清

                        //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
                        //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

                        #endregion
                        #region 分期付款

                        //oPayment.SendExtend.CreditInstallment = "3,6";//刷卡分期期數

                        #endregion 分期付款
                        #region 定期定額

                        //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
                        //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
                        //oPayment.SendExtend.Frequency = 1;//執行頻率
                        //oPayment.SendExtend.ExecTimes = 2;//執行次數
                        //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

                        #endregion

                        /* 產生訂單 */
                        enErrors.AddRange(oPayment.CheckOut());
                        /* 產生產生訂單 Html Code 的方法 */
                        string szHtml = String.Empty;
                        enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                        return szHtml;

                    }

                }
                catch (Exception ex)
                {
                    // 例外錯誤處理。
                    enErrors.Add(ex.Message);
                    return ex.Message;
                }
                finally
                {
                    // 顯示錯誤訊息。
                    if (enErrors.Count() > 0)
                    {
                        string szErrorMessage = String.Join("\\r\\n", enErrors);
                    }
                }

            }
            else
            {
                return "其他";
            }
        }

        public string RoomSendMail(string strHtml)
        {
            string htmlBody = strHtml.ToString();

            SingleApartmentEntities entity = new SingleApartmentEntities();
            try
            {
                //string sMemberEmail = "";
                MailMessage mail = new MailMessage();
                //string email = "dddd";
                mail.From = new MailAddress("singleapart@gmail.com");
                //多收信人, 使用,隔開, 而不是;喔

                #region 活動建立後媒合訊息發送
                List<int> MemberIDList = new List<int>();
                List<string> MemberMemberEmailList = new List<string>();

                var user = Session[CDictionary.welcome] as CMember;
                tMember tMember = new tMember();
             
                var membermessage = from mbmsg in entity.tMember
                                    where mbmsg.fMemberId == user.fMemberId
                                    select new { MbID = mbmsg.fMemberId, MbEmail = mbmsg.fEmail };
                
                foreach (var m in membermessage)
                {
                    MemberIDList.Add(m.MbID);
                    MemberMemberEmailList.Add(m.MbEmail);
                }
                for (int me = 0; me < MemberIDList.Count; me++)
                {
                    //if(MemberfActivityMessageList[me] == "TRUE")
                    //{

                    mail.To.Add(MemberMemberEmailList[me]);
                    //}


                }
                #endregion

                //mail.To.Add("singleapart@gmail.com,apple385827@gmail.com"); 
                mail.To.Add("singleapart@gmail.com");//new MailAddress("singleapart@gmail.com")
                mail.Subject = "窩居公寓:租房成功通知!";
                //mail.Date = DateTime.Now;

                mail.Body = htmlBody;
                //mail.Body = $@"<h1 style='text-align:center;color:#ff0000'>iTicket 訂購成功</h1>{email}";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
                {
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new NetworkCredential("singleapart@gmail.com", "wojuwoju");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}