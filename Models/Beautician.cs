using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Beautician : Employee
    {
        public Beautician()
        {
            AccountType = Enums.AccountType.BEAUTICIAN;
        }
    }
}
