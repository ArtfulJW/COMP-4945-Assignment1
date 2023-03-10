using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Server
{
    public class Response
    {
        // Member Variables
        string indexHTML = "HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";
        string uploadHTML = "HTTP/1.0 200 ok\r\nContent-Length: ";
        string htmlEnd = "\r\nContent-Type: text/plain\r\n\r\n";
        Socket socket = null;

        // Constructor
        public Response(Socket socket)
        {
            this.socket = socket;
        }

        public void renderHTML()
        {
            byte[] message = System.Text.Encoding.ASCII.GetBytes(indexHTML + '\0');
            socket.Send(message, message.Length, 0);
        }

        public void renderOkResponsePage()
        {
            string path = ".\\.\\images\\";
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] filePaths = d.GetFiles();
            string files = null;
            string fileList = null;
            List<string> fileArray = new List<string>();   
            foreach (FileInfo filePath in filePaths)
            {
                //Console.WriteLine(filePath.Name);
                fileArray.Add(filePath.Name);
            }
            fileArray.Sort();
            var charCount = 0;
            foreach (string filePath in fileArray)
            {
                //Console.WriteLine(filePath.Name);
                fileList += filePath + "\n";
                charCount += filePath.Length;
            }
            string size = fileList.Length.ToString();
            uploadHTML += size;
            uploadHTML += htmlEnd;
            uploadHTML += fileList;
            byte[] message = System.Text.Encoding.ASCII.GetBytes(uploadHTML + '\0');
            socket.Send(message, message.Length, 0);
        }

        public void renderOkCLI()
        {
            string path = ".\\.\\images\\";
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] filePaths = d.GetFiles();
            string files = null;
            List<string> result = new List<string>();
            foreach (FileInfo filePath in filePaths)
            {
                //Console.WriteLine(filePath.Name);
                result.Add(filePath.Name);
            }
            result.Sort();
            var json = JsonSerializer.Serialize(result);

            byte[] message = System.Text.Encoding.ASCII.GetBytes(json.ToString() + '\0');
            socket.Send(message, message.Length, 0);
        }

    }
}
