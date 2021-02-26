using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayCompute.Services.Implementation
{
    public class NationalInsuranceContributionService : INationalInsuranceContributionService
    {
        private decimal NIRate;
        private decimal NIC;
        public decimal NIContribution(decimal totalAount)
        {
            if (totalAount<719)
            {
                // Lower Earning Limit Rate & Below Primary Threshold

                NIRate = .0m;
                NIC = 0m;
            }
            else if(totalAount>=719 && totalAount<=4167)
            {
                //Between Primary Threshold and Upper Earnings Limit (UEL)

                NIRate = .12m;
                NIC = ((totalAount - 719) * NIRate);
            }
            else if(totalAount > 4167)
            {
                // Above Upper Earnings Limit (UEL)
                NIRate = .02m;
                NIC = ((4167 - 719) * .12m) + ((totalAount - 4167) * NIRate);
            }
            return NIC;
        }
    }
}
