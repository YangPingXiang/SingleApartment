using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CBack_ProductsViewModel
    {
        public List<CProductViewModel> Ready { get; set; }
        public List<CProductViewModel> Discontinued { get; set; }
    }
}