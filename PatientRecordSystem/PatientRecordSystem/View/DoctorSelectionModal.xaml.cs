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
    /// Interaction logic for DoctorSelectionModal.xaml
    /// </summary>
    public partial class DoctorSelectionModal : Window
    {
        public DoctorSelectionModal()
        {
            InitializeComponent();
            // Set the datacontext for the DataTable.
            DoctorTable.DataContext = UserManager.GetInstance().Users().Where(u => u.AccountType == Model.User.UserAccountType.Doctor).ToList();
        }

        // Set the Global parameter to the selected doctor, and close the dialog box.
        private void Select_Click (object sender, RoutedEventArgs e)
        {
            Globals.newAppointmentDoctor = DoctorTable.SelectedItem as User;
            DialogResult = true;
            Close();
        }
    }
}
