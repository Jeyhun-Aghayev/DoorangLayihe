using Layihe.Areas.Manage.Models.Post;
using Layihe.DAL;
using Layihe.Helper;
using Layihe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Layihe.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class PostController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _webHostEnvironment = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _db.Posts.Where(p => p.IsDeleted == false).ToListAsync();
            return View(posts);
        }
        
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            Post post = new Post()
            {
                Author = vm.Author,
                Title = vm.Title,
                Description = vm.Description,
                ImgUrl = vm.Image.Upload(_webHostEnvironment.WebRootPath,@"\Upload\PostImage\"),
                CreatedAt = DateTime.UtcNow,
            };
            await _db.Posts.AddAsync(post);
            int success = await _db.SaveChangesAsync();
            if (success == 0) return BadRequest(ModelState);
            return RedirectToAction(nameof(Index),"Post");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Post post = await _db.Posts.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if(post is null) return BadRequest();
            UpdatePostVm vm = new UpdatePostVm()
            {
                Id = id,
                Author = post.Author,
                Title = post.Title,
                Description = post.Description,
                ImgUrl = post.ImgUrl,
                CreatedAt = post.CreatedAt,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            Post post = await _db.Posts.Where(p => !p.IsDeleted&&p.Id == vm.Id).FirstOrDefaultAsync();
            if (post is null) return BadRequest();
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.Author = vm.Author;
            post.CreatedAt = vm.CreatedAt;
            post.UpdatedAt = DateTime.UtcNow;
            if (vm.Image is not null) post.ImgUrl = vm.Image.UploadForUpdate(_webHostEnvironment.WebRootPath, @"\Upload\PostImage\");
            _db.Update(post);
            var success = await _db.SaveChangesAsync();
            if (success == 0) return BadRequest();
            return RedirectToAction(nameof(Index), "Post");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Post post = await _db.Posts.Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if (post is null) return BadRequest();
            post.IsDeleted = true;
            var success = await _db.SaveChangesAsync();
            if (success == 0) return BadRequest();
            return RedirectToAction(nameof(Index), "Post");
        }
    }
}
