using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Request
    {
        Socket socket = null;
        string requestType = null;
        string url = null;
        string version = null;
        string filename = null;
        string caption = null;
        string date = null;
        string userAgent = null;
        string boundary = null;
        byte[] imageByteData = null;
        public Request(Socket socket) { this.socket = socket; }
        public void parsePayload()
        {
            // Recieved Payload
            string receivedMessage = buildRequestMessage();
            Console.WriteLine(receivedMessage);


            string[] req = receivedMessage.Split("\r\n");
            

            for (int i = 0; i < req.Length; i++)
            {
                Console.WriteLine("STRING: " + req[i]);   
                if (i == 0)
                {
                    string[] headerInfo = req[i].Split(' ');
                    requestType = headerInfo[0];
                    url = headerInfo[1];
                    version = headerInfo[2];
                    Console.WriteLine("THIS REQUEST TYPE IS: " + requestType);
                    Console.WriteLine("THIS REQUEST URL IS: " + url);
                    Console.WriteLine("THIS REQUEST VERSION IS: " + version);
                }

               
                if (req[i].Contains("User-Agent:")) 
                {
                    Console.WriteLine(req[i]);
                    string[] sub = req[i].Split(": ");
                    Console.WriteLine("USERAGENT: " + sub[1]);
                    userAgent = sub[1];
                }
                if (req[i].Contains("Content-Type: multipart/form-data"))
                {
                    Console.WriteLine("BOUNDARY: " + req[i]);
                    string[] sub = req[i].Split("; boundary=");
                    boundary = sub[1];
                    Console.WriteLine("BOUNDARY: " + boundary);
                }

                if (req[i].Contains("Content-Type: image/jpeg"))
                {
                    string imgBytes = req[i + 1] + req[i + 2];
                    imageByteData = Encoding.ASCII.GetBytes(imgBytes + '\0');
                    Console.WriteLine("JPEG IMG == "  + imgBytes);

                }

                if (req[i].Contains("Content-Type: image/png"))
                {
                    string imgBytes = req[i + 2] + "\r\n" + req[i + 3] + "\0";

                   
                    imageByteData = Encoding.Default.GetBytes(imgBytes);
                    byte[] imageByteDataReverse = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAB8AAAARCAYAAAAlpHdJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAC2SURBVEhL7ZXdDYMwDIS9YCTmQdnETBLm4CFTeIRrapLGRakQLTQP7cNJsXz4u/xIkIigl34YzgPBTbHZPKTZg8gjtHovdA18YThy4GXj2eg8uNUxOMMTpeRJAyMWQ94NT27tJflZEMbszbX1Bgl11k4AhVdThNZjWA06sJ5MzCEKUEOUsO8e+wN2lx1i141aw3wKf7pz/bAX3A75BrwObN35xfC9134YXjZhHmdL/x9LF3WEC25pAgP+h75e8AAAAABJRU5ErkJggg==");
                
                    
                    Console.WriteLine("PNG IMAGE === \n " + imgBytes);
                    Console.WriteLine("BASE 64 IMG CORRECT \n" + Convert.ToBase64String(imageByteDataReverse));
                    Console.WriteLine("BASE 64 IMG \n" + Convert.ToBase64String(imageByteData));

                }

                if (req[i].Contains("filename="))
                {
                    filename = req[i].Split("filename=")[1];
                    filename = filename.Substring(1, filename.Length - 2);


                }

                if (req[i].Contains("caption"))
                {
                    caption = req[i + 2];
                    
                }
                if (req[i].Contains("date"))
                {
                    date = req[i + 2];
                    
                }

            }
            Console.WriteLine("DATE ====" + date);
            Console.WriteLine("CAPTION ===" + caption);
            Console.WriteLine("FILE NAME ===" + filename);
            Console.WriteLine("IMG BYTE ====");
            
            
        }

        // TODO: Implement getters
        // Getters
        public string getRequestType()
        {
            return requestType;
        }

        public string getUserAgent()
        {
            return userAgent;
        }

        public string getFileName()
        {
            return filename;
        }

        public byte[] getImageByteCode()
        {
            // Preliminary Test
            return imageByteData;
        }

        public string buildRequestMessage()
        {
            //Set character buffer of one byte
            byte[] bytesReceived = new byte[1];
            string recievedMessage = "";
            string multipart = "";
            bool gotBoundary = false;

            // Infinitely Recurse - build message
            while (true)
            {
                int recv;
                //If recv is 0 the connection is closed
                bool isClosed = ((recv = socket.Receive(bytesReceived, bytesReceived.Length, 0)) == 0);

                //Check if client connection closed or end line received
                if (isClosed && (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\n'))
                {
                    Console.WriteLine("Connection Closed");
                    // Exit while loop
                    break;
                }
                // string a should now have the whole http request saved.s
                recievedMessage += Encoding.ASCII.GetString(bytesReceived, 0, 1);
                int x = 0;

                // Check for end of transmission
                //if (recievedMessage.Length >= 4)
                //{
                //    for (int i = 1; i < 5; i++)
                //    {
                //        if (i % 2 == 0)
                //        {
                //            if (recievedMessage[recievedMessage.Length - i] == '\r')
                //            {
                //                x++;
                //            }
                //        }
                //        else
                //        {
                //            if (recievedMessage[recievedMessage.Length - i] == '\n') { }
                //            x++;
                //        }
                //    }
                //    if (x == 4 && requestType == "GET")
                //    {
                //        Console.WriteLine("EOF");
                //        break;
                //    } else
                //    {

                //        // keep going until content length
                //    }
                //}

                // End of GET Request
                if (recievedMessage.Contains("\r\n\r\n") && recievedMessage.Contains("GET"))
                {
                    break;
                }

                if (recievedMessage.Contains("Content-Type: multipart/form-data; boundary=") && recievedMessage.Length >= recievedMessage.IndexOf("Content-Type: multipart/form-data; boundary=") + 44 + 38 && gotBoundary == false)
                {
                    Console.WriteLine("HELL");
                    string intermediate = recievedMessage.Substring(0, recievedMessage.IndexOf("Content-Type: multipart/form-data; boundary=")+44+38);
                    string[] arr = intermediate.Split("Content-Type: multipart/form-data; boundary=");
                    boundary = arr[1].Trim();
                    gotBoundary = true;
                    Console.WriteLine("BOUNDARY: " + boundary);

                }

                if (recievedMessage.Contains("--" + boundary + "--") && recievedMessage.Contains("POST") && gotBoundary == true)
                {
                    Console.WriteLine("Exit POST Request LOOP");
                    break;
                }
            }
            return recievedMessage;
        }

    }
}