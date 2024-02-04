using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Diagnostics;

namespace IpTracker
{
    public partial class Form1 : Form
    {
        private const string ApiBaseUrl = "http://ip-api.com/json/";

        private bool isDragging = false;
        private int offsetX, offsetY;

        public Form1()
        {
            InitializeComponent();
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;

            guna2hScrollBar1.Scroll += guna2hScrollBar1_Scroll;
        }


        private void ip_TextChanged(object sender, EventArgs e)
        {

        }

        private async void track_Click(object sender, EventArgs e)
        {
            string ipAddress = ip.Text;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string apiUrl = $"{ApiBaseUrl}{ipAddress}";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string response = await client.GetStringAsync(apiUrl);

                        IpDetails ipDetails = JsonConvert.DeserializeObject<IpDetails>(response);

                        label2.Text = $"IP Details:\r\n" +
                                      $"IP: {ipDetails.Query}\r\n" +
                                      $"Country: {ipDetails.Country}\r\n" +
                                      $"Region: {ipDetails.RegionName}\r\n" +
                                      $"City: {ipDetails.City}\r\n" +
                                      $"Zip code: {ipDetails.Zip}\r\n" +
                                      $"Latitude: {ipDetails.Lat}\r\n" +
                                      $"Longitude: {ipDetails.Lon}\r\n" +
                                      $"ISP: {ipDetails.Isp}";

                    }
                    catch (HttpRequestException ex)
                    {
                        label2.Text = $"HTTP request error : {ex.Message}";
                    }
                    catch (JsonException ex)
                    {
                        label2.Text = $"JSON deserialization error : {ex.Message}";
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid IP address.");
            }
        }

        private class IpDetails
        {
            public string Query { get; set; }
            public string Country { get; set; }
            public string RegionName { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public float Lat { get; set; }
            public float Lon { get; set; }
            public string Isp { get; set; }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            offsetX = e.X;
            offsetY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                this.Left = e.X + this.Left - offsetX;
                this.Top = e.Y + this.Top - offsetY;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void guna2hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label2.Left = -e.NewValue;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = "https://github.com/freeman649";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}