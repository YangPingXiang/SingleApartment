using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class CInformationCategory
    {
        public InformationCategory infoCategory_entity { get; set; }

        [DisplayName("訊息類別編號")]
        [Required(ErrorMessage = "類別編號為必填欄位")]
        public int InformationCategoryID { get { return this.infoCategory_entity.InformationCategoryID; } }
        [DisplayName("訊息類別名稱")]
        [Required(ErrorMessage = "類別名稱為必填欄位")]
        public string InformationCategoryName { get { return this.infoCategory_entity.InformationCategoryName; } }
    }
}