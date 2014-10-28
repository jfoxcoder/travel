using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Models;

namespace Travel.Web.ViewModels
{
    public class UserVm
    {
        public ApplicationUser ApplicationUser { get; set; }
    
        public string UserRoleClass
        {
            get
            {
                return IsAdmin ? "user-role-none" : "user-role-admin";
            }
        }

        public bool IsAdmin { get; set; }






        public bool IsLoggedInAdmin { get; set; }
    }
}