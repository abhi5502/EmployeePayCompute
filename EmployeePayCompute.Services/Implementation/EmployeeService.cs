using EmployeePayCompute.Entity;
using EmployeePayCompute.Persistence;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayCompute.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Employee newEmployee)
        {
           await _context.Employees.AddAsync(newEmployee);
           await _context.SaveChangesAsync();
        }

        public Employee GetByID(int employeeId) => _context.Employees.Where(e => e.ID == employeeId).FirstOrDefault();
       

        public async Task Delete(int employeeId)
        {
            var employee = GetByID(employeeId);
            _context.Remove(employee);
            await _context.SaveChangesAsync();

        }

        public IEnumerable<Employee> GetAll() => _context.Employees.AsNoTracking().OrderBy(emp=>emp.FullName);

        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetByID(id);
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }
       
        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetByID(id);
            if(employee.StudentLoan==StudentLoan.Yes && totalAmount > 1750 && totalAmount <2000)
            {
                studentLoanAmount = 15m;
            }
            else if(employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2000 && totalAmount < 2250)
            {
                studentLoanAmount = 38m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2250 && totalAmount < 2500)
            {
                studentLoanAmount = 60m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >=2500)
            {
                studentLoanAmount = 83m;
            }
            else
            {
                studentLoanAmount = 0m;
            }
            return studentLoanAmount;
        }

        public decimal UnionFees(int id)
        {
            var employee = GetByID(id);
            var fee = employee.UnionMember == UnionMember.Yes ? 10m : 0m;
            return fee;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayroll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.ID.ToString()
            }); ;
        }
    }
}
