using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CRoomViewModel
    {
        public Room entity_room { get; set; }

        //需要的資料
        [DisplayName("房間序號")]
        public int? roomID { get { return this.entity_room.ID; } }
        
        [DisplayName("房型序號")]
        public int? roomstyleID { get { return this.entity_room.RoomStyle.ID; } }

        [DisplayName("房型照片")]
        public HttpPostedFileBase image { get; set; }

        [DisplayName("建案編號")]
        public string buildcaseID { get { return this.entity_room.BuildCaseID; } }

        [DisplayName("房間名稱")]
        public string roomname { get { return this.entity_room.RoomName; } }

    }
}