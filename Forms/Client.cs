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
            OpenConnection();
            string msg;
            string userMsg;
            server = MainTestingGrounds.server;
            usedPort = server.port;
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            using (NetworkStream ns = new NetworkStream(serverSocket))
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns))
            {
                msg = sr.ReadLine();
                Console.WriteLine(msg);
                while (true)
                {
                    userMsg = Console.ReadLine();
                    sw.WriteLine(userMsg);
                    sw.Flush();
                    msg = sr.ReadLine();
                    Console.WriteLine(msg);
                }
            }
        }

        public static void OpenConnection()
        {
            const string IP_SERVER = "127.0.0.10";
            string msg;
            string userMsg;
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), usedPort);
            Console.WriteLine("Starting client. Press a key to init connection");

            Console.ReadKey();
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ie);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error connection: {0}\nError code: {1}({2})", e.Message, (SocketError)e.ErrorCode, e.ErrorCode);
                Console.ReadKey();
                return;
            }

            IPEndPoint ieServer = (IPEndPoint)server.RemoteEndPoint;
            Console.WriteLine("Server on IP:{0} at port {1}", ieServer.Address, ieServer.Port);

           
        }
    }
}
