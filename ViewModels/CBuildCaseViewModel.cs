using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using sln_SingelApartment.ViewModels;
using sln_SingleApartment.Models;

namespace sln_SingelApartment.ViewModels
{
    public class CBuildCaseViewModel
    {
        [DisplayName("窩居公寓-建案")]
        public BuildCase entity_buildcase { get; set; }

        public string ID { get { return this.entity_buildcase.ID; } }
        [DisplayName("建案名稱")]
        public string buildcasename { get { return this.entity_buildcase.BuildCaseName; } }

        [DisplayName("建案地址")]
        public string address { get { return this.entity_buildcase.Address; } }

        [DisplayName("總共樓層")]
        public int? totalfloor { get { return this.entity_buildcase.TotalFloor; } }
        [DisplayName("總房數")]
        public int? roomcounts { get { return this.entity_buildcase.RoomCounts; } }
        [DisplayName("建造時間")]
        public DateTime? buildtime { get { return this.entity_buildcase.BuildTimes; } }
    }
}