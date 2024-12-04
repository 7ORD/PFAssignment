using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PasswordResetView.xaml
    /// </summary>
    public partial class PasswordResetView : Page
    {
        public PasswordResetView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if the passwords entered match eachother, and checks the password contains at least one digit, one upper case
        /// character and is at least 8 characters long.
        /// </summary>
        public void Reset_Click (object sender, RoutedEventArgs e)
        {
            if (password.Password == confirmPassword.Password)
            {

                if (password.Password.Length >= 8)
                {
                    if (password.Password.Any(char.IsDigit) && password.Password.Any(char.IsUpper))
                    {
                        UserManager.GetInstance().UpdatePassword(UserManager.GetInstance().currentUser.Username, password.Password);
                        NavigationService.Navigate(new DashboardView());
                    } else
                    {
                        instruction.Text = "Error - Password must contain at least one upper case, and \n one numeric character.";
                    }
                    
                }
                else
                {
                    instruction.Text = "Error - Password must be at least 8 characters.";
                }
            } else
            {
                instruction.Text = "Error - Passwords do not match.";
            }
        }

        /// <summary>
        /// Logs out the current user, and navigates back to the login screen.
        /// </summary>
        public void Logout_Click (object sender, RoutedEventArgs e)
        {
            UserManager.GetInstance().Logout();
            NavigationService.Navigate(new LoginView());
        }
    }
}
