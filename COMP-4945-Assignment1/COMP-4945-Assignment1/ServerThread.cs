using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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


            string html = "HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";

            byte[] message = System.Text.Encoding.ASCII.GetBytes(html + '\0');
            socket.Send(message, message.Length, 0);

            string recievedMessage = buildRequestMessage();

            Console.WriteLine(recievedMessage);

            socket.Close();
        }

        public string buildRequestMessage()
        {
            //Set character buffer of one byte
            byte[] bytesReceived = new byte[1];
            string recievedMessage = "";

            // Infinitely Recurse - build message
            while (true)
            {
                //Read one byte from socket and get number of bytes read
                int recv = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                //If recv is 0 the connection is closed
                bool isClosed = (recv == 0);
                //Check if client connection closed or end line received
                if (isClosed || (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\0'))
                {
                    Console.WriteLine("Connection Closed");
                    // Exit while loop
                    break;
                }
                // string a should now have the whole http request saved.s
                recievedMessage += Encoding.ASCII.GetString(bytesReceived, 0, 1);
            }
            //Console.WriteLine(recievedMessage);
            return recievedMessage;
        }

    }
}