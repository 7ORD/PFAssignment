using System;
using System.Collections.Generic;
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
using PatientRecordSystem.Util;
using PatientRecordSystem.Model;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Page
    {
        public PatientsView()
        {
            InitializeComponent();
            // Sets the DataGrid DataContext.
            PatientTable.DataContext = PatientManager.GetInstance().Patients(); 

            // If the current user is of type doctor, disable patient registration functionality.
            if (UserManager.GetInstance ().currentUser.AccountType != User.UserAccountType.Doctor)
            {
                RegisterButton.IsEnabled = true;
            } else
            {
                RegisterButton.IsEnabled = false;
            }
        }
        
        /// <summary>
        /// Updates the DataGrid's DataContext with the contents of the search bar.
        /// Checks the patient's Name, hospital number, nhs number and date of birth.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_KeyUp (object sender, KeyEventArgs e)
        {
            List<Patient> refinedList = new List<Patient>();

            if (!string.IsNullOrEmpty (SearchBar.Text))
            {
                refinedList = PatientManager.GetInstance().Patients().FindAll(p => p.ParsedName.ToLower().Contains(SearchBar.Text.ToLower()) || p.HospitalNumber.ToLower().Contains (SearchBar.Text.ToLower()) || p.NHSNumber.ToLower().Contains (SearchBar.Text.ToLower()) || p.DateOfBirth.ToString().ToLower().Contains (SearchBar.Text.ToLower()));
                PatientTable.DataContext = refinedList;
            } else
            {
                PatientTable.DataContext = PatientManager.GetInstance().Patients();
            }
        }

        /// <summary>
        /// Opens a PatientRecordModal with the selected patient as a parameter
        /// </summary>
        private void PatientRecord_Click (object sender, RoutedEventArgs e)
        {
            PatientRecordModal patientRecordModal = new PatientRecordModal(PatientTable.SelectedItem as Patient);
            patientRecordModal.ShowDialog();

            if (patientRecordModal.DialogResult == true)
            {
                NavigationService.Navigate(new PatientsView());
            }
        }

        /// <summary>
        /// Open a PatientRegistrationModal dialog
        /// </summary>
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            PatientRegistrationModal patientRegistrationModal = new PatientRegistrationModal();
            patientRegistrationModal.ShowDialog();

            if (patientRegistrationModal.DialogResult == true)
            {
                NavigationService.Navigate(new PatientsView());
            }
        }
    }
}
