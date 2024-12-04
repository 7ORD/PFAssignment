using PatientRecordSystem.Model;
using PatientRecordSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for AppointmentCreationModal.xaml
    /// </summary>
    public partial class AppointmentCreationModal : Window
    {
        private Appointment newAppointment = new Appointment();
        private string patientId;
        private bool ReadOnly = false;

        public AppointmentCreationModal(bool readOnly = false, DateOnly date = new DateOnly(), TimeOnly time = new TimeOnly(), int slot = -1, string doctor = "", string patient = "", string summary = "", string description = "", bool editing = false)
        {
            InitializeComponent();

            ReadOnly = readOnly;
            patientId = patient;

            if (date != new DateOnly () && time != new TimeOnly () && slot > -1)
            {
                AppointmentTime.Text = $"{date} - {time}";
                newAppointment.Date = date;
                newAppointment.Time = time;
                newAppointment.Slot = slot;
            }

            if (!string.IsNullOrEmpty (doctor))
            {
                Doctor.Text = UserManager.GetInstance().Users().Find(u => u.Username == doctor).ParsedName;
                newAppointment.Doctor = doctor;
            }

            if (!string.IsNullOrEmpty (patient))
            {
                Patient.Text = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == patient).ParsedName;
                newAppointment.PatientId = patient;
            }

            if (!string.IsNullOrEmpty (summary))
            {
                Summary.Text = summary;
                newAppointment.BriefDescription = summary;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description.Text = description;
                newAppointment.Description = description;
            }

            if (readOnly)
            {
                Description.IsEnabled = false;
                Summary.IsEnabled = false;
                SelectDateButton.IsEnabled = false;
                SelectDoctorButton.IsEnabled = false;
                SelectPatientButton.IsEnabled = false;
                ReadOnly = readOnly;
                DoctorNotes.IsEnabled = false;
                AppointmentStatus.IsEnabled = false;

                Submit.Visibility = Visibility.Hidden;
                Grid.SetColumn(Cancel, 0);
                Grid.SetColumnSpan(Cancel, 2);
                Title.Text = $"Appointment for {Patient.Text = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == patient).ParsedName}";
            }

            if (!editing)
            {
                AppointmentStatus.IsEnabled = false;
            } else
            {
                AppointmentStatus.IsEnabled = true;
                DoctorNotes.IsEnabled = true;
                ReadOnly = false;
            }

            if (UserManager.GetInstance().currentUser.AccountType != User.UserAccountType.Doctor)
            {
                DoctorNotes.IsEnabled = false;
            }

            AppointmentStatus.SelectedIndex = 0;
        }

        private void SelectDoctor_Click (object sender, RoutedEventArgs e)
        {
            DoctorSelectionModal doctorSelectionModal = new DoctorSelectionModal();
            doctorSelectionModal.ShowDialog();

            if (doctorSelectionModal.DialogResult == true)
            {
                Doctor.Text = Globals.newAppointmentDoctor.ParsedName;
                newAppointment.Doctor = Globals.newAppointmentDoctor.Username;
                if (newAppointment.Doctor != null)
                {
                    SelectDateButton.IsEnabled = true;
                }

                Validate(Doctor, new RoutedEventArgs());
            }
        }

        private void SelectPatient_Click(object sender, RoutedEventArgs e)
        {
            PatientSelectionModal patientSelectionModal = new PatientSelectionModal();
            patientSelectionModal.ShowDialog();

            if (patientSelectionModal.DialogResult == true)
            {
                Patient.Text = Globals.newAppointmentPatient.ParsedName;
                newAppointment.PatientId = Globals.newAppointmentPatient.HospitalNumber;
                Validate(Patient, new RoutedEventArgs());
            }
        }

        private void SelectSlot_Click (object sender, RoutedEventArgs e)
        {
            SlotSelectionModal slotSelectionModal = new SlotSelectionModal();
            slotSelectionModal.ShowDialog();
            
            if (slotSelectionModal.DialogResult == true)
            {
                AppointmentTime.Text = $"{Globals.newAppointmentDate} - {Globals.newAppointmentTime}";
                newAppointment.Date = Globals.newAppointmentDate;
                newAppointment.Slot = Globals.newAppointmentSlot;

                Validate(SelectDateButton, new RoutedEventArgs());
            }
        }

        private void Validate (object sender, RoutedEventArgs e)
        {

            UpdateAppointmentDetails();

            if (AppointmentManager.GetInstance ().IsAppointmentValid (newAppointment))
            {
                Submit.IsEnabled = true;
            } else
            {
            if (Submit != null)
            {
                    Submit.IsEnabled = false;
                }   
            }
        }

        private void Cancel_Click (object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> appointments = AppointmentManager.GetInstance().Appointments();
            List<User> users = UserManager.GetInstance().Users();

            appointments.Add(newAppointment);
            users.Find(u => u.Username == newAppointment.Doctor).Appointments.Add(newAppointment.AppointmentId);

            AppointmentManager.GetInstance().UpdateData(appointments);
            UserManager.GetInstance().UpdateData(users);

            NotificationWindow notification = new NotificationWindow("Appointment Scheduled", "A new appointment has been entered in the system.");
            notification.ShowDialog();

            Globals.appointmentViewDoctor = users.Find(u => u.Username == newAppointment.Doctor);

            DialogResult = true;
            Close();
        }

        private void UpdateAppointmentDetails ()
        {
            newAppointment.AppointmentId = AppointmentManager.GetInstance().Appointments().Count;

            if (Summary != null)
            {
                newAppointment.BriefDescription = Summary.Text;
            }
            
            if (Description != null)
            {
                newAppointment.Description = Description.Text;
            }
            
            newAppointment.AppointmentCreator = UserManager.GetInstance().currentUser.Username;
        }

        private void PopulateForm ()
        {
            Description.IsEnabled = false;
            Summary.IsEnabled = false;
            SelectDateButton.IsEnabled = false;
            SelectDoctorButton.IsEnabled = false;
            SelectPatientButton.IsEnabled = false;

            DoctorNotes.IsEnabled = false;
            AppointmentStatus.IsEnabled = false;

            Submit.Visibility = Visibility.Hidden;
            Grid.SetColumn(Cancel, 0);
            Grid.SetColumnSpan(Cancel, 2);
            Title.Text = $"Appointment for {Patient.Text = PatientManager.GetInstance().Patients().Find(p => p.HospitalNumber == patientId).ParsedName}";
        }
    }
}
