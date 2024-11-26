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
    public sealed class AppointmentManager
    {
        private static AppointmentManager instance;

        private AppointmentManager ()
        {
        }

        public static AppointmentManager GetInstance ()
        {
            if (instance == null)
            {
                instance = new AppointmentManager ();
            }

            return instance;
        }

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

        public void UpdateData (List<Appointment> appointments)
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\Appointments.json";
            string jsonString = JsonSerializer.Serialize(appointments);
            File.WriteAllText(jsonPath, jsonString);
        }

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
