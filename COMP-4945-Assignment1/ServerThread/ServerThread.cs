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

        Socket socket;

        // Constructor
        public ServerThread(Socket socket)
        {
            this.socket = socket;
        }

        // Delegate Method
        public void delegateThreadMethod()
        {
            // Implement how each Thread deals with each client socket connection
            // For now, test by sending simple message to connection

            string html = "GET HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";

            byte[] message = System.Text.Encoding.ASCII.GetBytes(html + '\0');

            socket.Send(message, message.Length, 0);

            socket.Close();
        }

    }
}