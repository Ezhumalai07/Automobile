using Automobile.Data;
using Automobile.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automobile.Controllers
{
    public class BrandController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public BrandController(ApplicationDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();
            return View(brands);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFilename = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\brand");
                var extension = Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine(upload, newFilename + extension);
                using (var filestream = new FileStream(Path.Combine(upload, newFilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.Brandlogo = @"\images\brand\" + newFilename + extension;
            }

            if (ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();
                TempData["success"] = "Record Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }
        [HttpPost]
        public ActionResult Edit(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFilename = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\brand");
                var extension = Path.GetExtension(file[0].FileName);
                // delete old image
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                var oldImagePath = Path.Combine(webRootPath, objFromDb.Brandlogo.Trim('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                var filePath = Path.Combine(upload, newFilename + extension);
                using (var filestream = new FileStream(Path.Combine(upload, newFilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.Brandlogo = @"\images\brand\" + newFilename + extension;
            }
            if (ModelState.IsValid)
            {
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                objFromDb.Name = brand.Name;
                objFromDb.EstablishedYear = brand.EstablishedYear;
                if (brand.Brandlogo != null)
                {
                    objFromDb.Brandlogo = brand.Brandlogo;
                }
                _dbContext.Brand.Update(brand);
                _dbContext.SaveChanges();
                TempData["warning"] = "Record Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(brand.Brandlogo))
            {
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                if (objFromDb.Brandlogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.Brandlogo.Trim('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }
            _dbContext.Brand.Remove(brand);
            _dbContext.SaveChanges();
            TempData["error"] = "Record Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
