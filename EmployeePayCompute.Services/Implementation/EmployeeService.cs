using EmployeePayCompute.Entity;
using EmployeePayCompute.Persistence;
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

        public IEnumerable<Employee> GetAll() => _context.Employees;

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
            throw new NotImplementedException();
        }

        public decimal UnionFees(int id)
        {
            throw new NotImplementedException();
        }

        
    }
}
