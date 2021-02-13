using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeePayCompute.Models;
using EmployeePayCompute.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePayCompute.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
             _employeeService = employeeService;

        }

        public IActionResult Index()
        {
            var employee = _employeeService.GetAll().Select(employee=> new EmployeeIndexViewModel
            {
                ID= employee.ID,
                EmployeeNo= employee.EmployeeNo,
                ImageUrl= employee.ImageUrl,
                FullName= employee.FullName,
                Gender= employee.Gender,
                Designation= employee.Designation,
                City= employee.City,
                DateJoined= employee.DateJoined
            }).ToList();
            return View();
        }

    }
}
