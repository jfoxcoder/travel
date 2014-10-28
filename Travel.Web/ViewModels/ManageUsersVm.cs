using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.ViewModels
{
    public class ManageUsersVm
    {
        public IEnumerable<UserVm> Admins { get; set; }
        public IEnumerable<UserVm> Users { get; set; }
    }
}