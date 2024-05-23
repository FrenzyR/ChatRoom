﻿using BasicServerFunctionality;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class ClientFormSignIn : Form
    {
        public ClientFormSignIn()
        {
            InitializeComponent();
            
        }

        private void btn_signin_Click(object sender, EventArgs e)
        {
            Client.SignIn(txt_username.Text.ToLower, txt_password.Text);
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            //SignUpForm.ShowDialog();
        }
    }
}
