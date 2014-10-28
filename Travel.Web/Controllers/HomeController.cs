using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Travel.Web.DataContexts;
using Travel.Web.Enums;
using Travel.Web.Models;
using Travel.Web.Queries;
using Travel.Web.Queries.Sorting;
using Travel.Web.ViewModels;
using Travel.Web.ExtensionMethods;
using Travel.Web.Utilities;


namespace Travel.Web.Controllers
{
    public class HomeController : TravelControllerBase
    {


        //[Route("{sort?}/{page:int?}", Name = "Home")]
        [Route("", Name = "Home")]
        public ActionResult Home(string sort, int page = 1)
        {
            LayoutVm.InitPage("Home", "Find out about exciting travel destinations.", Nav.Home);

            var pager = new Pager<Destination>(page);

            var query = new DestinationQuery(               
                context: TravelDb,
                user: LoggedInUser,
                sort: sort,
                pager: pager);

            var homeVm = new HomeVm(query, 
                new SortsVm(Url.RouteUrl("Home"), query.Sorter.SortVms),
                new PagerVm<Destination>(pager));

            LayoutVm.SortsVm = new SortsVm(Url.RouteUrl("Home"), query.Sorter.SortVms);
            layoutVm.HasFind = true;

            return View(homeVm);
        }

        [Route("countries/{country}/{sort?}/{page:int?}", Name = "DestinationsByCountry")]
        public ActionResult DestinationsByCountry(string country, string sort, int page = 1)
        {
            var pager = new Pager<Destination>(page);

            var query = new DestinationQuery(                
                context: TravelDb,
                user: LoggedInUser,                
                countrySlug: country,                
                sort: sort,
                pager: pager);

            var sortsVm = new SortsVm(
                    url: Url.RouteUrl("Home"),
                    sortVms: query.Sorter.SortVms);

            var homeVm = new HomeVm(query,
             new SortsVm(Url.RouteUrl("Home"), query.Sorter.SortVms),
             new PagerVm<Destination>(pager));

            //var homeVm = new HomeVm(
            //    destinationVms: query.DestinationVms,
            //    sortsVm: sortsVm,
            //    pagerVm: new PagerVm<Destination>(pager),
            //    country: country);
            
            LayoutVm.InitPage(
                country.FromSlug(),
                "Find out about exciting travel destinations.", 
                Nav.Home);

          //  layoutVm.HasFind = true;

            // todo country specific view
            return View("~/Views/Home/Home.cshtml", homeVm); 
        }





     
        [HttpGet]
        [Route("find-destinations", Name = "FindDestinations")]
        public ActionResult FindDestinations(string terms)
        {
            // Thread.Sleep(2000); // simulate remote site

            var query = new DestinationQuery(
                  context: TravelDb,
                  user: LoggedInUser,                 
                  pager: new Pager<Destination>(),                  
                  terms: terms);

            return PartialView("_Destinations", query.DestinationVms);
        }

        [HttpGet]
        [Route("destination-sideshow-images/{destinationId:int}", Name = "DestinationSlideshowImages")]
        public ActionResult DestinationSlideshowImages(int destinationId)
        {
            var destination = TravelDb.Destinations
                                      .Include(d => d.Country)
                                      .SingleOrDefault(d => d.Id == destinationId);

            if (destination == null)
            {
                return HttpNotFound();
            }            

            var imagePathUtil = new ImagePathUtil();
            var urls = imagePathUtil.DestinationImageUrls(destination, HttpContext);

            return PartialView("_SlideshowImages", urls);
        }




        //public ActionResult HasWishes()
        //{ 
        //    if (LoggedInUser == null)
        //    {
        //        return new JsonResult
        //        {
        //            Data = new { hasUser = false }
        //        };
        //    }
        //    else
        //    {
        //        bool hasWishes = false;

        //        LoggedInUser.Wishlist.conta

        //        return new JsonResult
        //        {
        //            Data = new { hasUser = true, hasWishes = hasWishes }
        //        };
        //    }
        //}




        [Authorize]
        [HttpGet]
        [Route("wishlist", Name="Wishlist")]
        public ActionResult Wishlist()
        {
            LayoutVm.InitPage("Wishlist", "", Nav.Wishlist);

            var pager = new Pager<Destination>();

            var query = new DestinationQuery(
                context: TravelDb,
                user: LoggedInUser,
                sort: "Destination",
                pager: pager, 
                wishlist: true);

            var homeVm = new HomeVm(
                    query: query,
                    sortsVm: new SortsVm(Url.RouteUrl("Wishlist"), query.Sorter.SortVms),
                    pagerVm: new PagerVm<Destination>(pager)
                ) { IsWishlistPage = true };
                
                   
                
            

            //var homeVm = new HomeVm(
            //    destinationVms: query.DestinationVms,
            //    sortsVm: new SortsVm(Url.RouteUrl("Wishlist"), query.Sorter.SortVms),
            //    pagerVm: new PagerVm<Destination>(pager)) { IsWishlistPage = true };

            return View("~/Views/Home/Home.cshtml", homeVm);
        }
       

        [HttpGet]        
        [Route("load-destinations/{sort?}/{page?}/{country?}", Name = "LoadDestinations")]
        public ActionResult LoadDestinations(string sort, int page, string country)
        {
            // Thread.Sleep(2000); // simulate remote site

            var query = new DestinationQuery(
                  context: TravelDb,
                  user: LoggedInUser,
                  sort: sort,
                  pager: new Pager<Destination>(page: page),
                  countrySlug: country);

            

            return PartialView("_Destinations", query);
        }

        [Route("contact", Name = "Contact")]
        public ActionResult Contact()
        {
            LayoutVm.InitPage("Contact", LayoutVm.WebsiteName + " contact form and address." , Nav.Contact);
            return View();
        }

        [Route("destinations/{destinationSlug}/{countrySlug}", Name = "ViewDestination")]
        public ActionResult ViewDestination(string countrySlug, string destinationSlug)
        {
            var destination = 
                TravelDb.Destinations
                .Include("Country")
                .Where(d => d.Slug == destinationSlug && d.Country.Slug == countrySlug)
                .SingleOrDefault();

            if (destination != null)
            {
                WishState wishState = ApplicationUser.GetWishState(LoggedInUser, destination);

                var destinationVm = new DestinationVm(
                    destination.Id.ToString(),
                    destination.Name,
                    destination.Description,
                    destination,
                    wishState);

                LayoutVm.InitPage(destinationVm.NameWithCountry, destinationVm.DescriptionExcerpt, Nav.Home);

                return View(destinationVm);
            }

            return new HttpNotFoundResult("Destination not found.");
        }


        //[Authorize]        
        //[HttpPut]
        //[Route("/wish/{destinationId:int}", Name = "AddToWishlist")]
        //public ActionResult AddToWishlist(int destinationId)
        //{
        //    var d = travelDb.Destinations.Find(destinationId);

        //    System.Diagnostics.Debug.WriteLine("Adding {0} to {1}'s wishlist.",
        //        d.Name, User.Identity.Name);

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}

        
    
        [Authorize]
        [HttpPut]
        [Route("toggle-wishlist/{destinationId:int}", Name = "ToggleWishlist")]            
        public ActionResult ToggleWishlist(int destinationId)
        {
            var destination = TravelDb.Destinations.Find(destinationId);

            if (destination != null)
            {
                int wishes = 0;
                bool added = true;

                if (LoggedInUser.InWishlist(destination))
                {
                    added = false;
                    wishes = RemoveDestinationFromWishlist(LoggedInUser, destination);
                }
                else
                {
                    wishes = AddDestinationToWishlist(LoggedInUser, destination);
                }

                return new JsonResult 
                { 
                    Data = new 
                    {
                        wishes = wishes,
                        added = added
                    } 
                };
            }

            return HttpNotFound();
        }

        



        

        //[Authorize]
        //[HttpPut]
        //[Route("/unwish/{destinationId:int}", Name = "RemoveFromWishlist")            
        //public ActionResult RemoveFromWishlist(int destinationId)
        //{
        //    var d = travelDb.Destinations.Find(destinationId);

        //    System.Diagnostics.Debug.WriteLine("Removing {0} from {1}'s wishlist.",
        //        d.Name, User.Identity.Name);

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}
    }
}