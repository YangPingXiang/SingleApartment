using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class COrder
    {
        public Order order_entity { get; set; }

        [DisplayName("訂單編號")]
        public int OrderID { get { return this.order_entity.OrderID; } }
        [DisplayName("會員編號")]
        public int MemberID { get { return this.order_entity.MemberID; } }
        [DisplayName("訂單日期")]
        public System.DateTime OrderDate { get { return this.order_entity.OrderDate; } }
        [DisplayName("到貨日期")]
        public Nullable<System.DateTime> ArrivedDate { get { return this.order_entity.ArrivedDate; } }
        [DisplayName("訂單狀態")]
        public string OrderStatus { get { return this.order_entity.OrderStatus.OrderStatusName; } }
        [DisplayName("配送狀態")]
        public string SendingStatus { get { return this.order_entity.SendingStatus; } }
        [DisplayName("付款狀態")]
        public string PayStatus { get { return this.order_entity.PayStatus; } }
        [DisplayName("總金額")]
        public Nullable<int> TotalAmount { get { return this.order_entity.TotalAmount; } }
    }
}