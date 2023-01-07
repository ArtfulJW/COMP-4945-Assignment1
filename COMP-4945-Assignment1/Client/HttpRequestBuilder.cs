using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Client {
	internal class HttpRequestBuilder {

		public const string VERSION = "HTTP/1.1";
		public const string HOST = "HTTP/1.1";
		public const string BOUNDARY = "12345abcde";
		private IPEndPoint ipe;

		private HttpRequestBuilder() {
		}

		public static string buildMultipartRequest(string filePath, string caption, string date) {
			// build the body before hand so we can get the content length
			StringBuilder bodyBuilder = new StringBuilder();
			buildBody(bodyBuilder, filePath, caption, date);

			StringBuilder reqBuilder = new StringBuilder();

			// request line
			reqBuilder.Append("POST ").Append("/ ").Append(VERSION).Append("\r\n");

			// headers
			reqBuilder.Append("User-Agent: ").Append("CLI").Append("\r\n");
			reqBuilder.Append("Accept: ").Append("*/*").Append("\r\n");
			reqBuilder.Append("Host: ").Append(HOST).Append("\r\n");
			reqBuilder.Append("Accept-Encoding: gzip, deflate, br").Append("\r\n");
			reqBuilder.Append("Connection: ").Append("keep-alive").Append("\r\n");
			reqBuilder.Append("Content-Type: ").Append("multipart/form-data; boundary=").Append(BOUNDARY).Append("\r\n");
			reqBuilder.Append("Content-Length: ").Append(bodyBuilder.ToString().Length).Append("\r\n");

			// split body from head
			reqBuilder.Append("\r\n");

			// body (parameters)
			reqBuilder.Append(bodyBuilder);

			return reqBuilder.ToString();
		}

		private static void buildBody(StringBuilder bodyBuilder, string filePath, string caption, string date) {
			bodyBuilder.Append("--").Append(BOUNDARY).Append("\r\n");

			// parse the file
			buildFile(bodyBuilder, filePath);

			// parse the caption
			bodyBuilder.Append("Content-Disposition: form-data; name=\"caption\"").Append("\r\n");
			bodyBuilder.Append("\r\n");
			bodyBuilder.Append(caption).Append("\r\n");
			bodyBuilder.Append("--").Append(BOUNDARY).Append("\r\n");

			// parse the date
			bodyBuilder.Append("Content-Disposition: form-data; name=\"date\"").Append("\r\n");
			bodyBuilder.Append("\r\n");
			bodyBuilder.Append(date).Append("\r\n");
			bodyBuilder.Append("--").Append(BOUNDARY).Append("--\r\n");
		}

		private static bool buildFile(StringBuilder bodyBuilder, string filePath) {
			/*
			 * if (!File.Exists(filePath)) {
				Console.WriteLine("File not found");
				return false;
			}
		 
			*/
			try {
				// read the file and store it as a string
				FileStream fis = File.OpenRead(filePath);
				string fileName = Path.GetFileName(filePath);
				byte[] fileArray = new byte[fis.Length];
				fis.Read(fileArray, 0, fileArray.Length);
				fis.Close();

				// add content disposition
				bodyBuilder.Append("Content-Disposition: form-data; name=\"fileName\"; filename=\"").Append(fileName).Append("\"\r\n");
				// add the content type
				buildContentType(bodyBuilder, filePath);

				// add the file string as the body
				bodyBuilder.Append(Encoding.UTF8.GetString(fileArray)).Append("\r\n");

				bodyBuilder.Append("--").Append(BOUNDARY).Append("\r\n");
				return true;
			} catch (Exception e) {
				Console.WriteLine(e);
				return false;
			}
		}

		private static void buildContentType(StringBuilder bodyBuilder, string filePath) {
			int index = 0;
			while (filePath[index] != '.') {
				index++;
			}
			string fileType = filePath.Substring(index + 1);
			string contentType = "?";
			switch (fileType) {
				case "png":
					contentType = "image/png";
					break;
				case "jpg":
				case "jpeg":
					contentType = "image/jpeg";
					break;
				case "txt":
					contentType = "file/text";
					break;
			}
			bodyBuilder.Append("Content-Type: ").Append(contentType).Append(fileType).Append("\r\n\r\n");
		}

	}
}
