using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using PatientRecordSystem.Model;

namespace PatientRecordSystem.Util
{
    public static class UserManager
    {

        public static User currentUser { get; private set; }

        public enum ValidationStatus
        {
            Validated,
            InvalidCredentials
        }

        public static ValidationStatus ValidateUser(string username, string password)
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = File.ReadAllText(jsonPath);
            User[] users = JsonSerializer.Deserialize<User[]>(jsonString);

            foreach (User user in users)
            {

                Trace.WriteLine("\n" + user.Username);
                if (user.Username.ToLower() == username.ToLower())
                {
                    if (user.Password == password)
                    {
                        Login(user);
                        return ValidationStatus.Validated;
                    } else
                    {
                        Logout();
                        return ValidationStatus.InvalidCredentials;
                    }
                }
            }
            return ValidationStatus.InvalidCredentials;
        }


        // Sets the currentUser to a value of user.
        private static void Login(User user)
        {
            currentUser = user;
        }

        // Resets the currentUser to a null value.
        public static void Logout ()
        {
            currentUser = null;
        }
    }
}
