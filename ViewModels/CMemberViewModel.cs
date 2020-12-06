using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CMemberViewModel
    {
        public tMember entity_Member { get; set; }

        public int ID { get { return this.entity_Member.fMemberId; } }

        [DisplayName("會員姓名")]
        public string MemberName { get { return this.entity_Member.fMemberName; } }

        [DisplayName("會員身分證")]
        public string address { get { return this.entity_Member.fMemberName; } }


    }
}