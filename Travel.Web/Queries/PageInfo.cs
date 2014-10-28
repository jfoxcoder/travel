using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Queries
{
    public class PageInfo
    {
        public PageInfo()
        {
            TotalNumberOfPages = -1;
        }

        public int PageNumber { get; set; }
        public int TotalNumberOfPages { get; set; }
        public bool IsLastPage
        {
            get
            {
                if (TotalNumberOfPages != -1)
                {
                    return PageNumber == TotalNumberOfPages;
                }

                throw new InvalidOperationException("Cannot determine LastPage because TotalNumberOfPages has not been calculated. Call Pager<TModel>.Paginate first.");
            }
        }


        public int ModelsPerPage { get; set; }

        /// <summary>
        /// Number of models being paginated
        /// </summary>
        public int NumberOfModelsBeingPaginated { get; set; }
    }
}