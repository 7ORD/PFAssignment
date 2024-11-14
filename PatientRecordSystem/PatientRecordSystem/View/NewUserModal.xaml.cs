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

        public NewUserModal()
        {
            InitializeComponent();

            CreateButton.IsEnabled = false;
        }


        /// <summary>
        /// Event for create button click - Creates the new user in the database
        /// </summary>
        private void CreateAccount_Clicked (object sender, RoutedEventArgs e)
        {
            // Creates a new user
            Instances.userManager.AddUser(newUser);
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Event for validation - Is used upon TextBox value change and ComboBox value change
        /// Validates the entered credentials, and assigns the values to newUser if they are valid.
        /// </summary>
        private void Validate (object sender, RoutedEventArgs e)
        {
            User user = new User();

            // Populates the user's details to be validated
            user.Username = EmailAddress.Text;
            user.FirstName = FirstName.Text;
            user.LastName = LastName.Text;
            user.AccountType = (User.UserAccountType)AccountType.SelectedIndex;
            user.Password = UserManager.Hash("Example123");
            user.ResetFlag = true;

            // If the user is valid, enable the Create User button, else keep it disabled
            if (Instances.userManager.IsUserValid (user))
            {
                CreateButton.IsEnabled = true;
                newUser = user;
            } else
            {
                CreateButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Event for close button click - Event closes modal window
        /// </summary>
        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
