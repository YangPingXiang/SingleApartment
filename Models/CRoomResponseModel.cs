using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.Models
{
    public class CRoomResponseModel
    {
        public Room entity_room { get; set; }

        //需要的資料
        public int? roomID { get; set; }

        public int? roomstyleID { get; set; }

        public string roomname { get; set; }

        public int? roompicID { get; set; }

        public string roomtype { get; set; }

        public int? roomfacility { get; set; }

        public int? rent { get; set; }

        public string buildcaseID { get; set; }

        public int? squarefootage { get; set; }

        public int? floor { get; set; }

        public string display { get; set; }
    }
}