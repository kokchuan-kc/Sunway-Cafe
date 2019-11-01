﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunway_Cafe
{
    public partial class Form1 : Form
    {
        private static Form1 _obj;

        public static Form1 Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new Form1();
                }
                return _obj;
            }
        }

        public Panel SidePanel
        {
            get
            {
                return sidepanel;
            }
            set
            {
                sidepanel = value;
            }
        }

        public Panel MainPanel
        {
            get
            {
                return mainpanel;
            }
            set
            {
                mainpanel = value;
            }
        }

        public Button Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        public Button Manage
        {
            get
            {
                return manage;
            }
            set
            {
                manage = value;
            }
        }



        public Form1()
        {
            InitializeComponent();
            

        }

        public void button1_Click(object sender, EventArgs e)
        {
            home_Page(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _obj = this;

            UserControl1 uc1 = new UserControl1();
            MainPanel.Controls.Add(uc1);
        }

        public void order_Click(object sender, EventArgs e)
        {
            order_Page();
        }

        public void manage_Click(object sender, EventArgs e)
        {
            manage_Page();
        }

        public void order_Page()
        {
            if (!MainPanel.Controls.Contains(UserControl3.Instance))
            {
                MainPanel.Controls.Clear();
                SidePanel.Height = order.Height;
                SidePanel.Top = order.Top;
                MainPanel.Controls.Add(UserControl3.Instance);
                UserControl3.Instance.BringToFront();
            }
            else
                SidePanel.Height = order.Height;
                SidePanel.Top = order.Top;
                UserControl3.Instance.BringToFront();
        }

        public void manage_Page()
        {
            if (!MainPanel.Controls.Contains(UserControl2.Instance))
            {
                MainPanel.Controls.Clear();
                SidePanel.Height = manage.Height;
                SidePanel.Top = manage.Top;
                MainPanel.Controls.Add(UserControl2.Instance);
                UserControl2.Instance.BringToFront();
            }
            else
                SidePanel.Height = manage.Height;
                SidePanel.Top = manage.Top;
                UserControl2.Instance.BringToFront();
        }

        public void home_Page()
        {
            if (!MainPanel.Controls.Contains(UserControl1.Instance))
            {
                MainPanel.Controls.Clear();
                SidePanel.Height = home.Height;
                SidePanel.Top = home.Top;
                MainPanel.Controls.Add(UserControl1.Instance);
                UserControl1.Instance.BringToFront();
            }
            else
                SidePanel.Height = home.Height;
                SidePanel.Top = home.Top;
                UserControl1.Instance.BringToFront();
        }

        public void nav(Form form, Panel MainPanel)
        {
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(form);
            form.Show();
        }
    }

   
}
