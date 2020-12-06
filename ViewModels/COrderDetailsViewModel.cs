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

        public OrderDetails entity { get; set; }
        public int OrderdetailID { get { return entity.OrderdetailID; } }
        public int OrderID { get { return entity.OrderID; } }
        public int ProductID { get { return entity.ProductID; } }
        [DisplayName("數量")]
        public int Quantity { get { return entity.Quantity; } }
        [DisplayName("折扣")]
        public Nullable<float> Discount { get { return entity.Quantity; } }
        //需要的資料～
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }
        [DisplayName("單價")]
        public int? ProductPrice { get; set; }
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