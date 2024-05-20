using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatroomWithUserIdentification;
using System.Linq;

namespace BasicServerFunctionality
{
    public class Server
    {
        private static readonly object LockObject = new object();
        private static readonly Dictionary<Socket, StreamWriter> StreamWriterBySocket = new Dictionary<Socket, StreamWriter>();
        private static readonly Dictionary<Socket, string> UserBySocket = new Dictionary<Socket, string>();
        public int port;
        public void Init()
        {
            int startingPort = 31416;

            bool portFound = false; 
            port = startingPort;
            Socket socket = null;

            IPEndPoint ipEndPoint = null;
            while (!portFound && port < 65536 && port > -1)
            {
                
                try
                {
                    ipEndPoint = new IPEndPoint(IPAddress.Any, port);
                    portFound = true;
                }
                catch (SocketException)
                {
                    
                    port++;
                }
            }
            if (!portFound)
            {
                Console.WriteLine("No available port found.");
                return;
            }

            using (socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(ipEndPoint);
                socket.Listen(100);
                Console.WriteLine($"Server started on port {port}");
                Thread thread;
                while (true)
                {
                    Socket socketClient = socket.Accept();
                    thread = new Thread(() => ClientThread(socketClient));
                    thread.Start();
                }
            }
            
            
        }

        static void ClientThread(object socket)
        {
            

            SignUpAndSignIn signUpAndSignIn = new SignUpAndSignIn();
            string message = "";
            Socket client = (Socket)socket;
            IPEndPoint ipEndpointClient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ipEndpointClient.Address, ipEndpointClient.Port);
            NetworkStream networkStream = new NetworkStream(client);
            AddClientEndPointToCollections(client, networkStream);
            SendMessageToAnotherUsers($@" Has been connected", client);

            using (StreamReader streamReader = new StreamReader(networkStream))
            using (StreamWriter streamWriter = new StreamWriter(networkStream))
            {
                string welcome = "Will you sign-in or register?\n'Register'\n'Sign in'";
                string user;
                string password;

                while (message != "exit")
                {
                    streamWriter.WriteLine(welcome);
                    streamWriter.Flush();
                    try
                    {

                        try
                        {
                            message = streamReader.ReadLine();
                            streamWriter.WriteLine(message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            break;
                        }
                        streamWriter.Flush();

                        if (message != null && (message.ToLower() == "sign" || message.ToLower() == "sign-in" || message.ToLower() == "sign in" || message.ToLower() == "s"))
                        {
                            streamWriter.WriteLine("Give me your username");
                            streamWriter.Flush();
                            user = streamReader.ReadLine()?.ToLower();
                            streamWriter.WriteLine("Give me your password");
                            streamWriter.Flush();
                            password = streamReader.ReadLine()?.ToLower();

                            if(signUpAndSignIn.SignIn(user, password))
                            {
                                streamWriter.WriteLine("Very well, you have logged in as: " + user);
                                streamWriter.Flush();
                                while (true)
                                {
                                    streamWriter.WriteLine("Send message: \n'exit' to leave");
                                    streamWriter.Flush();
                                    message = streamReader.ReadLine();
                                    if (message == "exit")
                                    {
                                        break;
                                        
                                    }
                                    SendMessageToAnotherUsers(user+":"+message, client);

                                }
                            }
                            else
                            {
                                streamWriter.WriteLine("Failure");
                                streamWriter.Flush();
                            }
                        }
                        else if(message.ToLower() == "register" || message.ToLower() == "r")
                        {
                            streamWriter.WriteLine("Give me your username");
                            streamWriter.Flush();
                            user = streamReader.ReadLine().ToLower();
                            streamWriter.WriteLine("Give me your password");
                            streamWriter.Flush();
                            password = streamReader.ReadLine().ToLower();

                            if (signUpAndSignIn.CreateNewAccount(user, password))
                            {
                                streamWriter.WriteLine("Very well, it has been created");
                                streamWriter.Flush();
                            }
                            else
                            {
                                streamWriter.WriteLine("Failure to create");
                                streamWriter.Flush();
                            }
                        }
                        if (message != null)
                        {
                            Console.WriteLine("{0} says: {1}",
                            ipEndpointClient.Address, message);
                        }
                    }
                    catch (IOException)
                    {
                        //Salta al acceder al socket
                        //y no estar permitido
                        break;
                    }
                }
                SendMessageToAnotherUsers($@"Has been disconnected", client);
                DeleteClientEndPointToAllCollections(client);
                Console.WriteLine("Client disconnected.");
                Console.WriteLine("Finished connection with {0}:{1}",
                ipEndpointClient.Address, ipEndpointClient.Port);
            }
            client.Close();
        }

        private static void SendMessageToAnotherUsers(string message, Socket key)
        {
            var anotherStreamWriterNotOfMe = new Dictionary<Socket, StreamWriter>();
            var username = "";

            lock (LockObject)
            {
                foreach (var pair in StreamWriterBySocket)
                {
                    if (pair.Key != key)
                    {
                        anotherStreamWriterNotOfMe.Add(pair.Key, pair.Value);
                    }
                }
                
                var test = UserBySocket.TryGetValue(key, out username);
            }

            var address = ((IPEndPoint)key.RemoteEndPoint).Address.ToString();
            var port = ((IPEndPoint)key.RemoteEndPoint).Port.ToString();
            foreach (var pair in anotherStreamWriterNotOfMe)
            {
                pair.Value.WriteLine($@"{username}.{address}:{port}: {@message}");
                pair.Value.Flush();
            }
        }

        private static void AddClientEndPointToCollections(Socket client, NetworkStream networkStream)
        {
            lock (LockObject)
            {
                var streamWriter = new StreamWriter(networkStream, Encoding.UTF8);
                StreamWriterBySocket.Add(client, streamWriter);
            }
        }
        private static void DeleteClientEndPointToAllCollections(Socket socketClient)
        {
            lock (LockObject)
            {
                StreamWriterBySocket.Remove(socketClient);
                UserBySocket.Remove(socketClient);
            }
        }
    }
}
