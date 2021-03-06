﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeePayCompute.Entity;
using EmployeePayCompute.Models;
using EmployeePayCompute.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace EmployeePayCompute.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hostingEnviroment;


        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment hostingEnviroment)
        {
            _employeeService = employeeService;
            _hostingEnviroment = hostingEnviroment;
        }

        public IActionResult Index(int? pageNumber)
        {
            var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
            {
                ID = employee.ID,
                EmployeeNo = employee.EmployeeNo,
                ImageUrl = employee.ImageUrl,
                FullName = employee.FullName,
                Gender = employee.Gender,
                Designation = employee.Designation,
                City = employee.City,
                DateJoined = employee.DateJoined
            }).ToList();
            int pageSize = 4; 
            return View(EmployeeListPagination<EmployeeIndexViewModel>.Create(employees, pageNumber ?? 1, pageSize) );
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevent cross site Reuest Forgery Attacks
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    ID = model.ID,
                    EmployeeNo = model.EmployeeNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName=model.MiddleName,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Email = model.Email,
                    DOB = model.DOB,
                    DateJoined = model.DateJoined,
                    NationalInsurenceNo = model.NationalInsurenceNo,
                    PaymentMethod = model.PaymentMethod,
                    StudentLoan = model.StudentLoan,
                    UnionMember = model.UnionMember,
                    Address = model.Address,
                    City = model.City,
                    Phone = model.Phone,
                    PostCode = model.PostCode,
                    Designation = model.Designation,
                };

                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hostingEnviroment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                await _employeeService.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]

        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetByID(id);
            if (employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeEditViewModel()
            {
                ID = employee.ID,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                MiddleName=employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Email = employee.Email,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                NationalInsurenceNo = employee.NationalInsurenceNo,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Address = employee.Address,
                City = employee.City,
                Phone = employee.Phone,
                PostCode = employee.PostCode,
                Designation = employee.Designation,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeService.GetByID(model.ID);
                if (employee == null)
                {
                    return NotFound();
                }
                employee.EmployeeNo = model.EmployeeNo;
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.MiddleName = model.MiddleName;
                employee.NationalInsurenceNo = model.NationalInsurenceNo;
                employee.Gender = model.Gender;
                employee.Email = model.Email;
                employee.DOB = model.DOB;
                employee.DateJoined = model.DateJoined;
                employee.Phone = model.Phone;
                employee.Designation = model.Designation;
                employee.PaymentMethod = model.PaymentMethod;
                employee.StudentLoan = model.StudentLoan;
                employee.UnionMember = model.UnionMember;
                employee.Address = model.Address;
                employee.City = model.City;
                employee.PostCode = model.PostCode;
                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    var uploadDir = @"images/employee";
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var webRootPath = _hostingEnviroment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }
                await _employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetByID(id);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {

                ID = employee.ID,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsurenceNo = employee.NationalInsurenceNo,
                Phone = employee.Phone,
                Email = employee.Email,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Address = employee.Address,
                City = employee.City,
                ImageUrl = employee.ImageUrl,
                PostCode = employee.PostCode

            };
            return View(model);

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetByID(id);
            if (employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeDeleteViewModel()
            {
                ID = employee.ID,
                FullName = employee.FullName
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeService.Delete(model.ID);
            return RedirectToAction(nameof(Index));
        }
    }
}
