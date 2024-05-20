using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatroomWithUserIdentification;

namespace BasicServerFunctionality
{
    public class MainTestingGrounds
    {
        public static Server server = new Server();
        public static void Main(string[] args)
        {
            SignUpAndSignIn signUpAndSignIn = new SignUpAndSignIn();            
            server.Init();
            
        }
    }
}
