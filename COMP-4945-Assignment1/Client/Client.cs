
/*//============= DirClient.cs =============
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class DirClient {
    private IPEndPoint ipe;

    public DirClient(string ipaddr, int port) {
        this.ipe = new IPEndPoint(IPAddress.Parse(ipaddr), port);
    }

    public string getListing(string path) {
        string a = "";
        try {
            //Create client socket
            Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine(path);

            // Connect client socket to EndPoint
            s.Connect(ipe);

            if (s.Connected) {
                // Encode path to byte array
                Byte[] bytesSent = Encoding.ASCII.GetBytes(path + '\0');
                // Send over client socket to EndPoint bytesSent 
                s.Send(bytesSent, bytesSent.Length, 0);

                //Set character buffer of one byte
                Byte[] bytesReceived = new Byte[1];

                while (true) {
                    //Read one byte from socket and get number of bytes read
                    int recv = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    //If recv is 0 the connection is closed
                    bool isClosed = (recv == 0);
                    //Check if server connection closed or end line received
                    if (isClosed || (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0')) {
                        // Exit while loop
                        break;
                    }
                    a += Encoding.ASCII.GetString(bytesReceived, 0, 1);
                }
            }
            // Close client socket
            s.Close();
            return a;
        } catch (ArgumentNullException e) {
            throw new Exception(@"[ArguementNullException From Sync.DirClient]",
                e);
        } catch (SocketException e) {
            throw new Exception(@"[SocketException From Sync.DirClient]", e);
        }
    }

    static void Main(string[] args) {
        // Create DirClient obj with intended EndPoint ipAddress and port number
        DirClient myDirClient = new DirClient("127.0.0.1", 8888);
        Console.WriteLine(myDirClient.getListing("c:\\Windows"));

        //the following call is just to block the main thread so that the results are listed to the screen
        Console.Read();
    }
}
==========================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client {
    internal class Client {
        private IPEndPoint ipe;
        public Client(string ipaddr, int port) {
            this.ipe = new IPEndPoint(IPAddress.Parse(ipaddr), port);
        }

        void uploadImage() {
            try {
                Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(ipe);
                if (s.Connected) {
                    //do file upload process
                }
                s.Close();
            } catch (ArgumentNullException e) {
                throw new Exception(@"[ArguementNullException From Sync.DirClient]", e);
            } catch (SocketException e) {
                throw new Exception(@"[SocketException From Sync.DirClient]", e);
            }
        }
        static void Main(string[] args) {
            //Promt for server connection
            /*
             * Console.WriteLine("Enter Ip Address of Server: ");
             * string address = Console.ReadLine();
             * Console.WriteLine("Enter Port of Server: ");
             * int port = Convert.ToInt32(Console.ReadLine());
             * Client client = new Client(address, port);
            */

            Client client = new Client("127.0.0.1", 8888);
            client.uploadImage();

            Console.Read();
        }
    }
}
