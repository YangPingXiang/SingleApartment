using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class COrderDetails
    {
        public Product product_entity { get; set; }  /*為了取的產品名稱*/

        public OrderDetails entity { get; set; }

        public int OrderdetailID { get { return this.entity.OrderdetailID; } }
        [DisplayName("訂單編號")]
        public int OrderID { get { return this.entity.OrderID; } }
        [DisplayName("產品編號")]
        public int ProductID { get { return this.entity.ProductID; } }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "數量為必填欄位")]
        [DisplayName("數量")]
        public int Quantity { get { return this.entity.Quantity; } }
        [DisplayName("折扣")]
        public Nullable<decimal> Discount { get { return this.entity.Discount.Discount1; } }
        [DisplayName("單價")]
        public int UnitPrice { get { return this.entity.UnitPrice; } }
    }
}