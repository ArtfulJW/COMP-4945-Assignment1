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
 * ================== DirServer.cs ==================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class DirListing
{
    Socket cls = null;
    public DirListing(Socket socket) { this.cls = socket; }
    public void threadMethod()
    {
        Byte[] bytesReceived = new Byte[1];
        string a = "";
        while (true)
        {
            if ((cls.Receive(bytesReceived, bytesReceived.Length, 0) == 0) ||
             (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0'))
            {
                break;
            }
            // string a should now have the whole http request saved.s
            a += Encoding.ASCII.GetString(bytesReceived, 0, 1);
        }
        Console.WriteLine(a);
        DirectoryInfo di = new DirectoryInfo(a);
        FileInfo[] fiArr = di.GetFiles();
        string files = "";
        foreach (FileInfo fri in fiArr) { files = files + fri.Name; }
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(files + '\0');
        cls.Send(msg, msg.Length, 0);
        cls.Close();
    }
    static void Main(string[] args)
    {
        try
        {
            int port = 8888; IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(ipe); s.Listen(10);
            while (true)
            {
                // Accept is a blocking call
                Socket cls = s.Accept();
                DirListing dirListing = new DirListing(cls);
                // Delegate: ThreadStart, provide the thread with the method. Google .net threadstart delegate
                Thread thread = new Thread(new ThreadStart(dirListing.threadMethod));
                thread.Start();
            }
            s.Close();
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket exception: {0}", e);
        }
    }
}
================== DirServer.cs ==================
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Namespace is a container for related classes
namespace Program 
{
    class Program 
    { 

        public static void ExecuteServer()
        {

        }

        static void Main(string[] args)
        {
            
        }
    }
}

