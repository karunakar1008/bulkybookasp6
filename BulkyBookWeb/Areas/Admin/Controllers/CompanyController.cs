using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                //create product
                return View(company);
            }
            else
            {
                //update Product
                company = _unitOfWork.CompanyRepository.GetFirstOrDefault(c => c.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                    _unitOfWork.CompanyRepository.Add(obj);
                else
                    _unitOfWork.CompanyRepository.Update(obj);

                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpDelete, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var company = _unitOfWork.CompanyRepository.GetFirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.CompanyRepository.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.GetAll();
            return Json(new { data = companies });
        }
        #endregion
    }
}
