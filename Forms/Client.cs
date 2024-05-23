using BasicServerFunctionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Forms
{
    internal class Client
    {
        private static Server server;
        public static int usedPort;
        internal static void SignIn(Func<string> toLower, string text)
        {
            
            string msg;
            string userMsg;
            server = Program.server;
            if (server == null)
            {
                Console.WriteLine("Server instance is null.");
                return;
            }
            usedPort = Program.port;

            
            IPAddress usedIPAddress = Program.address;
            if (usedIPAddress == null)
            {
                Console.WriteLine("Server IPAddress is not set.");
                return;
            }
            IPEndPoint ie = new IPEndPoint(usedIPAddress, usedPort);


            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Connect(ie);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error connection: {0}\nError code: {1}({2})", e.Message, (SocketError)e.ErrorCode, e.ErrorCode);

                return;
            }

            IPEndPoint ieServer = (IPEndPoint)serverSocket.RemoteEndPoint;
            Console.WriteLine("Server on IP:{0} at port {1}", ieServer.Address, ieServer.Port);

            using (NetworkStream ns = new NetworkStream(serverSocket))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                msg = "";
                Console.WriteLine(msg);
                while (true)
                {
                    
                    sw.WriteLine("hey");
                    sw.Flush();
                    
                    Console.WriteLine(msg);
                }
            }
        }

        public static void OpenConnection()
        {
            
            string msg;
            string userMsg;
            

           
        }
    }
}
