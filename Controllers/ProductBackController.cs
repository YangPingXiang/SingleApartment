
using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModel;
using sln_SingleApartment.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln_SingleApartment.Controllers
{
    //(12.3)新增
    public class ProductBackController : Controller
    {
        
        // GET: ProductBack
        public ActionResult ProductList()/*(ok)*/
        {   //關鍵字
            //================================
            string KeyWord = Request.Form["TXTKEYWORD"];

            IEnumerable<Product> prod = null;

            if (string.IsNullOrEmpty(KeyWord))
            {
                prod = from p in (new SingleApartmentEntities()).Product
                       select p;
            }
            else
            {
                prod = from p in (new SingleApartmentEntities()).Product
                       where p.ProductName.Contains(KeyWord)
                       select p;
            }
            //===================================


            List<CProductViewModel> cProd = new List<CProductViewModel>();

            SingleApartmentEntities db = new SingleApartmentEntities();


            //var list = db.ProductPictures.ToList();

            foreach (Product item in prod)
            {

                cProd.Add(new CProductViewModel() { entity = item });

                

            }
            return View(prod);
        }
        //====================================================================
        //團購商品新增(ok)
        public ActionResult Create(int id)
        {            
            SingleApartmentEntities db = new SingleApartmentEntities();
            
            Product prod = new Product();
            
            Activity AV = db.Activity.Where(p => p.ActivityID == id).FirstOrDefault();
            
            prod.ActivityID = AV.ActivityID;

            prod.Stock = AV.PeopleCount;
            //=====================================================================
            //下拉式選單
            var PROsubNamelist = (from p in db.ProductSubCategory
                                  select p.ProductSubCategoryName).ToList();
            
            SelectList SUBNamelist = new SelectList(PROsubNamelist, "Name");
            ViewBag.SUBNAME = SUBNamelist;
            //=====================================================================
            
            CProductViewModel cprod = new CProductViewModel() { entity = prod };//抓一筆資料
            
            return View(cprod);
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase imgPhoto,Product p,string SUBNAME)
        {  
            /*新增照片*/
            SingleApartmentEntities db = new SingleApartmentEntities();

            //======================================================
            
            imgPhoto.SaveAs("c:\\temp\\111.jpg");

            FileStream fs = new FileStream("c:\\temp\\111.jpg", FileMode.Open, FileAccess.Read);

            int length = (int)fs.Length;

            byte[] image = new byte[length];

            fs.Read(image, 0, length);

            fs.Close();
            //=====================================================
            
            //下拉式選單抓id
            
            int SUBID = (from S in db.ProductSubCategory
                        where S.ProductSubCategoryName == SUBNAME
                        select S.ProductSubCategoryID).FirstOrDefault();

            var PROID = (from P in db.Product
                         where P.ProductSubCategoryID == SUBID
                         select P.ProductSubCategoryID).FirstOrDefault();

            p.Discontinued = "N";

            p.ProductSubCategoryID = PROID;
                
            db.Product.Add(p);
            
           //-----------------------

           ProductPictures prodpic = new ProductPictures();

            prodpic.ProductID = p.ProductID;

            //prodpic.ProductPictureID = 2;

            prodpic.ProductPicture = image;

            db.ProductPictures.Add(prodpic);
                
            db.SaveChanges();
            //======================================================
            
            return RedirectToAction("Home","Product");

        }
        //==========================================================
        //商城後台上架商品
        public ActionResult CreatAllProduct()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            //=====================================================================
            //下拉式選單
            var PROsubNamelist = (from p in db.ProductSubCategory
                                  select p.ProductSubCategoryName).ToList();

            SelectList SUBNamelist = new SelectList(PROsubNamelist, "Name");
            ViewBag.SUBNAME = SUBNamelist;
            //====================================================================
            
            return View();
        }
        [HttpPost]
        public ActionResult CreatAllProduct(HttpPostedFileBase imgPhoto, Product p, string SUBNAME)
        {
            /*新增照片*/
            SingleApartmentEntities db = new SingleApartmentEntities();

            //======================================================

            imgPhoto.SaveAs("c:\\temp\\111.jpg");

            FileStream fs = new FileStream("c:\\temp\\111.jpg", FileMode.Open, FileAccess.Read);

            int length = (int)fs.Length;

            byte[] image = new byte[length];

            fs.Read(image, 0, length);

            fs.Close();
            //=====================================================

            //下拉式選單抓id

            int SUBID = (from S in db.ProductSubCategory
                         where S.ProductSubCategoryName == SUBNAME
                         select S.ProductSubCategoryID).FirstOrDefault();

            var PROID = (from P in db.Product
                         where P.ProductSubCategoryID == SUBID
                         select P.ProductSubCategoryID).FirstOrDefault();

            p.Discontinued = "N";

            p.ProductSubCategoryID = PROID;

            db.Product.Add(p);

            //-----------------------


            ProductPictures prodpic = new ProductPictures();

            prodpic.ProductID = p.ProductID;

                //prodpic.ProductPictureID = 2;

            prodpic.ProductPicture = image;

            db.ProductPictures.Add(prodpic);
          
            db.SaveChanges();
            //======================================================

            return RedirectToAction("Home","Product");

        }

        //=====================================================================
        //修改(還未加入商品圖片修改)
        public ActionResult Edit(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            
            Product prod = db.Product.FirstOrDefault(p => p.ProductID == id);

            if (prod == null)
            {
                return RedirectToAction("ProductList");
            }

            return View(prod);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase imgPhoto,Product p)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            //======================================================
            
             imgPhoto.SaveAs("c:\\temp\\111.jpg");

             FileStream fs = new FileStream("c:\\temp\\111.jpg", FileMode.Open, FileAccess.Read);

             int length = (int)fs.Length;

             byte[] image = new byte[length];

             fs.Read(image, 0, length);

             fs.Close();
            
            //=====================================================

            Product prod = db.Product.FirstOrDefault(q => q.ProductID == p.ProductID);
            
            if (prod != null)
            {
              
                prod.ProductName = p.ProductName;

                prod.UnitPrice = p.UnitPrice;

                prod.Stock = p.Stock;

                prod.Sales = p.Sales;

                prod.ProductID = p.ProductID;

                prod.ProductSubCategoryID = p.ProductSubCategoryID;

                prod.Discontinued = p.Discontinued;

                prod.ProductPictures.FirstOrDefault().ProductPicture = image;

                db.SaveChanges();
            }
            //-----------------------
            
            return RedirectToAction("ProductList");

        }
        //=====================================================================
        ////刪除
        public ActionResult Delete(int id)
        {
            SingleApartmentEntities db = new SingleApartmentEntities();

            Product prod = db.Product.FirstOrDefault(p => p.ProductID == id);

            if (prod != null)
            {
                db.Product.Remove(prod);
                db.SaveChanges();


            }

            return RedirectToAction("ProductList");


        }
        //後台訂單(全部人的訂單)
        public ActionResult BackupOrderList(int order_id = 0)
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
        //後台訂單明細(金額,商品名稱,付款狀態)
        public ActionResult BackupList(int ID)
        {
            using (SingleApartmentEntities db = new SingleApartmentEntities())
            {
                var table = (from p in db.OrderDetails
                             where p.OrderID == ID
                             select p).ToList();

                if (table.Count == 0)
                {
                    return RedirectToAction("Home");
                }
                else
                {
                    return View(table);
                }

            }
        }
    }
}