using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace student
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        Form1 f1;
        private void button1_Click(object sender, EventArgs e)
        {
            if(f1 == null)
            {
                f1 = new Form1();
                f1.Mdiparent = this;
                f1.FormClosed += new FormClosedEventHandler(f1_FormClosed);

                f1.Show(); 
            }
            else
            {
                f1.Activate();
            }
         
        }
        private void f1_FormClosed(object sender, FormClosedEventArgs e)
        {
            f1 = null;
            //throw new NotImplementedException();
        }

        Form2 f2;
        private void button2_Click(object sender, EventArgs e)
        {
            if (f2 == null)
            {
                f2 = new Form2();
                f2.Mdiparent = this;
                f2.FormClosed += new FormClosedEventHandler(f2_FormClosed);

                f2.Show();
            }
            else
            {
                f2.Activate();
            }
        }
        private void f2_FormClosed(object sender, FormClosedEventArgs e)
        {
            f2 = null;
            //throw new NotImplementedException();
        }


        Form3 f3;
        private void button3_Click(object sender, EventArgs e)
        {
            if (f3 == null)
            {
                f3 = new Form3();
                f3.Mdiparent = this;
                f3.FormClosed += new FormClosedEventHandler(f3_FormClosed);

                f3.Show();
            }
            else
            {
                f3.Activate();
            }
        }
        private void f3_FormClosed(object sender, FormClosedEventArgs e)
        {
            f3 = null;
            //throw new NotImplementedException();
        }

        Form4 f4;
        private void button4_Click(object sender, EventArgs e)
        {
            if (f4 == null)
            {
                f4 = new Form4();
                f4.Mdiparent = this;
                f4.FormClosed += new FormClosedEventHandler(f4_FormClosed);

                f4.Show();
            }
            else
            {
                f4.Activate();
            }
        }
        private void f4_FormClosed(object sender, FormClosedEventArgs e)
        {
            f4 = null;
            //throw new NotImplementedException();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GroupStudent form = new GroupStudent();
            form.Show();
            //this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form8 form = new Form8();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form7 form = new Form7();
            form.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form6 form = new Form6();
            form.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form9 form = new Form9();
            form.Show();
            this.Hide();
        }
    }
}
