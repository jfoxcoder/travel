using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Travel.Web.DataContexts;
using System;
using System.Collections.Generic;

namespace Travel.Web.Models
{
    

 

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options)
        {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new IdentityDbContext()));
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new IdentityDb())); 
            // Configure the application user manager
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new MinimumLengthValidator(6);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.PasswordResetTokens = new DataProtectorTokenProvider(dataProtectionProvider.Create("PasswordReset"));
                manager.UserConfirmationTokens = new DataProtectorTokenProvider(dataProtectionProvider.Create("ConfirmUser"));
            }
            return manager;
        }
    }
}