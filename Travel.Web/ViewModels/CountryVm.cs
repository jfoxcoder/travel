using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.ViewModels;

namespace Travel.Web.ViewModels
{
    public class CountryVm
    {
        public string Id { get; private set; }
        
        public string Name { get; private set; }

        public IEnumerable<DestinationVm> DestinationVms;

     
        
        public CountryVm(string id, string name, IEnumerable<DestinationVm> destinationVms)
        {
            Id = id;
            Name = name;
            DestinationVms = destinationVms;          
        }
    }
}