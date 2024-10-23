using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Objects
{
    [Serializable]
    public class User
    {
        public enum UserAccountType
        {
            Nurse,
            Doctor,
            Admin
        }

        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required UserAccountType AccountType { get; set; }
    }
}
