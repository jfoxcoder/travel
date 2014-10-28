using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.ExtensionMethods;
using Travel.Web.ViewModels;

namespace Travel.Web.Queries
{
    public class Sort<TModel>
    {
        /// <summary>
        /// Name has three roles
        /// 1. Unique identifier within a Sorter instance
        /// 2. Query parameter
        /// 3. Display name for the UI
        /// </summary>
        public string Name { get; private set; }

        public SortVm SortVm
        {
            get
            {
                return new SortVm()
                {
                    Name = Name,
                    QueryParameter = "sort=" + Name.AsEncodedUrl(),
                    Active = Active
                };
            }
        }

        

        //public string QueryParameter
        //{
        //    get
        //    {
        //        return "sort=" + Name.AsEncodedUrl();
        //    }
        //}

        /// <summary>
        /// Encapsulates the OrderBy etc calls, i.e., performs the sorting.
        /// </summary>
        public Sorter<TModel>.SortFunc Apply { get; private set; }


        /// <summary>
        /// Determines the order that the sorts will appear in the UI.
        /// Set to the order that the sorts were added to the Sorter instance.
        /// </summary>
        public int Index { get; private set; }



        /// <summary>
        /// Creates a Sort
        /// </summary>
        /// <param name="name">Name property value</param>
        /// <param name="sortFunc">SortFunc property value</param>
        /// <param name="index">Index property value</param>
        public Sort(string name, Sorter<TModel>.SortFunc sortFunc, int index)
        {
            Name = name;
            Apply = sortFunc;
            Index = index;
        }

        

        public bool Active { get; set; }
    }
}