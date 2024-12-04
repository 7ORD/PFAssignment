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
    /// Interaction logic for SlotSelectionModal.xaml
    /// </summary>
    public partial class SlotSelectionModal : Window
    {
        public SlotSelectionModal()
        {
            InitializeComponent();
            DatePicker.SelectedDate = DateTime.Today;
            UpdateTable();
        }

        /// <summary>
        /// Sets the global newAppointment parameters to the selected time, slot and date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Globals.newAppointmentDate = DateOnly.FromDateTime((DateTime)DatePicker.SelectedDate);
            Globals.newAppointmentTime = (SlotTable.SelectedItem as Appointment).Time;
            Globals.newAppointmentSlot = SlotTable.SelectedIndex;

            this.DialogResult = true;
            this.Close();
        }
        
        /// <summary>
        /// Validates data in the form
        /// </summary>
        private void Validate (object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        /// <summary>
        /// Updates the table with the doctor's appointments on the selected date.
        /// </summary>
        private void UpdateTable ()
        {
            SlotTable.DataContext = Globals.newAppointmentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)DatePicker.SelectedDate));
        }
    }
}
