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

        private void Request_Click (object sender, RoutedEventArgs e)
        {
            List<User> users = Instances.userManager.Users();

            User user = users.Find(u => u.Username == UsernameInput.Text);

            if (user != null)
            {
                user.ResetRequestFlag = true;
                Instances.userManager.UpdateData(users);


                NotificationWindow notification = new NotificationWindow("Forgot Password", "If the user exists, a password reset request\nwill be send to the admin.", new LoginView ());
                notification.ShowDialog();

                notification.Closed += (s, e) =>
                {
                    NavigationService.Navigate(new LoginView());
                };
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}