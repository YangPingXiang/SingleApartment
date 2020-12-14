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

using HttpMethod = System.Net.Http.HttpMethod;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using static sln_SingleApartment.Models.CUser;
using AllPay.Payment.Integration;

namespace sln_SingleApartment.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        #region index.html
        public ActionResult Home()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            //登入
            //============================================
            var user = Session[CDictionary.welcome] as CMember;

            if (user == null) { return RedirectToAction("Login", "Member"); }

            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };

            //如果活動ID有東西將商品加入到HOME 此為團購商品(12/7)
            //===================================================================

            Product pod = new Product();

            ShopViewModel shopv = new ShopViewModel();

            List<CProductViewModel> list = new List<CProductViewModel>();

            List<CProductMainCategoryViewModel> cmcv = new List<CProductMainCategoryViewModel>();

            List<CProductSubCategoryViewModel> cscv = new List<CProductSubCategoryViewModel>();


            //List<CActivity> Activity = new List<CActivity>();
            //=====================================================================
            //取得團購商品(活動尚未結束)
            var ActivityProduct = from g in db.Activity.AsEnumerable()
                                  join p in db.Product.AsEnumerable()
                                  on g.ActivityID equals p.ActivityID
                                  where (g.EndTime >= DateTime.Now && p.Discontinued=="N")|| (g.EndTime <= DateTime.Now || p.Discontinued == "Y")
                                  select p;
        
            foreach (var item in ActivityProduct)
            {
              
               list.Add(new CProductViewModel() { entity = item });

            }
            //=====================================================================
            //分類商品
            var mainCategory = from k in db.ProductMainCategory
                               select k;

            foreach (var m in mainCategory)
            {
                cmcv.Add(new CProductMainCategoryViewModel { entity_MainCategory = m });
            }
            //=====================================================================
          
            shopv.MainCategory = cmcv;

            shopv.product = list;
            
            return View(shopv);
            
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
        #region 以圖搜關鍵字 & 價格搜尋
        [HttpPost]
        public async Task<ActionResult> shop(HttpPostedFileBase imgPhoto)
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }
            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            var result = theUser.SearchProduct();
            if (imgPhoto != null)
            {
                imgPhoto.SaveAs("c:\\temp\\111.jpg");
                FileStream fs = new FileStream("c:\\temp\\111.jpg", FileMode.Open, FileAccess.Read);
                int length = (int)fs.Length;
                byte[] image = new byte[length];
                fs.Read(image, 0, length);
                fs.Close();
                List<string> strs = await MakePredictionRequest(image);
                string Keyword = "";
                foreach (var str in strs)
                {
                    Keyword += (str + " ");
                }
                var list_product = theUser.SearchProductsBy(null, null, Keyword); result.product = list_product;
                ViewBag.ByPhoto = "true";
                
                ViewBag.Keyword = Keyword;
            }
            return View(result);
        }
        public static async Task<List<string>> MakePredictionRequest(byte[] byteData)
        {
            var client = new HttpClient();
            // Request headers - replace this example key with your valid Prediction-Key.
            client.DefaultRequestHeaders.Add("Prediction-Key", "55f6001a7163417b95ab02f37900cf76");

            // Prediction URL - replace this example URL with your valid Prediction URL.
            string url = "https://sgapart-customvision.cognitiveservices.azure.com/customvision/v3.0/Prediction/fafe23de-ddc4-483c-acdc-ec5567cb35aa/classify/iterations/ProductModel/image";

            HttpResponseMessage response = new HttpResponseMessage();

            // Request body. Try this sample with a locally stored image.
            //byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();

                var test = JsonConvert.DeserializeObject<Answer>(answer);
                List<string> Myreturn = new List<string>(); ;
                foreach (var item in test.predictions)
                {
                    var ans = JsonConvert.SerializeObject(item);

                    var anstest = JsonConvert.DeserializeObject<MyObject>(ans);
                    if (anstest.probability > 0.9)
                        Myreturn.Add(anstest.tagName);
                }
                return Myreturn;
            }

        }
        #endregion
        public ActionResult PartialProductTabPane(string MemberID, int page = 1, string pageSize = "6", string MainCategory = null, string SubCategory = null, string KeyWord = "", string firstprice = "", string lastprice = "")
        {
            int currentPage = page < 1 ? 1 : page;
            CUser user = new CUser();
            List<CProductViewModel> lt;
            if(firstprice !="" && lastprice!= "")
                firstprice = firstprice.TrimStart('$'); lastprice = lastprice.TrimStart('$');
            if (KeyWord != "")
            {
                lt = user.SearchProductsBy(null, null, KeyWord);
            }
            else if (int.TryParse(firstprice, out int first) &&int.TryParse(lastprice, out int last) && last > first)
                lt = user.SearchProductsByPrice(first, last);
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
        public ActionResult PartialProductProfile(string MemberID, int page = 1, string pageSize = "6", string MainCategory = null, string SubCategory = null, string KeyWord = "", string firstprice = "", string lastprice = "")
        {
            int currentPage = page < 1 ? 1 : page;
            CUser user = new CUser();
            List<CProductViewModel> lt;
            if (firstprice != "" && lastprice != "")
                firstprice = firstprice.TrimStart('$'); lastprice = lastprice.TrimStart('$');
            if (KeyWord != "")
                lt = user.SearchProductsBy(null, null, KeyWord);
            else if (int.TryParse(firstprice, out int first) && int.TryParse(lastprice, out int last) && last > first)
                lt = user.SearchProductsByPrice(first, last);
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
            if (list != null && list.Count != 0)
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
        //更改單一商品(數量)
        public JsonResult ChangeONEProductQuantity(string ProductID, string Quantity)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            Product prod = db.Product.FirstOrDefault(p => p.ProductID.ToString() == ProductID);
            if (prod != null)
            {
                List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
                if (list != null && list.Count != 0)
                {
                    int Sum = 0;
                    for (int i = 0; i < list.Count; i++)     //foreach沒有辦法去修改自己本身的陣列
                    {
                        if (list[i].txtProductID.ToString() == ProductID)
                        {
                            if (int.TryParse(Quantity, out int qty))
                            {
                                list[i].txtQuantity = qty;
                                Sum += list[i].txtQuantity * prod.UnitPrice;
                                
                            }
                        }
                        else
                        {
                            Sum += list[i].txtQuantity * db.Product.AsEnumerable().FirstOrDefault(p => p.ProductID == list[i].txtProductID).UnitPrice;
                        }
                    }
                    return Json(new { ans = "成功", sum = Sum });
                }
            }
            return Json(new{ans= "發生錯誤"});
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
            return View(orderlist);
        }
        [HttpPost]
        public string CheckOut(string payment_method)
        {
            var user = Session[CDictionary.welcome] as CMember;
            //if (user == null) { return RedirectToAction("Login", "Member"); }

            SingleApartmentEntities db = new SingleApartmentEntities();
            ViewBag.MemberID = user.fMemberId;
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
            List<CAddtoSessionView> list = Session[CDictionary.PRODUCTS_IN_CART] as List<CAddtoSessionView>;
            List<COrderDetailsViewModel> orderlist = theUser.SearchProductInCart(list);
            int TotalPrice = 0;

            foreach (var item in orderlist)
            {

                TotalPrice += (item.ProductPrice == null ? 0 : (int)item.ProductPrice) * item.Quantity;
            }

            List<string> enErrors = new List<string>();

            try
            {
                using (AllInOne oPayment = new AllInOne())
                {

                    var order1 = new Order();

                    var orderdetails = orderlist.Where(p => p.OrderID == order1.OrderID);

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

                    foreach (var item in orderlist)
                    {
                        oPayment.Send.Items.Add(new Item()
                        {
                            Name = item.ProductName,

                            Price = (decimal)item.ProductPrice,

                            Currency = "元",

                            Quantity = item.Quantity
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
            //if (list == null || list.Count == 0)
            //{
            //    return RedirectToAction("ShowProductInCart");
            //}
            //if (theUser.MakeOrder(list)== "成功下訂！") {
            //    Session[CDictionary.PRODUCTS_IN_CART] = null;
            //    return RedirectToAction("OrderList");
            //}
            //else
            //{
            //    return RedirectToAction("CheckOut");
            //}
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
        public ActionResult OrderList()
        {
            var user = Session[CDictionary.welcome] as CMember;
            if (user == null) { return RedirectToAction("Login", "Member"); }
            ViewBag.MemberID = user.fMemberId;
            return View();
        }

        public ActionResult PartialOrders(string MemberID, int page = 1, int pageSize = 6)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId.ToString() == MemberID).FirstOrDefault() };
            int currentPage = page < 1 ? 1 : page;
            var lt = theUser.SearchOrders();
            var result = lt.ToPagedList(currentPage, pageSize);
            ViewData.Model = result;
            ViewBag.MemberID = MemberID;
            return PartialView("_PartialOrders");
        }
        public ActionResult PartialONEOrder(string MemberID, int OrderID)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId.ToString() == MemberID).FirstOrDefault() };
           
            var result = theUser.SearchOrder(OrderID);
            ViewData.Model = result;
            ViewBag.MemberID = MemberID;
            return PartialView("_PartialONEOrder");
        }
        //取消訂單
        public JsonResult Delete(int id)
        {
            try
            {
                var user = Session[CDictionary.welcome] as CMember;
                SingleApartmentEntities db = new SingleApartmentEntities();
                CUser theUser = new CUser() { tMember = db.tMember.Where(r => r.fMemberId == user.fMemberId).FirstOrDefault() };
                ViewBag.MemberID = user.fMemberId;
                var answer = theUser.DeleteAnOrder(id);
                return Json(answer);
            }
            catch(Exception)
            {
                return Json("發生錯誤");
            }
        }

        //#endregion



    }

} 

