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

        public static byte[] buildMultipartRequest(string filePath, string caption, string date) {
            // build the body before hand so we can get the content length
            //StringBuilder bodyBuilder = new StringBuilder();
            List<byte> bodyBuilder = new List<byte>();
            buildBody(bodyBuilder, filePath, caption, date);

            List<byte> reqBuilder = new List<byte>();

            // request line
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("POST "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("/ "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes(VERSION));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

            // headers
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("User-Agent: "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("CLI"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Accept: "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("*/*"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Host: "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes(HOST));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Accept-Encoding: gzip, deflate, br"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Connection: "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("keep-alive"));

            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Type: "));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("multipart/form-data; boundary="));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes(BOUNDARY));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Length: "));
            reqBuilder.AddRange(bodyBuilder);
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

            // split body from head
            reqBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

            // body (parameters)
            reqBuilder.AddRange(bodyBuilder);

            return reqBuilder.ToArray();
        }

        //private static void buildBody(StringBuilder bodyBuilder, string filePath, string caption, string date) {
        private static void buildBody(List<byte> bodyBuilder, string filePath, string caption, string date) {

            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("--"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(BOUNDARY));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

            // parse the file
            buildFile(bodyBuilder, filePath);

            // parse the caption
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"caption\""));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(caption));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("--"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(BOUNDARY));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

            // parse the date
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"date\""));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(date));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("--"));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(BOUNDARY));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("--\r\n"));
        }

        //private static void buildFile(StringBuilder bodyBuilder, string filePath) {
        private static void buildFile(List<byte> bodyBuilder, string filePath) {

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
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"fileName\"; filename=\""));
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes(fileName));
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\"\r\n"));
                // add the content type
                buildContentType(bodyBuilder, filePath);

                // add the file string as the body
                bodyBuilder.AddRange(fileArray);
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));

                bodyBuilder.AddRange(Encoding.ASCII.GetBytes("--"));
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes(BOUNDARY));
                bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n"));
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        //private static void buildContentType(StringBuilder bodyBuilder, string filePath) {
        private static void buildContentType(List<byte> bodyBuilder, string filePath) {
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
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("Content-Type: "));
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes(contentType));            
            bodyBuilder.AddRange(Encoding.ASCII.GetBytes("\r\n\r\n"));
        }

    }
}
