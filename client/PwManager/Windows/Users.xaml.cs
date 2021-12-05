using PwManager.Entities;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PwManager.Windows
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        RestClient restClient = null;
        public Users()
        {
            InitializeComponent();
            string server = ConfigurationSettings.AppSettings["server"];
            int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            restClient = new RestClient(string.Format("http://{0}:{1}/server/php/users/index.php", server, port));
            ListUsers();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.PUT);
            request.RequestFormat = RestSharp.DataFormat.Json;

            if (MainWindow.UserLoggedIn == null)
            {
                MessageBox.Show("You must log in for this feature");
                return;
            }
            
            request.AddJsonBody(new
            {
                isAdmin = (bool)isAdminCb.IsChecked ? "1" : "0",
                userid = userIdTb.Text,
                UpdateUsername = usernameTb.Text,
                UpdatePassword = passwordTb.Text,
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            });


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            
            ListUsers();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.DELETE);
            request.RequestFormat = RestSharp.DataFormat.Json;

            if (MainWindow.UserLoggedIn == null)
            {
                MessageBox.Show("You must log in for this feature");
                return;
            }

            request.AddJsonBody(new
            {
                id = userIdTb.Text,
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            });

            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            ListUsers();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;

            if (MainWindow.UserLoggedIn == null)
            {
                MessageBox.Show("You must log in for this feature");
                return;
            }

            request.AddJsonBody(new
            {
                Addusername = usernameTb.Text,
                Addpassword = passwordTb.Text,
                isAdmin = (bool)isAdminCb.IsChecked ? "1" : "0",
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            }) ;

            var response = restClient.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            ListUsers();
        }

        private void AccountsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isAdminCb.IsEnabled = true;
            isAdminCb.IsChecked = false;
            var data = UsersDG.SelectedItem;
            if (data != null)
            {
                string uid = (UsersDG.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                string username = (UsersDG.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                string isAdmin = (UsersDG.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                usernameTb.Text = username;
                userIdTb.Text = uid;
                if (isAdmin == "1")
                {
                    isAdminCb.IsChecked = true;
                    if (uid == "1")
                        isAdminCb.IsEnabled = false;
                }
            }
        }

        private void ListUsers()
        {
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            List<User> users = new JsonSerializer().Deserialize<List<User>>(response);
            UsersDG.ItemsSource = users;
        }
    }
}
