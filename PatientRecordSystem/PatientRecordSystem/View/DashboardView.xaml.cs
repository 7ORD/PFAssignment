﻿using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Page
    {

        private enum CurrentTab
        {
            Patients,
            Appointments,
            UserManagement,
            Account
        }

        SolidColorBrush selectedColor;
        SolidColorBrush defaultColor;

        private CurrentTab currentTab;

        public DashboardView()
        {
            InitializeComponent();
            UserLabel.Text = "Logged in as " + UserManager.GetInstance().currentUser.FirstName + " " + UserManager.GetInstance().currentUser.LastName;

            selectedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("LightGray"));
            defaultColor = new SolidColorBrush((Color)(Color)ColorConverter.ConvertFromString("#FFDDDDDD"));

            if (UserManager.GetInstance().currentUser.AccountType != Model.User.UserAccountType.Admin)
            {
                UserManagementButton.Visibility = Visibility.Hidden;
            }

            SwitchTab(CurrentTab.Dashboard);
        }

        private void SwitchTab (CurrentTab tab)
        {
            currentTab = tab;

            switch (tab)
            {
                case CurrentTab.Patients:
                    PatientsButton.Background = selectedColor;
                    AppointmentsButton.Background = defaultColor;
                    UserManagementButton.Background = defaultColor;
                    break;
                case CurrentTab.Appointments:
                    PatientsButton.Background = defaultColor;
                    AppointmentsButton.Background = selectedColor;
                    UserManagementButton.Background = defaultColor;
                    break;
                case CurrentTab.UserManagement:
                    PatientsButton.Background = defaultColor;
                    AppointmentsButton.Background = defaultColor;
                    UserManagementButton.Background = selectedColor;
                    break;
            }
        }

        public void Logout_Click (object sender, RoutedEventArgs e)
        {
            UserManager.GetInstance().Logout();

            NavigationService.Navigate(new LoginView());
        }

        public void Patients_Click(object sender, RoutedEventArgs e)
        {
            SwitchTab(CurrentTab.Patients);
            ContentFrame.Navigate(new PatientsView());
        }
        public void Appointments_Click(object sender, RoutedEventArgs e)
        {
            SwitchTab(CurrentTab.Appointments);
            ContentFrame.Navigate(new AppointmentsView());
        }
        public void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            SwitchTab(CurrentTab.UserManagement);
            ContentFrame.Navigate(new UserManagementView());
        }

    }
}
