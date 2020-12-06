using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CLogIn
    {
        public string txtAccount { get; set; }
        public string txtPassword { get; set; }
        public string txtMemberName { get; set; }
        public string txtEmail { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}