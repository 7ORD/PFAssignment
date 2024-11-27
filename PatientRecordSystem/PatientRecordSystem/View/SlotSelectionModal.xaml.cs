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

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Globals.newAppointmentDate = DateOnly.FromDateTime((DateTime)DatePicker.SelectedDate);
            Globals.newAppointmentTime = (SlotTable.SelectedItem as Appointment).Time;
            Globals.newAppointmentSlot = SlotTable.SelectedIndex;

            this.DialogResult = true;
            this.Close();
        }
        
        private void Validate (object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void UpdateTable ()
        {
            SlotTable.DataContext = Globals.newAppointmentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)DatePicker.SelectedDate));
        }
    }
}
