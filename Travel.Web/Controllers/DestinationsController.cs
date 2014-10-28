using Backload;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Travel.Web.DataContexts;
using Travel.Web.Enums;
using Travel.Web.ExtensionMethods;
using Microsoft.AspNet.Identity;
using Travel.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Travel.Web.Utilities;
using Travel.Web.ViewModels;


namespace Travel.Web.Controllers
{
    [Authorize(Roles = "admin")]
    [RouteArea("Admin", AreaPrefix = "admin")]
    [RoutePrefix("destinations")]
    [Route("{action=Index}")]
    public class DestinationsController : TravelControllerBase
    {
        private ImagePathUtil imagePathUtil;

        public DestinationsController()
        {
            imagePathUtil = new ImagePathUtil();
        }

        // GET: Destinations        
        [Route("", Name = "AdminDestinationsHome")]
        public ActionResult Index()
        {   
            LayoutVm.InitPage(
                webPageName: "Manage Destinations", 
                metaDescription: "View, edit, add and remove destinations.", 
                activeNav: Nav.AdminDestinations);

            var destinationVms = from d in TravelDb.Destinations.Include(d => d.Country)
                                                   .OrderBy(d => d.Name)
                                                   .ToList()
                                 select new DestinationVm
                                 {
                                     Id = d.Id.ToString(),
                                     Name = d.Name,
                                     Description = d.Description,
                                     Destination = d                                     
                                 };

            return View(destinationVms);
        }

      


        [Route("create", Name = "DestinationCreate")]
        public ActionResult Create(string country)
        {
            LayoutVm.InitPage(webPageName: "Create Destination");
            ViewBag.CountryId = new SelectList(TravelDb.Countries, "Id", "Name", country);
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CountryId")] Destination destination)
        {
            EnsureDestinationIsUniqueWithinCountry(destination);

            if (ModelState.IsValid)
            {
                TravelDb.Destinations.Add(destination);
                TravelDb.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(TravelDb.Countries, "Id", "Name", destination.CountryId);
            return View(destination);
        }




        [HttpGet]
        [Route("{id}/edit", Name = "DestinationEdit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = TravelDb.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(TravelDb.Countries, "Id", "Name", destination.CountryId);

            LayoutVm.InitPage(webPageName: "Edit " + destination.Name);

            return View(destination);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}/edit")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CountryId")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                var d = TravelDb.Destinations.Find(destination.Id);
                d.Name = destination.Name;
                d.Description = destination.Description;
                d.CountryId = destination.CountryId;                
                TravelDb.Entry(d).State = EntityState.Modified;
                TravelDb.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(TravelDb.Countries, "Id", "Name", destination.CountryId);
            return View(destination);
        }





        [Route("{id}/delete", Name = "DestinationDelete")]        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = TravelDb.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }
            return View(destination);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{id}/delete")]        
        public ActionResult DeleteConfirmed(int id)
        {
            Destination destination = TravelDb.Destinations.Find(id);
            TravelDb.Destinations.Remove(destination);
            TravelDb.SaveChanges();
            return RedirectToAction("Index");
        }







        [Route("{id}/images", Name = "DestinationImages")]
        public ActionResult Images(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = TravelDb.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }

            LayoutVm.InitPage(destination.Name + " Images");

            return View(destination);
        }



       









        private void EnsureDestinationIsUniqueWithinCountry(Destination destination)
        {
            var country = TravelDb.Countries.Where(c => c.Id == destination.CountryId).First();

            var countryDestinationNameInUse = country.Destinations.Any(
                d => d.Name.Equals(destination.Name,
                    StringComparison.InvariantCultureIgnoreCase));

            if (countryDestinationNameInUse)
            {
                ModelState.AddModelError("Name",
                    string.Format("Country already has a destination named {0}. Destinations must have unique names within their country.",
                        destination.Name));
            }
        }



        [Route("Upload/UploadHandler", Name = "UploadEndpoint")]
        public async Task<ActionResult> UploadHandler(int? objectContext)
        {
            var handler = new FileUploadHandler(Request, this);
            handler.IncomingRequestStarted += handler_IncomingRequestStarted;
            handler.StoreFileRequestStarted += handler_StoreFileRequestStarted;
            handler.GetFilesRequestStarted += handler_GetFilesRequestStarted;


            ActionResult result = await handler.HandleRequestAsync();
            return result;
        }





        void handler_IncomingRequestStarted(object sender, Backload.Eventing.Args.IncomingRequestEventArgs e)
        {
            //imagePathUtil = new ImagePathUtil(
            //  HttpContext.Server.MapPath(Properties.Settings.Default.DestinationImageRootDirectory),
            //  HttpContext.Server.MapPath(Properties.Settings.Default.DestinationThumbRootDirectory));

            // imagePathUtil = new ImagePathUtil();
        }





        void handler_StoreFileRequestStarted(object sender, Backload.Eventing.Args.StoreFileRequestEventArgs e)
        {
            Destination d;
            if (TryFindDestinationByObjectContext(e.Param.BackloadValues.ObjectContext, out d))
            {
                ImagePaths imagePaths = imagePathUtil.CreatePaths(d, e.Param.FileStatusItem.FileName);
                e.Param.FileStatusItem.StorageInfo.FilePath = imagePaths.Image;
                e.Param.FileStatusItem.StorageInfo.ThumbnailPath = imagePaths.Thumb;
            }
            else
            {
                // todo error handling
            }
        }





        void handler_GetFilesRequestStarted(object sender, Backload.Eventing.Args.GetFilesRequestEventArgs e)
        {
            Destination d;
            if (TryFindDestinationByObjectContext(e.Param.BackloadValues.ObjectContext, out d))
            {
                e.Param.SearchPath = imagePathUtil.CreatePhysicalSearchPath(d);
            }
            else
            {
            }
        }




        private bool TryFindDestinationByObjectContext(string objectContext, out Destination d)
        {
            int destinationId;
            if (int.TryParse(objectContext, out destinationId))
            {
                d = TravelDb.Destinations.Find(destinationId);

                return d != null;
            }
            d = null;
            return false;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TravelDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
