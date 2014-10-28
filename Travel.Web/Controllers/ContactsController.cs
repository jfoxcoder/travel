using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Travel.Web.DataContexts;
using Travel.Web.Enums;
using Travel.Web.Models;

namespace Travel.Web.Controllers
{
    
   
  //  [Route("{action=Index}")]
    public class ContactsController : TravelControllerBase
    {
      
        [Authorize(Roles="admin")]    
        [Route("admin/contacts", Name = "AdminContactsHome")]
        public ActionResult Index()
        {
            LayoutVm.InitPage(webPageName: "Manage Contacts");

            var contacts = TravelDb.Contacts
                                   .OrderByDescending(c => c.Id)
                                   .ToList();
            
            return View(contacts);
        }

        
        [Authorize(Roles = "admin")]
        [Route("admin/contacts/{id}/view", Name = "ViewContact")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = TravelDb.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


       

      
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]        
        [Route("contacts/create", Name="CreateContact")]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                TravelDb.Contacts.Add(contact);
                TravelDb.SaveChanges();

                FlashMessage(
                    string.Format("Hi {0}, thanks for the message. We'll get back to you soon.", 
                        contact.Name), 
                    FlashMessageMode.Success);

                return RedirectToRoute("Home");
            }

            return View("~/Views/Home/Contact.cshtml", contact);
        }

        

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("admin/contacts/{id}/edit", Name = "EditContactForm")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = TravelDb.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        [Route("admin/contacts/{id}/edit", Name = "EditContact")]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,Message")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                TravelDb.Entry(contact).State = EntityState.Modified;
                TravelDb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("admin/contacts/{id}/delete", Name = "DeleteContactForm")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = TravelDb.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        [Route("admin/contacts/{id}/delete", Name = "DeleteContact")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = TravelDb.Contacts.Find(id);
            TravelDb.Contacts.Remove(contact);
            TravelDb.SaveChanges();
            return RedirectToAction("Index");
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
