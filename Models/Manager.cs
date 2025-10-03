using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Manager : Employee
    {
        public Manager() : base()
        {
            this.AccountType = Enums.AccountType.MANAGER;
        }

        public Manager(int userId, string username, string password, string emailAddress, AccountType accountType,
                       string firstName, string lastName, string address, DateTime employmentDate, double salary, int beautySalonId)
            : base(userId, username, password, emailAddress, accountType, firstName, lastName, address, employmentDate, salary, beautySalonId)
        {
        }
    }
}
