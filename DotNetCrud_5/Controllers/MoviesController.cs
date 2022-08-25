using DotNetCrud_5.Models;
using DotNetCrud_5.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCrud_5.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext db;
        private new List<string> allowedExtintions = new List<string>{".jpg",".png" };
        private readonly IToastNotification toastNotification;

        public MoviesController(ApplicationDbContext db, IToastNotification toastNotification)
        {
            this.db = db;
            this.toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await db.Movies.ToListAsync();
            return View(Movies);
        }
        #region Create
        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieVM()
            {
                Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync(),
            };
            return View("MovieForm", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            //Get All Files Attched With This Form ==>
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                // AddModelError ==> Take A (Key,message)
                ModelState.AddModelError("Poster", "Plese Select Movie Poster");
                return View("MovieForm", model);
            }
            else
            {
                var Poster = files.FirstOrDefault();

                //Allowed Extntions ==> To Control Extintions File Like (pdf,jpg,png,etc ..)

                if (!allowedExtintions.Contains(Path.GetExtension(Poster.FileName).ToLower()))
                {
                    model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                    // AddModelError ==> Take A (Key,message)
                    ModelState.AddModelError("Poster", "Plese Select Movie Poster Extension .jpg , .png only");
                    return View("MovieForm", model);
                }
                // To Control A Size Of File 
                if (Poster.Length > 1048576 )
                {
                    model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Plese Select Movie Poster Max Size 1MB Only");
                    return View(model);


                }
                using var datastream=new MemoryStream();
                await Poster.CopyToAsync(datastream);

                var movies = new Movie()
                {
                    Title = model.Title,
                    StoryLine = model.StoryLine,
                    Rate = model.Rate,
                    Year = model.Year,
                    Poster = datastream.ToArray(),
                    GenreId=model.GenreId
                    
                };
                db.Movies.Add(movies);
                db.SaveChanges();
            }
            model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
            toastNotification.AddSuccessToastMessage("Movie Has Created Done");
            return RedirectToAction("index");           
     
           
        }
        #endregion

        #region Handell Delete
        public async Task<IActionResult> Delete(int Id)
        {
            
            var OldData=await db.Movies.FindAsync(Id);
            db.Movies.Remove(OldData);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id==null)
            {
                return BadRequest();

            }
            else
            {
                var data = await db.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();
                if (data==null)
                {
                    return NotFound();

                }
                else
                {
                    var ViewModel = new MovieVM()
                    {
                        id = data.Id,
                        GenreId=data.GenreId,
                        Poster=data.Poster,
                        Year = data.Year,
                        Rate=data.Rate,
                        StoryLine=data.StoryLine,
                        Title=data.Title,
                        Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync(),
                    };
                return View("MovieForm", ViewModel);
                }
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Update(MovieVM model)
        {

            if (ModelState.IsValid)
            {
                model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                var movie = await db.Movies.FindAsync(model.id);
                if (movie == null)
                return NotFound();
              
                var files = Request.Form.Files;
                if (files.Any())
                {
                    var poster = files.FirstOrDefault();
                using var data = new MemoryStream();
                    await poster.CopyToAsync(data);
                    model.Poster = movie.Poster;
                    //Allowed Extntions ==> To Control Extintions File Like (pdf,jpg,png,etc ..)

                    if (!allowedExtintions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                    {
                        model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                        // AddModelError ==> Take A (Key,message)
                        ModelState.AddModelError("Poster", "Plese Select Movie Poster Extension .jpg , .png only");
                        return View("MovieForm", model);
                    }
                    // To Control A Size Of File 
                    if (poster.Length > 1048576)
                    {
                        model.Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync();
                        ModelState.AddModelError("Poster", "Plese Select Movie Poster Max Size 1MB Only");
                        return View(model);


                    }

                    movie.Poster=data.ToArray();
                }
                movie.Title = model.Title;
                movie.Year = model.Year;
                movie.Rate = model.Rate;
                movie.StoryLine = model.StoryLine;
                movie.GenreId = model.GenreId;
                db.SaveChanges();






            }
            toastNotification.AddSuccessToastMessage("Movie Has Updated Done");

            return RedirectToAction(nameof(Index));
        }


        #endregion
    }
}
