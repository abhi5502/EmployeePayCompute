﻿using EmployeePayCompute.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayCompute.Services
{
    public interface IEmployeeService
    {
        Task CreateAsync( Employee newEmployee );
        Employee GetByID(int employeeId);
        Task UpdateAsync(Employee employee);
        Task UpdateAsync(int id);
        Task Delete(int employeeId);
        decimal UnionFees(int id);
        decimal StudentLoanRepaymentAmount(int id, decimal totalAmount );
        IEnumerable<Employee> GetAll();


    }
}