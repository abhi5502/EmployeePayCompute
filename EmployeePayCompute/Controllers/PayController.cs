﻿using EmployeePayCompute.Entity;
using EmployeePayCompute.Models;
using EmployeePayCompute.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePayCompute.Controllers
{
    public class PayController : Controller
    {
        private readonly IPayComputationService _payComputationService;
        private readonly IEmployeeService _employeeService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceContributionService _nationalInsuranceContributionService;
        private decimal overtimeHrs;
        private decimal contractualEarnings;
        private decimal overtimeEarnings;
        private decimal totalEarnings;
        private decimal tax;
        private decimal unionFee;
        private decimal studentLoan;
        private decimal nationalInsurance;
        private decimal totalDeduction;

        public PayController(IPayComputationService payComputationService,
            IEmployeeService employeeService, ITaxService taxService,
            INationalInsuranceContributionService nationalInsuranceContributionService
            )
        {
            _payComputationService = payComputationService;
            _employeeService = employeeService;
            _taxService = taxService;
            _nationalInsuranceContributionService = nationalInsuranceContributionService;
        }

        public IActionResult Index()
        {
            var payRecords = _payComputationService.GetAll().Select(pay => new PaymentRecordIndexViewModel
            {
                Id = pay.ID,
                EmployeeId = pay.EmployeeID,
                FullName = pay.FullName,
                PayDate = pay.PayDate,
                PayMonth = pay.PayMonth,
                TaxYearId = pay.TaxYearID,
                Year = _payComputationService.GetTaxYearById(pay.TaxYearID).YearOfTax,
                TotalEarnings = pay.TotalEarnings,
                TotalDeduction = pay.TotalDeduction,
                NetPayment = pay.NetPayment,
                Employee = pay.Employee

            });

            return View(payRecords);
        }

        public IActionResult Create()
        {
            ViewBag.employee = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();
            var model = new PaymetRecordCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(PaymetRecordCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payrecord = new PaymentRecord()
                {
                    ID = model.ID,
                    EmployeeID = model.EmployeeID,
                    FullName = _employeeService.GetByID(model.EmployeeID).FullName,
                    NiNo = _employeeService.GetByID(model.EmployeeID).NationalInsurenceNo,
                    PayDate = model.PayDate,
                    PayMonth = model.PayMonth,
                    TaxYearID = model.TaxYearID,
                    TaxCode = model.TaxCode,
                    HourlyRate = model.HourlyRate,
                    HoursWorked = model.HoursWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = overtimeHrs = _payComputationService.OvertimeHours(model.HoursWorked, model.ContractualHours),
                    ContractualEarnings = contractualEarnings = _payComputationService.ContractualEarnings(model.ContractualEarnings, model.HoursWorked, model.HourlyRate),
                    OvertimeEarnings = overtimeEarnings = _payComputationService.OvertimeEarnings(_payComputationService.OvertimeRate(model.HourlyRate), overtimeHrs),
                    TotalEarnings = totalEarnings=_payComputationService.TotalEarnings(overtimeEarnings, contractualEarnings),
                    Tax =tax =_taxService.TaxAmount(totalEarnings),
                    UnionFee= unionFee =_employeeService.UnionFees(model.EmployeeID),
                    SLC= studentLoan=_employeeService.StudentLoanRepaymentAmount(model.EmployeeID, totalEarnings),
                    NIC= nationalInsurance =_nationalInsuranceContributionService.NIContribution(totalEarnings),
                     TotalDeduction= totalDeduction= _payComputationService.TotalDeduction(tax, nationalInsurance, studentLoan, unionFee),
                     NetPayment= _payComputationService.NetPay(totalEarnings, totalDeduction)
                };
                await  _payComputationService.CreateAsync(payrecord);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.employee = _employeeService.GetAllEmployeesForPayroll();
            ViewBag.taxYears = _payComputationService.GetAllTaxYear();
            return View();
        }
    }

}