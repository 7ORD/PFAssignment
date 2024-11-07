using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using PatientRecordSystem.Model;
using System.Security.Cryptography;

namespace PatientRecordSystem.Util
{
    public static class UserManager
    {

        public static User currentUser { get; private set; }

        public enum ValidationStatus
        {
            Validated,
            ValidatedReset,
            InvalidCredentials
        }

        private static string UserJson()
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            return File.ReadAllText(jsonPath);
        }

        public static List<User> Users ()
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<User>>(jsonString);
        }

        public static void AddUser(User user)
        {

            List<User> users = Users();
            users.Add(user);

            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonPath, jsonString);
        }

        public static ValidationStatus ValidateUser(string username, string password)
        {

            foreach (User user in Users ())
            {

                if (user.Username.ToLower() == username.ToLower())
                {
                    if (user.Password == password)
                    {
                        Login(user);

                        if (user.ResetFlag)
                        {
                            return ValidationStatus.ValidatedReset;
                        }

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

        //Returns a hashed password from string 'password' - Encryption is achieved using the MD5 hashing algorithm.
        //All characters are then made into lower case, and hyphens (-) removed
        public static string Hash(string password)
        {
            byte[] passBytes = new UTF8Encoding().GetBytes(password.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passBytes);
            string encoded = BitConverter.ToString(hash).ToLower().Replace("-", "");

            return encoded;
        }

        public static void UpdatePassword (string username, string password)
        {

            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = File.ReadAllText(jsonPath);
            User[] users = JsonSerializer.Deserialize<User[]>(jsonString);

            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Username == username)
                {
                    users[i].Password = password;
                    users[i].ResetFlag = false;
                    break;
                }
            }

            string newJsonString = JsonSerializer.Serialize(users);
            File.WriteAllText (jsonPath, newJsonString);
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
