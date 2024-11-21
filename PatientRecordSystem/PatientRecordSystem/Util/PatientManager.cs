using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PatientRecordSystem.Model;

namespace PatientRecordSystem.Util
{
    public sealed class PatientManager
    {

        //PatientManager singleton property
        private static PatientManager instance;

        /// <summary>
        /// Method to retrieve the current instance of this object.
        /// If there is already an active instance of the object, then return it. Otherwise, create a new instance.
        /// </summary>
        /// <returns>Returns the instance of PatientManager if it exists, otherwise returns a new instance.</returns>
        public static PatientManager GetInstance ()
        {
            if (instance == null)
            {
                instance = new PatientManager ();
            }

            return instance;
        }

        public List<Patient> Patients ()
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\patients.json";
            string jsonString = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<Patient>>(jsonString);
        }

        public void UpdateData (List<Patient> patients)
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\patients.json";
            string jsonString = JsonSerializer.Serialize(patients);
            File.WriteAllText(jsonPath, jsonString);
        }

        public bool IsPatientValid (Patient patient)
        {
            // Checks if Patient patient is assigned a value.
            if (patient == null)
            {
                return false;
            }

            // NHS Number Regex - Checks for the format of 000X000X0000 where 0 = int and X = whitespace
            Regex nhsNumberRegex = new Regex("\\d{3}\\s\\d{3}\\s\\d{4}$");

            // Phone number Regex - Digits 0-9, length 11.
            Regex phoneNumberRegex = new Regex("\\d{11}$");

            // Name Regex - Accepts characters A-Z, disregards case.
            Regex nameRegex = new Regex(@"^[a-zA-Z]+$");

            if (nhsNumberRegex.IsMatch (patient.NHSNumber))
            {
                if (phoneNumberRegex.IsMatch(patient.ContactNumber))
                {
                    if (patient.DateOfBirth <= DateOnly.FromDateTime(DateTime.Now))
                    {
                        if (nameRegex.IsMatch(patient.FirstName))
                        {
                            if (nameRegex.IsMatch(patient.LastName))
                            {
                                if (patient.Address.Parse())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
