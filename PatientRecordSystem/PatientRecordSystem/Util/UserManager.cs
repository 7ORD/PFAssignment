﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using PatientRecordSystem.Objects;

namespace PatientRecordSystem.Util
{
    public static class UserManager
    {
        public enum ValidationStatus
        {
            Validated,
            InvalidPassword,
            InvalidUsername,
            Null
        }


        public static ValidationStatus ValidateUser(string username, string password)
        {

            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = File.ReadAllText(jsonPath);

            User[] users = JsonSerializer.Deserialize<User[]>(jsonString);

            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Username.ToLower () == username.ToLower ())
                {
                    if (users[i].Password == password)
                    {
                        return ValidationStatus.Validated;
                    } else
                    {
                        return ValidationStatus.InvalidPassword;
                    }
                } else
                {
                    return ValidationStatus.InvalidUsername;
                }
            }
            return ValidationStatus.Null;
        }
    }
}