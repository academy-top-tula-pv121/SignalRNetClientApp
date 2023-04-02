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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRNetClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7101/chat").Build();

            connection.On<string, string>("Receive", (username, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var messageNew = $"{username}: {message}";
                    lstChat.Items.Insert(0, messageNew);
                });
            });

        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("Send", txtUserName.Text, txtMessage.Text);
            }
            catch (Exception ex)
            {
                lstChat.Items.Add(ex.Message);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StartAsync();
                lstChat.Items.Add("Welcome to Chat!");
                btnSend.IsEnabled = true;
            }
            catch(Exception ex)
            {
                lstChat.Items.Add(ex.Message);
            }
        }
    }
}
