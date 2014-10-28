
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.ViewModels
{
    public class NavDestinationVm
    {
        
        public string LinkText { get; set; }

        public string TitleText { get; set; }
        public string Slug { get; set; }

        public string CountrySlug { get; set; }
    }
}