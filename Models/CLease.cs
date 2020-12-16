using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.Models
{
    public class CLease
    {
        [DisplayName("合約序號")]
        public int? leaseID { get; set; }

        [DisplayName("房間代號")]
        public int? roomID { get; set; }

        [DisplayName("房間代號")]
        public string roomName { get; set; }

        [DisplayName("房號")]
        public string roomname { get; set; }

        [DisplayName("租約開始日")]
        public DateTime? startdate { get; set; }

        [DisplayName("租約到期日")]
        public DateTime? expirydate { get; set; }

        [DisplayName("會員編號")]
        public int? memberID { get; set; }

        [DisplayName("租金")]
        public int? rent { get; set; }



    }
}