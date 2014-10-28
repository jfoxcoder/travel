using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Enums;
using Travel.Web.Models;

namespace Travel.Web.ViewModels
{
    public class LayoutVm
    {
        private SessionType sessionType;

        public string WebsiteName { get; private set; }

        public string Username { get; private set; }







        public LayoutVm(ICollection<NavCountryVm> navCountries, SessionType sessionType, string username = null)
        {
            NavDestinations = navCountries;

            this.sessionType = sessionType;

            WebsiteName = Properties.Settings.Default.WebsiteName;

            Username = username;
        }




        /// <summary>
        /// Link text and route data for the topbar
        /// </summary>
        public ICollection<NavCountryVm> NavDestinations
        {
            get;
            private set;
        }


        public bool IsGuest
        {
            get
            {
                return sessionType == SessionType.Guest;
            }
        }

        public bool IsUser
        {
            get
            {
                return sessionType == SessionType.User;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return sessionType == SessionType.Admin;
            }
        }


        /// <summary>
        /// Used by the view to determine whether to add the "active" class to the dropdown li for <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">The destination associated with a drop-down item</param>
        /// <returns>The string "active" if the <paramref name="destination"/> is the active destination.</returns>
        public string GetDestinationActiveClass(Destination destination)
        {
            if ((ActiveDestination != null) && (destination == ActiveDestination))
            {
                return "active";
            }
            return null;
        }

        public Destination ActiveDestination { get; set; }


        /// <summary>
        /// Title element inner text in the form {WebPageName} – {WebsiteName}.
        /// Throws an InvalidOperationException if WebPageName property is null.
        /// </summary>
        public string WebPageTitle
        {
            get
            {
                if (WebPageName != null)
                {
                    return WebPageName + " – " + WebsiteName;
                }
                return "Unnamed - " + WebsiteName;

                // 
                // throw new InvalidOperationException(
                //    "Accessed WebPageTitle property before setting WebPageName property.");
            }
        }

        public string WebPageName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nav"></param>
        /// <returns>The string "active" if <paramref name="nav"/> is currently active.  Otherwise null.</returns>
        public string GetNavActiveClass(Nav nav)
        {
            return nav == ActiveNav ? "active" : null;
        }

        /// <summary>
        /// Determines which topbar link is set active.  Set in controller action.
        /// </summary>
        public Nav ActiveNav
        {
            set;
            private get;
        }




        /// <summary>
        /// Description meta tag content.
        /// </summary>
        public string MetaDescription { get; set; }


        public void InitPage(string webPageName = null, string metaDescription = "",
            Nav activeNav = Nav.None)
        {
            WebPageName = webPageName ?? WebsiteName;
            MetaDescription = metaDescription;
            ActiveNav = activeNav;
        }


        public SortsVm SortsVm { get; set; }

        public bool HasSort 
        {
            get 
            {
                return this.SortsVm != null; 
            }
        }

        public bool HasFind { get; set; }
        
    }
}