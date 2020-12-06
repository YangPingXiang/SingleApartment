using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CFacilityViewModel
    {
        [DisplayName("設備總表")]
        public Facility entity_Facility { get; set; }

        //需要的資料
        [DisplayName("設備序號")]
        public int facilityId { get { return this.entity_Facility.ID; } }
        [DisplayName("設備名稱")]
        public string facilityName  { get { return this.entity_Facility.FacilityName; } }
    }
}