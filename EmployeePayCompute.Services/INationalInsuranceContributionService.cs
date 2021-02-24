using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayCompute.Services
{
    public interface INationalInsuranceContributionService
    {
        decimal NIContribution(decimal totalAount);

    }
}
