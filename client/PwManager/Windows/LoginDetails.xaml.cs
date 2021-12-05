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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        RestClient restClient = null;
        private int appId;
        public Window2(int appId, string appName)
        {
            InitializeComponent();
            Title.Content = "Account(s) of '" + appName + "'";
            this.appId = appId;
            string server = ConfigurationSettings.AppSettings["server"];
            int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            restClient = new RestClient(string.Format("http://{0}:{1}/server/php/accounts/index.php", server, port));
            ListAccs();
        }

        private void AccountsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = AccountsDG.SelectedItem;
            if(data != null)
            {
                string accid = (AccountsDG.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                string username = (AccountsDG.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                string passwd = (AccountsDG.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;

                AccIDtb.Text = accid;
                usernameTb.Text = username;
                passwordTb.Text = passwd;
            }
            
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
                userUsername = MainWindow.UserLoggedIn.UserName,
                userPassword = MainWindow.UserLoggedIn.Password,
                accid = AccIDtb.Text,
                accUsername = usernameTb.Text,
                accPassword = passwordTb.Text
            });


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            ListAccs();
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
                username = MainWindow.UserLoggedIn.UserName,
                password = MainWindow.UserLoggedIn.Password,
                accid = AccIDtb.Text
            });


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            ListAccs();
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
                userUsername = MainWindow.UserLoggedIn.UserName,
                userPassword = MainWindow.UserLoggedIn.Password,
                appid = this.appId,
                accUsername = usernameTb.Text,
                accPassword = passwordTb.Text
            });


            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            ListAccs();
        }

        private void ListAccs()
        {
            
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            
            var response = restClient.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            List<Accounts> accounts = new JsonSerializer().Deserialize<List<Accounts>>(response);
            List<Accounts> accsToDelete = new List<Accounts>();
            foreach (Accounts account in accounts)
            {
                if(account.AppId != this.appId)
                {
                    accsToDelete.Add(account);
                }
            }
            
            foreach (Accounts acc in accsToDelete)
            {
                accounts.Remove(acc);
            }
            accsToDelete = new List<Accounts>();
            AccountsDG.ItemsSource = accounts;
        }
    }
}
