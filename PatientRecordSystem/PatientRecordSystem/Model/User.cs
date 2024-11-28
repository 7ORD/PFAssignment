using PatientRecordSystem.Util;
using PatientRecordSystem.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    [Serializable]
    public class User
    {
        public enum UserAccountType
        {
            Nurse,
            Doctor,
            Admin,
            Null
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserAccountType AccountType { get; set; }
        public List<int> Appointments { get; set; }
        public bool ResetFlag { get; set; }
        public bool ResetRequestFlag { get; set; }
        public bool Disabled { get; set; }

        [JsonIgnore]
        public string ParsedName
        {
            get => new string($"{FirstName} {LastName}");
            private set { }
        } 

        public List<Appointment> AppointmentsToday (DateOnly date)
        {
            List<Appointment> allAppointments = new List<Appointment>();
            List<Appointment> todaysAppointments = new List<Appointment>();


            for (int i = 0; i < Appointments.Count; i++)
            {
                allAppointments.Add(AppointmentManager.GetInstance().Appointments().Where(a => a.AppointmentId == Appointments[i]).FirstOrDefault());
            }

            for (int i = 0; i < 16; i++)
            {
                todaysAppointments.Add(new Appointment());
                todaysAppointments[i].Slot = i;

                for (int a = 0; a < allAppointments.Count; a++)
                {
                    if (allAppointments[a].Date == date)
                    {
                        if (allAppointments[a].Slot == i)
                        {
                            todaysAppointments[i] = allAppointments[a];
                        }
                    }
                }
            }

            return todaysAppointments;
        }
    }
}