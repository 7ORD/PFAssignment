using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    [Serializable]
    public class User
    {
        public enum UserAccountType
        {
            Nurse,
            Doctor,
            Admin,
            Null
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserAccountType AccountType { get; set; }
        public bool ResetFlag { get; set; }
        public bool ResetRequestFlag { get; set; }
    }
}