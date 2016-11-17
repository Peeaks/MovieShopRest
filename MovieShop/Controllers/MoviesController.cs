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
    public class MoviesController : ApiController {
        private MovieShopContext db = new MovieShopContext();

        // GET: api/Movies
        [OverrideAuthorization]
        public IQueryable<Movie> GetMovies() {
            return db.Movies.Include(x => x.Genre);
        }

        // GET: api/Movies/5
        [ResponseType(typeof(Movie))]
        [OverrideAuthorization]
        public IHttpActionResult GetMovie(int id) {
            Movie movie = db.Movies.Include(x => x.Genre).FirstOrDefault(x => x.Id == id);
            if (movie == null) {
                return NotFound();
            }

            return Ok(movie);
        }

        // PUT: api/Movies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovie(int id, Movie movie) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != movie.Id) {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!MovieExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Movies
        [ResponseType(typeof(Movie))]
        public IHttpActionResult PostMovie(Movie movie) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var findGenre = db.Genres.FirstOrDefault(x => x.Id == movie.Genre.Id);
            if (findGenre == null) {
                return BadRequest();
            }
            movie.Genre = findGenre;

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {id = movie.Id}, movie);
        }

        // DELETE: api/Movies/5
        [ResponseType(typeof(Movie))]
        public IHttpActionResult DeleteMovie(int id) {
            Movie movie = db.Movies.Find(id);
            if (movie == null) {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id) {
            return db.Movies.Count(e => e.Id == id) > 0;
        }
    }
}