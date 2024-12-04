using PatientRecordSystem.Model;
using PatientRecordSystem.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PatientRecordSystem.Util
{
    /// <summary>
    /// Contains some global parameters for use when creating and viewing appointments
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Validation status enum used when logging in
        /// </summary>
        public enum ValidationStatus
        {
            Validated,
            ValidatedReset,
            InvalidCredentials,
            AccountDisabled
        }

        /// <summary>
        /// Some global parameters used to store data when creating a new appointment
        /// </summary>
        public static Patient newAppointmentPatient;
        public static User newAppointmentDoctor;
        public static DateOnly newAppointmentDate;
        public static TimeOnly newAppointmentTime;
        public static int newAppointmentSlot;

        /// <summary>
        /// Global parameter used to store the current doctor when viewing appointments
        /// </summary>
        public static User appointmentViewDoctor;
    }
}
