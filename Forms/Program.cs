using BasicServerFunctionality;
using System.Net;

namespace Forms
{
  internal class Program
  {
        public static Server server = new Server();
        public static int port;
        public static IPAddress address;
        public static void Main(string[] args)
    {
            server.Init();
            port = server.port;
            address = server.AnyIPAddress;
            ClientFormSignIn form = new ClientFormSignIn();
            form.ShowDialog();
            
            

        }
  }
}