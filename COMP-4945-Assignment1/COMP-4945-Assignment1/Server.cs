/*
 * Program.cs will be compiled as server.exe, with additional dependencies.
 * 
 * ========================
 * 
 * Server will infinitely recurse (Keep the socket open for connection) and wait for potential users to make a connection to the ServerSocket.
 * When a connection doest finally happen, the server will create a "ServerThread(Socket)", which is a thread the server creates
 * to deal with any connections that happen.

 * =======================
 * 
 * Depending on the type of User-Agent that a connection has, the server will serve up different interactions.
 * User-Agent: CLI
 * The server expects a POST Request from this type of User-Agent, but will still check the type of request anyways.
 * 
 * User-Agent: Browser "So, NOT CLI"
 * Server will check for a GET Request for the HTML Page that the browser is asking for, and respond back with it.
 * later on, the browser will then use the UploadServlet, to make a POST Request, similiar to how the CLI does, with 
 * multi-part form request containing all the details for uploading a image.
 * 
 * =======================
 */

/*
//* ================== DirServer.cs ==================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class DirListing {
    //This is the client socket
    Socket cls = null;
    public DirListing(Socket socket) {
        this.cls = socket;
    }
    public void threadMethod() {
        //Set character buffer of one byte
        Byte[] bytesReceived = new Byte[1];
        string a = "";

        //
        while (true) {
            //Read one byte from socket and get number of bytes read
            int recv = cls.Receive(bytesReceived, bytesReceived.Length, 0);
            //If recv is 0 the connection is closed
            bool isClosed = (recv == 0);
            //Check if client connection closed or end line received
            if ( isClosed || (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0')) {
                // Exit while loop
                break;
            }
            // string a should now have the whole http request saved.s
            a += Encoding.ASCII.GetString(bytesReceived, 0, 1);
        }
        
        Console.WriteLine(a);
        
        DirectoryInfo di = new DirectoryInfo(a);
        //Get list of files in di as array
        FileInfo[] fiArr = di.GetFiles();
        string files = "";
        //Get every file name in fiArr
        foreach (FileInfo fri in fiArr) {
            files = files + fri.Name;
        }

        //Convert files into HTTP byte format
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(files + '\0');
        //send msg to client
        cls.Send(msg, msg.Length, 0);
        //close client socket
        cls.Close();
    }
    static void Main(string[] args) {
        try {
            int port = 8888;
            IPAddress address = IPAddress.Parse("127.0.0.1");

            // Create EndPoint object from port and ipAddress
            IPEndPoint ipe = new IPEndPoint(address, port);

            // Create serverside listener socket
            Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind socket to EndPoint
            s.Bind(ipe);

            // Set socket to listen for upto 10 connection
            s.Listen(10);

            while (true) {
                // Accept is a blocking call
                Socket cls = s.Accept();
                DirListing dirListing = new DirListing(cls);
                // Delegate: ThreadStart, provide the thread with the method. Google .net threadstart delegate
                Thread thread = new Thread(new ThreadStart(dirListing.threadMethod));
                thread.Start();
            }
            // Close server socket
            s.Close();
        } catch (SocketException e) {
            Console.WriteLine("Socket exception: {0}", e);
        }
    }
}*/
//================== DirServer.cs ==================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;


// Namespace is a container for related classes
namespace Server {
    public class Program {


        static void Main(string[] args) {
            try
            {
                // Intended Endpoint Paramaters
                int port = 8888;
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

                // Instantiate Endpoint
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

                // Create Server-side listener Socket
                Socket serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Bind Server Socket Listener to Endpoint
                serverSocket.Bind(endPoint);

                // Allow connections of up to 1;
                serverSocket.Listen(10);
                
                // Infinite Loop to deal with incoming Socket Connections
                while (true)
                {

                    Console.WriteLine("Waiting for a connection...");

                    // Accept Incoming client Socket connection 
                    Socket client = serverSocket.Accept();

                    // Create 
                    // Program program = new Program(client);
                    ServerThread serverThread = new ServerThread(client);

                    // Delegate Method
                    Thread thread = new Thread(new ThreadStart(serverThread.processClient));
                    thread.Start();
                    Console.WriteLine("Started processClient");
                }
                // Close Server Socket Listener
                serverSocket.Close();

            } catch (SocketException e)
            {
                Console.WriteLine("Socket exception: {0}", e);
            }

        }
    }
}

