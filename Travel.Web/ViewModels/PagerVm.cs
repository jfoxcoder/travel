using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Web.Queries;

namespace Travel.Web.ViewModels
{
    public class PagerVm<TModel>
    {
        public Pager<TModel> Pager { get; private set; }
       
        public PagerVm(Pager<TModel> pager)
        {
            Pager = pager;
        }

        public string LastPageClass
        {
            get
            {
                return Pager.Info.IsLastPage ? "pagination-last-page" : null;
            }
        }
    }
}