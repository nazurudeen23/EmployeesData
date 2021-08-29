using EmployeesData.Data;
using EmployeesData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesData.Controllers
{
    public class DepartmentController : Controller
    {

        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<Department> Dep = _db.Departments;
            return View(Dep);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department Dp)
        {
            await _db.Departments.AddAsync(Dp);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var emp = _db.Departments.Find(id);

            if (emp == null)
            {
                return NotFound();
            }
            _db.Departments.Remove(emp);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
