using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatroomWithUserIdentification;

namespace BasicServerFunctionality
{
    internal class Server
    {
        
        public void Init()
        {
            SignUpAndSignIn attempt = new SignUpAndSignIn();
            IPEndPoint ipEndPoint = new IPEndPoint (IPAddress.Any, 31416);
            
            Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            
            socket.Bind (ipEndPoint);
            
            socket.Listen (10);
            while (true)
            {
                Socket socketClient = socket.Accept();
                Thread thread = new Thread(clientThread);
                thread.Start(socketClient);

            }



        }

        static void clientThread(object socket)
        {
            SignUpAndSignIn signUpAndSignIn = new SignUpAndSignIn();
            string message;
            Socket client = (Socket)socket;
            IPEndPoint ipEndpointClient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with client {0} at port {1}",
            ipEndpointClient.Address, ipEndpointClient.Port);
            using (NetworkStream networkStream = new NetworkStream(client))
            using (StreamReader streamReader = new StreamReader(networkStream))
            using (StreamWriter streamWriter = new StreamWriter(networkStream))
            {
                string welcome = "Will you sign-in or register?\n'Register'\n'Sign in'";
                string user;
                string password;
                
                while (true)
                {
                    streamWriter.WriteLine(welcome);
                    streamWriter.Flush();
                    try
                    {
                        message = streamReader.ReadLine();
                        streamWriter.WriteLine(message);
                        streamWriter.Flush();

                        if (message.ToLower() == "sign" || message.ToLower() == "sign-in" || message.ToLower() == "sign in" || message.ToLower() == "s")
                        {
                            streamWriter.WriteLine("Give me your username");
                            streamWriter.Flush();
                            user = streamReader.ReadLine()?.ToLower();
                            streamWriter.WriteLine("Give me your password");
                            streamWriter.Flush();
                            password = streamReader.ReadLine()?.ToLower();

                            if(signUpAndSignIn.SignIn(user, password))
                            {
                                streamWriter.WriteLine("Very well, you have logged in");
                                streamWriter.Flush();
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
                Console.WriteLine("Finished connection with {0}:{1}",
                ipEndpointClient.Address, ipEndpointClient.Port);
            }
            client.Close();
        }
    }
}