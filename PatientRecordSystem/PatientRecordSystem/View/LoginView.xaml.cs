using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
using Microsoft.VisualBasic;
using PatientRecordSystem.Util;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {

        public LoginView()
        {
            InitializeComponent();
        }

        //Returns a hashed password from string 'password' - Encryption is achieved using the MD5 hashing algorithm.
        //All characters are then made into lower case, and hyphens (-) removed
        private string hash (string password)
        {
            byte[] passBytes = new UTF8Encoding().GetBytes(password.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passBytes);
            string encoded = BitConverter.ToString(hash).ToLower ().Replace ("-", "");

            return encoded;
        }

        //Button click logic for the login button - Gets a ValidationStatus dependant on the username and password entered.
        private void Login_Click (object sender, RoutedEventArgs e)
        {
            UserManager.ValidationStatus status = UserManager.ValidateUser(username.Text.ToString (),  hash(password.Password));
            ValidationInformation(status);

            if (status == UserManager.ValidationStatus.Validated)
            {
                NavigationService.Navigate(new DashboardView());
            }
        }

        private void ForgotPassword_Click (object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ForgotPasswordView());
        }

        private void PasswordBoxKeyDown (object sender, KeyEventArgs e)
        {

            ToolTip tt = new ToolTip();
            tt.Content = "Caps lock is enabled";
            tt.PlacementTarget = sender as UIElement;
            tt.Placement = PlacementMode.Bottom;

            if ((Keyboard.GetKeyStates (Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
            {
                if (password.ToolTip == null)
                {
                    password.ToolTip = tt;
                    tt.IsOpen = true;
                } 
            } else
            {
                ToolTip currentToolTip = password.ToolTip as ToolTip;
                if (currentToolTip != null)
                {
                    currentToolTip.IsOpen = false;
                }
                password.ToolTip = null;
            }
        }

        //This method sets the "instruction" TextBlock's Text field to provide the user with some validation feedback.
        private void ValidationInformation(UserManager.ValidationStatus status)
        {
            switch (status)
            {
                case UserManager.ValidationStatus.InvalidCredentials:
                    instruction.Text = "Invalid username or password";
                    instruction.Foreground = Brushes.Red;
                    break;
                case UserManager.ValidationStatus.Validated:
                    instruction.Text = "Validation success";
                    instruction.Foreground = Brushes.Green;
                    break;
            }
        }
    }
}