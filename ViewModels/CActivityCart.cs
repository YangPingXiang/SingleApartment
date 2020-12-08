using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using sln_SingleApartment.Models;
using Activity = sln_SingleApartment.Models.Activity;

namespace sln_SingleApartment.ViewModels
{
    public class CActivityCart
    {
        public tActivityCart entity { get; set; }

        public int fJoinedId { get; set; }
        [DisplayName("發起人")]
        public Nullable<int> fMemberId { get; set; }
        [DisplayName("活動名稱")]
        public string fAvtivityName { get; set; }
        [DisplayName("開始時間")]
        public Nullable<System.DateTime> fStartTime { get; set; }
        [DisplayName("結束時間")]
        public Nullable<System.DateTime> fEndTime { get; set; }
        [DisplayName("地點")]
        public string fLocation { get; set; }
        [DisplayName("人數")]
        public Nullable<int> fPeopleCount { get; set; }

        public string fStatus { get; set; }
        [DisplayName("備註")]
        public string fNote { get; set; }


    }
}