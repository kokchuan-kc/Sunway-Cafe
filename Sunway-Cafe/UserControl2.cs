﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunway_Cafe
{
    public partial class UserControl2 : UserControl
    {
        private static UserControl2 _instance;

        public static UserControl2 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserControl2();
                return _instance;
            }
        }
        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {

        }
    }
}
