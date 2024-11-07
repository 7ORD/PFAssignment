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

        public void Reset_Click (object sender, RoutedEventArgs e)
        {
            if (password.Password == confirmPassword.Password)
            {

                if (password.Password.Length >= 8)
                {
                    if (password.Password.Any(char.IsDigit) && password.Password.Any(char.IsUpper))
                    {
                        UserManager.UpdatePassword(UserManager.currentUser.Username, UserManager.Hash(password.Password));
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

        public void Logout_Click (object sender, RoutedEventArgs e)
        {
            UserManager.Logout();
            NavigationService.Navigate(new LoginView());
        }

        private void PasswordBoxKeyDown(object sender, KeyEventArgs e)
        {

            ToolTip tt = new ToolTip();
            tt.Content = "Caps lock is enabled";
            tt.PlacementTarget = sender as UIElement;
            tt.Placement = PlacementMode.Bottom;

            if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
            {
                if (password.ToolTip == null)
                {
                    password.ToolTip = tt;
                    tt.IsOpen = true;
                }
            }
            else
            {
                ToolTip currentToolTip = password.ToolTip as ToolTip;
                if (currentToolTip != null)
                {
                    currentToolTip.IsOpen = false;
                }
                password.ToolTip = null;
            }
        }

    }
}
