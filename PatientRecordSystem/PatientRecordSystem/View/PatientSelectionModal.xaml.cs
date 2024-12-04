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
    /// Interaction logic for PatientSelctionModal.xaml
    /// </summary>
    public partial class PatientSelectionModal : Window
    {
        public PatientSelectionModal()
        {
            InitializeComponent();
            // Sets the DataGrid DataContext.
            PatientTable.DataContext = PatientManager.GetInstance().Patients();
        }

        /// <summary>
        /// Sets the global parameter newAppointmentPatient to the selected Patient.
        /// </summary>
        private void Select_Click (object sender, RoutedEventArgs e)
        {
            Globals.newAppointmentPatient = PatientTable.SelectedItem as Patient;
            DialogResult = true;
            Close();
        }
    }
}
