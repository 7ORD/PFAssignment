using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public class Appointment
    {
        public enum AppointmentStatus
        {
            Scheduled,
            Completed,
            Cancelled
        }

        public int AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string Doctor { get; set; }
        public DateOnly Date { get; set; }
        public int Slot { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public string DoctorsNotes { get; set; }
        public string AppointmentCreator { get; set; }
        public AppointmentStatus Status { get; set; }



        [JsonIgnore]
        public string AppointmentDetails
        {
            get
            {
                Patient patient = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == PatientId);

                if (patient == null)
                {
                    return "Free";
                } else
                {
                    return $"{patient.FirstName} {patient.LastName} - {BriefDescription}";
                }
                
            }
            set { }
        }

        [JsonIgnore]
        public TimeOnly Time {
            get 
            {
                int hour = 9;
                int min = 0;

                if (Slot % 2 != 0)
                {
                    hour += (Slot / 2);
                    min = 30;
                } else
                {
                    hour += (Slot / 2);
                }

                return new TimeOnly(hour, min);
            }
            set { }
        }

        [JsonIgnore]
        public bool Populated
        {
            get
            {
                if (AppointmentId != -1)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            set { }
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
