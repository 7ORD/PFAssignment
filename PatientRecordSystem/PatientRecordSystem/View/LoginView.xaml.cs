﻿using System;
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

        //Button click logic for the login button - Gets a ValidationStatus dependant on the username and password entered.
        private void Login_Click (object sender, RoutedEventArgs e)
        {
            UserManager.ValidationStatus status = UserManager.ValidateUser(username.Text.ToString (),  UserManager.Hash(password.Password));
            ValidationInformation(status);

            switch (status)
            {
                case UserManager.ValidationStatus.Validated:
                    NavigationService.Navigate(new DashboardView());
                    break;
                case UserManager.ValidationStatus.ValidatedReset:
                    NavigationService.Navigate(new PasswordResetView());
                    break;
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