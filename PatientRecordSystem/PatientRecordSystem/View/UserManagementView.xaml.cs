using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using PatientRecordSystem.Model;
using PatientRecordSystem.Util;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : Page
    {
        private bool disabledView = false;

        public UserManagementView()
        {
            InitializeComponent();

            // Sets the DataGrid's DataContext to show all users who's accounts are not disabled.
            UserTable.DataContext = UserManager.GetInstance().Users().Where(u => !u.Disabled);
        }
        /// <summary>
        /// Opens an EditUserModal dialog window with the selected user as a parameter.
        /// </summary>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            EditUserModal editUserModal = new EditUserModal(UserTable.SelectedItem as User);
            editUserModal.ShowDialog();

            if (editUserModal.DialogResult == true)
            {
                NavigationService.Navigate (new UserManagementView ());
            }
        }

        /// <summary>
        /// Reset the selected user's password and open a new notification window to inform the user.
        /// </summary>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            User user = UserTable.SelectedItem as User;

            UserManager.GetInstance().ResetPassword(user.Username);

            NotificationWindow notificationWindow = new NotificationWindow("Reset Password", "The user's password has been reset to\nthe default value (Example123)");
            notificationWindow.ShowDialog();

            if (notificationWindow.DialogResult == true)
            {
                NavigationService.Navigate(new UserManagementView());
            }
        }

        /// <summary>
        /// Opens a new NewUserModal window.
        /// </summary>
        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserModal newUserModal = new NewUserModal();
            newUserModal.ShowDialog();

            if (newUserModal.DialogResult == true)
            {
                NavigationService.Navigate(new UserManagementView());
            }
        }

        /// <summary>
        /// Toggles views between disabled accounts, and enabled accounts.
        /// </summary>
        private void ShowDisabled_Click (object sender, RoutedEventArgs e)
        {
            if (disabledView)
            {
                disabledView = false;
                UserTable.DataContext = UserManager.GetInstance().Users().Where(u => !u.Disabled);
                ShowDisabledButton.Content = "Show Disabled Users";
            } else
            {
                disabledView = true;
                UserTable.DataContext = UserManager.GetInstance().Users().Where(u => u.Disabled);
                ShowDisabledButton.Content = "Show Active Users";
            }

            // Refresh the page
            NavigationService.Navigate(this);
        }

        /// <summary>
        /// Toggles the selected user's Disabled field and updates the users.json file. Shows a notification informing the user of this, then refreshes the page.
        /// </summary>
        private void Enable_Click (object sender, RoutedEventArgs e)
        {
            List<User> users = UserManager.GetInstance().Users();
            User user = UserTable.SelectedItem as User;

            user.Disabled = !user.Disabled;

            //Update users list with the newly disabled user
            users[users.FindIndex(u => u.Username == user.Username)] = user;

            //Update the users json file
            UserManager.GetInstance().UpdateData(users);

            NotificationWindow notificationWindow = new NotificationWindow("Enabled", "The user account has been re enabled");
            notificationWindow.ShowDialog();

            if (notificationWindow.DialogResult == true)
            {
                disabledView = false;
                NavigationService.Navigate(new UserManagementView ());
            }
        }

        /// <summary>
        /// Deletes the selected user from the users.json file, shows a notifcation informing the user of this, then refreshes the page.
        /// </summary>

        private void Delete_Click (object sender, RoutedEventArgs e)
        {

            List<User> users = UserManager.GetInstance().Users();

            users.RemoveAt (users.FindIndex (u => u.Username == (UserTable.CurrentItem as User).Username));

            UserManager.GetInstance().UpdateData(users);

            NotificationWindow notificationWindow = new NotificationWindow("Deleted", "The user account has been deleted");
            notificationWindow.ShowDialog();

            if (notificationWindow.DialogResult == true)
            {
                disabledView = false;
                NavigationService.Navigate(new UserManagementView());
            }
        }
    }
}
