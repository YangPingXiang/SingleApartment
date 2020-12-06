using sln_SingleApartment.Models;
using sln_SingleApartment.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln_SingleApartment.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: 參加活動或團購商品列表
        public ActionResult List()
        {
            SingleApartmentEntities db = new SingleApartmentEntities();
            IEnumerable<Activity> table = from p in db.Activity
                                          select p;

            List<CActivity> list = new List<CActivity>();
            foreach (Activity a in table)
                list.Add(new CActivity() { entity = a });
            return View(list);     
        }
    }
}