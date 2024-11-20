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
        public PatientRecordModal(Patient CurrentPatient)
        {

            InitializeComponent();

            Editing = true;
            currentPatient = CurrentPatient;
            ToggleEditMode();
           
            PopulateFields(CurrentPatient);
        }

        private bool populated = false;
        private bool Editing;
        private Patient currentPatient;
        private Patient editedPatient;

        private void PopulateFields (Patient patient)
        {
            Trace.WriteLine($"{currentPatient.FirstName} {currentPatient.LastName}");

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

        private void Edit_Click (object sender, RoutedEventArgs e)
        {

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

        private void Records_Click (object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                PopulateFields(editedPatient);
                ToggleEditMode();
                return;
            }
        }

        private void Validate (object sender, RoutedEventArgs e)
        {

            if (populated)
            {
                UpdatePatient();
            }

            Trace.WriteLine(currentPatient.NHSNumber);
            Trace.WriteLine(editedPatient.NHSNumber);

                if (PatientManager.GetInstance().IsPatientValid(currentPatient))
                {
                    EditButton.IsEnabled = true;
                }
                else 
                { 
                    EditButton.IsEnabled = false; 
                }
        }

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
