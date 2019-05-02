using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace student
{
    public partial class Form3 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";
        
        internal Form5 Mdiparent;
        int Id;

        public Form3()
        {
            InitializeComponent();
        }
        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            string query = "INSERT INTO Project(Description, Title) VALUES('" + desctxt.Text + "', '" + titletxt.Text + "')";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            SDA.SelectCommand.ExecuteNonQuery();
            con1.Close();
            MessageBox.Show("Data Inserted!");
            clear();
            view();

        }
        private void select_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT Description, Title FROM Project";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            desctxt.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            titletxt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            SqlConnection con2 = new SqlConnection(con);

            con2.Open();
            string query1 = "SELECT Id FROM Project WHERE Description = '" + desctxt.Text + "' AND Title = '" + titletxt.Text + "'";
            SqlCommand com = new SqlCommand(query1, con2);
            Id = (Int32)com.ExecuteScalar();
        }
        private void update_Click(object sender, EventArgs e)
        {
            SqlConnection con1 = new SqlConnection(con);
            
            //SqlDataAdapter SDA2 = new SqlDataAdapter(query1, con2);
            //SDA2.SelectCommand.ExecuteNonQuery(); 

            con1.Open();
            string query = "UPDATE Project SET Description = '"+desctxt.Text+ "', Title = '" +titletxt.Text+ "' WHERE Id='"+Id+ "' ";
            SqlDataAdapter SDA1 = new SqlDataAdapter(query, con1);
            SqlCommand com1 = new SqlCommand(query, con1);
            SDA1.SelectCommand.ExecuteNonQuery();
            con1.Close();
           
            MessageBox.Show("Data Updated!");
            clear();
            view();

        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You want to deleted this data", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection con1 = new SqlConnection(con);
                SqlConnection con2=new SqlConnection(con);
                con1.Open();
                con2.Open();

                string query1 = "SELECT Id FROM Project WHERE Description = '" + desctxt.Text + "'AND Title = '" + titletxt.Text + "'";
                SqlCommand com = new SqlCommand(query1, con2);
                int Id = (Int32)com.ExecuteScalar();

                string query = "DELETE FROM Project WHERE Id = '" + Id + "'";
                SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
                SqlCommand com1 = new SqlCommand(query, con1);
                SDA.SelectCommand.ExecuteNonQuery();
                MessageBox.Show("Data Deleted!");
                con1.Close();
                con2.Close();
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
            desctxt.Text = "";
            titletxt.Text = "";
            
        }
        private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT Description, Title FROM Project ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
