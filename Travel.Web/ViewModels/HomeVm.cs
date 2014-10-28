using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Models;
using Travel.Web.Queries;

namespace Travel.Web.ViewModels
{
    public class HomeVm
    {
        public IEnumerable<DestinationVm>  DestinationVms { get; private set; }

        public SortsVm SortsVm { get; private set; }

        public string CountryFilterValue { get; set; }

        public PagerVm<Destination> PagerVm { get; set; }

        public DestinationQuery Query { get; set; }

        

        public HomeVm(DestinationQuery query, SortsVm sortsVm, PagerVm<Destination> pagerVm)
        {
            Query = query;
            DestinationVms = query.DestinationVms;
            SortsVm = sortsVm;

            if (query.CountryFilter != null)            
                CountryFilterValue = query.CountryFilter.Slug;
            PagerVm = pagerVm;
        }

        public bool IsWishlistPage { get; set; }

        
        /// <summary>
        /// Data for scripts.
        /// 
        /// Todo: integrate into TravelControllerBase and make 
        /// controller-action configurable.
        /// </summary>
        public object ClientData
        {
            get
            {
                return new
                {
                    pageConfig = new 
                    {                     
                        page = PagerVm.Pager.Info.PageNumber,
                        
                        country = CountryFilterValue,
                        sort = SortsVm.ActiveSortName                       
                    },
                    isWishlistPage = IsWishlistPage
                };
            }
        }
    }
}