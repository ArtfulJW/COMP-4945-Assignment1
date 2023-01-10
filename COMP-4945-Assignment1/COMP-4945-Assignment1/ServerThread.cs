/*
 * Settings for Reflection:
 * Go to Project -> Add Reference.. -> Projects -> Select DataStorageAPI
 * Notice that the project compiles even when no implementation of UploadServlet exists
 * Copy the UploadServlet.dll to the bin/debug directory of Server project
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;

namespace Server
{
    public class ServerThread
    {

        Socket socket = null;

        // Constructor
        public ServerThread(Socket socket)
        {
            this.socket = socket;
        }

        public UploadServlet createServlet()
        {

            // Get curret Directory
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.Split("Server.exe")[0]);
            string servletdllPath = Path.Combine(currentDirectory, "UploadServlet.dll");
            string className = "Server.UploadServlet";

            Assembly assembly = AppDomain.CurrentDomain.Load(Assembly.LoadFrom(servletdllPath).GetName());

            // Output: UploadServlet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            // So we know for sure Reflection is working
            // Console.WriteLine(assembly);

            Type servletType = assembly.GetType(className);

            return (UploadServlet)Activator.CreateInstance(servletType) ;

        }

        // Delegate Method
        public void processClient()
        {
            // Implement how each Thread deals with each client socket connection
            // For now, test by sending simple message to connection
            /*
             * Objective:
             * Depending on the User-Agent: (CLI or NOT CLI), react accordingly.
             */

            // Recieve and determine Request

            //string recievedMessage = buildRequestMessage();

            //string html = "HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";

            //byte[] message = System.Text.Encoding.ASCII.GetBytes(html + '\0');
            //socket.Send(message, message.Length, 0);

            //Console.WriteLine(recievedMessage);

            //socket.Close();



            //string html = "HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";

            //byte[] message = System.Text.Encoding.ASCII.GetBytes(html + '\0');
            //socket.Send(message, message.Length, 0);

            //string recievedMessage = buildRequestMessage();

            //Console.WriteLine(recievedMessage);

            //socket.Close();


            // Forming parameters for interaction
            Request request = new Request(socket);
            request.parsePayload(); 
            // Console.WriteLine("RECIEVED MESSAGE\n" + buildRequestMessage());
            
            Response response = new Response(socket);

            // Use Reflection to serve up UploadServlet
            //UploadServlet reflectedServlet = createServlet();

            //// Execute
            //reflectedServlet.execute(request, response);
            UploadServlet upload = new UploadServlet();
            upload.execute(request, response);
            socket.Close();

        }

        //public string buildRequestMessage()
        //{
        //    //Set character buffer of one byte
        //    byte[] bytesReceived = new byte[1];
        //    string recievedMessage = "";

        //    // Infinitely Recurse - build message
        //    while (true)
        //    {
        //        int recv;
        //        //If recv is 0 the connection is closed
        //        bool isClosed = ((recv = socket.Receive(bytesReceived, bytesReceived.Length, 0)) == 0);
        //        Console.WriteLine(recv);
        //        //Check if client connection closed or end line received
        //        if (isClosed || (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0'))
        //        {
        //            Console.WriteLine("Connection Closed");
        //            // Exit while loop
        //            break;
        //        }
        //        // string a should now have the whole http request saved.s
        //        recievedMessage += Encoding.ASCII.GetString(bytesReceived, 0, 1);
        //        int x = 0;

        //        // Check for end of transmission
        //        if (recievedMessage.Length >= 4)
        //        {
        //            for (int i = 1; i < 5; i++)
        //            {
        //                if (i % 2 == 0)
        //                {
        //                    if (recievedMessage[recievedMessage.Length - i] == '\r')
        //                    {
        //                        x++;
        //                    }
        //                } else
        //                {
        //                    if (recievedMessage[recievedMessage.Length - i] == '\n') { }
        //                    x++;
        //                }
        //            }
        //            if (x == 4)
        //            {
        //                Console.WriteLine("EOF");
        //                break;
        //            }
        //        }
        //    }
        //    return recievedMessage;
        //}


    }
}