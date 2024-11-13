using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using PatientRecordSystem.Model;
using System.Security.Cryptography;

namespace PatientRecordSystem.Util
{
    public class UserManager
    {
        public User currentUser { get; private set; }

        public List<User> Users ()
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonText = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<User>>(jsonText);
        }

        public void UpdateData(List<User> users)
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonPath, jsonString);
        }

        public void ResetPassword (string username)
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            List<User> users = Users();

            users.Find(u => u.Username == username).ResetFlag = true;
            users.Find(u => u.Username == username).ResetRequestFlag = false;
            users.Find(u => u.Username == username).Password = Hash("Example123");

            UpdateData(users);
        }



        public void AddUser(User user)
        {
            List<User> users = Users();
            users.Add(user);

            UpdateData(users);
        }

        public Instances.ValidationStatus ValidateUser(string username, string password)
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
                            return Instances.ValidationStatus.ValidatedReset;
                        }
                        return Instances.ValidationStatus.Validated;
                    } else
                    {
                        Logout();
                        return Instances.ValidationStatus.InvalidCredentials;
                    }
                }
            }
            return Instances.ValidationStatus.InvalidCredentials;
        }

        /// <summary>
        /// Encryption function for hashing a password.
        /// </summary>
        /// <param name="password">Password input to be hashed</param>
        /// <returns>
        /// Returns a hashed password from string 'password' All characters are then made into lower case, and hyphens (-) removed
        /// </returns>
        public static string Hash(string password)
        {
            byte[] passBytes = new UTF8Encoding().GetBytes(password.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passBytes);
            string encoded = BitConverter.ToString(hash).ToLower().Replace("-", "");

            return encoded;
        }

        public void UpdatePassword (string username, string password)
        {
            List<User> users = Users();

            users.Find(u => u.Username == username).Password = Hash(password);
            users.Find(u => u.Username == username).ResetFlag = false;

            UpdateData(users);
        }

        /// <summary>
        /// Sets the currentUser to a value of user.
        /// </summary>
        /// <param name="user">User to log in as</param>
        private void Login(User user)
        {
            currentUser = user;
        }

        /// <summary>
        /// Resets the currentUser to a null value.
        /// </summary>
        public void Logout ()
        {
            currentUser = new User();
        }
    }
}