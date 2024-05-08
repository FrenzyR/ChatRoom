using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatroomWithUserIdentification;

namespace BasicServerFunctionality
{
    internal class MainTestingGrounds
    {

        public static void Main(string[] args)
        {
            SignUpAndSignIn signUpAndSignIn = new SignUpAndSignIn();
            Server server = new Server();
            server.Init();
            
        }
    }
}
