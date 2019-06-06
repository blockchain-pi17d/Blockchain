using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace Blockchain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private static int y;
        private static int x;
        private static string txt;

        private int placeX;
        private int placeY;
        private int placeX2;
        private int placeY2;
        private void Form1_Load(object sender, EventArgs e)
        {
            placeX = label1.Location.X;
            placeY = label1.Location.Y;

            placeX2 = label2.Location.X;
            placeY2 = label2.Location.Y;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            if (radioButton1.Checked)
            {
                SendRequest("Nauseda");                   
                label1.Text = "Nausėda: "+y.ToString();
            }

            if (radioButton2.Checked)
            {
                SendRequest("Simonyte");
                label2.Text = "Šimonytė: "+x.ToString();
            }           
            if(x>y)
            {
                label1.Location = new Point(placeX2, placeY2);
                label2.Location = new Point(placeX, placeY);

            }
            else if(x<y)
            {
                label1.Location = new Point(placeX, placeY);
                label2.Location = new Point(placeX2, placeY2);
            }
                
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            SendRequest("Request blocks");
            textBox1.Text=txt+Environment.NewLine;
            
        }

        public static void SendRequest(string data)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[134217728];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);                                     

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);                   
                    txt=Encoding.ASCII.GetString(bytes, 0, bytesRec);                       
                    if (txt.IndexOf("<Nauseda>") > -1)
                    {
                        txt = txt.Remove(txt.Length - 9);
                        y = Convert.ToInt32(txt);
                    }
                    if (txt.IndexOf("<Simonyte>") > -1)
                    {
                        txt = txt.Remove(txt.Length - 10);
                        x = Convert.ToInt32(txt);
                    }                   
                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException)
                {
                    MessageBox.Show("Serveris neatsako");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
