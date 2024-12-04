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
    /// Interaction logic for PatientRecordModal.xaml
    /// </summary>
    public partial class PatientRecordModal : Window
    {

        private bool populated = false;
        private bool Editing;
        private Patient currentPatient;
        private Patient editedPatient;

        public PatientRecordModal(Patient CurrentPatient)
        {
            InitializeComponent();

            Editing = true;
            currentPatient = CurrentPatient;
            ToggleEditMode();
           
            PopulateFields(CurrentPatient);
        }



        /// <summary>
        /// Populates all fields with data from Patient patient.
        /// </summary>
        /// <param name="patient"></param>
        private void PopulateFields (Patient patient)
        {
            Title.Text = $"Patient Record - {patient.HospitalNumber}";
            FirstName.Text = patient.FirstName;
            LastName.Text = patient.LastName;
            AddressFirstLine.Text = patient.Address.FirstLine;
            AddressSecondLine.Text = patient.Address.SecondLine;
            AddressCity.Text =  patient.Address.Town;
            AddressPostcode.Text = patient.Address.PostCode;
            DOB.SelectedDate = patient.DateOfBirth.ToDateTime(new TimeOnly (0));
            ContactNumber.Text = patient.ContactNumber;
            NHSNumber.Text = patient.NHSNumber;

            EditButton.IsEnabled = true;
            populated = true;

            Trace.WriteLine(currentPatient.LastName);
        }

        /// <summary>
        /// Updates currentPatient with data from the form.
        /// </summary>
        private void UpdatePatient ()
        {
            currentPatient.FirstName = FirstName.Text;
            currentPatient.LastName = LastName.Text;
            currentPatient.Address.FirstLine = AddressFirstLine.Text;
            currentPatient.Address.SecondLine = AddressSecondLine.Text;
            currentPatient.Address.Town = AddressCity.Text;
            currentPatient.Address.PostCode = AddressPostcode.Text;
            currentPatient.ContactNumber = ContactNumber.Text;
            currentPatient.DateOfBirth = DateOnly.FromDateTime((DateTime) DOB.SelectedDate);
            currentPatient.NHSNumber = NHSNumber.Text;
        }

        /// <summary>
        /// Applies all edits and opens a notification informing the user of this action
        /// </summary>
        private void Edit_Click (object sender, RoutedEventArgs e)
        {
            // If not editing, start editing.
            if (!Editing)
            {
                ToggleEditMode();
                return;
            }

            List<Patient> patientList = PatientManager.GetInstance().Patients();

            UpdatePatient();

            patientList[PatientManager.GetInstance().Patients().FindIndex(p => p.Id == currentPatient.Id)] = currentPatient;


            PatientManager.GetInstance().UpdateData(patientList);

            NotificationWindow notificationWindow = new NotificationWindow("Patient Edited", "The patient's information has been updated");
            notificationWindow.ShowDialog();

            if (notificationWindow.DialogResult == true)
            {
                this.DialogResult = true;
                Close();
            }
        }

        /// <summary>
        /// If editing, toggle edit mode, otherwise, open a PatientArchiveModal dialog for the selected patient.
        /// </summary>
        private void Records_Click (object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                PopulateFields(editedPatient);
                ToggleEditMode();
            } else
            {
                PatientArchiveModal patientArchiveModal = new PatientArchiveModal(currentPatient);
                patientArchiveModal.ShowDialog();

                if (patientArchiveModal.DialogResult == true)
                {
                    PopulateFields(currentPatient);
                }
            }
        }

        /// <summary>
        /// Validates all data entered into the form
        /// </summary>
        private void Validate (object sender, RoutedEventArgs e)
        {
            if (populated)
            {
                UpdatePatient();
            }

            if (PatientManager.GetInstance().IsPatientValid(currentPatient))
            {
                EditButton.IsEnabled = true;
            }
            else 
            { 
                EditButton.IsEnabled = false; 
            }
        }

        /// <summary>
        /// Toggles edit mode, and updates button labels depending on whether Edit is true or false.
        /// </summary>
        private void ToggleEditMode ()
        {
            Editing = !Editing;

            FirstName.IsEnabled = Editing;
            LastName.IsEnabled = Editing;
            AddressBox.IsEnabled = Editing;
            ContactNumber.IsEnabled = Editing;
            DOB.IsEnabled = Editing;
            NHSNumber.IsEnabled = Editing;

            if (Editing)
            {
                editedPatient = currentPatient;
                EditButton.Content = "Save Edits";
                RecordsButton.Content = "Discard Edits";
            } else
            {
                editedPatient = new Patient();
                EditButton.Content = "Edit Patient";
                RecordsButton.Content = "View Records";
            }
        }
    }
}
