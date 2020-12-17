using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.Models
{
    public class CImage
    {
        public Room entity_room { get; set; }
        public RoomFacilities entity_roomfacility { get; set; }
        //需要的資料
        [DisplayName("房間序號")]
        public int? roomID { get; set; }

        [DisplayName("房間格局序號")]
        public int? roomstyleID { get; set; }

        [DisplayName("房間名稱")]
        public string roomname { get; set; }

        [DisplayName("房間照片序號")]
        public int? roompicID { get; set; }

        [DisplayName("房間類型")]
        public string roomtype { get; set; }

        [DisplayName("房間設備")]
        public int? roomfacility { get; set; }

        [DisplayName("租金")]
        public int? rent { get; set; }

        [DisplayName("建案編號")]
        public string buildcaseID { get; set; }

        [DisplayName("坪數")]
        public int? squarefootage { get; set; }

        [DisplayName("樓層")]
        public int? floor { get; set; }

        [DisplayName("描述")]
        public string description { get; set; }

        public HttpPostedFileBase mypic { get; set; }

        [DisplayName("房型照片")]
        public string roompic { get; set; }

    }
}