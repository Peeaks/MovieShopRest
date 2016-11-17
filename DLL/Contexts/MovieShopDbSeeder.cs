using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DLL.Contexts {
    class MovieShopDbSeeder : DropCreateDatabaseAlways<MovieShopContext> {
        protected override void Seed(MovieShopContext context) {
            var adminRole = new IdentityRole { Id = "1", Name = "admin" };
            context.Roles.Add(adminRole);
            context.Users.Add(new Customer { Email = "admin", UserName = "admin", PasswordHash = "admin", Roles = { new IdentityUserRole { RoleId = "1" } } });

            base.Seed(context);
        }
    }
}
 