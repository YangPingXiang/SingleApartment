using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CMultiple
    {
        public tMember t { get; set; }
        public List<Participant> part { get; set; }
        public List<Activity> atv { get; set; }
        public List<Product> prod { get; set; }
        public Room r { get; set; }
        public RoomStyle rs {get;set;}
    }
}