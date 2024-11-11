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
    public class UserManager
    {

        public User currentUser { get; private set; }



        private string UserJson()
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            return File.ReadAllText(jsonPath);
        }

        public List<User> Users ()
        {
            return JsonSerializer.Deserialize<List<User>>(UserJson());
        }

        public void ResetPassword (string username)
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            List<User> users = Users();

            users.Find(u => u.Username == username).ResetFlag = true;
            users.Find(u => u.Username == username).Password = Hash("Example123");

            string jsonString = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonPath, jsonString);

        }

        public void AddUser(User user)
        {

            List<User> users = Users();
            users.Add(user);

            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonPath, jsonString);
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

        //Returns a hashed password from string 'password' - Encryption is achieved using the MD5 hashing algorithm.
        //All characters are then made into lower case, and hyphens (-) removed
        public string Hash(string password)
        {
            byte[] passBytes = new UTF8Encoding().GetBytes(password.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passBytes);
            string encoded = BitConverter.ToString(hash).ToLower().Replace("-", "");

            return encoded;
        }

        public void UpdatePassword (string username, string password)
        {
            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            List<User> users = Users();

            users.Find(u => u.Username == username).Password = Hash(password);

            string newJsonString = JsonSerializer.Serialize(users);
            File.WriteAllText (jsonPath, newJsonString);
        }


        // Sets the currentUser to a value of user.
        private void Login(User user)
        {
            currentUser = user;
        }

        // Resets the currentUser to a null value.
        public void Logout ()
        {
            currentUser = null;
        }
    }
}
