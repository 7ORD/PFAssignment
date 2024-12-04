using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for EditUserModal.xaml
    /// </summary>
    public partial class EditUserModal : Window
    {

        private User editingUser;

        public EditUserModal(User user)
        {
            InitializeComponent();
            editingUser = user;

            PrepopulateFields();
        }

        /// <summary>
        /// Populates all of the fields in the form with the values of the user you're editing
        /// </summary>
        public void PrepopulateFields ()
        {
            FirstName.Text = editingUser.FirstName;
            LastName.Text = editingUser.LastName;
            EmailAddress.Text = editingUser.Username;
            AccountType.SelectedIndex = ((int)editingUser.AccountType);

            DisableButton.Content = "Disable User";
        }

        /// <summary>
        /// Event for validation - Is used upon TextBox value change and ComboBox value change
        /// Validates the entered credentials, and assigns the values to newUser if they are valid.
        private void Validate(object sender, RoutedEventArgs e)
        {
            User user = new User();

            // Workaround for UserManager.Validation email check - the check in the UserManager.Validation method will make sure that the email address doesn't
            // already exist, but since we're editing a user and not creating a new one, the check will always return invalid if we don't edit the user's email
            // which sometimes we don't want to do.
            // As a workaround, if the email input box's text equals the email address of the user we're editing, we simply add "EDIT" as a prefix to the email
            // address we want to validate, this prefix isn't added to the json file when the edits are saved.
            if (EmailAddress.Text == editingUser.Username)
            {
                user.Username = "EDIT" + EmailAddress.Text;
            } else
            {
                user.Username = EmailAddress.Text;
            }
            
            // Set the rest of the fields for the user we're going to validate
            user.FirstName = FirstName.Text;
            user.LastName = LastName.Text;
            user.AccountType = (User.UserAccountType)AccountType.SelectedIndex;

            // If the user is valid, enabled the edit button, else disable it.
            if (UserManager.GetInstance().IsUserValid(user))
            {
                EditButton.IsEnabled = true;
            }
            else
            {
                EditButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Event for the cancel button - Closes the window.
        /// </summary>
        private void Cancel_Click (object sender, RoutedEventArgs e)
        {
            // Close the window.
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Event for the edit button - Applies all edits to the json file.
        /// </summary>
        private void Edit_Click (object sender, RoutedEventArgs e)
        {
            // Get the current list of users.
            List<User> users = UserManager.GetInstance().Users();
            // Create a new user equal to the user we're editing
            User updatedUser = editingUser;

            // Update the FirstName, LastName, UserName, and AccountType of the editingUser with values entered in the EditUser form
            updatedUser.FirstName = FirstName.Text;
            updatedUser.LastName = LastName.Text;
            updatedUser.Username = EmailAddress.Text;
            updatedUser.AccountType = (User.UserAccountType)AccountType.SelectedIndex;

            // Check if the password reset checkbox is checked, and update the ResetFlag on the User
            if (PasswordResetCheckBox.IsChecked == true)
            {
                updatedUser.ResetFlag = true;
            }

            // Update the User in the users list to equal the edited user.
            users[users.FindIndex(u => u.Username.ToLower () == editingUser.Username.ToLower ())] = updatedUser;

            // Apply the edits to the json file and close this window.
            DialogResult = true;
            UserManager.GetInstance().UpdateData(users);
            Close();
        }

        /// <summary>
        /// Event for the Disable button - Disables a user account and shows a notification window informing the user of this.
        /// </summary>
        private void Disable_Click (object sender, RoutedEventArgs e)
        {

            if (editingUser.Username != UserManager.GetInstance ().currentUser.Username)
            {
                List<User> users = UserManager.GetInstance().Users();

                User user = users.Find(u => u.Username == editingUser.Username);

                user.Disabled = !user.Disabled;

                // Update users list with the newly disabled user
                users[users.FindIndex(u => u.Username == user.Username)] = user;

                // Update the users json file
                UserManager.GetInstance().UpdateData(users);

                // Create a new NotificationWindow and show this as a Dialog.
                NotificationWindow notificationWindow = new NotificationWindow("Disabled", "The user account has been disabled");
                notificationWindow.ShowDialog();

                // When the NotificationWindow has been closed, close the EditUserModal window.
                if (notificationWindow.DialogResult == true)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            } else
            {
                // Create a new notification window to show that it's not possible to disable your own account.
                NotificationWindow notificationWindow = new NotificationWindow("Error", "You cannot disable your own account.");
                notificationWindow.ShowDialog();

            }
        }
    }
}
