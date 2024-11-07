using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using PatientRecordSystem.Model;

namespace PatientRecordSystem.View
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : Page
    {
        private User[] users;

        public UserManagementView()
        {

            string jsonPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\users.json";
            string jsonString = File.ReadAllText(jsonPath);
            users = JsonSerializer.Deserialize<User[]>(jsonString);


            InitializeComponent();

            UserTable.DataContext = users;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserModal newUserModal = new NewUserModal();
            newUserModal.ShowDialog();
        
        }
    }
}
