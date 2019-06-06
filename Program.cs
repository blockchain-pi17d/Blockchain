using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace BlockchainServer
{
    class Program
    {
        private static BlockchainClass blockRegister = new BlockchainClass();

        public static string data = null;
        private static int kandidatasPoint=0;
        private static int kandidatasPoint2=0;
        static void Main(string[] args)
        {
            StartListening();
            //return 0;
        }                   

        public static void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Laukiama uzklausos...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("Nauseda") > -1)
                        {
                            blockRegister.AddBlock(new Block(DateTime.Now, null, data));
                            kandidatasPoint++;                          
                            byte[] msg2 = Encoding.ASCII.GetBytes(kandidatasPoint.ToString()+"<Nauseda>");
                            handler.Send(msg2);
                        }
                        if (data.IndexOf("Simonyte") > -1)
                        {
                            blockRegister.AddBlock(new Block(DateTime.Now, null, data));
                            kandidatasPoint2++;
                            byte[] msg2 = Encoding.ASCII.GetBytes(kandidatasPoint2.ToString()+"<Simonyte>");
                            handler.Send(msg2);
                        }
                        if (data.IndexOf("Request blocks") > -1)
                        {
                            string validationStatus="Nezinomas";
                            if (blockRegister.IsValid())
                                validationStatus = "Patvirtinta";
                            if (!blockRegister.IsValid())
                                validationStatus = "Negalioja";
                            
                            string txt=JsonConvert.SerializeObject(blockRegister, Formatting.Indented)+Environment.NewLine+ validationStatus+Environment.NewLine;
                            Console.WriteLine("Blocks validation is: "+blockRegister.IsValid());
                            byte[] msg2 = Encoding.ASCII.GetBytes(txt);
                            handler.Send(msg2);
                        }
                        break;
                        /*if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }*/
                    }

                    // Show the data on the console.                    
                    Console.WriteLine("Gautas balsas : {0}", data);

                    // Echo the data back to the client.  
                    //byte[] msg = Encoding.ASCII.GetBytes(data);

                    //handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Isjunkite programa");
            Console.Read();

        }
    }
}
