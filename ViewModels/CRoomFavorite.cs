using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CRoomFavorite
    {
        SingleApartmentEntities db = new SingleApartmentEntities();
        public RoomFavorite entity_RoomFavorite { get; set; }
        public int? MemberID { get { return entity_RoomFavorite.MemberID; } }
        public int? RoomID { get { return entity_RoomFavorite.RoomID; } }
        public Room Room { get { return db.Room.Where(r => r.ID == this.RoomID).FirstOrDefault(); } }
        public CRoomViewModel room { get { return new CRoomViewModel() { entity_room = this.Room }; } }

    }
}