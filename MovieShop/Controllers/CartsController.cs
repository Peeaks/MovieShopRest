using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DLL.Contexts;
using DLL.Entities;

namespace MovieShop.Controllers {
    [RoutePrefix("api/carts")]
    public class CartsController : ApiController {
        private MovieShopContext db = new MovieShopContext();

        // GET: api/Carts/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult GetCart(string id) {
            Cart cart = db.Carts.Include(x => x.Movies).Include(x => x.PromoCode).FirstOrDefault(x => x.Id == id);
            if (cart == null) {
                return NotFound();
            }

            return Ok(cart);
        }

        // POST: api/Carts/AddToCart/5
        [Route("AddToCart")]
        public IHttpActionResult AddToCart(string id, Movie movie) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var movieInDb = db.Movies.Include(x => x.Genre).FirstOrDefault(x => x.Id == movie.Id);
            if (movieInDb == null) {
                return BadRequest("Movie not found");
            }

            var cartInDb = db.Carts.Include(x => x.Movies).Include(x => x.PromoCode).FirstOrDefault(x => x.Id == id);
            if (cartInDb == null) {
                return BadRequest("Cart not found");
            }
            cartInDb.Movies.Add(movieInDb);

            try {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            } catch (DbUpdateException) {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // POST: api/carts/RemoveFromCart/5
        [Route("RemoveFromCart")]
        public IHttpActionResult RemoveFromCart(string id, int movieId) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var cartInDb = db.Carts.Include(x => x.Movies).Include(x => x.PromoCode).FirstOrDefault(x => x.Id == id);
            if (cartInDb == null) {
                return BadRequest("Cart not found");
            }

            cartInDb.Movies.RemoveAll(x => x.Id == movieId);

            db.SaveChanges();


            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/carts/ClearCart/5
        [Route("ClearCart")]
        [ResponseType(typeof(Cart))]
        public IHttpActionResult ClearCart(string id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var cartInDb = db.Carts.Include(x => x.Movies).Include(x => x.PromoCode).FirstOrDefault(x => x.Id == id);
            if (cartInDb == null) {
                return BadRequest("Cart not found");
            }
            cartInDb.Movies.Clear();
            cartInDb.PromoCode = null;

            db.SaveChanges();


            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Carts
        [ResponseType(typeof(Cart))]
        public IHttpActionResult PostCart(Cart cart) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var tempList = new List<Movie>();
            foreach (var movie in cart.Movies) {
                tempList.Add(db.Movies.Include(x => x.Genre).FirstOrDefault(x => x.Id == movie.Id));
            }
            cart.Movies = tempList;
            cart.PromoCode = db.PromoCodes.FirstOrDefault(x => x.Code == cart.PromoCode.Code);

            db.Carts.Add(cart);

            try {
                db.SaveChanges();
            } catch (DbUpdateException) {
                if (CartExists(cart.Id)) {
                    return Conflict();
                } else {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new {id = cart.Id}, cart);
        }

        // POST: api/Carts/AddPromoToCart/5
        [Route("AddPromoToCart")]
        public IHttpActionResult AddPromoToCart(string id, string code) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var promoInDb = db.PromoCodes.FirstOrDefault(x => x.Code == code);
            if (promoInDb == null) {
                return BadRequest("Promo not found");
            }

            var cartInDb = db.Carts.Include(x => x.Movies).Include(x => x.PromoCode).FirstOrDefault(x => x.Id == id);
            if (cartInDb == null) {
                return BadRequest("Cart not found");
            }
            cartInDb.PromoCode = promoInDb;

            try {
                db.SaveChanges();
                return StatusCode(HttpStatusCode.OK);
            } catch (DbUpdateException) {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(string id) {
            return db.Carts.Count(e => e.Id == id) > 0;
        }
    }
}