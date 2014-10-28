using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Enums
{
    /// <summary>
    /// The login status, i.e., is someone logged in, if so are they an admin.
    /// 
    /// Used in the views to determine visible navigation options etc.
    /// </summary>
    public enum SessionType
    {   
        /// <summary>
        /// Unregistered user
        /// </summary>
        Guest,

        /// <summary>
        /// Logged-in, non-admin
        /// </summary>
        User,

        /// <summary>
        /// Logged-in admin
        /// </summary>
        Admin
    }
}