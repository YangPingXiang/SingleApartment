using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class CActivity
    {
        public Activity entity { get; set; }

        [DisplayName("編號")]
        public int ActivityID { get { return this.entity.ActivityID; } }

        [DisplayName("活動類別")]
        public int SubCategoryDetailID { get { return this.entity.SubCategoryDetailID; } }

        [Required(ErrorMessage = "活動名稱欄位必填!")]
        [DisplayName("活動名稱")]
        public string ActivityName { get { return this.entity.ActivityName; } }

        [DisplayName("活動人數")]
        public int PeopleCount { get { return this.entity.PeopleCount; } }

        public int MemberID { get { return this.entity.MemberID; } }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("開始時間")]
        public System.DateTime StartTime { get { return this.entity.StartTime; } }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("結束時間")]
        public System.DateTime EndTime { get { return this.entity.EndTime; } }

        public string Step { get { return this.entity.Step; } }

        [DisplayName("備註")]
        public string Note { get { return this.entity.Note; } }

        [Required(ErrorMessage = "此處欄位必填!")]
        [DisplayName("活動地點")]
        public string MeetingPoint { get { return this.entity.MeetingPoint; } }

        [DisplayName("活動狀態")]
        public string Status { get { return this.entity.Status; } }


        
    }
}