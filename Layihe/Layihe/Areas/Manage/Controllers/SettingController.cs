using Layihe.Areas.Manage.Models.Setting;
using Layihe.DAL;
using Layihe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Layihe.Areas.Manage.Controllers
{
   
    namespace Business.Areas.Manage.Controllers
    {
        [Area("Manage")]
        [Authorize(Roles = "Admin")]
        public class SettingController : Controller
        {
            AppDbContext _db;

            public SettingController(AppDbContext db)
            {
                _db = db;
            }

            public IActionResult Index()
            {
                List<Setting> list = _db.Settings.ToList();

                return View(list);
            }

            public async Task<IActionResult> Update(int id)
            {
                if (!_db.Settings.Any(p => p.Id == id))
                {
                    return RedirectToAction("Index");
                }
                Setting setting = await _db.Settings.Where(p => p.Id == id).FirstOrDefaultAsync();
                UpdateSettingVm vm = new UpdateSettingVm()
                {
                    Id = id,
                    Value = setting.Value
                };
                return View(vm);
            }
            [HttpPost]
            public async Task<IActionResult> Update(UpdateSettingVm newSetting)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                Setting oldSetting = _db.Settings.Find(newSetting.Id);
                if (oldSetting == null)
                {
                    return View();
                }
                ViewBag.Key = $"{oldSetting.Key}";
                oldSetting.Value = char.ToUpper(newSetting.Value[0]) + newSetting.Value.Substring(1);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

        }
    }
}
