using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CMemberRegister
    {
        public tMember entity { get; set; }
        public HttpPostedFileBase myImage { get; set; }

        [DisplayName("會員編號")]
        public int fMemberId { get { return this.entity.fMemberId; } }

        [DisplayName("會員姓名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名必須輸入...")]
        public string fMemberName { get { return this.entity.fMemberName; } }

        [DisplayName("會員帳號")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "帳號必須輸入...")]
        public string fAccount { get { return this.entity.fAccount; } }

        [DisplayName("會員密碼")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "密碼長度需大於6字元...")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密碼必須輸入...")]
        public string fPassword { get { return this.entity.fPassword; } }

        [DisplayName("會員信箱")]
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "信箱必須輸入...")]
        public string fEmail { get { return this.entity.fEmail; } }

        [DisplayName("房間編號")]
        public Nullable<int> fRoomId { get { return this.entity.fRoomId; } }

        [DisplayName("手機號碼")]
        [DataType(DataType.PhoneNumber)]
        public string fPhone { get { return this.entity.fPhone; } }

        [DisplayName("年齡")]
        public Nullable<int> fAge { get { return this.entity.fAge; } }

        [DisplayName("性別")]
        public string fSex { get { return this.entity.fSex; } }

        [DisplayName("出生日期")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> fBirthDate { get { return this.entity.fBirthDate; } }

        [DisplayName("年收入")]
        public string fSalary { get { return this.entity.fSalary; } }

        [DisplayName("職業")]
        public string fCareer { get { return this.entity.fCareer; } }

        [DisplayName("會員照片")]
        public string fImage { get { return this.entity.fImage; } }

        [DisplayName("是否已退租")]
        public Nullable<bool> fLeave { get { return this.entity.fLeave; } }

        [DisplayName("角色")]
        public string fRole { get { return this.entity.fRole; } }

        [DisplayName("欲接收活動種類的訊息")]
        public string fActivityMessage { get { return this.entity.fActivityMessage; } }


    }
}