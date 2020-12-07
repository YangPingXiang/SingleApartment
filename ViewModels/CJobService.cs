using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CJobService
    {
        public JobService job_service_entity { get; set; }

        [DisplayName("服務工作ID")]
        public int JobID { get { return this.job_service_entity.JobID; } }

        [DisplayName("服務工作名稱")]
        [Required(ErrorMessage = "服務名稱為必填欄位")]
        public string JobName { get { return this.job_service_entity.JobName; } }

        [DisplayName("服務工作內容")]
        [Required(ErrorMessage = "服務工作內容為必填欄位")]
        public string JobDescription { get { return this.job_service_entity.JobDescription; } }

        [DisplayName("執行頻率")]
        [Required(ErrorMessage = "執行頻率為必填欄位")]
        public string JobCycle { get { return this.job_service_entity.JobCycle; } }

        [DisplayName("啟動時機(天)")]
        [Required(ErrorMessage = "啟動時機(天)為必填欄位")]
        public int BeforeDays { get { return this.job_service_entity.BeforeDays; } }

    }
}