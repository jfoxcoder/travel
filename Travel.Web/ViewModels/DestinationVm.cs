using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel.Web.Enums;
using Travel.Web.Exceptions;
using Travel.Web.ExtensionMethods;
using Travel.Web.Models;
using Travel.Web.Utilities;

namespace Travel.Web.ViewModels
{

    /// <summary>
    /// This viewmodel needs major refactoring.
    ///
    /// </summary>
    public class DestinationVm
    {



        public Destination Destination { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }



        public string DescriptionExcerpt
        {
            get
            {
                return Description.TruncateAtWord(200);
            }
        }


        public DestinationVm()
        {

        }





        public DestinationVm(string id, string name, string description,
            Destination destination, WishState wishState)
        {
            Id = id;
            Name = name;
            Description = description;
            Destination = destination;

            FlagClass = "flag " + destination.Country.IsoCode.ToLower();


            InWishlistMessage = string.Format("Remove {0} from your wishlist", Name);
            NotInWishlistMessage = string.Format("Add {0} to your wishlist", Name);

            switch (wishState)
            {
                case WishState.NotInWishlist:
                    WishlistStateClass = "wish-off-btn";
                    WishlistIconClass = "icon-wish-off";
                    WishlistTitleText = NotInWishlistMessage;
                    Add = true;
                    break;
                case WishState.InWishlist:
                    WishlistStateClass = "wish-on-btn";
                    WishlistIconClass = "icon-wish-on";
                    WishlistTitleText = InWishlistMessage;
                    InWishlistClass = "in-wishlist";
                    Add = false;
                    break;
                case WishState.Unknown:
                    WishlistStateClass = "wish-guest-btn";
                    WishlistIconClass = "icon-wish-on";
                    WishlistTitleText = GuestMessage;
                    break;
                default:
                    throw new TravelException("Invalid WishState encountered while constructing "
                        + name + " ViewModel.");

            }
        }

        public string WishlistTooltipOn { get; private set; }
        public string WishlistTooltipOff { get; private set; }


        /// <summary>
        /// True if the destination is in the users wishlist, otherwise false.
        /// 
        /// Null if the user is a guest.
        /// </summary>
        private bool? WishlistOn { get; set; }

        /// <summary>
        /// CSS class used in styling and javascript.
        /// </summary>
        public string WishlistStateClass { get; set; }

        /// <summary>
        /// CSS class that determines which icon-font symbol to use.
        /// </summary>
        public string WishlistIconClass { get; set; }

        public string WishlistTitleText { get; set; }

        public string InWishlistClass { get; set; }


        //public object WishlistParam
        //{
        //    get
        //    {
        //        return new  { destinationId = Id, add = Add };
        //    }
        //}

        public string FlagClass { get; set; }




        /// <summary>
        /// The parameter value that determines if the destination should
        /// be added or removed from the user's wishlist.
        /// </summary>
        public bool Add { get; set; }

        public IEnumerable<string> LoadImagePaths(HttpContextBase context)
        {
            var imagePathUtil = new ImagePathUtil();

            return imagePathUtil.DestinationImageUrls(Destination, context);
        }



        public string NameWithCountry
        {
            get
            {
                return string.Format("{0}, {1}", Destination.Name, Destination.Country.Name);
            }
        }

        public string NotInWishlistMessage { get; set; }

        public string InWishlistMessage { get; set; }

        public static string GuestMessage
        {
            get
            {
                return "Add to wishlist (requires sign-in).";
            }
        }

    }
}

