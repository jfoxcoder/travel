using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.ViewModels
{
    public class SortVm
    {
        public string Name { get; set; }

        public string QueryParameter { get; set; }

        public bool Active { get; set; }

        public string ActiveCssClass
        {
            get
            {
                return Active ? "active" : null;
            }
        }

        public string Href(string url, bool firstParameter=true)
        {
            if (Active)
            {
                return "#";
            }
            else
            {
                return firstParameter
                    ? url + "?" + QueryParameter 
                    : url + "&" + QueryParameter;
            }
        }
    }
}