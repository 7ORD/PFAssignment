using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public struct Address
    {
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string ParsedAddress ()
        {
            return new string($"{FirstLine}, {SecondLine}, {Town}, {PostCode}");
        }
    }
}
