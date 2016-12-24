using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace WindowsFormsApplication2.dataEntry
{
    class device
    {
        Socket socket;
        device(Socket socket)
        {
            this.socket = socket;
        }

    }
}
