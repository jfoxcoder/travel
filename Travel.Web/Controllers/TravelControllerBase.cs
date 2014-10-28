using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel.Web.DataContexts;
using Travel.Web.Enums;
using Travel.Web.Models;
using Travel.Web.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Travel.Web.Controllers
{
    public abstract class TravelControllerBase : Controller
    {
    
        protected LayoutVm layoutVm;


        protected TravelDb TravelDb { get; private set; }
        protected IdentityDb IdentityDb { get; private set; }
        


        protected LayoutVm LayoutVm
        {
            get { return layoutVm; }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Set Database Context properties used by subclass controllers.
            TravelDb = new TravelDb();
            IdentityDb = new IdentityDb();

            // Set session type (Guest, User, Admin)
            SessionType = requestContext.GetSessionType();

            // Set the logged in user.
            if (SessionType != SessionType.Guest)
            {
                LoggedInUser = IdentityDb.FindUserById(User.Identity.GetUserId());
            }
            
            // Set the layout view model.
            layoutVm = new LayoutVm(
                navCountries: NavCountries, 
                sessionType: requestContext.GetSessionType(), 
                username: requestContext.HttpContext.User.Identity.Name);            

            // Pass the layout viewmodel to the view.
            ViewBag.LayoutVm = layoutVm;
        }

        public ApplicationUser LoggedInUser{ get; private set; }

        public SessionType SessionType { get; private set; }

        public string ReturnUrl { get; set; }

        public void FlashMessage(string message, FlashMessageMode mode = FlashMessageMode.Info, bool useCookies=true)
        {
            if (useCookies)
            {
                var cookie = new HttpCookie("flash-message", message);
                //cookie.Expires = DateTime.Now.AddSeconds(10);
                cookie.Path = "/";
                Response.Cookies.Add(cookie);
            }
            else
            {
                TempData["flashMessage"] = message;
                TempData["flashMessageMode"] = mode;
            }
        }

    



        /// <summary>
        /// The link data for the countries and destinations that will appear in the navbar
        /// </summary>
        public ICollection<NavCountryVm> NavCountries
        {
            get
            {
                var destinations = TravelDb.Destinations.Include(d => d.Country)
                                               .GroupBy(d => d.Country)
                                               .OrderBy(cd => cd.Key.Name)
                                               .ToList();


                var navCountries = destinations.Select(cd => new NavCountryVm
                {
                    LinkText = cd.Key.Name,
                    Slug = cd.Key.Slug,
                    FlagClass = "flag " + cd.Key.IsoCode.ToLower(),

                    NavDestinations = cd.Select(d => new NavDestinationVm
                    {
                        CountrySlug = cd.Key.Slug,
                        Slug = d.Slug,
                        LinkText = d.Name,
                        TitleText = d.Description
                       
                    }).OrderBy(d => d.LinkText)
                                        .ToList()
                }).ToList();

                return navCountries;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="destination"></param>
        /// <returns>The number of destinations in the wishlist after adding <paramref name="destination"/>.</returns>
        public int AddDestinationToWishlist(ApplicationUser user, Destination destination)
        {
            if (! string.IsNullOrEmpty(user.Wishlist))
            {
                RemoveDestinationFromWishlist(user, destination); // prevent duplicates
                user.Wishlist = user.Wishlist + "[" + destination.Id.ToString() + "]";
            }
            else
            {
                user.Wishlist = "[" + destination.Id + "]";
            }

            IdentityDb.Entry(user).State = EntityState.Modified;
            IdentityDb.SaveChanges();

            return user.Wishlist.ToCharArray().Count(c => c == '[');
        }

        /// <returns>The number of destinations in the wishlist after removing <paramref name="destination"/>.</returns>
        public int RemoveDestinationFromWishlist(ApplicationUser user, Destination destination)
        {
            user.Wishlist = user.Wishlist.Replace("[" + destination.Id.ToString() + "]", string.Empty);
            IdentityDb.Entry(user).State = EntityState.Modified;
            IdentityDb.SaveChanges();


            return user.Wishlist.ToCharArray().Count(c => c == '[');
        }

      
        
    }
}