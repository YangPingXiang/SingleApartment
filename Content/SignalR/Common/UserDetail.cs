using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Common
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }

        public string Image {
            get {
                return (new sln_SingleApartment.Models.SingleApartmentEntities()).tMember.FirstOrDefault(r => r.fMemberName == UserName).fImage;
            }
        }
    }
}