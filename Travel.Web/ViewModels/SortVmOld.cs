using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Queries;

namespace Travel.Web.ViewModels
{
    

    public class SortVmOld<TModel> 
    {
        private IEnumerable<Sort<TModel>> sorts;
        public SortVmOld(IEnumerable<Sort<TModel>> sorts)
        {
            this.sorts = sorts;
        }

        public Sort<TModel> Active
        {
            get
            {
                return sorts.First(s => s.Active);
            }
        }

        public IEnumerable<Sort<TModel>> Sorts
        {
            get
            {
                return sorts.Where(s => ! s.Active);
            }
        }
    }
}