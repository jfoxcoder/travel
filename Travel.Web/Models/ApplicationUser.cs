using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Travel.Web.Enums;

namespace Travel.Web.Models
{
    // You can add profile data for the user by adding more properties to your
    // ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // wishlist
        //      private ICollection<Destination> destinations;

        public ApplicationUser()
        {
            //  destinations = new List<Destination>();
            Wishlist = "";
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //public ICollection<Destination> Destinations
        //{
        //    get
        //    {
        //        return destinations;
        //    }
        //    set
        //    {
        //        destinations = value;
        //    }
        // }



        public bool InWishlist(Destination destination)
        {
            if (string.IsNullOrEmpty(Wishlist))            
                return false;            
            
            return Wishlist.IndexOf("[" + destination.Id + "]") >= 0;
        }

        public string Wishlist { get; set; }


        public static WishState GetWishState(ApplicationUser user, Destination destination)
        {
            if (user == null)
            {
                return WishState.Unknown;
            }
            
            return user.InWishlist(destination)
                ? WishState.InWishlist : WishState.NotInWishlist;            
        }
    }
}