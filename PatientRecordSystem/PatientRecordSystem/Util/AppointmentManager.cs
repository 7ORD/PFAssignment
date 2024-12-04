using PatientRecordSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatientRecordSystem.Util
{
    /// <summary>
    /// AppointmentManager utility class - Handles all appointment operations
    /// </summary>
    public sealed class AppointmentManager
    {
        // AppointmentManager singleton property.
        private static AppointmentManager instance;

        private AppointmentManager ()
        {
        }

        /// <summary>
        /// Method to retrieve the current instance of this object.
        /// If there is already an active instance of the object, return it, otherwise create a new one.
        /// </summary>
        /// <returns>Returns the instance of UserManager if it exists, otherwise, returns a new instance.</returns>
        public static AppointmentManager GetInstance ()
        {
            if (instance == null)
            {
                instance = new AppointmentManager ();
            }

            return instance;
        }

        /// <summary>
        /// Deserializes the appointments.josn file and casts it into a new List of type Appointment
        /// </summary>
        /// <returns>Returns a new List of type Patient populated from the appointments.json</returns>
        public List<Appointment> Appointments()
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\appointments.json";

            if (File.Exists(jsonPath))
            {
                string jsonText = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<List<Appointment>>(jsonText);
            }
            else
            { 
                File.Create (jsonPath).Dispose ();
                File.WriteAllText(jsonPath, "[]");
                string jsonText = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<List<Appointment>>(jsonText);
            }
        }

        /// <summary>
        /// Serializes a List of type Appointment into json format and writes the result to the appointments.json file
        /// </summary>
        /// <param name="appointments"> The list of type Appointment to write to the json file</param>
        public void UpdateData (List<Appointment> appointments)
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\Appointments.json";
            string jsonString = JsonSerializer.Serialize(appointments);
            File.WriteAllText(jsonPath, jsonString);
        }

        /// <summary>
        /// Validation function for appointments
        /// </summary>
        /// <param name="appointment">The Appointment to validate</param>
        /// <returns>A boolean dependant on whether the appointment is valid</returns>
        public bool IsAppointmentValid (Appointment appointment)
        {
            if (string.IsNullOrEmpty (appointment.PatientId)) {
                return false;
            }

            if (string.IsNullOrEmpty (appointment.Doctor))
            {
                return false;
            }

            if (appointment.Date == null)
            {
                return false;
            }

            if (appointment.Slot < 0 || appointment.Slot > 15)
            {
                return false;
            }

            if (string.IsNullOrEmpty (appointment.BriefDescription))
            {
                return false;
            }
            
            if (string.IsNullOrEmpty(appointment.Description))
            {
                return false;
            }

            return true;
        }
    }
}
