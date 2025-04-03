using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Core.DTOs
{
    public class RenewLicenseApplicationDTO
    {
        public int ApplicationId {  get; set; }
        public string Notes {  get; set; }
        public decimal PaidFees {  get; set; }
    }
}
