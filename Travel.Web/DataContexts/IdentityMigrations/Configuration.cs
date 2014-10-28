namespace Travel.Web.DataContexts.IdentityMigrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using Travel.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Travel.Web.DataContexts.IdentityDb>
    {
        private readonly string AdminRoleName = "admin";
        //private readonly string DefaultAdminEmail = "admin@admin.com";
        private readonly string Password = "12341234";


        private readonly string[] DefaultAdminEmails = new string[]
        { 
            "admin@admin.com", "Jeffrey.Hong@visioncollege.ac.nz", "jfoxcoder@gmail.com"
        };

        private readonly string[] DefaultUserEmails = new string[]
        { 
            "homer@simpson.com", "marge@simpson.com", "bart@simpson.com", "lisa@simpson.com", "maggie@simpson.com"
        };

        

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"DataContexts\IdentityMigrations";
            ContextKey = "Identity.Models.ApplicationDbContext";
        }

        

       

        protected override void Seed(IdentityDb context)
        {
          
            using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                CreateAdminRole(rm);                
                
                // delete all users
                foreach (var u in um.Users.ToList())
                {
                    um.Delete(u);
                }

                // create admins
                foreach (var adminEmail in DefaultAdminEmails)
                {
                    var adminUser = new ApplicationUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail
                    };

                    var admin = um.Create(adminUser, Password);

                    um.AddToRole(adminUser.Id, AdminRoleName);
                }

                // create standard users
                foreach (var adminEmail in DefaultUserEmails)
                {
                    var user = new ApplicationUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail
                    };

                    um.Create(user, Password);
                }


            }
            
        }

        //private void AddAdminToRole(ApplicationUser defaultAdmin, UserManager<ApplicationUser> um)
        //{
        //    if (!um.IsInRole(defaultAdmin.Id, AdminRoleName))
        //    {
        //        var addToAdmin = um.AddToRole(defaultAdmin.Id, AdminRoleName);

        //        if (!addToAdmin.Succeeded)
        //            throw new ApplicationException("Adding user '" + defaultAdmin.Email +
        //                "' to '" + AdminRoleName + "' role failed with error(s): " + addToAdmin.Errors);
        //    }
        //}
        

        private void CreateAdminRole(RoleManager<IdentityRole> rm)
        {
            if (!rm.RoleExists(AdminRoleName))
            {
                // create the role
                IdentityResult createAdminRole = rm.Create(new IdentityRole(AdminRoleName));
                
                if (!createAdminRole.Succeeded)
                {
                    throw new ApplicationException("Creating role " + AdminRoleName +
                        " failed with error(s): " + createAdminRole.Errors);
                }
            }
        }

        private ApplicationUser CreateDefaultAdmin(UserManager<ApplicationUser> um)
        {
            
            var defaultAdmin = new ApplicationUser
            {
                Email = DefaultAdminEmails[0],
                UserName = DefaultAdminEmails[0]
            };

            var createDefaultAdmin = um.Create(defaultAdmin, Password);

            if (!createDefaultAdmin.Succeeded)
            {
                throw new ApplicationException("Creating default admin user '" + defaultAdmin.Email +
                    "' failed with error(s): " + createDefaultAdmin.Errors);
            }

            return defaultAdmin;
        }

       
    }
}
