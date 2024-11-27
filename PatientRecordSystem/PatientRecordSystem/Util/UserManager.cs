using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using PatientRecordSystem.Model;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PatientRecordSystem.Util
{
    public sealed class UserManager
    {

        private UserManager ()
        {
        }

        // UserManager singleton property.
        private static UserManager instance;

        /// <summary>
        /// Method to retrieve the current instance of this object.
        /// If there is already an active instance of the object, then return it. Otherwise, create a new instance.
        /// </summary>
        /// <returns>Returns the instance of UserManager if it exists, otherwise returns a new instance.</returns>
        public static UserManager GetInstance ()
        {
            if (instance == null)
            {
                instance = new UserManager();
            }

            return instance;
        }

        public User currentUser { get; private set; }

        /// <summary>
        /// Deserializes the users.json file and casts it into a new List of type User
        /// </summary>
        /// <returns>Returns a new List of type User populated from the users.json file</returns>
        public List<User> Users ()
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\users.json";

            if (File.Exists(jsonPath))
            {
                string jsonString = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            else
            {
                File.Create(jsonPath).Dispose();
                File.WriteAllText(jsonPath, "[]");
                string jsonString = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }


        }

        /// <summary>
        /// Serializes a List of type User into json format and writes the result to the users.json file
        /// </summary>
        /// <param name="users">The list of type User to write to the json file.</param>
        public void UpdateData(List<User> users)
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\users.json";
            string jsonString = JsonSerializer.Serialize(users);
            File.WriteAllText(jsonPath, jsonString);
        }

        /// <summary>
        /// Resets the User with username equal to "username" back to the default value of "Example123"
        /// Also sets the User's ResetFlag to true, and ResetRequestFlag to false.
        /// </summary>
        /// <param name="username">The username of the User to reset the password of</param>
        public void ResetPassword (string username)
        {
            string jsonPath = Environment.CurrentDirectory + @"\Data\users.json";
            List<User> users = Users();

            users.Find(u => u.Username == username).ResetFlag = true;
            users.Find(u => u.Username == username).ResetRequestFlag = false;
            users.Find(u => u.Username == username).Password = Hash("Example123");

            UpdateData(users);
        }

        /// <summary>
        /// Method which validates a new user against preset criteria
        /// </summary>
        /// <param name="user">The user which will be validated</param>
        /// <returns>Returns true if valid, false if invalid</returns>
        public bool IsUserValid(User user)
        {
            //Regular expressions for email and name validation - Checks is the email address is a valid email address, and checks that the names only contains alphabetic characters
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Regex nameRegex = new Regex(@"^[a-zA-Z]+$");

            if (emailRegex.IsMatch(user.Username))  //Checks username against the emailRegex
            {
                if (!UserManager.GetInstance().Users().Any(u => u.Username == user.Username)) //Checks if the email address does not already exist in the users.json
                {
                    if (nameRegex.IsMatch(user.FirstName))  //Checks firstName against the nameRegex
                    {
                        if (nameRegex.IsMatch(user.LastName)) //Checks lastName against the nameRegex
                        {
                            if (((int)user.AccountType) != -1)   //Checks accountType has a value assigned
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        public void AddUser(User user)
        {
            List<User> users = Users();
            users.Add(user);

            UpdateData(users);
        }

        /// <summary>
        /// Validates a user's username and password against the values stored in the database
        /// </summary>
        /// <param name="username">The username of the user to validate</param>
        /// <param name="password">The password of the user to validate</param>
        /// <returns>Returns an Globals.ValidationStatus value to give more context on the validation attempt</returns>
        public Globals.ValidationStatus ValidateUser(string username, string password)
        {
            foreach (User user in Users ())
            {
                if (user.Username.ToLower() == username.ToLower())
                {
                    if (user.Password == password)
                    {
                        if (user.Disabled)
                        {
                            return Globals.ValidationStatus.AccountDisabled;
                        }

                        Login(user);

                        if (user.ResetFlag)
                        {
                            return Globals.ValidationStatus.ValidatedReset;
                        }
                        return Globals.ValidationStatus.Validated;
                    } else
                    {
                        Logout();
                        return Globals.ValidationStatus.InvalidCredentials;
                    }
                }
            }
            return Globals.ValidationStatus.InvalidCredentials;
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

        /// <summary>
        /// Finds the user with username "username" in database, and updates the password with hashed version of
        /// "password"
        /// </summary>
        /// <param name="username">The username of the User who's password to change</param>
        /// <param name="password">The new password to change the User's current password to</param>
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