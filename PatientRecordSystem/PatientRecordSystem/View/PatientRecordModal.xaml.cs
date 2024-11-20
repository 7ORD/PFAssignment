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
            currentPatient = CurrentPatient;

            Trace.WriteLine(CurrentPatient.FirstName);
            PopulateFields();
            ToggleEnable(false);
        }


        private Patient currentPatient;
        public bool Editing = true;


        private void PopulateFields ()
        {
            Title.Text = $"Patient Record - {currentPatient.HospitalNumber}";
            FirstName.Text = currentPatient.FirstName;
            LastName.Text = currentPatient.LastName;
            AddressFirstLine.Text = currentPatient.Address.FirstLine;
            AddressSecondLine.Text = currentPatient.Address.SecondLine;
            AddressCity.Text = currentPatient.Address.Town;
            AddressPostcode.Text = currentPatient.Address.PostCode;
            DOB.Text = currentPatient.DateOfBirth.ToString ();
            ContactNumber.Text = currentPatient.ContactNumber;
            NHSNumber.Text = currentPatient.NHSNumber;
        }

        private void Edit_Click (object sender, RoutedEventArgs e)
        {
            ToggleEnable(!Editing);
        }

        private void ToggleEnable(bool state)
        {
            Editing = state;

            FirstName.IsEnabled = Editing;
            LastName.IsEnabled = Editing;
            AddressFirstLine.IsEnabled = Editing;
            AddressSecondLine.IsEnabled = Editing;
            AddressCity.IsEnabled = Editing;
            AddressPostcode.IsEnabled = Editing;
            AddressPostcode.IsEnabled = Editing;
            DOB.IsEnabled = Editing;
            EditDoBButton.IsEnabled = Editing;
            ContactNumber.IsEnabled = Editing;
            NHSNumber.IsEnabled = Editing;
        }
    }
}
