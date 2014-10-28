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
using Travel.Web.ViewModels;

namespace Travel.Web.Controllers
{
    [Authorize(Roles="admin")]
    public class ApplicationUsersController : TravelControllerBase
    {
        

        // GET: ApplicationUsers
        [Route("admin/users", Name = "AdminUsersHome")]
        public ActionResult Index()
        {
            LayoutVm.InitPage(webPageName: "Users");

            var adminsAndUsers = IdentityDb.GetAdminsAndUsers();

            return View(new ManageUsersVm
            {
                Admins = adminsAndUsers.Admins.Select(admin => 
                    new UserVm
                    { 
                        ApplicationUser = admin,
                        IsAdmin = true,
                        IsLoggedInAdmin = admin.Id == LoggedInUser.Id 
                    }),
                Users = adminsAndUsers.Users.Select(user => 
                    new UserVm 
                    { 
                        ApplicationUser = user  
                    }),
            });
        }

        [Route("admin/users/promote/{userId}", Name = "PromoteToAdmin")]
        public ActionResult PromoteToAdmin(string userId)
        {
            var errors = new string[] { };
            
            if (! IdentityDb.TryAddAdminRoleToUser(userId, out errors))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.InternalServerError, string.Join(", ", errors));                
            }

            // OK
            return new EmptyResult();
        }


        [Route("admin/users/demote/{userId}", Name = "DemoteToUser")]
        public ActionResult DemoteToUser(string userId)
        {
            if (userId == LoggedInUser.Id)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.InternalServerError, "Sorry, you can't demote yourself.");
            }



            var errors = new string[] { };

            if (! IdentityDb.TryRemoveAdminRoleFromUser(userId, out errors))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.InternalServerError, string.Join(", ", errors));
            }

            // OK
            return new EmptyResult();
        }

    

      
        [HttpDelete]       
        [Route("admin/users/delete", Name = "DeleteUser")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = IdentityDb.Users.Find(id);


            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            if (applicationUser.Id == LoggedInUser.Id)
            {
                return new HttpStatusCodeResult(
                   HttpStatusCode.InternalServerError, "Sorry, you can't delete yourself.");
            }

            IdentityDb.Users.Remove(applicationUser);
            IdentityDb.SaveChanges();

            return new EmptyResult();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IdentityDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
