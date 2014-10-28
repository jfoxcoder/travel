using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Travel.Web.Models;

namespace Travel.Web.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        private UserManager<ApplicationUser> UserManager { get; set; }

        public IdentityDb()
            : base("DefaultConnection")
        {

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this));
        }
        internal ApplicationUser FindUserById(string id)
        {
            return UserManager.FindById(id);
        }


        //IdentityDb.Users.Where(u => u.Roles.Any(r => r.RoleId == adminRole.Id)).ToList()

        private IQueryable<ApplicationUser> GetAdmins()
        {
            // Get user ids of admins.
            var adminUserIds = AdminRole.Users.Select(u => u.UserId);

            return Users.Where(u => adminUserIds.Contains(u.Id));

        }

        private IdentityRole AdminRole
        {
            get
            {
                return Roles.Where(r => r.Name == "admin").Single();
            }
        }

        public AdminsAndUsers GetAdminsAndUsers()
        {            
            var admins = GetAdmins();

            return new AdminsAndUsers
            {
                Admins = admins,
                Users = Users.Except(admins)
            };            
        }

        public bool TryAddAdminRoleToUser(string userId, out string[] errors)
        {
            return ChangeUserRole(userId, "admin", true, out errors);
        }

        public bool TryRemoveAdminRoleFromUser(string userId, out string[] errors)
        {
            return ChangeUserRole(userId, "admin", false, out errors);
        }

        private bool ChangeUserRole(string userId, string role, bool addTo, out string[] errors)
        {
            // Change user role.
            IdentityResult result = addTo ? 
                UserManager.AddToRole(userId, role) 
                : UserManager.RemoveFromRole(userId, "admin");

            // Success
            if (result.Succeeded)
            {
                errors = null;
                return true;
            }

            // Failure
            errors = result.Errors.ToArray();
            return false;     
        }

        
    }

}