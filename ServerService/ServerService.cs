using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BasicServerFunctionality;

namespace ServerService
{
    public partial class ServerService : ServiceBase
    {
        private readonly Server _serverStart = new Server();
        public ServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _serverStart.Init();
        }

        protected override void OnStop()
        {
        }
        
        protected override void OnPause()
        {
        }
        
        protected override void OnContinue()
        {
        }
    }
}
