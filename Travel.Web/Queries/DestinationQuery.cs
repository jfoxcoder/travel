using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Travel.Web.DataContexts;
using Travel.Web.Models;
using Travel.Web.Queries.Sorting;
using Travel.Web.Utilities;
using Travel.Web.ViewModels;

namespace Travel.Web.Queries
{
    public class DestinationQuery
    {

      

        public Pager<Destination> Pager { get; private set; }

        public FindFilter FindFilter { get; set; }

        private string sortName;
        private TravelDb context;
        private string sort;
        private string terms;

        

        private bool? Wishlist { get; set; }

        internal Sorter<Destination> Sorter { get; set; }

        public CountryFilter CountryFilter { get; set; }

        private ApplicationUser User { get; set; }

        public IQueryable<Destination> Destinations { get; set; }



        public IEnumerable<DestinationVm> DestinationVms { get; private set; }
       

        public DestinationQuery(TravelDb context,
            string sort=null,
            Sorter<Destination> sorter=null,
            ApplicationUser user=null,            
            string countrySlug=null,             
            Pager<Destination> pager=null,
            bool? wishlist=null,
            string terms=null)
        {
            Destinations = context.Destinations.Include("Country").Select(d => d);

            Sorter = sorter ?? SorterFactory.CreateDefaultDestinationSorter();

            sortName = Sorter.ContainsSort(sort) ? sort : SorterFactory.DefaultDestinationSortName;

            if (Wishlist.HasValue && User == null)
            {
                throw new ArgumentException("Cannot filter by wishlist because User is null");
            }

            User = user;

            Wishlist = wishlist;

            ApplyWishlistFilter();
            

            if (! string.IsNullOrEmpty(countrySlug))
            {
                CountryFilter = new CountryFilter(countrySlug);
            }

            if (!string.IsNullOrEmpty(terms))
            {
                FindFilter = new FindFilter(terms);
            }

            Pager = pager;

            ApplyFilters();
            BuildViewModels();
        }

        private void BuildViewModels()
        {   
            DestinationVms = 
                from d in Destinations.ToList()
                let wishState = ApplicationUser.GetWishState(User, d)
                select new DestinationVm(
                        id: d.Id.ToString(),
                        name: d.Name,
                        description: d.Description,
                        destination: d,
                        wishState: wishState);
        }

        

        
        private void ApplyFilters()
        {
            // filter by country
            if (CountryFilter != null)
            {
                Destinations = CountryFilter.Filter(Destinations);
            }

            // filter by country
            if (FindFilter != null)
            {
                Destinations = FindFilter.Filter(Destinations);
            }

            // todo wishlist filter            

            // sort
            Destinations = Sorter.Sort(Destinations, sortName);

            // paginate
            if (Pager != null)
            {
                Destinations = Pager.Paginate(Destinations);
            }
        }

        

        


        /// <summary>
        /// TODO 
        /// </summary>
        private void ApplyWishlistFilter()
        {
            if (Wishlist.HasValue)
            {
                // because I couldn't get the many-to-many entity framework 
                // relationship working with the built-in IdentityUser class
                // I had to use a hack to get the wishlist working.
                // (storing destination ids in a concatenated string in the
                // AspNetUser table via ApplicationUser.Wishlist property.

                // this also unfortunately means I have to switch to 
                // LINQ-to-collections (and read all of the destinations into memory...)

                Destinations = Destinations.ToList()
                                           .Where(d => User.InWishlist(d))
                                           .AsQueryable();
            }
        }

        /// <summary>
        /// Returns the country model if the CountryFilter is
        /// present and the country slug was valid.
        /// Otherwise null.
        /// </summary>
        //public Country Country
        //{
        //    get
        //    {
        //        if (CountryFilter == null)
        //            return null;

        //        if (firstDestination == null)
        //            return null;

        //        return firstDestination.Country;
        //    }
        //}



    
    }
}
