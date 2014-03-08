using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Get customer by id
        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox2.Text);
            CustomerOrder.Customer c = Program.customer.GetCustomerById(id);
            textBox1.Text += c.id + "\t" + c.name + "\t" + c.adresa + Environment.NewLine;
        }
        //insert customer
        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox3.Text;
            string adr = textBox4.Text;
            Program.customer.InsertCustomer(name, adr);
        }
        //update custumer
        private void button3_Click(object sender, EventArgs e)
        {
            CustomerOrder.Customer c= new CustomerOrder.Customer();
            c.id = Convert.ToInt32(textBox5.Text);
            c.name = textBox7.Text;
            c.adresa = textBox6.Text;
            Program.customer.UpdateCustomer(c);
        }
        //delete customer
        private void button4_Click(object sender, EventArgs e)
        {
            CustomerOrder.Customer c = new CustomerOrder.Customer();
            c.id = Convert.ToInt32(textBox10.Text);
            c.name = textBox9.Text;
            c.adresa = textBox8.Text;
            Program.customer.DeleteCustomer(c);
        }
        // all orders
        private void button5_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox11.Text);
            CustomerOrder.Order[] ords = Program.customer.GetAllOrders(id);
            foreach (var o in ords)
            {
                textBox1.Text += o.id + "\t" + o.data + "\t" + o.idC + "\t" + o.valoare + Environment.NewLine;
            }
        }
        //get order
        private void button6_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox12.Text);
            CustomerOrder.Order o = Program.order.GetOrder(id);
            textBox1.Text += o.id + "\t" + o.data + "\t" + o.idC + "\t" + o.valoare + Environment.NewLine;
        }
        //insert order
        private void button7_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox13.Text);
            string date = textBox14.Text;
            string val = textBox15.Text;
            Program.order.InsertOrder(id, Convert.ToDateTime(date), Convert.ToDecimal(val));
        }
        //update order
        private void button8_Click(object sender, EventArgs e)
        {
            CustomerOrder.Order o = new CustomerOrder.Order();
            o.id = Convert.ToInt32(textBox18.Text);
            o.data = textBox17.Text;
            o.idC = Convert.ToInt32(textBox19.Text);
            o.valoare = Convert.ToDecimal(textBox16.Text);
            Program.order.UpdateOrder(o);
        }
        // subscribe
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Program.sub.Subscribe();
            }
            else
            {
                Program.sub.Unsubscribe();
            }
        }
        //detele order
        private void button9_Click(object sender, EventArgs e)
        {
            CustomerOrder.Order o = new CustomerOrder.Order();
            o.id = Convert.ToInt32(textBox23.Text);
            o.data = textBox22.Text;
            o.idC = Convert.ToInt32(textBox20.Text);
            o.valoare = Convert.ToDecimal(textBox21.Text);
            Program.order.DeleteOrder(o);
        }
        // get all customers
        private void button10_Click(object sender, EventArgs e)
        {
            ReportService.Customer[] customs = Program.report.GetAllCustomer();
            foreach (var c in customs)
            {
                textBox1.Text += c.id + "\t" + c.name + "\t" + c.adresa + Environment.NewLine;
            }
        }
        // get all orders for customer
        private void button11_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox24.Text);
            ReportService.Order[] orders = Program.report.GetAllOrdersForCustomer(id);
            foreach (var o in orders)
            {
                textBox1.Text += o.id + "\t" + o.data + "\t" + o.idC + "\t" + o.valoare + Environment.NewLine;
            }
        }

        public void showMessage(string message)
        {
            this.SetText(message);
            //textBox1.Text += message + Environment.NewLine;
        }
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text += text + Environment.NewLine;
            }
        }
    }
}
