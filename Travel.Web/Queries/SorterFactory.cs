using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Models;

namespace Travel.Web.Queries
{
    /// <summary>
    /// Creates Sorters with default settings
    /// </summary>
    internal static class SorterFactory
    {
        public static Sorter<Destination> CreateDefaultDestinationSorter()
        {
            return new Sorter<Destination>()
                .SortBy("Featured",
                    destinations => destinations.OrderByDescending(d => d.Featured).ThenBy(d => d.Name))

                .DescendBy<int>("Featured", d => d.Featured)
                .AscendBy<string>("Destination", d => d.Name)
                .AscendBy<string>("Country", d => d.Country.Name);             
        }

        public static string DefaultDestinationSortName
        {
            get
            {
                return "Featured";
            }
        }

    }
}