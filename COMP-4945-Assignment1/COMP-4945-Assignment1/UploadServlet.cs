using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Server
{
    public class UploadServlet : Servlet { 

        public void execute(Request request, Response response)
        {
            if (request.getUserAgent() == "Browser")
            {
                switch (request.getRequestType()) {
                    case "GET":
                        get(request, response);
                        break;
                    case "POST":
                        post(request, response);
                        break;
                }
            }
            else if (request.getUserAgent() == "CLI" && request.getRequestType() == "POST")
            {
                post(request, response);
            }
        }
    
        public void get(Request request, Response response)
        {
            // Serve up HTML to browser
            response.renderHTML();
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
            string imageString = Encoding.UTF8.GetString(request.getImageByteCode());

            FileStream fs = File.Create(imageFolderPath);
        }
    }
}
