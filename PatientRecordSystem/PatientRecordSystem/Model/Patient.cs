using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    /// <summary>
    /// Patient data model - Contains a constructor which sets some default values.
    /// </summary>
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

        /// <summary>
        /// Parses Address into the following format: "First Line, Second Line, Town, POSTCODE"
        /// </summary>
        [JsonIgnore]
        public string ParsedAddress
        {
            get => new string($"{Address.FirstLine}, {Address.SecondLine}, {Address.Town}, {Address.PostCode}");
            private set { }
        }
        /// <summary>
        /// Creates a patient hospital ID in the following format: "PRS-'CREATIONDATE'-'PATIENTID'" eg: "PRS-01012024-001"
        /// </summary>
        [JsonIgnore]
        public string HospitalNumber 
        {
            get => new string($"PRS-{DateCreated}-{Id.ToString("000")}").Replace("/", "");
            private set { }
        }

        /// <summary>
        /// Joins the patients first and last names together into one string in the following format: 'FirstName LastName'
        /// </summary>
        [JsonIgnore]
        public string ParsedName
        {
            get => new string($"{FirstName} {LastName}");
            private set { }
        }

        /// <summary>
        /// Constructor which sets some default values when creating a new patient
        /// </summary>
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
            Appointments = new List<int>();
        }
    }
}
