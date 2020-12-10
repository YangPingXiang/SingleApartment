using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CActivityNew
    {
        public Activity entity { get; set; }
        public HttpPostedFileBase myImage { get; set; }
        [DisplayName("編號")]
        public int ActivityID { get; set; }

        [DisplayName("活動類別")]
        public int SubCategoryDetailID { get; set; }

        [Required(ErrorMessage = "活動名稱欄位必填!")]
        [DisplayName("活動名稱")]
        public string ActivityName { get; set; }

        [DisplayName("活動人數")]
        public int PeopleCount { get; set; }

        public int MemberID { get; set; }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("開始時間")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public System.DateTime StartTime { get; set; }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("結束時間")]
        //[DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public System.DateTime EndTime { get; set; }

        public string Step { get; set; }

        [DisplayName("備註")]
        public string Note { get; set; }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("活動地點")]
        public string MeetingPoint { get; set; }

        [DisplayName("活動狀態")]
        public string Status { get; set; }

        [DisplayName("活動照片")]
        public string ActivityImage { get; set; }
    }
}
