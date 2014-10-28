using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Queries
{
    public class Pager<TModel>
    {
        private PageInfo pageInfo;

        

        public Pager(int page, int modelsPerPage)
        {
            pageInfo = new PageInfo { PageNumber = page, ModelsPerPage = modelsPerPage };
        }

        public Pager(int page)
            : this(page, Properties.Settings.Default.DefaultDestinationTake)
        {            
        }        

        public Pager()        
            : this(1)
        {             
        }

        

        public IQueryable<TModel> Paginate(IQueryable<TModel> models)        
        {
            pageInfo.NumberOfModelsBeingPaginated = models.Count();

            if (pageInfo.PageNumber != 1)
            {
                int skip = (pageInfo.PageNumber - 1) * Info.ModelsPerPage;
                models = models.Skip(skip);
            }



            pageInfo.TotalNumberOfPages = (int) Math.Ceiling(((double)pageInfo.NumberOfModelsBeingPaginated / pageInfo.ModelsPerPage));

           

            return models.Take(pageInfo.ModelsPerPage);
        }

        


        public PageInfo Info 
        {
            get
            {
                return pageInfo;
            }
        
        }
       
        
    
    
    
    }
}