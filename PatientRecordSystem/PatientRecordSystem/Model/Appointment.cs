using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public class Appointment
    {
        public string PatientId { get; set; }
        public string Doctor { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public string AppointmentCreator { get; set; }
    }
}
