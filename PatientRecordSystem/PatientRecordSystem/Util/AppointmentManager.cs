using PatientRecordSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Appointments();
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
    }
}
