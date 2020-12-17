using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CLeaseViewModel
    {
        public Lease entity_lease { get; set; }

        [DisplayName("¦X¬ù§Ç¸¹")]
        public int? leaseID { get { return this.entity_lease.ID; } }

        [DisplayName("©Ð¶¡¥N¸¹")]
        public int? roomID { get { return this.entity_lease.RoomID; } }

        [DisplayName("©Ð¸¹")]
        public string roomname { get { return this.entity_lease.Room.RoomName; } }
        [DisplayName("ç§Ÿç??‹å???)]
        [DisplayFormat(DataFormatString = "{0:yyyyå¹?MM??dd?¥}")]
        public DateTime? startdate { get { return this.entity_lease.StartDate; } }

        [DisplayName("ç§Ÿç??°æ???)]
        [DisplayFormat(DataFormatString = "{0:yyyyå¹?MM??dd?¥}")]

        [DisplayName("¯²¬ù¶}©l¤é")]
        public DateTime? startdate { get { return this.entity_lease.StartDate; } }

        [DisplayName("¯²¬ù¨ì´Á¤é")]

        public DateTime? expirydate { get { return this.entity_lease.ExpiryDate; } }

        [DisplayName("·|­û½s¸¹")]
        public int? memberID { get { return this.entity_lease.MemberID; } }


        [DisplayName("¯²ª÷")]
        public int? rent { get { return this.entity_lease.Room.Rent; } }
        [DisplayName("©Ð«È¯²ª÷")]
        public int? personalrent { get { return this.entity_lease.PersonalRent;  } }

    }
}