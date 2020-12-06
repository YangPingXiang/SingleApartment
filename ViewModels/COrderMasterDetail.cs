using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModel
{
    public class COrderMasterDetail
    {
        public bool display_flag { get; set;}
        public List<COrder> t_order { get; set; }
        public List<COrderDetails> t_orderDetail { get; set; }
    }
}