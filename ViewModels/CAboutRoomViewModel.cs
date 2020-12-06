using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sln_SingleApartment.ViewModels;

namespace sln_SingelApartment.ViewModels
{
    public class CAboutRoomViewModel
    {

        public List<CRoomViewModel> roomViewModels { get; set; }
        public List<CRoomStyleViewModel> roomStyleViewModels { get; set; }
        public List<CPictureViewModel> roomPicViewModels { get; set; }
        public List<CFacilityViewModel> facilityViewModels { get; set; }       
        public List<CBuildCaseViewModel> buildcaseViewModels { get; set; }
        public List<CLeaseViewModel> leaseViewModels { get; set; }
        public List<CMemberViewModel> memberViewModels { get; set; }
        public List<CRoomFacilityViewModel> roomfacilityViewModel { get; set; }


    }
}