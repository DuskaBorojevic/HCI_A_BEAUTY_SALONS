using HCI_A.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public AccountType AccountType { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }

        public User()
        {
            AccountType = AccountType.CLIENT;
        }

        public User(int userId, string username, string password, string emailAddress, AccountType accountType)
        {
            UserId = userId;
            Username = username;
            Password = password;
            EmailAddress = emailAddress;
            AccountType = accountType;
        }

        public User(int userId, string username, string password, string emailAddress, AccountType accountType, string theme, string language)
        {
            UserId = userId;
            Username = username;
            Password = password;
            EmailAddress = emailAddress;
            AccountType = accountType;
            Theme = theme;
            Language = language;
        }

        public User(int userId, string username, string firstName, string lastName, string emailAddress)
        {
            UserId = userId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
    }
}
