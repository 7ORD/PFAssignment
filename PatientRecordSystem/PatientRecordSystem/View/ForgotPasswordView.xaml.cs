using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using PatientRecordSystem.Model;
using PatientRecordSystem.Util;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : Page
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the 'ResetRequestFlag' to true if the username entered is found in the database, and updates users.json
        /// </summary>
        private void Request_Click (object sender, RoutedEventArgs e)
        {
            List<User> users = UserManager.GetInstance().Users();

            User user = users.Find(u => u.Username == UsernameInput.Text.ToLower ());

            if (user != null)
            {
                user.ResetRequestFlag = true;
                UserManager.GetInstance().UpdateData(users);
            }

            // Opens a notification informing the user that their request has been sent.
            NotificationWindow notificationWindow = new NotificationWindow("Forgot Password", "If the entered user exists, a password\nreset request will be sent to the system admin.");

            notificationWindow.ShowDialog();

            // Go back to the login screen when dialog has closed.
            if (notificationWindow.DialogResult == true)
            {
                NavigationService.GoBack();
            }
            
        }

        // Go back to the login screen.
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}