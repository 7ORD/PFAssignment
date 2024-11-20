using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    public class Address
    {
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string ParsedAddress ()
        {
            return new string($"{FirstLine}, {SecondLine}, {Town}, {PostCode}");
        }
        public bool Parse ()
        {
            // Postcode Regex - Taken from the UK Government website as a standard for parsing UK Postcodes.
            // Source: https://webarchive.nationalarchives.gov.uk/ukgwa/+/http://www.cabinetoffice.gov.uk/media/291370/bs7666-v2-0-xsd-PostCodeType.htm
            Regex postCodeRegex = new Regex("([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\\s?[0-9][A-Za-z]{2})");

            if (FirstLine.Length == 0)
            {
                return false;
            }
            if (SecondLine.Length == 0)
            {
                return false;
            }
            if (Town.Length == 0)
            {
                return false;
            }
            if (!postCodeRegex.IsMatch (PostCode))
            {
                return false;
            }

            return true;
        }
    }
}
