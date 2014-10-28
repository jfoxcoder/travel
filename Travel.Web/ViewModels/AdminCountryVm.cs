using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Models;

namespace Travel.Web.ViewModels
{
    public class AdminCountryVm
    {
        public Country Model { get; private set; }

        public string FlagClass { get; set; }

        public AdminCountryVm(Country country)
        {
            Model = country;

            FlagClass = "flag " + country.IsoCode.ToLower();

            AdminDestinationVms = 
                   (from d in Model.Destinations
                   orderby d.Name
                   select new AdminDestinationVm
                   {
                       Id = d.Id,
                       Name = d.Name
                   }).ToList();
        }


        public List<AdminDestinationVm> AdminDestinationVms { get; private set; }

        public string Id
        {
            get
            {
                return Model.Id.ToString();
            }
        }


        public string Name
        {
            get
            {
                return Model.Name;
            }
        }

        public string CountryFilterClass
        {
            get
            {
                return "c" + Model.Id;
            }
        }
        
        
     
        public int NumberOfDestinations
        {
            get
            {
                return Model.Destinations.Count;
            }
        }

        /// <summary>
        /// Comma-separated list of destinations for this country.
        /// </summary>
        public string DestinationNames
        {
            get
            {
                
                var destinationNames = (from d in Model.Destinations
                                       orderby d.Name
                                       select d.Name).ToList();

                switch (destinationNames.Count)
                { 
                    case 0: return "No destinations!";
                    case 1: return destinationNames.First() + ".";
                    default: return string.Join(", ", destinationNames) + ".";                    
                }
            }
        }
        

        

        


        
        
    }
}