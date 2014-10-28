using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Enums
{
    
    /// <summary>
    /// The user-destination wishlist association (if there is one).
    /// </summary>
    public enum WishState
    {   
        /// <summary>
        /// It's unknown if a certain destination is in a certain user's wishlist. Probably
        /// because the user is not logged in.
        /// </summary>
        Unknown,

        /// <summary>
        /// A certain user has a certain destination in his wishlist.
        /// </summary>
        InWishlist,

        /// <summary>
        /// The certain user does not have a certain destination in his wishlist.
        /// </summary>
        NotInWishlist
    }
}