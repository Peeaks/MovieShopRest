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
    [Authorize(Roles = "admin")]
    public class PromoCodesController : ApiController {
        private MovieShopContext db = new MovieShopContext();

        // GET: api/PromoCodes
        public IQueryable<PromoCode> GetPromoCodes() {
            return db.PromoCodes;
        }

        // GET: api/PromoCodes/5
        [ResponseType(typeof(PromoCode))]
        public IHttpActionResult GetPromoCode(int id) {
            PromoCode promoCode = db.PromoCodes.Find(id);
            if (promoCode == null) {
                return NotFound();
            }

            return Ok(promoCode);
        }

        // PUT: api/PromoCodes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPromoCode(int id, PromoCode promoCode) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != promoCode.Id) {
                return BadRequest();
            }

            db.Entry(promoCode).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!PromoCodeExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PromoCodes
        [ResponseType(typeof(PromoCode))]
        public IHttpActionResult PostPromoCode(PromoCode promoCode) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.PromoCodes.Add(promoCode);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {id = promoCode.Id}, promoCode);
        }

        // DELETE: api/PromoCodes/5
        [ResponseType(typeof(PromoCode))]
        public IHttpActionResult DeletePromoCode(int id) {
            PromoCode promoCode = db.PromoCodes.Find(id);
            if (promoCode == null) {
                return NotFound();
            }

            db.PromoCodes.Remove(promoCode);
            db.SaveChanges();

            return Ok(promoCode);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PromoCodeExists(int id) {
            return db.PromoCodes.Count(e => e.Id == id) > 0;
        }
    }
}