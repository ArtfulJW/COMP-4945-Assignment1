using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Server
{
    public class UploadServlet : Servlet
    {
        public void get(Request request, Response response)
        {
            // Serve up HTML to browser
            if (    request.getRequestType()    == "GET" &&     request.getUserAgent()     == "Browser")
            {
                response.renderHTML();
            }
        }

        public void post(Request request, Response response)
        {
            // TODO: Implement Image Uploading

            // Get current time 
            DateTime currrentTime = DateTime.Now;
            String fileName = currrentTime.ToString();

            // images FolderPath relative to this file.
            string imageFolderPath = "..\\..\\image\\" + fileName + ".png";

            // Convert ImageByteCode (byte[]) into string
            string imageString = Encoding.UTF8.GetString(    request.getImageByteCode()    );

            using (FileStream fs = File.Create(imageFolderPath));

        }
    }
}
