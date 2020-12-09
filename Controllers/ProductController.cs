using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using global::sln_SingleApartment.Models;
using Newtonsoft.Json;
using sln_SingleApartment.ViewModels;
using PagedList;
using sln_SingleApartment.ViewModel;
using System.Net.Http;
using AllPay.Payment.Integration;
using HttpMethod = System.Net.Http.HttpMethod;

namespace sln_SingleApartment.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        #region index.html
        public ActionResult Home()
        {

            SingleApartmentEntities db = new SingleApartmentEntities();

            //關鍵字查詢
            //============================================
            string KeyWord = Request.Form["TXTkeyword"];

            IEnumerable<Product> table = null;

            if (string.IsNullOrEmpty(KeyWord))
            {
                table = from p in db.Product
                        select p;
            }
            else
            {
                table = from p in db.Product
                        where p.ProductName.Contains(KeyWord)
                        select p;
            }
        
            //============================================
            var user = Session[CDictionary.welcome] as CMember;

            if (user == null) { return RedirectToAction("Login", "Member"); }

           
            
            List<CProductViewModel> list = new List<CProductViewModel>();


            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            //如果活動ID有東西將商品加入到HOME 此為團購商品(12/7)
            //===================================================================

            DateTime time = DateTime.Now;

            Activity AV = new Activity();

            Product PID = db.Product.FirstOrDefault(P => P.ActivityID >= 0);

            if (AV.EndTime >= time && PID != null)
            {
                foreach (var item in db.Product.Where(q => q.ActivityID != null && q.Discontinued == "N"))
                {

                    list.Add(new CProductViewModel() { entity = item });

                }
              
            }
            else
            {
                PID.Discontinued = "Y";
                db.SaveChanges();

            }
            return View(list);
        }
        //=======================================================================
           
        #endregion
        public ActionResult test()
        {
            return View();
        }
        #region shop.cshtml
        public ActionResult shop()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }
            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            var mymodel = theUser.SearchProduct();
            return View(mymodel);
        }
        public JsonResult GetProductShowing(string condition, string id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            List<CProductViewModel> list_product = new List<CProductViewModel>();
            if (condition.Trim() == "all")
            {
                foreach (var item in db.Product.Where(r => r.Discontinued == "N" && r.Stock >= 0))
                {
                    list_product.Add(new CProductViewModel() { entity = item });
                }
            }
            else if (condition == "SubCategory" && id != null)
            {
                foreach (var item in db.Product.Where(r => r.Discontinued == "N" && r.Stock >= 0 && r.ProductSubCategoryID.ToString() == id))
                {
                    list_product.Add(new CProductViewModel() { entity = item });
                }
            }
            else if (condition == "MainCategory" && id != null)
            {
                foreach (var item in db.Product.Where(r => r.Discontinued == "N" && r.Stock >= 0 && r.ProductSubCategory.ProductMainCategoryID.ToString() == id))
                {
                    list_product.Add(new CProductViewModel() { entity = item });
                }
            }
            List<string> ArrayList = new List<string>();
            foreach (var item in list_product)
            {
                var json = JsonConvert.SerializeObject(item);
                ArrayList.Add(json);
            }
            return Json(ArrayList);
        }
        public ActionResult PartialProductTabPane(string MemberID, int page = 1, string pageSize = "6", string MainCategory = null, string SubCategory = null, string KeyWord = "")
        {
            int currentPage = page < 1 ? 1 : page;
            CUser user = new CUser();
            List<CProductViewModel> lt;
            if (KeyWord != "")
                lt = user.SearchProductsBy(null, null, KeyWord);
            else if (SubCategory != null)
                lt = user.SearchProductsBy(null, int.Parse(SubCategory));
            else if (MainCategory != null)
                lt = user.SearchProductsBy(int.Parse(MainCategory));
            else
                lt = user.SearchProductsBy();
            var result = lt.ToPagedList(currentPage, int.Parse(pageSize));
            ViewData.Model = result;
            ViewBag.MemberID = MemberID;
            ViewBag.pageSize = pageSize;
            return PartialView("_PartialProduct_TabPane");
        }
        public ActionResult PartialProductProfile(string MemberID, int page = 1, string pageSize = "6", string MainCategory = null, string SubCategory = null, string KeyWord = "")
        {
            int currentPage = page < 1 ? 1 : page;
            CUser user = new CUser();
            List<CProductViewModel> lt;
            if (KeyWord != "")
                lt = user.SearchProductsBy(null, null, KeyWord);
            else if (SubCategory != null)
                lt = user.SearchProductsBy(null, int.Parse(SubCategory));
            else if (MainCategory != null)
                lt = user.SearchProductsBy(int.Parse(MainCategory));
            else
                lt = user.SearchProductsBy();
            var result = lt.ToPagedList(currentPage, int.Parse(pageSize));
            ViewData.Model = result;
            ViewBag.MemberID = MemberID;
            ViewBag.pageSize = pageSize;
            return PartialView("_PartialProduct_Profile");
        }


        #endregion
        #region FavoriteList.cshtml (1130改)
        public JsonResult AddToFavorite(string ProductID)
        {
            var user = Session[CDictionary.welcome] as CMember;
            SingleApartmentEntities db = new SingleApartmentEntities();
            var result = "發生錯誤，請重新登入再試一次！";
            if (user != null && int.TryParse(ProductID, out int pdID))
            {
                CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
                result = theUser.AddToFavorite(pdID);
            }
            return Json(JsonConvert.SerializeObject(new { ans = result }));
        }

        public ActionResult Favoritelist()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }
            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            var list = theUser.SearchFavorite();
            return View(list);
        }

        public JsonResult DeleteFavorite(string ProductID)
        {
            var user = Session[CDictionary.welcome] as CMember;
            SingleApartmentEntities db = new SingleApartmentEntities();
            var result = "發生錯誤，請重新登入再試一次！";
            if (user != null && int.TryParse(ProductID, out int pdID))
            {
                CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
                result = theUser.DeleteFavorite(pdID);
            }
            return Json(JsonConvert.SerializeObject(new { ans = result }));
        }

        public ActionResult PartialFavorite(string MemberID, int page = 1, int pageSize = 6)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            var list = db.FavoriteList.Where(r => r.MemberID.ToString() == MemberID);
            int currentPage = page < 1 ? 1 : page;
            List<CFavoriteList> lt = new List<CFavoriteList>();
            foreach (var item in list)
            {
                lt.Add(new CFavoriteList { entity = item });
            }
            var result = lt.ToPagedList(currentPage, pageSize);
            ViewData.Model = result;
            ViewBag.MemberID = MemberID;
            return PartialView("_PartialFavorite");
        }
        #endregion
        #region 購物車
        //加到購物車，同步更新右上角的購物車區塊
        public ActionResult AddToCart(string ProductID = null, string Quantity = "1")
        {
            var list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            if (list == null)
            {
                list = new List<CAddtoSessionView>();
            }
            if (ProductID != null)
            {
                //如果購物車裡還沒有就Add、已經有了就加數量
                if (int.TryParse(ProductID, out int pdID))
                {
                    if (list.Where(r => r.txtProductID == pdID).Count() == 0)
                    {
                        list.Add(new CAddtoSessionView() { txtProductID = pdID, txtQuantity = int.Parse(Quantity) });
                    }
                    else
                    {
                        list.Where(r => r.txtProductID == pdID).FirstOrDefault().txtQuantity += int.Parse(Quantity);
                    }
                }
                Session[CDictionary.PRODUCTS_IN_CART] = list;
            }
            if (list.Count != 0)
            {
                ViewData.Model = (new CUser()).SearchProductInCart(list);
            }
            return PartialView("_PartialShoppingCart");
        }
        //顯示購物車內容
        public ActionResult ShowProductInCart()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            if (list != null &&list.Count != 0)
                return View(theUser.SearchProductInCart(list));
            else
                return View();
        }
        //刪除購物車商品(一鍵清除)
        public ActionResult RemoveProductsInCart()
        {
            List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            if (list != null)
            {
                list = new List<CAddtoSessionView>();
                Session[CDictionary.PRODUCTS_IN_CART] = list;
            }
            return RedirectToAction("ShowProductInCart");
        }
        //刪除單一商品
        public JsonResult RemoveONEProductInCart(string ProductID)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Product prod = db.Product.FirstOrDefault(p => p.ProductID.ToString() == ProductID);

            if (prod != null)
            {
                List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
                if (list != null && list.Count != 0)
                {
                    for (int i = 0; i < list.Count; i++)     //foreach沒有辦法去修改自己本身的陣列
                    {
                        if (list[i].txtProductID.ToString() == ProductID)
                        {
                            list.Remove(list[i]);
                            return Json("成功");
                        }
                    }
                }
            }
            return Json("沒有此商品");
        }
        #endregion
        #region Checkout

        //結帳畫面{12.6)
        public ActionResult CheckOut()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }

            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            if (list == null || list.Count == 0)
            {
                return RedirectToAction("ShowProductInCart");
            }
            List<COrderDetailsViewModel> orderlist = theUser.SearchProductInCart(list);

                //CUser theUser = new CUser();
                ////===================================================================
                //var user = Session[CDictionary.welcome] as CMember;
                ////必須先登入會員 
                //if (user != null)
                //{
                //    user.fMemberName = Request.Form["TXTMEMBERNAME"];
                //    user.fPhone= Request.Form["TXTPHONE"];
                //    user.fEmail = Request.Form["TXTEMAIL"];
                //    //user.fBirthDate =Request.Form[""];
                //}
                //===================================================================
                
             return View(orderlist);
        }
        #endregion 
        //#region 秉庠


        ////===========================================================================
        ////傳回資料庫
        //[HttpPost]
        //public ActionResult ShowProductInCart(int MemberID)//第四步
        //{
        //    int totalPrice = 0;

        //    SingleApartmentEntities DB = new SingleApartmentEntities();

        //    List<COrderDetailsViewModel> list = Session[CDictionary.PRODUCTS_IN_CART] as List<COrderDetailsViewModel>;

        //    Order od = new Order();
        //    //抓出多筆資料
        //    foreach (var item in list)
        //    {
        //        OrderDetails ODD = new OrderDetails();

        //        ODD.ProductName = DB.Product.Where(p => p.ProductID == item.ProductID).FirstOrDefault().ProductName;

        //        ODD.Quantity = item.Quantity;

        //        ODD.ProductID = item.ProductID;

        //        ODD.UnitPrice = DB.Product.Where(p => p.ProductID == item.ProductID).FirstOrDefault().UnitPrice;

        //        totalPrice += item.Quantity * (DB.Product.Where(p => p.ProductID == item.ProductID).FirstOrDefault().UnitPrice);

        //        od.OrderDetails.Add(ODD);
        //    }

        //    //訂單日期
        //    od.OrderDate = DateTime.Now;
        //    //到貨日期
        //    od.ArrivedDate = DateTime.Now.AddDays(7);
        //    //總金額
        //    od.TotalAmount = totalPrice;

        //    od.OrderStatusID = 1;

        //    od.SendingStatus = "配送中";

        //    od.PayStatus = "尚未付款";

        //    od.MemberID = MemberID;//到時候要改成使用者的memberID


        //    DB.Order.Add(od);

        //    DB.SaveChanges();

        //    return RedirectToAction("Home");

        //}
        //訂單
        public ActionResult OrderList(int order_id = 0)
        {

            bool l_flag = false;  //顯示訂單明細
            SingleApartmentEntities db = new SingleApartmentEntities();

            int member_id = db.Order.FirstOrDefault().MemberID;


            IEnumerable<Order> l_order = from x in db.Order
                                         where x.MemberID > 0   //之後要改成memberID  先抓全部
                                         select x;


            List<COrder> list = new List<COrder>();
            foreach (Order o in l_order)
            {
                list.Add(new COrder() { order_entity = o });
            }

            IEnumerable<OrderDetails> l_orderdetail = from p in db.OrderDetails
                                                      where p.OrderID == order_id
                                                      select p;
            List<COrderDetails> odlist = new List<COrderDetails>();
            foreach (OrderDetails od in l_orderdetail)
            {
                var prod = db.Product.FirstOrDefault(x => x.ProductID == od.ProductID);
                odlist.Add(new COrderDetails() { entity = od, product_entity = prod });
            }

            COrderMasterDetail a = new COrderMasterDetail() { display_flag = l_flag, t_order = list, t_orderDetail = odlist };

            return View(a);

        }
        //訂單明細
        public ActionResult List(int ID)
        {
            using (SingleApartmentEntities db = new SingleApartmentEntities())
            {
                var table = (from p in db.OrderDetails
                             where p.OrderID == ID
                             select p).ToList();
                
                //===================================================================
                //歐付寶頁面
                //=======================================
                List<string> enErrors = new List<string>();
                try
                {
                    using (AllInOne oPayment = new AllInOne())
                    {
                        /* 服務參數 */
                        oPayment.ServiceMethod = AllPay.Payment.Integration.HttpMethod.HttpPOST;
                        oPayment.ServiceURL = "Http://payment-stage.opay.tw/Cashier/AioCheckOut/V5";
                        oPayment.HashKey = "5294y06JbISpM5x9";
                        oPayment.HashIV = "v77hoKGq4kWxNNIS";
                        oPayment.MerchantID = "	2000132";
                        /* 基本參數 */
                        oPayment.Send.ReturnURL = "http://localhost:44332/Product/CheckOut";
                        oPayment.Send.ClientBackURL = "http://localhost:44332/Product/CheckOut";
                        oPayment.Send.MerchantTradeNo = string.Format("{0:00000}", (new Random()).Next(100000));//亂數
                        oPayment.Send.MerchantTradeDate = DateTime.Now;
                        oPayment.Send.TotalAmount = Decimal.Parse("<<您此筆訂單的交易總金額>>");
                        oPayment.Send.TradeDesc = "買起來!!!!!!";
                        oPayment.Send.DeviceSource = DeviceType.PC;


                        //加入選購商品資料。
                        //foreach (var AA in list)
                        //{
                        //    oPayment.Send.Items.Add(new Item()
                        //    {
                        //        Name =li,
                        //        Price =,
                        //        Currency = "元",
                        //        Quantity =

                        //     });
                        //}


                        // 當付款方式為 ALL 時，建議增加的參數。
                        // oPayment.SendExtend.PaymentInfoURL = "<<您要接收回傳自動櫃員機/超商/條碼付款相關資訊的網
                        //址。>> ";
                        /* 產生訂單 */

                        enErrors.AddRange(oPayment.CheckOut());
                        /* 產生產生訂單 Html Code 的方法 */
                        string szHtml = String.Empty;
                        enErrors.AddRange(oPayment.CheckOutString(ref szHtml));
                    }
                }
                catch (Exception ex)
                {
                    // 例外錯誤處理。
                    enErrors.Add(ex.Message);
                }
                finally
                {
                    // 顯示錯誤訊息。
                    if (enErrors.Count() > 0)
                    {
                        string szErrorMessage = String.Join("\\r\\n", enErrors);
                    }
                }
                //==============================================================================




        }
        //取消訂單
        public ActionResult Delete(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            Order od = db.Order.FirstOrDefault(p => p.OrderID == id);

            var odd = db.OrderDetails.Where(q => q.OrderID == id);

            if (odd != null)
            {

                foreach (var ITEM in odd)
                {
                    db.OrderDetails.Remove(ITEM);

                }
                if (od != null)
                {
                    db.Order.Remove(od);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Home");
        }

        //#endregion


    }
}