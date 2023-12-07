using SERVER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CLIENT
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListenClientAsync();
        }

        private async void ListenClientAsync()
        {
            UdpClient udpClient = new UdpClient(27001);
            while(true)
            {
                var result = await udpClient.ReceiveAsync();
                var bytes = result.Buffer;
                CarsListView.ItemsSource = ByteArrayToObject(bytes) as List<Car>;
            }
        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            var command = new Command() { Method = HttpMethods.GET };
            var client = new UdpClient();

            var connectEP = new IPEndPoint(IPAddress.Parse("192.168.100.8"), 45678);

            
            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectEP);
            
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            // add
            var command = new Command() { Method = HttpMethods.POST, Car = CarsListView.SelectedItem as Car };
            var client = new UdpClient();

            var connectEP = new IPEndPoint(IPAddress.Parse("192.168.100.8"), 45678);


            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectEP);
        }

        private void PutButton_Click(object sender, RoutedEventArgs e)
        {
            // update
            var command = new Command() { Method = HttpMethods.PUT, Car = CarsListView.SelectedItem as Car };
            var client = new UdpClient();

            var connectEP = new IPEndPoint(IPAddress.Parse("192.168.100.8"), 45678);


            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectEP);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var command = new Command() { Method = HttpMethods.DELETE, Car = CarsListView.SelectedItem as Car };
            var client = new UdpClient();

            var connectEP = new IPEndPoint(IPAddress.Parse("192.168.100.8"), 45678);


            var bytes = ObjectToByteArray(command);
            client.SendAsync(bytes, bytes.Length, connectEP);
        }


        static byte[] ObjectToByteArray(object command)
        {
            if (command == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, command);
                return memoryStream.ToArray();
            }
        }

        static object ByteArrayToObject(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return formatter.Deserialize(memoryStream);
            }
        }




    }
}
