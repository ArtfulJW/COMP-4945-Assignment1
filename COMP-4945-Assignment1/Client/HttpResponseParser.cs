using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace Client {
    internal class HttpResponseParser {
        public const string SECTION_BOUNDARY = "\r\n\r\n";
        private string response;

        public HttpResponseParser(byte[] res) {
            response = Encoding.ASCII.GetString(res, 0, res.Length);
        }
        /**
        * Split raw request into 3 parts.
        * head, headers, body.
        *
        * Then call helper methods to parse each.
        */
        public void parse() {
            string[] splitRes = response.Split(new string[] { SECTION_BOUNDARY }, StringSplitOptions.RemoveEmptyEntries);
            string body = splitRes[1];
            string[] splitBody = body.Split(new char[] { ',', '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string entry in splitBody) {
                Console.WriteLine(entry);
            }

        }

    }
}
