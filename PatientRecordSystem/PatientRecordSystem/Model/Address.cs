using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PatientRecordSystem.Model
{
    /// <summary>
    /// Address data model
    /// </summary>
    public class Address
    {
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        
        /// <summary>
        /// Function which returns a bool dependant on whether the address is a valid address or not
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Constructor which takes in 4 strings and sets the object's corresponding fields to the entered strings.
        /// </summary>
        /// <param name="firstLine">First line of the address</param>
        /// <param name="secondLine">Second line of the address</param>
        /// <param name="town">Town of the address</param>
        /// <param name="postCode">Postcode of the address</param>
        public Address (string firstLine, string secondLine, string town, string postCode)
        {
            FirstLine = firstLine;
            SecondLine = secondLine;
            Town = town;
            PostCode = postCode;
        }
    }
}
