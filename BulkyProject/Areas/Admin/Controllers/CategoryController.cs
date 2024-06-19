using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BulkyProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryList()
        {
            try
            {
                List<Category> categories = unitOfWork.category.GetAll().ToList();
                return View(categories);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework like NLog or Serilog)
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving categories.");
                return View(new List<Category>()); // Return an empty list or appropriate view
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            try
            {
                if (obj.Name == obj.DisplayOrder.ToString())
                {
                    ModelState.AddModelError("name", "The display order cannot exactly match the name.");
                }

                if (ModelState.IsValid)
                {
                    unitOfWork.category.Add(obj);
                    unitOfWork.Save();
                    return RedirectToAction("CategoryList", "Category");
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var category = unitOfWork.category.Get(x => x.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the category for editing.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.category.Update(obj);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
                return View(obj);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var category = unitOfWork.category.Get(x => x.Id == id);
                if (category != null)
                {
                    unitOfWork.category.Remove(category);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the category.");
                return RedirectToAction("Index");
            }
        }
    }
}
