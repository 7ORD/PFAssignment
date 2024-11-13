using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public UserManagementView()
        {
            InitializeComponent();

            UserTable.DataContext = Instances.userManager.Users();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            User user = UserTable.SelectedItem as User;

            Instances.userManager.ResetPassword(user.Username);

            NotificationWindow notificationWindow = new NotificationWindow("Reset Password", "The user's password has been reset to\nthe default value (Example123)");
            notificationWindow.ShowDialog();

            if (notificationWindow.DialogResult == true)
            {
                NavigationService.Refresh();
            }
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserModal newUserModal = new NewUserModal();
            newUserModal.ShowDialog();

            if (newUserModal.DialogResult == true)
            {
                NavigationService.Navigate(new UserManagementView());
            }
        }
    }
}
