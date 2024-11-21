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
            PatientTable.DataContext = PatientManager.GetInstance().Patients();
        }
        
        private void SearchBar_KeyUp (object sender, KeyEventArgs e)
        {
            // TODO - Search bar functionality
        }

        private void PatientRecord_Click (object sender, RoutedEventArgs e)
        {
            PatientRecordModal patientRecordModal = new PatientRecordModal(PatientTable.SelectedItem as Patient);
            patientRecordModal.ShowDialog();

            if (patientRecordModal.DialogResult == true)
            {
                NavigationService.Navigate(new PatientsView());
            }
        }

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
