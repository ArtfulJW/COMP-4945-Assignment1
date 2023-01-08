using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Request
    {
        Socket socket = null;
        public Request(Socket socket) { this.socket = socket; }
        public void parsePayload()
        {
            // Build Recieved Message
            /*
            Byte[] bytesReceived = new Byte[1];
            string a = "";
            while (true)
            {
                if ((socket.Receive(bytesReceived, bytesReceived.Length, 0) == 0) ||
                 (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\r'))
                {
                    break;
                }
                a += Encoding.ASCII.GetString(bytesReceived, 0, 1);
            }
            */

            string receivedMessage = buildRequestMessage();
            Console.WriteLine(receivedMessage + " this ");


            string[] req = receivedMessage.Split('\n');
            string[] header = req[0].Split(' ');
            req = req.Skip(1).ToArray();
            string reqType = header[0];
            string URL = header[1];
            string version = header[2];

            //Console.WriteLine(req[0]);
            //Console.WriteLine(URL);
            //Console.WriteLine(version);

            //Console.WriteLine(lines[0]);

            Dictionary<string, string> map = new Dictionary<string, string>();

            /*  for(int i = 0; i < lines.Length-2 ; i++)
              {
                  string key = lines[i].Split(':')[0].Trim();
                  string val = lines[i].Split(':')[1].Trim();
                  Console.WriteLine("Key = " + key);
                  Console.WriteLine("Val = " + val);
                  map.Add(key, val);
              }*/
            foreach (string line in req)
            {
                if (line != null)
                {
                    Console.WriteLine(line + "a");
                    /*      string key = line.Split(':')[0].Trim();
                          string val = line.Split(':')[1].Trim();
                          Console.WriteLine(key);
                          Console.WriteLine(val);
                          map.Add(key, val);*/
                }
            }
            //Console.WriteLine("user agent = " +  map["User-Agent"]);

            /*      DirectoryInfo di = new DirectoryInfo(a);
                  FileInfo[] fiArr = di.GetFiles();
                  string files = "";
                  foreach (FileInfo fri in fiArr) { files = files + fri.Name; }*/


            //string files = """
            //HTTP/1.1 200 ok
            //Content-Type: text/html
            
            //<!DOCTYPE html>
            //<html>
            //    <head>
            //        <title>File Upload Form</title>
            //    </head>
            //    <body>
            //<h1>Upload file</h1>
            //<form method="POST" action="/" enctype="multipart/form-data">
            //<input type="file" name="fileName"/><br/><br/>
            //Caption: <input type="text" name="caption"<br/><br/>
            //<br />
            //Date: <input type="date" name="date"<br/><br/>
            //<br />
            //<input type="submit" value="Submit"/>
            //</form>
            //</body>
            //</html>
            //""";

            string files = "HTTP/1.1 200 OK\nContent-Type:text/html\nContent-Length: 600\n\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title> File Upload Form</title>\r\n</head>\r\n<body>\r\n<h1>Upload file</h1>\r\n<form id =\"form\" method=\"POST\" action=\"/\" enctype=\"multipart/form-data\">\r\n<input type=\"file\" name=\"fileName\"/><br/><br/>\r\nCaption: <input type =\"text\" name=\"caption\"<br/><br/>\r\n <br/>\nDate : <input type=\"date\" name=\"date\"<br/><br/>\r\n <br/>\n <input id='formBtn' type=\"submit\" name=\"submit\" value=\"Submit\"/>\r\n </form>\r\n</body>\r\n</html>\r\n";

            //foreach (FileInfo fri in fiArr) { files = files + fri.Name; }
            byte[] msg = Encoding.ASCII.GetBytes(files);

            socket.Send(msg, msg.Length, 0);
            socket.Close();
        }

        // TODO: Implement getters
        // Getters
        public string getRequestType()
        {
            // Preliminary Test
            return "GET";
        }

        public string getUserAgent()
        {
            // Preliminary Test
            return "Browser";
        }

        public byte[] getImageByteCode()
        {
            byte[] ar = new byte[2];
            return ar;
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