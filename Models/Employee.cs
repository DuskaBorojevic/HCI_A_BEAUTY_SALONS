using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Employee : User
    {
        public string Address { get; set; }
        public DateTime EmploymentDate { get; set; }
        public double Salary { get; set; }
        public int BeautySalonId { get; set; }
        public bool Deleted { get; set; }

        public Employee() : base() { }

        public Employee(int userId, string username, string password, string emailAddress, AccountType accountType,
                        string firstName, string lastName, string address, DateTime employmentDate, double salary, int beautySalonId)
            : base(userId, username, password, emailAddress, accountType)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            EmploymentDate = employmentDate;
            Salary = salary;
            BeautySalonId = beautySalonId;
        }
    }
}
