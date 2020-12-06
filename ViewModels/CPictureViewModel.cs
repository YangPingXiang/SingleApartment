using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CPictureViewModel
    {
        public Picture entity_picture { get; set; }

        //需要的資料
        [DisplayName("房型照片")]
        public byte[] roompicture { get { return this.entity_picture.RoomStylePicture; } }
    }
}