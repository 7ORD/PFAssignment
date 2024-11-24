using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly DateCreated { get; set; }
        public string ContactNumber { get; set; }
        public string NHSNumber { get; set; }
        public Address Address { get; set; }
        public List<int> Appointments { get; set; }

        [JsonIgnore]
        public string ParsedAddress
        {
            get => new string($"{Address.FirstLine}, {Address.SecondLine}, {Address.Town}, {Address.PostCode}");
            private set { }
        }
        [JsonIgnore]
        public string HospitalNumber 
        {
            get => new string($"PRS-{DateCreated}-{Id.ToString("000")}").Replace("/", "");
            private set { }
        }

        public Patient ()
        {
            Id = 0;
            FirstName = "";
            LastName = "";
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now);
            DateCreated = DateOnly.FromDateTime(DateTime.Now);
            ContactNumber = "";
            NHSNumber = "";
            Address = new Address("", "", "", "");
        }
    }
}
