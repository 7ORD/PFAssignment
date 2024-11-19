using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\patients.json";
            string jsonString = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<Patient>>(jsonString);
        }
    }
}
