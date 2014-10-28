using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Travel.Web.Queries
{
    public class FindFilter
    {
        private string terms;

        public FindFilter(string terms)
        {            
            this.terms = terms.Trim().ToLower();
        }

        internal IQueryable<Models.Destination> Filter(IQueryable<Models.Destination> Destinations)
        {
           // do a test filter to look for exact destination name match;


            return Destinations.Where(d => d.Name.Contains(terms));

        }

        //public IQueryable<Destination> Filter(IQueryable<Destination> destinations)
        //{
        //    return countryId.HasValue ? destinations.Where(d => d.CountryId == countryId)
        //                              : destinations.Where(d => d.Country.Slug == slug);
        //}
    }
}
