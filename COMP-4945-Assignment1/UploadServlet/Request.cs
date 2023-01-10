using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server {
	public class Request {
		List<byte> b1 = new List<byte>();

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
		public void parsePayload() {
			// Recieved Payload
			string receivedMessage = buildRequestMessage();
			Console.WriteLine(receivedMessage);


			string[] req = receivedMessage.Split(boundary);


			for (int i = 0; i < req.Length; i++) {
				Console.WriteLine("STRING: " + req[i]);
				if (i == 0) {
					string[] headerInfo = req[i].Split(' ');
					requestType = headerInfo[0];
					url = headerInfo[1];
					version = headerInfo[2];
					Console.WriteLine("THIS REQUEST TYPE IS: " + requestType);
					Console.WriteLine("THIS REQUEST URL IS: " + url);
					Console.WriteLine("THIS REQUEST VERSION IS: " + version);
				}

				if (req[i].Contains("User-Agent:")) {
					Console.WriteLine(req[i]);
					string[] sub = req[i].Split(": ");
					Console.WriteLine("USERAGENT: " + sub[1]);
					userAgent = sub[1].Split("\r\n")[0];
				}

				if (req[i].Contains("Content-Type: image/png") || req[i].Contains("Content-Type: image/jpeg")) {
					/*  string imgString = req[i].Split("--")[0];
                      imgString = imgString.Split("\r\n\r\n")[1];
                      imgString.Trim();
                      Console.WriteLine("HEREERERE  \n" + imgString +"ENDDDD");

                      imageByteData = Encoding.ASCII.GetBytes(imgString);
                      foreach(byte data in imageByteData)
                      {
                          Console.WriteLine(data);
                      }*/

					byte[] imgBytes = b1.ToArray();



					string checkMsg = null;
					int k = 0;
					while (true) {
						if(k >= imgBytes.Length) {
							break;
						}
						checkMsg += (char)(imgBytes[k]);

						if (checkMsg.Contains("--")) {
							break;
						}
						else {
							k++;
						}

					}
					b1.RemoveAt(0);
					b1.RemoveRange(k - 3, b1.Count - k + 3);
					imageByteData = b1.ToArray();
				}

				if (req[i].Contains("filename=")) {
					filename = req[i].Split("filename=\"")[1];
					filename = filename.Split('.')[0];
				}

				if (req[i].Contains("caption")) {
					caption = req[i + 2];


				}
				if (req[i].Contains("date")) {
					//    date = req[i + 2];

				}

			}

		}

		// TODO: Implement getters
		// Getters
		public string getRequestType() {
			return requestType;
		}

		public string getUserAgent() {
			return userAgent;
		}

		public string getFileName() {
			return filename;
		}

		public byte[] getImageByteCode() {
			// Preliminary Test
			return imageByteData;
		}

		public string buildRequestMessage() {
			//Set character buffer of one byte
			byte[] bytesReceived = new byte[1];

			string recievedMessage = "";
			string multipart = "";
			bool gotBoundary = false;

			// Infinitely Recurse - build message
			while (true) {
				int recv;
				//If recv is 0 the connection is closed
				bool isClosed = ((recv = socket.Receive(bytesReceived, bytesReceived.Length, 0)) == 0);

				//Check if client connection closed or end line received
				if (isClosed && (Encoding.ASCII.GetString(bytesReceived, 0, 1)[0] == '\n')) {
					Console.WriteLine("Connection Closed");
					// Exit while loop
					break;
				}
				// string a should now have the whole http request saved.s
				recievedMessage += Encoding.ASCII.GetString(bytesReceived, 0, 1);
				if (recievedMessage.Contains("Content-Type: image/png\r\n\r\n") || recievedMessage.Contains("Content-Type: image/jpeg\r\n\r\n")) {
					try {
						b1.Add(bytesReceived[0]);
					}
					catch {
						Console.WriteLine("FAIL ADD");
					}
				}

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
				if (recievedMessage.Contains("\r\n\r\n") && recievedMessage.Contains("GET")) {
					break;
				}

				if (recievedMessage.Contains("Content-Type: multipart/form-data; boundary=") && recievedMessage.Length >= recievedMessage.IndexOf("Content-Type: multipart/form-data; boundary=") + 44 + 38 && gotBoundary == false) {
					try {
						string sub = recievedMessage.Split("; boundary=")[1];
						if (sub.Contains("\r\n")) {
							boundary = sub.Split("\r\n")[0];
							Console.WriteLine("BOUNDARY: FIRST :  " + boundary + " END ");
							gotBoundary = true;
						}
					}
					catch {
						Console.WriteLine("Boundary not caught");
					}

				}

				if (recievedMessage.Contains("--" + boundary + "--") && recievedMessage.Contains("POST") && gotBoundary == true) {
					Console.WriteLine("Exit POST Request LOOP");
					break;
				}
			}
			return recievedMessage;
		}

	}
}