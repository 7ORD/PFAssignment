using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem.Util
{
    public static class Instances
    {
        public enum ValidationStatus
        {
            Validated,
            ValidatedReset,
            InvalidCredentials
        }

        public static UserManager userManager = new UserManager();
    }
}
