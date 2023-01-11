using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface Servlet
    {
        public void get(Request request, Response response);
        public void post(Request request, Response response);
    }
}
