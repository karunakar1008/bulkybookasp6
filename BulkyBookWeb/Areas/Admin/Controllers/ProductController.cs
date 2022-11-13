using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> cateforyList = _unitOfWork.ProductRepository.GetAll();
            return View(cateforyList);
        }
       
        public IActionResult Upsert(int? id)
        {
            Product product = new();
            if (id == null || id == 0)
            {
                //create product
                return View(product);
            }
            else { 
                //update Product
                 product = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}
