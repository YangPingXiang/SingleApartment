using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CFavoriteList
    {
        SingleApartmentEntities db = new SingleApartmentEntities();
        public FavoriteList entity { get; set; } 
        public int MemberID { get { return entity.MemberID; } }
        public int ProductID { get { return entity.ProductID;  } }
        public Product Product { get { return db.Product.Where(r => r.ProductID == this.ProductID).FirstOrDefault(); } }
    }
}