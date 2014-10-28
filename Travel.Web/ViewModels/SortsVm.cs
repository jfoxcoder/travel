using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.ViewModels
{
    public class SortsVm
    {
        public string Url { get; private set; }

        public IEnumerable<SortVm> SortVms { get; private set; }

        public string ActiveSortName
        {
            get
            {
                return SortVms.First(s => s.Active).Name;
            }

        }
        public SortsVm(string url, IEnumerable<SortVm> sortVms)
        {
            Url = url;
            SortVms = sortVms;            
        }
    }
}