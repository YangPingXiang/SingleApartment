using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln_SingleApartment.ViewModels
{
    public class CRoomListViewModel
    {
        public CRoomSearchViewModel SearchParameter { get; set; }

        public SelectList BuildCaseName { get; set; }
        public SelectList RoomStyleNmae { get; set; }
        public SelectList RoomType { get; set; }

    }
}