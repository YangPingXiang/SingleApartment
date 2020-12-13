using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CMessage
    {
        [DisplayName("留言分類")]
        public string InformationCategory { get; set; }

        public Message message_entity { get; set; }

        public int MessageID { get { return this.message_entity.MessageID; } }

        [DisplayName("留言日期")]
        public System.DateTime MessageDate { get { return this.message_entity.MessageDate; } }

        [DisplayName("留言分類ID")]
        public int InformationCategoryID { get { return this.message_entity.InformationCategoryID; } }

        [DisplayName("使用者ID")]
        public int? MemberID { get { return this.message_entity.MemberID; } }

        [DisplayName("留言者")]
        public string GuestName { get { return this.message_entity.GuestName; } }

        [DisplayName("郵件地址")]
        public string Email { get { return this.message_entity.Email; } }

        [DisplayName("主旨")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "主旨必須輸入...")]
        public string MessageSubject { get { return this.message_entity.MessageSubject ; } }

        [DisplayName("留言內文")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "留言內文必須輸入...")]
        public string MessageContent { get { return this.message_entity.MessageContent; } }

        [DisplayName("管理者回覆")]
        public string AdminContent { get { return this.message_entity.AdminContent; } }

        //public string AdditionField1 { get; set; }
        //public string AdditionField2 { get; set; }
        //public Nullable<int> AdditionField3 { get; set; }
    }
}