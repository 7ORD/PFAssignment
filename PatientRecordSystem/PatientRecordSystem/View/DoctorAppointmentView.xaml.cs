using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
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

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for DoctorAppointmentView.xaml
    /// </summary>
    public partial class DoctorAppointmentView : Page
    {

        private List<User> Doctors = UserManager.GetInstance ().Users ().Where (u => u.AccountType == User.UserAccountType.Doctor).ToList ();
        private User currentDoctor;
        private List<Appointment> appointments;
        private bool Visible;

        public DoctorAppointmentView()
        {
            InitializeComponent();
            DoctorTable.DataContext = Doctors;
            currentDoctor = new User();
            UpdateSchedule();
        }

        private void Schedule_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = DoctorTable.SelectedItem as User;
            Visible = true;
            UpdateSchedule();
        }

        private void Close_Click (object sender, RoutedEventArgs e)
        {
            currentDoctor = new User();
            Visible = false;
            UpdateSchedule();
        }
        
        private void NewAppointment_Click (object sender, RoutedEventArgs e)
        {

        }

        private void Date_Changed (object sender, RoutedEventArgs e)
        {
            UpdateSchedule();
        }

        private void UpdateSchedule ()
        {
            if (Visible)
            {
                Grid.SetColumnSpan(DoctorTable, 1);
                Grid.SetColumnSpan(ScheduleBox, 1);
                ScheduleBox.Visibility = Visibility.Visible;
                
                SBTitle.Text = $"Schedule for {currentDoctor.FirstName} {currentDoctor.LastName}";

                if (SBDatePicker.SelectedDate != null) { 

                    SBSchedule.DataContext = currentDoctor.AppointmentsToday(DateOnly.FromDateTime((DateTime)SBDatePicker.SelectedDate));

                }
            } 
            else
            {
                ScheduleBox.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(DoctorTable, 2);
                Grid.SetColumnSpan(ScheduleBox, 1);
            }
        }
    }
}
