using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for NewUserModal.xaml
    /// </summary>
    public partial class NewUserModal : Window
    {
        public User newUser;

        //Enum used for form input validation
        private enum NewUserValidation
        {
            Valid,
            InvalidEmail,
            InvalidFirstName,
            InvalidLastName,
            InvalidAccountType,
            EmailExists
        }

        public NewUserModal()
        {
            InitializeComponent();

            CreateButton.IsEnabled = false;
        }


        //Event for create button click
        private void CreateAccount_Clicked (object sender, RoutedEventArgs e)
        {
            Instances.userManager.AddUser(newUser);
            this.Close();
        }

        //Event for validation - Is used upon TextBox value change and ComboBox value change
        private void Validate (object sender, RoutedEventArgs e)
        {

            User user = new User();

            user.Username = EmailAddress.Text;
            user.FirstName = FirstName.Text;
            user.LastName = LastName.Text;
            user.AccountType = (User.UserAccountType)AccountType.SelectedIndex;
            user.Password = Instances.userManager.Hash("Example123");
            user.ResetFlag = true;

            switch (ValidateUser(user.Username, user.FirstName, user.LastName, user.AccountType))
            {
                case NewUserValidation.Valid:
                    newUser = user;
                    CreateButton.IsEnabled = true;
                    break;
                case NewUserValidation.InvalidEmail:
                    CreateButton.IsEnabled = false;
                    break;
                case NewUserValidation.InvalidFirstName:
                    CreateButton.IsEnabled = false;
                    break;
                case NewUserValidation.InvalidLastName:
                    CreateButton.IsEnabled = false;
                    break;
                case NewUserValidation.InvalidAccountType:
                    CreateButton.IsEnabled = false;
                    break;
                case NewUserValidation.EmailExists:
                    CreateButton.IsEnabled = false;
                    break;
            }
        }

        //Event for close button click
        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            //Closes modal window
            this.Close();
        }

        //Returns a NewUserValidation enum value - takes in the form inputs and validates them.
        private NewUserValidation ValidateUser (string username, string firstName, string lastName, User.UserAccountType accountType)
        {

            //Regular expressions for email and name validation - Checks is the email address is a valid email address, and checks that the names only contains alphabetic characters
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Regex nameRegex = new Regex(@"^[a-zA-Z]+$");

            if (emailRegex.IsMatch (username))  //Checks username against the emailRegex
            {
                if (!Instances.userManager.Users().Any (u => u.Username == username)) //Checks if the email address does not already exists in the users.json
                {
                    if (nameRegex.IsMatch (firstName))  //Checks firstName against the nameRegex
                    {
                        if (nameRegex.IsMatch(lastName)) //Checks lastName against the nameRegex
                        {
                            if (((int)accountType) != -1)   //Checks accountType has a value assigned
                            {
                                return NewUserValidation.Valid;
                            } else
                            {
                                return NewUserValidation.InvalidAccountType;
                            }
                        } else
                        {
                            return NewUserValidation.InvalidLastName;
                        }
                    } else
                    {
                        return NewUserValidation.InvalidFirstName;
                    }
                } else
                {
                    return NewUserValidation.EmailExists;
                }
            } else
            {
                return NewUserValidation.InvalidEmail;
            } 
        }
    }
}
