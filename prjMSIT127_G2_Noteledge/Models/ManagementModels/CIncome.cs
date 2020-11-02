using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ManagementModels
{
    public class CIncome
    {
        public int fIncomeId { get; set; }
        public int fIncome { get; set; }
        public DateTime fPaymentDateTime { get; set; }
        public string fIncomeCategory { get; set; }
        public int fMemberId { get; set; }
    }
}
