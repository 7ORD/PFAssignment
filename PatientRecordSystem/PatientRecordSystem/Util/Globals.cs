using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PatientRecordSystem.Util
{
    public static class Globals
    {
        public enum ValidationStatus
        {
            Validated,
            ValidatedReset,
            InvalidCredentials,
            AccountDisabled
        }

        public enum NewUserValidation
        {
            Valid,
            Invalid
        }
    }
}
