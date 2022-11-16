using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                CoverTypeList = _unitOfWork.CoverTypeRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString(),
            //});
            //IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverTypeRepository.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString(),
            //});
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update Product
                productVM.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwRottPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwRottPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwRottPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                    _unitOfWork.ProductRepository.Add(obj.Product);
                else
                    _unitOfWork.ProductRepository.Update(obj.Product);

                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpDelete, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var product = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }
        #endregion
    }
}
