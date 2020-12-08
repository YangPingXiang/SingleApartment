using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CRoomFacilityViewModel
    {
        public RoomFacilities entity_roomfacilities { get; set; }

        public int ID { get { return this.entity_roomfacilities.ID; } }

        public int? roomID { get { return this.entity_roomfacilities.RoomID; } }

        public int? facilityID { get { return this.entity_roomfacilities.FacilityID; } }
    }
}