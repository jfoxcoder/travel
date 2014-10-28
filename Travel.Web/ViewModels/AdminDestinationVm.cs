using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.ViewModels
{
    public class AdminDestinationVm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return "Edit " + Name;
            }
        }
    }
}