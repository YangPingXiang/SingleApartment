using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CLeaseViewModel
    {
        public Lease entity_lease { get; set; }

        [DisplayName("合約序號")]
        public int? leaseID { get { return this.entity_lease.ID; } }

        [DisplayName("房間代號")]
        public int? roomID { get { return this.entity_lease.RoomID; } }

        [DisplayName("房號")]
        public string roomname { get { return this.entity_lease.Room.RoomName; } }

        [DisplayName("租約開始日")]
        public DateTime? startdate { get { return this.entity_lease.StartDate; } }

        [DisplayName("租約到期日")]
        public DateTime? expirydate { get { return this.entity_lease.ExpiryDate; } }

        [DisplayName("會員編號")]
        public int? memberID { get { return this.entity_lease.MemberID; } }
    }
}