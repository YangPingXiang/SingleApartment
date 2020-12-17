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

        [DisplayName("房間格局序號")]
        public int? roomstyleID { get { return this.entity_room.RoomStyle.ID; } }

        [DisplayName("房間名稱")]
        public string roomname {  get { return this.entity_room.RoomName; } }

        [DisplayName("目前房間狀態")]
        public int? status { get { return this.entity_room.Status; } }

        [DisplayName("房間照片序號")]
        public int? roompicID { get { return this.entity_room.RoomPictureID; } }

        [DisplayName("房間格局")]
        public string roomtype { get { return this.entity_room.RoomType; } }
        [DisplayName("房間設備序號")]
        public int? roomfacilityID { get { return this.entity_room.RoomFacilityID; } }

        [DisplayName("租金")]
        public int? rent { get { return this.entity_room.Rent; } }

        [DisplayName("建案編號")]
        public string buildcaseID { get { return this.entity_room.BuildCaseID; } }

        [DisplayName("坪數")]
        public int? squarefootage { get { return this.entity_room.SquareFootage; } }

        [DisplayName("樓層")]
        public int? floor { get { return this.entity_room.Floor; } }

        [DisplayName("描述")]
        public string description { get { return this.entity_room.Description; } }

        public HttpPostedFileBase mypic { get; set; }

        [DisplayName("房型照片")]
        public string roompic { get; set; }

    }



    
}