using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.Models
{
    public class CRoomBooking
    {

        [Display(Name = "房間編號")]
        public int RoomId { get; set; }

        [Display(Name = "會員編號")]
        public int MemberId { get; set; }
        public string RoomName { get; set; }

        [Display(Name = "入住時間")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Display(Name = "退租時間")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        public string MemberName { get; set; }

    }
}