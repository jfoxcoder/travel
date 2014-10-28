using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Travel.Web.Enums;

namespace Travel.Web
{
    public static class RequestContextExtensions
    {
        public static SessionType GetSessionType(this RequestContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                return context.HttpContext.User.IsInRole("admin")
                    ? SessionType.Admin
                    : SessionType.User;                    
            }

            return SessionType.Guest;
        }
    }
}