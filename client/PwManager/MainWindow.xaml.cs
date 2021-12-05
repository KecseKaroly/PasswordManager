using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using RestSharp.Serialization.Json;
using PwManager.Entities;
using PwManager.Windows;

namespace PwManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RestClient client = null;

        public static User UserLoggedIn = null;
        public MainWindow()
        {
            InitializeComponent();
            string server = ConfigurationSettings.AppSettings["server"];
            string port = ConfigurationSettings.AppSettings["port"];
            
            client = new RestClient(string.Format("http://{0}:{1}/server/php/users/index.php", server, port));
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.AddObject(new
            {
                username = Usernametb.Text,
                password = Passwordtb.Password
            });

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
            if(response.Content == "false")
            {
                MessageBox.Show("Wrong login details!");
            }
            else
            {
                UserLoggedIn = new JsonSerializer().Deserialize<User>(response);
            }
            if (UserLoggedIn != null)
            {
                Window1 Applications = new Window1();
                Applications.ShowDialog();
            }
        }

        
    }
}
