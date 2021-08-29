using EmployeesData.Data;
using EmployeesData.Logic;
using EmployeesData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeesData.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;

        }
        public IActionResult Index(
             string sortOrder,
             string searchText,
             int? page)
        {
            ViewData["NameSort"] = sortOrder == "Name" ? "name_desc" : "name_asc";
            ViewData["DesignationSort"] = sortOrder == "Designation" ? "desg_desc" : "desg_asc";
            ViewData["DepartmentIdSort"] = sortOrder == "DepartmentId" ? "Id_desc" : "Id_asc";
            ViewData["currentSearch"] = searchText;

            var EmployeesList = from Employees in _db.Employees select Employees;

            if (!string.IsNullOrEmpty(searchText))
            {
                EmployeesList = EmployeesList.Where(x => x.Name.ToLower().
                    Contains(searchText.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_asc":
                    EmployeesList = EmployeesList.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    EmployeesList = EmployeesList.OrderByDescending(x => x.Name);
                    break;
                case "desg_asc":
                    EmployeesList = EmployeesList.OrderBy(x => x.Designation);
                    break;
                case "desg_desc":
                    EmployeesList = EmployeesList.OrderByDescending(x => x.Designation);
                    break;
                case "Id_asc":
                    EmployeesList = EmployeesList.OrderBy(x => x.DepartmentId);
                    break;
                case "Id_desc":
                    EmployeesList = EmployeesList.OrderByDescending(x => x.DepartmentId);
                    break;
                default:
                    EmployeesList = EmployeesList.OrderBy(x => x.Id);
                    break;
            }

            // IEnumerable < Employee > EmployeeList = _db.Employees;

            // return View(EmployeesList);

            int pageSize = 3;
            //return View(await EmployeesList.ToListAsync());
            return View(Pagination<Employee>.Display(EmployeesList, page ?? 1, pageSize));

        }

        public IActionResult Create()
        {
            // get the employee department from Db
            IEnumerable<SelectListItem> Dp = _db.Departments.Select(x =>
                new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                });

            ViewBag.myDropdown = Dp;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(emp);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(emp);
        }


        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emp = _db.Employees.FirstOrDefault(x => x.Id == id);
            if (emp == null)
            {
                return NotFound();
            }
            // get the employee department from Db
            IEnumerable<SelectListItem> Dp = _db.Departments.Select(x =>
                new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                });

            ViewBag.myDropdown = Dp;

            return View(emp);
        }

        [HttpPost]
        public IActionResult Update(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(emp);
                _db.SaveChanges();
            }

            ViewBag.Message = string.Format("The updation is successfull");

            return View(emp);
        }


        //[HttpPost]
        public IActionResult Delete(int? id)
        {
            var emp = _db.Employees.Find(id);

            if (emp == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(emp);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
