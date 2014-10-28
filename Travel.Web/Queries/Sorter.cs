using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Travel.Web.ViewModels;

namespace Travel.Web.Queries
{
    /// <summary>
    /// Generic sorter that can sort any model type.
    /// 
    /// Sorts are stored in a dictonary and then looked up via a query string parameter
    /// eg s=Country
    /// 
    /// Also used to generate the sort widget in the UI.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class Sorter<TModel>
    {
        /// <summary>
        /// The Sort objects keyed by their Name property.
        /// </summary>
        private Dictionary<string, Sort<TModel>> sortDict;


        public delegate IOrderedQueryable<TModel> SortFunc(IQueryable<TModel> models);

        public Sorter()
        {
            sortDict = new Dictionary<string, Sort<TModel>>();
        }

        public Sorter<TModel> SortBy(string key, SortFunc sortFunc)
        {
            sortDict[key] = new Sort<TModel>(key, sortFunc, sortDict.Count);
            return this;
        }

        /// <summary>
        /// Adds an ascending sort and a descending sort to the sorter.
        /// </summary>
        /// <typeparam name="TProperty">The type of the model property that the sort is performed on</typeparam>
        /// <param name="ascendingName">The name used for the OrderBy sort.</param>
        /// <param name="descendingName">The name used for the OrderByDescending sort.</param>
        /// <param name="orderByKeySelector">Lambda expression that selects the model property for sorting.</param>
        /// <returns>This (fluent-style)</returns>
        public Sorter<TModel> AscendAndDescendBy<TProperty>(string ascendingName, string descendingName,
            Expression<Func<TModel, TProperty>> orderByKeySelector)
        {
            sortDict[ascendingName] = new Sort<TModel>(
                name: ascendingName,
                sortFunc: new SortFunc(models => models.OrderBy(orderByKeySelector)),
                index: sortDict.Count);

            sortDict[descendingName] = new Sort<TModel>(
                name: descendingName,
                sortFunc: new SortFunc(models => models.OrderByDescending(orderByKeySelector)),
                index: sortDict.Count);

            return this;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="sortName"></param>
        /// <param name="orderByKeySelector"></param>
        /// <returns></returns>
        public Sorter<TModel> AscendBy<TProperty>(string sortName,
         Expression<Func<TModel, TProperty>> orderByKeySelector)
        {
            sortDict[sortName] = new Sort<TModel>(
               name: sortName,
               sortFunc: new SortFunc(models => models.OrderBy(orderByKeySelector)),
               index: sortDict.Count);

            return this;
        }

        public Sorter<TModel> DescendBy<TProperty>(string sortName,
         Expression<Func<TModel, TProperty>> orderByKeySelector)
        {
            sortDict[sortName] = new Sort<TModel>(
               name: sortName,
               sortFunc: new SortFunc(models => models.OrderByDescending(orderByKeySelector)),
               index: sortDict.Count);

            return this;
        }

         
        


        //public IEnumerable<Sort<TModel>> SortsByIndex
        //{
        //    get
        //    {
        //        return sortDict.Values.OrderBy(s => s.Index);
        //    }
        //}

        public bool ContainsSort(string sortName)
        {
            return (sortName != null) && sortDict.ContainsKey(sortName);
        }

        /// <summary>
        /// Looks up the sort and executes it on the models.
        /// Sets the sorts Active property to true (used when generating views).
        /// </summary>
        /// <param name="models">The entities to be sorted.</param>
        /// <param name="sortName">The Name property of the sort.</param>
        /// <returns>The sorted models.</returns>
        public IOrderedQueryable<TModel> Sort(IQueryable<TModel> models, string sortName)
        {
            var sort = sortDict[sortName];
            sort.Active = true;
            return sort.Apply(models);
        }



        public IEnumerable<SortVm> SortVms
        { 
            get
            {
                return from s in sortDict.Values
                       orderby s.Index
                       select s.SortVm;
            }
            
        }
    }
}