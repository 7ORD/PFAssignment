using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
using PatientRecordSystem.Util;

namespace PatientRecordSystem.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {

        public LoginView()
        {
            InitializeComponent();
        }

        //Returns a hashed password from parameter 'password' - Encryption is achieved using the MD5 hashing algorithm.
        private string encodedPassword (string password)
        {
            byte[] passBytes = new UTF8Encoding().GetBytes(password.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passBytes);
            string encoded = BitConverter.ToString(hash);
            encoded = encoded.ToLower();
            encoded = encoded.Replace("-", "");

            return encoded;
        }

        //Button click logic for the login button - Gets a ValidationStatus dependant on the username and password entered.
        private void Login_Click (object sender, RoutedEventArgs e)
        {
            UserManager.ValidationStatus status = UserManager.ValidateUser(username.Text.ToString (),  encodedPassword (password.Password));

            test.Text = status.ToString();
        }
    }
}