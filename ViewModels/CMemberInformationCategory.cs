using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class CMemberInformationCategory
    {
        public MemberInformationCategory mEntity { get; set; }

        [DisplayName("使用者代號")]
        public int MemberID { get { return this.mEntity.MemberID; } }

        [DisplayName("訊息類別編號")]
        [Required(ErrorMessage = "類別編號為必填欄位")]
        public int MemberCategoryID { get { return this.mEntity.MemberCategoryID; } }

        [DisplayName("訊息類別名稱")]
        [Required(ErrorMessage = "類別名稱為必填欄位")]
        public string MemberCategoryName { get { return this.mEntity.MemberCategoryName; } }
    }
}