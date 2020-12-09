using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class COrderDetailsViewModel
    {
        private SingleApartmentEntities db = new SingleApartmentEntities();
        public OrderDetails entity { get; set; }
        public int OrderdetailID { get { return entity.OrderdetailID; } }
        public int OrderID { get { return entity.OrderID; } }
        public int ProductID { get { return entity.ProductID; } }
        [DisplayName("數量")]
        public int Quantity { get { return entity.Quantity; } }
        [DisplayName("折扣")]
        public Nullable<float> Discount { get { return entity.Quantity; } }
        //需要的資料～
        private Product product { get { return db.Product.Where(r => r.ProductID == this.ProductID).FirstOrDefault(); } }
        [DisplayName("商品名稱")]
        public string ProductName { get { return product.ProductName; } }
        [DisplayName("單價")]
        public int? ProductPrice { get { return product.UnitPrice; } }
        [DisplayName("商品首圖")]
        public ProductPictures prodpic { get { return product.ProductPictures.FirstOrDefault(); } }
        [DisplayName("小計")]
        public int? TotalPrice
        {
            get
            {
                if ( ProductPrice != null)
                    return (int)Quantity * (int)ProductPrice;
                else return null;
            }
        }
    }
}