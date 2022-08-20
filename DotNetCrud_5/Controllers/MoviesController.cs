using DotNetCrud_5.Models;
using DotNetCrud_5.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCrud_5.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext db;
        public MoviesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var Movies = await db.Movies.ToListAsync();
            return View(Movies);
        }
        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieVM()
            {
                Genres = await db.Genres.OrderBy(m => m.Name).ToListAsync(),
            };
            return View(ViewModel);
        }
    }
}
