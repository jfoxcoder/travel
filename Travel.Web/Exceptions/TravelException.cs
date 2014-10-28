using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class TravelException : Exception
    {
        public TravelException(string message)
            : base(message)
        {

        }
    }
}