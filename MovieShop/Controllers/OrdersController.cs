using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DLL.Contexts;
using DLL.Entities;
using Microsoft.AspNet.Identity;

namespace MovieShop.Controllers {
    public class OrdersController : ApiController {
        private MovieShopContext db = new MovieShopContext();

        // GET: api/Orders
        [Authorize(Roles = "admin")]
        public List<Order> GetOrders() {
            return db.Orders.Include(x => x.Movies).Include(x => x.PromoCode).Include(x => x.Customer.Address).ToList();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        [Authorize(Roles = "admin")]
        public IHttpActionResult GetOrder(int id) {
            Order order = db.Orders.Include(x => x.Customer.Address).Include(x => x.PromoCode).Include(x => x.Movies).FirstOrDefault(x => x.Id == id);
            if (order == null) {
                return NotFound();
            }

            return Ok(order);
        }

        // GET: api/Orders/GetOrderByUser
        [Route("api/Orders/GetOrdersByUser")]
        [Authorize]
        public List<Order> GetOrdersByUser() {
            return db.Orders.Include(x => x.Customer.Address).Include(x => x.PromoCode).Include(x => x.Movies).Where(x => x.Customer.Email == HttpContext.Current.User.Identity.Name).ToList();
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        [Authorize]
        public IHttpActionResult PostOrder(Order order) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            order.Customer = db.Users.Include(x => x.Address).FirstOrDefault(x => x.Id == order.Customer.Id);

            order.Movies =
                order.Movies.Select(movie => db.Movies.Include(x => x.Genre).FirstOrDefault(x => x.Id == movie.Id))
                    .ToList();

            order.PromoCode = order.PromoCode != null
                ? db.PromoCodes.FirstOrDefault(x => order.PromoCode.Code == x.Code)
                : null;
            order.Time = DateTime.Now;

            db.Orders.Add(order);
            try {
                db.SaveChanges();
            } catch (DbUpdateException) {
                if (OrderExists(order.Id)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new {id = order.Id}, order);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id) {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}