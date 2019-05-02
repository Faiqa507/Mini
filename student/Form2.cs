using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace student
{
    public partial class Form2 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";
        
        internal Form5 Mdiparent;
        int id;
        public Form2()
        {
            InitializeComponent();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            SqlConnection con2 = new SqlConnection(con);
            SqlConnection con3 = new SqlConnection(con);
            SqlConnection con4 = new SqlConnection(con);
            SqlConnection con5 = new SqlConnection(con);
            con1.Open();
            string query = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}' AND Category = 'GENDER'", comboBox1.Text);
            //SqlDataAdapter SDA3 = new SqlDataAdapter(query, con1);
            SqlCommand com = new SqlCommand(query, con1);
            int Id = (Int32)com.ExecuteScalar();
        
            con2.Open();
            com.CommandText = "INSERT INTO Person(FirstName, LastName, Contact, Email, DateOfBirth, Gender) VALUES('" + name1txt.Text + "' , '" + name2txt.Text + "', '" + contacttxt.Text + "', '" + emailtxt.Text + "', '" + dateTimePicker1.Value + "', '" + Id + "')";
            com.ExecuteNonQuery();

            con3.Open();
            string query1 = "SELECT Id FROM Person WHERE Email = '" + emailtxt.Text + "'";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con3);
            SqlCommand com1 = new SqlCommand(query1, con3);
            int Id1 = (Int32)com1.ExecuteScalar();
            //SDA.SelectCommand.ExecuteNonQuery();

            con5.Open();
            string query4 = string.Format("SELECT Id FROM LookUp WHERE Value = '{0}' AND Category = 'DESIGNATION' ", comboBox2.Text);
            // com1.CommandText = "SELECT Id FROM LookUp WHERE Value = '" + destxt.Text + "' AND Category = 'DESIGNATION'";
            SqlCommand com2 = new SqlCommand(query4, con5);
            int desID = (Int32)com2.ExecuteScalar();
            con4.Open();
            string query2 = "INSERT INTO Advisor(Id, Designation, Salary) VALUES('" + Id1 + "', '" + desID + "', '" + salarytxt.Text + "')";
            SqlDataAdapter SDA2 = new SqlDataAdapter(query2, con4);
            SDA2.SelectCommand.ExecuteNonQuery();
            MessageBox.Show("Data Inserted!");

            con1.Close();
            con2.Close();
            con3.Close();
            con5.Close();
            con4.Close();
            //MessageBox.Show("Data Insered!");
            clear();
            view();
        }

        private void select_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT FirstName+ ' ' + LastName, Contact, Email, DateOfBirth, G.Value AS Gender, D.Value AS Designation, Salary FROM Person JOIN Advisor ON Person.Id = Advisor.Id JOIN LookUP AS G ON Person.gender = G.Id JOIN LookUP AS D ON Advisor.Designation = D.ID ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string Name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            var names = Name.Split(' ');
            name1txt.Text = names[0];
            name2txt.Text = names[1];
            contacttxt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            emailtxt.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            // dateTimePicker1.Value = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            salarytxt.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();

            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            string command = "SELECT Id FROM Person WHERE Email = '" + emailtxt.Text + "'";
            SqlCommand cmd = new SqlCommand(command, conn);
            id = (Int32)cmd.ExecuteScalar();
        }

        private void update_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            SqlConnection con2 = new SqlConnection(con);
            SqlConnection con3 = new SqlConnection(con);
            SqlConnection con4 = new SqlConnection(con);

            con1.Open();
            string query = string.Format ("SELECT Id from LookUp WHERE Value = '{0}' AND Category = 'DESIGNATION'", comboBox2.Text);
            SqlCommand cmd = new SqlCommand(query, con1);
            int desId = (Int32)cmd.ExecuteScalar();

            con2.Open();
            string query1 = "UPDATE Advisor SET Designation = '" + desId+ "', Salary = '"+salarytxt.Text+"' WHERE Id = '" + id + "' ";
            SqlDataAdapter SDA1 = new SqlDataAdapter(query1, con2);
            SqlCommand com1 = new SqlCommand(query1, con2);
            SDA1.SelectCommand.ExecuteNonQuery();

            con4.Open();
            string query3 = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}' AND Category = 'GENDER'", comboBox1.Text);
            //SqlDataAdapter SDA3 = new SqlDataAdapter(query3, con4);
            SqlCommand com2 = new SqlCommand(query3, con4);
            int Id1 = (Int32)com2.ExecuteScalar();
            //SDA3.SelectCommand.ExecuteNonQuery();

            con3.Open();
            string query2 = "UPDATE Person SET FirstName = '" + name1txt.Text + "', LastName = '" + name2txt.Text + "', Contact = '" + contacttxt.Text + "', Email = '" + emailtxt.Text + "', DateOfBirth = '" + dateTimePicker1.Value + "', Gender = '" + Id1 + "' WHERE Id = '" + id + "'";
            SqlDataAdapter SDA2 = new SqlDataAdapter(query2, con3);
            SDA2.SelectCommand.ExecuteNonQuery();

            MessageBox.Show("Data Updated!");
            con2.Close();
            con3.Close();
            con4.Close();
            clear();
            view();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You want to deleted this data", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection con1 = new SqlConnection(con);
                SqlConnection con2 = new SqlConnection(con);
                con1.Open();
                string query = "SELECT Id FROM Person WHERE Email = '" + emailtxt.Text + "'";
                SqlCommand com = new SqlCommand(query, con1);
                int Id = (Int32)com.ExecuteScalar();

                con2.Open();
                string query1 = "DELETE FROM Advisor WHERE Id = '" + Id + "'";
                SqlDataAdapter SDA = new SqlDataAdapter(query1, con2);
                SqlCommand com1 = new SqlCommand(query1, con2);
                SDA.SelectCommand.ExecuteNonQuery();

                con1.Close();
                con2.Close();
                MessageBox.Show("Data deleted!");
            }
            else
            {
                MessageBox.Show("Data not deleted!", "Delete data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            clear();
            view();
        }

        //validations

        private void emailtxt_TextChanged(object sender, EventArgs e)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (Regex.IsMatch(emailtxt.Text, pattern))
            {
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(this.emailtxt, "Please provide valid email address");
                return;
            }
        }

        private void salarytxt_TextChanged(object sender, EventArgs e)
        {
            if (salarytxt.Text.Length < 18)
            {
                errorProvider2.Clear();
            }
            else
            {
                errorProvider2.SetError(this.salarytxt, "you enetred the salary that is out of range");
                return;
            }
        }

        private void contacttxt_TextChanged(object sender, EventArgs e)
        {
            if (contacttxt.Text.Length != 11)
            {
                errorProvider3.Clear();
            }
            else
            {
                errorProvider3.SetError(this.contacttxt, "Please enter valid contact number");
                return;
            }
        }

        private void name1txt_TextChanged(object sender, EventArgs e)
        {
            string pattern = "^[a-zA-Z ]+$";
            if (Regex.IsMatch(name1txt.Text, pattern))
            {
                errorProvider4.Clear();
            }
            else
            {
                errorProvider4.SetError(this.name1txt, "Please provide valid name");
                return;
            }
        }

        private void name2txt_TextChanged(object sender, EventArgs e)
        {
            string pattern = "^[a-zA-Z ]+$";
            if (Regex.IsMatch(name1txt.Text, pattern))
            {
                errorProvider5.Clear();
            }
            else
            {
                errorProvider5.SetError(this.name1txt, "Please provide valid name");
                return;
            }
        }
       private void clear()
        {
            name1txt.Text = "";
            name2txt.Text = "";
            salarytxt.Text = "";
            emailtxt.Text = "";
            contacttxt.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.Text = "";
            comboBox2.Text = "";
        }
       private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT FirstName+ ' ' + LastName, Contact, Email, DateOfBirth, G.Value AS Gender, D.Value AS Designation, Salary FROM Person JOIN Advisor ON Person.Id = Advisor.Id JOIN LookUP AS G ON Person.gender = G.Id JOIN LookUP AS D ON Advisor.Designation = D.ID ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }
    }
}
