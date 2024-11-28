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
using System.Windows.Shapes;
using PatientRecordSystem.Model;
using PatientRecordSystem.Util;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for PatientArchiveModal.xaml
    /// </summary>
    public partial class PatientArchiveModal : Window
    { 
        private List<Appointment> appointments;

        public PatientArchiveModal(Patient patient)
        {
            InitializeComponent();

            appointments = AppointmentManager.GetInstance().Appointments().Where(a => a.PatientId == patient.HospitalNumber).ToList();

            ArchiveGrid.DataContext = appointments;

            foreach (Appointment a in appointments)
            {
                Trace.WriteLine(a.Time);
            }
        }

        private void AppointmentDetails_Click(object sender, RoutedEventArgs e)
        {
            AppointmentCreationModal appointmentCreationModal = new AppointmentCreationModal(true, ((Appointment)ArchiveGrid.SelectedItem).Date, ((Appointment)ArchiveGrid.SelectedItem).Time, ArchiveGrid.SelectedIndex, ((Appointment)ArchiveGrid.SelectedItem).Doctor, ((Appointment)ArchiveGrid.SelectedItem).PatientId, ((Appointment)ArchiveGrid.SelectedItem).BriefDescription, ((Appointment)ArchiveGrid.SelectedItem).Description);
            appointmentCreationModal.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
