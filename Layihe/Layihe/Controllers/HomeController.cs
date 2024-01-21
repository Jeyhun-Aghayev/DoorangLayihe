using Layihe.DAL;
using Layihe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Layihe.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _db.Posts?.Where(p=>!p.IsDeleted)?.ToListAsync();
            ViewBag.Logourl = await _db.Settings.ToListAsync();
            return View(posts);
        }

    }
}
