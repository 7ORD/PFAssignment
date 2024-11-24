using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string Doctor { get; set; }
        public DateOnly Date { get; set; }
        public int Slot { get; set; }
        public string Description { get; set; }
        public string AppointmentCreator { get; set; }

        [JsonIgnore]
        public TimeOnly Time { get; set; }

        [JsonIgnore]
        public string PatientName
        {
            get
            {
                Patient patient = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == PatientId);

                if (patient == null)
                {
                    return "No appointment schedule in this time slot";
                } else
                {
                    return $"{patient.FirstName} {patient.LastName}";
                }
            }
            set {

            }
        }

        public Appointment ()
        {
            AppointmentId = -1;
            PatientId = "null";
            Doctor = "null";
            Date = new DateOnly(1, 1, 1);
            Slot = -1;
            Description = "null";
            AppointmentCreator = "null";
            Time = new TimeOnly(1, 1);
        }
    }
}
