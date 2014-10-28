using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel.Web.Enums;
using Travel.Web.ViewModels;

namespace Travel.Web.Controllers
{

  
  
    [Authorize(Roles = "admin")]
    [RouteArea("Admin", AreaPrefix = "admin")]        
    public class AdminController : TravelControllerBase
    {
     
        [Route("home", Name="AdminHome")]
        public ActionResult Index()
        {
            LayoutVm.InitPage("Admin", "", Nav.None);

            return View(AdminAreaVms);
        }


        /// <summary>
        /// The navigation link boxes in the admin area home page.
        /// </summary>
        private IEnumerable<AdminAreaVm> AdminAreaVms
        {
            get
            {
                return new List<AdminAreaVm>
                {         
                    new AdminAreaVm 
                    { 
                        Name = "Users", 
                        Description = "View and delete users and manage roles", 
                        RouteName = "AdminUsersHome"
                    },
                    new AdminAreaVm 
                    { 
                        Name = "Destinations", 
                        Description = "View, edit, add and remove destinations.", 
                        RouteName = "AdminDestinationsHome"
                    },
                    new AdminAreaVm 
                    { 
                        Name = "Countries", 
                        Description = "View, edit, add and remove countries.", 
                        RouteName = "AdminCountriesHome"
                    },
                    new AdminAreaVm 
                    { 
                        Name = "Messages", 
                        Description = "View, delete and respond to contact messages.", 
                        RouteName = "AdminContactsHome"
                    }                
                };
            }
        }
        
    }
}