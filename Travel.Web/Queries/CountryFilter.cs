using System.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using Travel.Web.Models;


namespace Travel.Web.Queries.Sorting
{
    public class CountryFilter
    {
        private int? countryId;
        public string Slug { get; set; }

        public CountryFilter(int countryId)
        {
            this.countryId = countryId;
        }

        public CountryFilter(string slug)
        {
            Slug = slug;
        }             

        public IQueryable<Destination> Filter(IQueryable<Destination> destinations)
        {
            return countryId.HasValue ? destinations.Where(d => d.CountryId == countryId)
                                      : destinations.Where(d => d.Country.Slug == Slug);
        }
    }
}