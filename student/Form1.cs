using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace student
{
    public partial class Form1 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";
       
        internal object Mdiparent;
        int id;

        public Form1()
        {
            InitializeComponent();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            SqlConnection con2 = new SqlConnection(con);
            SqlConnection con3 = new SqlConnection(con);
            SqlConnection con4 = new SqlConnection(con);


            con4.Open();
            string query3 = string.Format("SELECT Id FROM Lookup WHERE Value = '{0}' AND Category = 'GENDER'", comboBox1.Text);
            //SqlDataAdapter SDA3 = new SqlDataAdapter(query3, con4);
            SqlCommand com1 = new SqlCommand(query3, con4);
            int Id = (Int32)com1.ExecuteScalar();
            //SDA3.SelectCommand.ExecuteNonQuery();

            con2.Open();
            com1.CommandText = "INSERT INTO Person(FirstName, LastName, Contact, Email, DateOfBirth, Gender) VALUES('" + name1txt.Text + "' , '" + name2txt.Text + "', '" + contacttxt.Text + "', '" + emailtxt.Text + "', '" + dateTimePicker1.Value + "', '" + Id + "')";
            com1.ExecuteNonQuery();

            con1.Open();
            string query = "SELECT Id FROM Person WHERE Email = '" + emailtxt.Text + "'";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            SqlCommand com = new SqlCommand(query, con1);
           int Id1 = (Int32)com.ExecuteScalar();
            //SDA.SelectCommand.ExecuteNonQuery();

            con3.Open();
            string query2 = "INSERT INTO Student(Id, RegistrationNo) VALUES('"+Id1+"', '" + regtxt.Text + "')";
            SqlDataAdapter SDA2 = new SqlDataAdapter(query2, con3);
            SDA2.SelectCommand.ExecuteNonQuery();
            MessageBox.Show("Data Inserted!");
            clear();
            view();

            con4.Close();
            con2.Close();
            con1.Close();
            con3.Close();
        }

        private void select_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
           // SqlConnection con2 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT FirstName+ ' ' + LastName, Contact, Email, DateOfBirth, Value AS Gender, RegistrationNo FROM Person JOIN Student ON Person.Id = Student.Id JOIN LookUp ON Person.gender = Lookup.Id";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            
            dataGridView1.DataSource = dt;
            con1.Close();
            
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            /*SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            string query = "SELECT Id FROM Person WHERE Email = '" + emailtxt.Text + "'";
            SqlCommand com = new SqlCommand(query, con1);
            int Id = (Int32)com.ExecuteScalar();
            //Id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString(); */

            string Name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            var names = Name.Split(' ');
            name1txt.Text = names[0];
            name2txt.Text = names[1];
            contacttxt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            emailtxt.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            // dateTimePicker1.Value = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            regtxt.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            string command = "SELECT Id FROM Student WHERE RegistrationNo = '" + regtxt.Text + "'";
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

             con2.Open();
             string query1 = "UPDATE Student SET RegistrationNo = '"+ regtxt.Text + "' WHERE Id = '"+id+"' ";
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
             string query2 = "UPDATE Person SET FirstName = '" + name1txt.Text + "', LastName = '" + name2txt.Text + "', Contact = '" + contacttxt.Text + "', Email = '" + emailtxt.Text + "', DateOfBirth = '" + dateTimePicker1.Value + "', Gender = '" + Id1 + "' WHERE Id = '"+id+"'";
             SqlDataAdapter SDA2 = new SqlDataAdapter(query2, con3);
             SDA2.SelectCommand.ExecuteNonQuery();

             
             con1.Close();
             con2.Close();
             con3.Close();
             con4.Close();
            MessageBox.Show("Data Updated!");
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
                string query1 = "DELETE FROM Student WHERE Id = '" +Id + "'";
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

        private void clear()
        {
            regtxt.Text = "";
            name1txt.Text = "";
            name2txt.Text = "";
            emailtxt.Text = "";
            contacttxt.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.Text = "";
            
        }
        private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT RegistrationNo, FirstName+ ' ' + LastName, Contact, Email, DateOfBirth, G.Value AS Gender FROM Student JOIN Person ON Student.Id = Person.Id JOIN LookUP AS G ON Person.gender = G.Id ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void name2_Click(object sender, EventArgs e)
        {

        }

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

        private void contacttxt_TextChanged(object sender, EventArgs e)
        {
            if (contacttxt.Text.Length == 11)
            {
                errorProvider4.Clear();
            }
            else
            {
                errorProvider4.SetError(this.contacttxt, "Please enter valid contact number");
                return;
            }
        }

        private void name1txt_TextChanged(object sender, EventArgs e)
        {
            string pattern = "^[a-zA-Z ]+$";
            if (Regex.IsMatch(name1txt.Text, pattern))
            {
                errorProvider2.Clear();
            }
            else
            {
                errorProvider2.SetError(this.name1txt, "Please provide valid name");
                return;
            }
        }

        private void name2txt_TextChanged(object sender, EventArgs e)
        {
            string pattern = "^[a-zA-Z ]+$";
            if (Regex.IsMatch(name2txt.Text, pattern))
            {
                errorProvider6.Clear();
            }
            else
            {
                errorProvider6.SetError(this.name2txt, "Please provide valid name");
                return;
            }
        }

        private void regtxt_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^\d{4}(-[A-Za-z][A-Za-z])(-\d{2,3})";
            if (Regex.IsMatch(regtxt.Text, pattern))
            {
                errorProvider3.Clear();
            }
            else
            {
                errorProvider3.SetError(this.regtxt, "Please provide valid registration num");
                return;
            }
        }

    }
}
