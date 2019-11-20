﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using Sunway_Cafe.Model;

namespace Sunway_Cafe
{
    public partial class OrderPage : UserControl
    {
        public DataGridView DataGridView1
        {
            get
            {
                return dataGridView1;
            }
            set
            {
                dataGridView1 = value;
            }
        }

        private static OrderPage _instance;
        int i = 0;

        public static OrderPage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderPage();
                return _instance;
            }
        }
        public OrderPage()
        {
            InitializeComponent();
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                flowLayoutPanel1.Controls.Remove(control);
                flowLayoutPanel2.Controls.Remove(control);
                control.Dispose();
            }
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel2.Controls.Clear();


            using (var db = new SunwayCafeContext())
            {
                var query = db.Items.Where(d => d.Type == "food").ToList();
                OrderOptions[] order = new OrderOptions[query.Count];

                foreach (var itemList in query)
                {
                    order[i] = new OrderOptions(this);
                    order[i].ID = itemList.Id;
                    order[i].Name_details = itemList.Name;
                    if (itemList.ImageURL != null)
                    {
                        order[i].displayImage = ConvertBinaryToImage(itemList.ImageURL);
                    }
                    order[i].WasClicked += OrderGrid_WasClicked;
                    order[i].Price = itemList.SellingPrice;
                    order[i].CostPrice = itemList.CostPrice;

                    flowLayoutPanel1.Controls.Add(order[i]);
                    i++;
                }
                i = 0;
            }

            using (var db = new SunwayCafeContext())
            {
                var query = db.Items.Where(d => d.Type == "drinks").ToList();
                OrderOptions[] order = new OrderOptions[query.Count];

                foreach (var itemList in query)
                {
                    order[i] = new OrderOptions(this);
                    order[i].ID = itemList.Id;
                    order[i].Name_details = itemList.Name;
                    if (itemList.ImageURL != null)
                    {
                        order[i].displayImage = ConvertBinaryToImage(itemList.ImageURL);
                    }
                    order[i].WasClicked += OrderGrid_WasClicked;
                    order[i].Price = itemList.SellingPrice;
                    order[i].CostPrice = itemList.CostPrice;

                    flowLayoutPanel2.Controls.Add(order[i]);
                    i++;
                }
                i = 0;
            }

        }

        private void OrderGrid_WasClicked(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is OrderOptions)
                {
                    ((OrderOptions)c).IsSelected = false;
                }
            }
        }

        Image ConvertBinaryToImage(byte[] image)
        {
            using (MemoryStream ms = new MemoryStream(image))
            {
                return Image.FromStream(ms);
            }
        }

        public void Total()
        {
            var total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                total += Convert.ToInt32(row.Cells["Price"].Value) * Convert.ToInt32(row.Cells["Quantity"].Value);
            }
            lbltotal.Text = total.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.CurrentRow;
            var quantity = (Convert.ToInt32(row.Cells[dataGridView1.Columns["Quantity"].Index].Value));


            if (row.Cells["Product"].Value != null)
            {
                if (e.ColumnIndex == dataGridView1.Columns["Add"].Index)
                {
                    quantity++;
                    row.Cells["Quantity"].Value = quantity;
                    Total();
                }
                else if (e.ColumnIndex == dataGridView1.Columns["Deduct"].Index)
                {
                    if (quantity > 0)
                    {
                        quantity--;
                        row.Cells["Quantity"].Value = quantity;
                        Total();
                    }
                    else if (quantity < 1)
                    {
                        //remove current row that is 0
                        dataGridView1.Rows.Remove(row);
                        MessageBox.Show("Quantity is already at 0");
                    }
                }
                else if (e.ColumnIndex == dataGridView1.Columns["Clear"].Index)
                {
                    dataGridView1.Rows.Remove(row);
                    MessageBox.Show("Item is removed from the order");
                    Total();

                }

            }
        }

        //Retrieve data from datagridview
        private void Retrieve_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DataGridView1.Rows.Count - 1; i++)
            {
                //Count-2 cause last two column is not needed
                for (int j = 0; j < dataGridView1.Columns.Count - 3; j++)
                {
                    MessageBox.Show(dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
            }
        }

        private void Pay_Click(object sender, EventArgs e)
        {

            
            var order = new Order()
            {
                NetPrice = decimal.Parse(lbltotal.Text),
                TotalPrice = decimal.Parse(lbltotal.Text) * 1.16M,
                Status = "Processing",
                DateTimeCreated = Global.ConvToDateTimeString(DateTime.Now),

            };

            using (var db = new SunwayCafeContext())
            {
                var lst = new List<OrderedItem>();
                //Add all item object from datagrid using id
                for (int i = 0; i < DataGridView1.Rows.Count - 1; i++)
                {
                    var id = Convert.ToInt32((dataGridView1.Rows[i].Cells[0].Value));
                    var item = db.Items.Where(x => x.Id == id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("Unable to retrieve item from database", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        lst.Add(new OrderedItem()
                        {
                            Order = order,
                            Item = item,
                            Qty = Convert.ToInt32((dataGridView1.Rows[i].Cells["Quantity"].Value))
                        });
                    }
                }

                order.OrderedItems = lst;
                db.Orders.Add(order);
                db.SaveChanges();

            }

            var receiptItems = new List<ReceiptItem>();
            foreach (var item in order.OrderedItems)
            {
                receiptItems.Add(new ReceiptItem() { Qty = item.Qty, Name = item.Item.Name, UnitPrice = item.Item.SellingPrice });
            }

            var receipt = new Receipt() { Subtotal = order.NetPrice, ReceiptItems = receiptItems , Date = order.DateTimeCreated};

            var reportPage = new ReportTest(receipt);
            reportPage.Show();
        }
    }
}
