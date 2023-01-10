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

        public void execute(Request request, Response response)
        {
            
            switch (request.getRequestType())
            {
                case "GET":
                    get(request, response);
                    break;
                case "POST":
                    post(request, response);
                    break;
            }
        }

        public void get(Request request, Response response)
        {
            // Serve up HTML to browser
            Console.WriteLine("Serving HTML");
            response.renderHTML();
        }

        public void post(Request request, Response response)
        {
            // TODO: Implement Image Uploading
            bool fileOk = true;
            // Get current time 
            // DateTime currrentTime = DateTime.Now;
            // String fileName = request.getFileName();

            // images FolderPath relative to this file.
            string imageFolderPath = ".\\.\\images\\" + request.getFileName()+".png";

            // Convert ImageByteCode (byte[]) into string
            // string imageString = Encoding.UTF8.GetString(request.getImageByteCode());

            try
            {
                using var writer = new BinaryWriter(File.OpenWrite(imageFolderPath));
                writer.Write(request.getImageByteCode());
            }
            catch
            {
                Console.WriteLine("Failed to upload image");
                fileOk = false;
            }

            // byte[] bytes = request.getImageByteCode();
            // foreach ( byte b in bytes)  
            // {  
            //     Console.WriteLine(b);  
            // }  


            // try {
            //     FileStream fs = File.Create(imageFolderPath);
            // } catch {
            //     Console.WriteLine("Failed to upload image");
            //     fileOk = false;
            // }

            if (fileOk)
            {
                if(request.getUserAgent() == "CLI") {
                

                } else
                {
                    // Serve up HTML to browser
                    Console.WriteLine("File uploaded, refreshing page");
                    response.renderOkResponsePage();
                }                
            }
        }
    }
}
