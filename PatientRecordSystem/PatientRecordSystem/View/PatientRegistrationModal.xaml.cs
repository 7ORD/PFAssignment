using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
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
using System.Windows.Shapes;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for PatientRegistrationModal.xaml
    /// </summary>
    public partial class PatientRegistrationModal : Window
    {
        private Patient newPatient = new Patient();
        public PatientRegistrationModal()
        {
            InitializeComponent();
            RegisterButton.IsEnabled = false;
        }

        /// <summary>
        /// Event for Register button - Grabs the patient data list, adds a new patient to the end, and updates the json file.
        /// </summary>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Get the patient list from the PatientManager instance.
            List<Patient> patients = PatientManager.GetInstance().Patients();

            // Auto increment the patient ID
            newPatient.Id = patients.Count + 1;

            // Add the new patient to the list
            patients.Add(newPatient);

            // Update the patients json file.
            PatientManager.GetInstance().UpdateData(patients);

            // Show a notification window telling the user that the patient has been registered, and then close this modal dialog.
            NotificationWindow notification = new NotificationWindow ("Patient Registered", "The new patient has been registered");
            notification.ShowDialog();

            if (notification.DialogResult == true)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        /// <summary>
        /// Event for the cancel button - Closes the modal dialog.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        /// <summary>
        /// Event for validation - Checks if the form is valid, if so, the register button should be enabled.
        /// </summary>
        private void Validate(object sender, RoutedEventArgs e)
        {
            UpdateUserDetails();

            if (PatientManager.GetInstance().IsPatientValid(newPatient))
            {
                RegisterButton.IsEnabled = true;
            }
            else
            {
                RegisterButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Updates the newPatient prop with data from the form.
        /// </summary>
        private void UpdateUserDetails()
        {
            //If the DatePicker in the form hasn't been changed, just use today's date as a placeholder value
            if (DOB.SelectedDate == null)
            {
                newPatient.DateOfBirth = DateOnly.FromDateTime(DateTime.Now);
            } else
            {
                newPatient.DateOfBirth = DateOnly.FromDateTime (DOB.SelectedDate.Value);
            }

            newPatient.FirstName = FirstName.Text;
            newPatient.LastName = LastName.Text;
            newPatient.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            newPatient.ContactNumber = ContactNumber.Text;
            newPatient.NHSNumber = NHSNumber.Text;
            newPatient.Address = new Address(AddressFirstLine.Text, AddressSecondLine.Text, AddressCity.Text, AddressPostcode.Text);
            
        }
    }
}
