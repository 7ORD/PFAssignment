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
    /// <summary>
    /// Appointment data model - Contains a constructor which contains some default values.
    /// </summary>
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


        /// <summary>
        /// Returns a string used when showing an appointment schedule - If the appointment has no details
        /// assigned, then it will return "Free", otherwise, return the patient's full name and the summary
        /// of their appointment
        /// </summary>
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

        /// <summary>
        /// Converts the slot int into a TimeOnly struct. Checks if the slot is odd, if it is, the
        /// minutes are set to 30. Increases the hour by Slot/2.
        /// </summary>
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

        /// <summary>
        /// Checks if the appointment slot is populated - Used in the DoctorAppointmentView.xaml file to determine which button to show.
        /// </summary>
        [JsonIgnore]
        public bool Populated
        {
            get
            {
                if (AppointmentId != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set { }
        }

        /// <summary>
        /// Constructor which sets some default values when creating a new appointment
        /// </summary>
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
