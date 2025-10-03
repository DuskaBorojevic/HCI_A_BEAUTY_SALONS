using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Client : User
    {
        public Client() : base() { }

        public Client(int userId, string username, string firstName, string lastName, string emailAddress)
        : base(userId, username, firstName, lastName, emailAddress)
        {
            AccountType = Enums.AccountType.CLIENT;
        }
    }
}
