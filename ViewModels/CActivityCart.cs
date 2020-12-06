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

        public int fJoinedId { get { return this.entity.fJoinedId; } }
        [DisplayName("發起人")]
        public Nullable<int> fMemberId { get { return this.entity.fMemberId; } }
        [DisplayName("活動名稱")]
        public string fAvtivityName { get { return this.entity.fAvtivityName; } }
        [DisplayName("開始時間")]
        public Nullable<System.DateTime> fStartTime { get { return this.entity.fStartTime; } }
        [DisplayName("結束時間")]
        public Nullable<System.DateTime> fEndTime { get { return this.entity.fEndTime; } }
        [DisplayName("地點")]
        public string fLocation { get { return this.entity.fLocation; } }
        [DisplayName("人數")]
        public Nullable<int> fPeopleCount { get { return this.entity.fPeopleCount; } }

        public string fStatus { get { return this.entity.fStatus; } }
        [DisplayName("備註")]
        public string fNote { get { return this.entity.fNote; } }


    }
}