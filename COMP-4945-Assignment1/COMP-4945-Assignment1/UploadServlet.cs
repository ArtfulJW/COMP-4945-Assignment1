using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class UploadServlet : Servlet
    {
        public void get(Request request, Response response)
        {
            // Serve up HTML to browser
            if (request.getRequestType() == "GET" && request.getUserAgent() == "Browser")
            {
                response.renderHTML();
            }
        }

        public void post(Request request, Response response)
        {
            // TODO: Implement Image Uploading



        }
    }
}
