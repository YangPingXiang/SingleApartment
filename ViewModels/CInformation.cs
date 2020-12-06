using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class CInformation
    {
        public Information information_entity { get; set; }

        [DisplayName("訊息編號")]
        public int InformationID { get { return this.information_entity.InformationID; } }

        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DisplayName("訊息日期")]
        public System.DateTime InformationDate { get { return this.information_entity.InformationDate; } }

        [DisplayName("系統分類ID")]
        public int InformationCategoryID { get { return this.information_entity.InformationCategoryID; } }

        [DisplayName("個人分類ID")]
        public int? MemberCategoryID { get { return this.information_entity.MemberCategoryID; } }

        [DisplayName("訊息來源")]
        public int? InformationSource { get { return this.information_entity.InformationSource; } }
        //todo:修改  int ==> int?

        [DisplayName("來源單號")]
        public int DocumentID { get { return this.information_entity.DocumentID; } }

        [DisplayName("訊息內容")]
        public string InformationContent { get { return this.information_entity.InformationContent; } }

        [DisplayName("優先等級")]
        public string Priority { get { return this.information_entity.Priority; } }

        [DisplayName("已讀否")]
        public string Read_YN { get { return this.information_entity.Read_YN; } }

        [DisplayName("系統分類")]
        public string InformationCategoryName { get; set; }

        [DisplayName("個人分類")]
        public string UserCategoryName { get; set; }
    }
}