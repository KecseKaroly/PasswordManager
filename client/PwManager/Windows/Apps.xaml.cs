using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PwManager.Entities;
using PwManager.Windows;
using RestSharp;
using RestSharp.Serialization.Json;

namespace PwManager.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        RestClient restClient = null;
        public Window1()
        {
            InitializeComponent();
            if (MainWindow.UserLoggedIn.IsAdmin != "1" || MainWindow.UserLoggedIn == null)
            {
                addUserBtn.Content = "...";
                addUserBtn.Visibility = Visibility.Hidden;
            }
            string server = ConfigurationSettings.AppSettings["server"];
            int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            restClient = new RestClient(string.Format("http://{0}:{1}/server/php/apps/index.php", server, port));

            ListApps();
        }
        private void ListApps()
        {
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            List<Apps> applications = new JsonSerializer().Deserialize<List<Apps>>(response);
            List<Apps> appsToDelete = new List<Apps>();
            foreach (Apps application in applications)
            {
                if (application.UserId != MainWindow.UserLoggedIn.ID)
                {
                    appsToDelete.Add(application);
                }
            }

            foreach (Apps application in appsToDelete)
            {
                applications.Remove(application);
            }
            appsToDelete = new List<Apps>();
            ApplicationsDG.ItemsSource = applications;
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
                id = AppIDtb.Text,
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            });

            var response = restClient.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            

            ListApps();
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
                appid = AppIDtb.Text,
                appname = appNameTb.Text,
                userid = MainWindow.UserLoggedIn.ID,
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            }) ;


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            ListApps();
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
                appname = appNameTb.Text,
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password
            });


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            ListApps();
        }


        private void AppIDtb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var data = ApplicationsDG.SelectedItem;
            if(data != null)
            {
                string aid = (ApplicationsDG.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                string uid = (ApplicationsDG.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                string aname = (ApplicationsDG.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                appNameTb.Text = aname;
                AppIDtb.Text = aid;
            }
        }

       

        private void ListAccounts_Click_1(object sender, RoutedEventArgs e)
        {
            var data = ApplicationsDG.SelectedItem;
            string aid = (ApplicationsDG.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            string uid = (ApplicationsDG.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            string aname = (ApplicationsDG.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            
            Window2 loginDetails = new Window2(int.Parse(aid), aname);
            loginDetails.ShowDialog();
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            Users UsersWindow = new Users();
            UsersWindow.ShowDialog();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.UserLoggedIn = null;
            this.Close();
            
        }
    }
}
