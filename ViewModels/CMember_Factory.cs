using sln_SingleApartment.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sln_SingleApartment.ViewModels
{
    public class CMember_Factory
    {

        SingleApartmentEntities db = new SingleApartmentEntities();

        public CMember isAuthticated(string account,string pwd)
        {
            tMember table = (from p in db.tMember
                              where( p.fAccount == account||p.fEmail == account ||p.fPhone==account) && p.fPassword == pwd 
                              select p).FirstOrDefault();
            CMember member = new CMember();
            if (table != null)
            {
                member.fMemberId = table.fMemberId;
                member.fMemberName = table.fMemberName;
                member.fAccount = table.fAccount;
                member.fPassword = table.fPassword;
                member.fEmail = table.fEmail;
                member.fRoomId = table.fRoomId;
                member.fPhone = table.fPhone;
                member.fAge = table.fAge;
                member.fSex = table.fSex;
                member.fBirthDate = table.fBirthDate;
                member.fSalary = table.fSalary;
                member.fCareer = table.fCareer;
                member.fImage = table.fImage;
                member.fLeave = table.fLeave;
                member.fRole = table.fRole;

                return member;
            }
            return null;
        }
    }
}