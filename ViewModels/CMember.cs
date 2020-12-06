using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CMember
    {
        [DisplayName("會員編號")]
        public int fMemberId { get; set; }

        [DisplayName("會員姓名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名必須輸入...")]
        public string fMemberName { get; set; }

        [DisplayName("會員帳號")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "帳號必須輸入...")]
        public string fAccount { get; set; }

        [DisplayName("會員密碼")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "密碼長度需大於6字元...")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密碼必須輸入...")]
        public string fPassword { get; set; }

        [DisplayName("會員信箱")]
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "信箱必須輸入...")]
        public string fEmail { get; set; }

        [DisplayName("房間編號")]
        public Nullable<int> fRoomId { get; set; }

        [DisplayName("手機號碼")]
        [DataType(DataType.PhoneNumber)]
        public string fPhone { get; set; }

        [DisplayName("年齡")]
        public Nullable<int> fAge { get; set; }

        [DisplayName("性別")]
        public string fSex { get; set; }

        [DisplayName("出生日期")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> fBirthDate { get; set; }

        [DisplayName("年收入")]
        public string fSalary { get; set; }

        [DisplayName("職業")]
        public string fCareer { get; set; }

        [DisplayName("會員照片")]
        public string fImage { get; set; }

        [DisplayName("是否已退租")]
        public Nullable<bool> fLeave { get; set; }

        [DisplayName("角色")]
        public string fRole { get; set; }

        public HttpPostedFileBase myImage { get; set; }

        [DisplayName("欲接收活動種類的訊息")]
        public string fActivityMessage { get; set; }
    }
}