using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Travel.Web.Enums;
using Travel.Web.Properties;
using Travel.Web.Queries;
using Travel.Web.Utilities;
using Travel.Web.ViewModels;

namespace Travel.Web
{
    public static class TravelHtmlHelpers
    {
        

        /// <summary>
        /// 
        /// Replaces the following duplicated razor markup
        /// <i class="wish-btn @dvm.WishlistStateClass @dvm.WishlistIconClass" title="@dvm.WishlistTitleText"
        ///    data-name="@dvm.Name"
        ///    data-url="@Url.RouteUrl("ToggleWishlist", new  { destinationId = dvm.Id, @add = dvm.Add })">
        /// </i>
        /// 
        /// </summary>                
        public static MvcHtmlString WishlistButton(this HtmlHelper helper, DestinationVm dvm)
        {
            var btn = new TagBuilder("i");            
            
            btn.AddCssClass("wish-btn");            
            btn.AddCssClass(dvm.WishlistStateClass);
            btn.AddCssClass(dvm.WishlistIconClass);
            btn.Attributes.Add("data-name", dvm.Name);
            btn.Attributes.Add("data-not-in-wishlist-message", dvm.NotInWishlistMessage);
            btn.Attributes.Add("data-in-wishlist-message", dvm.InWishlistMessage);
            btn.Attributes.Add("title", dvm.WishlistTitleText);
          
            // Create the wishlist toggle url data attribute.
            string dataUrl = helper.CreateUrlHelper()
                                   .HttpRouteUrl("ToggleWishlist", new { destinationId = dvm.Id });
                                                  
            btn.Attributes.Add("data-url", dataUrl);
            return new MvcHtmlString(btn.ToString());
        }

       

        public static string DestinationUrl(this HtmlHelper helper, string countrySlug, string destinationSlug)
        {
            return CreateUrlHelper(helper).RouteUrl("ViewDestination", new
            {
                countrySlug = countrySlug,
                destinationSlug = destinationSlug
            });
        }

        public static string DestinationUrl(this HtmlHelper helper, DestinationVm destinationVm)
        {
            return CreateUrlHelper(helper).RouteUrl("ViewDestination", new
            {
                countrySlug = destinationVm.Destination.Country.Slug,
                destinationSlug = destinationVm.Destination.Slug
            });
        }
        public static MvcHtmlString DestinationLink(this HtmlHelper helper, NavDestinationVm destinationVm)
        {
            return DestinationLink(
                helper: helper,
                destinationName: destinationVm.LinkText,
                countrySlug: destinationVm.CountrySlug,
                destinationSlug: destinationVm.Slug);
        }

        public static MvcHtmlString DestinationLink(this HtmlHelper helper, 
            string destinationName, string countrySlug, string destinationSlug)
        {
            var link = new TagBuilder("a");

            link.Attributes.Add("href", DestinationUrl(helper, countrySlug, destinationSlug));
            link.Attributes.Add("title", destinationName);
            link.SetInnerText(destinationName);

            return new MvcHtmlString(link.ToString());
        }



        public static MvcHtmlString BackLink(this HtmlHelper helper, string text, string routeName)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var backLinkHtml = string.Format("<div><a href=\"{0}\">Back to {1}</a></div>",                 
                    urlHelper.RouteUrl(routeName), text);

            return new MvcHtmlString(backLinkHtml);
        }


           //@Html.DestinationImage(destination);

        //public static MvcHtmlString DestinationImage(this HtmlHelper helper, DestinationVm dvm, int index = 0)
        //{
        //    var img = new TagBuilder("img");

        //    img.AddCssClass("destination-image");

        //    img.Attributes["alt"] = dvm.Name;
        //    img.Attributes["title"] = dvm.Description;
        //    img.Attributes["src"] = 
        //        TravelImageUtil.DestinationImageUrl(helper.ViewContext.RequestContext, dvm);
            
        //    return new MvcHtmlString(img.ToString());
        //}

        public static string DestinationImageUrl(this HtmlHelper helper, DestinationVm destination, ImageType imageType = ImageType.Image,  int index = 0)
        {
            return ImagePathUtil.DestinationImageUrl(helper.ViewContext.RequestContext, destination, imageType, index);
        }

      



        //<div class="sort-dropdown">
        //    <a href="#" class="button" data-dropdown="drop">Sort by @Model.SortVm.Active.Name &raquo;</a>
        //    <ul id="drop" class="tiny f-dropdown" data-dropdown-content>
        //        @foreach (var s in Model.SortVm.Sorts)
        //        {
        //            <li>@Html.RouteLink(s.Name, "Home", new { @sort = s.Name }, new { @class = "" })</li>
        //        }
        //    </ul>
        //</div>
        private static UrlHelper CreateUrlHelper(this HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }
        //public static MvcHtmlString SortDropdown<TModel>(this HtmlHelper helper, Sorter<TModel> sorter, string routeName)
        //{
        //    var activeSort = sorter.ActiveSort;

        //    if (activeSort == null)
        //        return new MvcHtmlString("<!-- SortDropDown no active sort -->");

        //    var activeSortLink = new TagBuilder("a");
        //    activeSortLink.AddCssClass("button");
        //    activeSortLink.Attributes.Add("data-dropdown", "drop");
        //    activeSortLink.SetInnerText(sorter.ActiveSort.Name + " »");
            
        //    var ulBuilder = new TagBuilder("ul");
        //    ulBuilder.AddCssClass("");
        //    ulBuilder.AddCssClass("f-dropdown");
        //    ulBuilder.Attributes.Add("id", "drop");
        //    ulBuilder.Attributes.Add("data-dropdown-content", "data-dropdown-content");
            
        //    var ulInnerHtmlBuilder = new StringBuilder();
        //    foreach (var s in sorter.DisplaySorts)
        //    {
        //        var sortLink = new TagBuilder("a");
        //        sortLink.SetInnerText(s.Name);
             
        //        sortLink.Attributes.Add("href", string.Format("{0}?{1}",
        //            CreateUrlHelper(helper).RouteUrl(routeName), s.QueryParameter));

        //        ulInnerHtmlBuilder.AppendFormat("<li>{0}</li>", sortLink);
        //    }

        //    ulBuilder.InnerHtml = ulInnerHtmlBuilder.ToString();


        //    var container = new TagBuilder("div");
        //    container.AddCssClass("sort-dropdown");
        //    container.InnerHtml = string.Format("{0}{1}", activeSortLink, ulBuilder.ToString());

        //    return new MvcHtmlString(container.ToString());
        //}




        public static MvcHtmlString AdminBreadcrumbs(this HtmlHelper helper,
            string activeLinkText, params string[] routeInfo)
        {
            return Breadcrumbs(helper, activeLinkText, true, routeInfo);
        }

        public static MvcHtmlString Breadcrumbs(this HtmlHelper helper,
            string activeLinkText, params string[] routeInfo)
        {
            return Breadcrumbs(helper, activeLinkText, false, routeInfo);
        }
        
        /// <summary>
        /// Foundation 5 breadcrumbs
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="activeLinkText"></param>
        /// <param name="admin"></param>
        /// <param name="routeInfo"></param>
        /// <returns></returns>
        private static MvcHtmlString Breadcrumbs(this HtmlHelper helper, 
            string activeLinkText, bool admin = false, params string[] routeInfo)
        {            
            // To generate route-based URLs for the breadcrumb links
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            
            var breadcrumbHtml = new StringBuilder();
            
            if (admin)
            {
                breadcrumbHtml.AppendFormat("<li><a href=\"{0}\">Admin</a></li>", 
                    urlHelper.RouteUrl("AdminHome"));
            }

            for (int i = 0; i < routeInfo.Length; i += 2)
            {
                breadcrumbHtml.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", 
                    urlHelper.RouteUrl(routeName: routeInfo[i + 1]), routeInfo[i]);
            }
            breadcrumbHtml.AppendFormat("<li class=\"current\">{0}</li>", activeLinkText);

            return new MvcHtmlString(
                string.Format("<div class=\"row\"><ol class=\"breadcrumbs\">{0}</ol></div>", breadcrumbHtml.ToString()));
        }
    }
}