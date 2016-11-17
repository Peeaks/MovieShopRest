//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;

//namespace MovieShop.Models {
//    // You can add profile data for the user by adding more properties to your Customer class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
//    public class Customer : IdentityUser {
//        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Customer> manager,
//            string authenticationType) {
//            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
//            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
//            // Add custom user claims here
//            return userIdentity;
//        }
//    }
//}