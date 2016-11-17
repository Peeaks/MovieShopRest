using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DLL.Entities {
    public class Customer : IdentityUser {
        [Display (Name = "First name")]
        public string FirstName { get; set; }
        [Display (Name = "Last name")]
        public string LastName { get; set; }
        public Address Address { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Customer> manager,
            string authenticationType) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

    }
}