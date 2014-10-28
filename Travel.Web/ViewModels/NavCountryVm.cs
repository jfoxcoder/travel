using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.ExtensionMethods;

namespace Travel.Web.ViewModels
{
    public class NavCountryVm
    {
        public string LinkText { get; set; }

        public List<NavDestinationVm> NavDestinations { get; set; }      

        public string Slug { get; set; }

        public string FlagClass { get; set; }
    }
}