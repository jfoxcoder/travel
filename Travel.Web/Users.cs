using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Models;

namespace Travel.Web
{
    public class AdminsAndUsers
    {
        public IQueryable<ApplicationUser> Admins { get; set; }

        public IQueryable<ApplicationUser> Users { get; set; }
    }
}