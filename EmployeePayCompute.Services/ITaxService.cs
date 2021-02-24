using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayCompute.Services
{
    public interface ITaxService
    {
        decimal TaxAmount( decimal totalAmount);

    }
}
