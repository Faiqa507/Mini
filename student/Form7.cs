using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace student
{
    public partial class Form7 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";

        public Form7()
        {
            InitializeComponent();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            string Ptitle = comboBox2.Text;
            string Groupnum = comboBox1.Text;

            string query = "SELECT Id FROM Project WHERE Title = '" + Ptitle + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int Projectid = (Int32)cmd.ExecuteScalar();



            cmd.CommandText = "INSERT INTO GroupProject(GroupId, ProjectId, AssignmentDate) Values('" + Convert.ToInt32(Groupnum) + "', '" + Projectid + "', '" + DateTime.Now + "')";
            cmd.ExecuteNonQuery();

            view();
            clear();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            SqlConnection conn1 = new SqlConnection(con);
            conn.Open();
            string query = (" SELECT Title From Project");
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader[0]);
            }

            conn1.Open();
            SqlConnection conn2 = new SqlConnection(con);
            conn2.Open();
            string query1 = "SELECT Id FROM [Group] WHERE Id NOT IN (SELECT GroupId FROM GroupProject)";
            SqlCommand cmd2 = new SqlCommand(query1, conn2);
            //cmd0.CommandText = query1;
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                comboBox1.Items.Add(reader1[0]);
            }

            conn1.Close();
            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // comboBox2.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        //VIEW BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            view();
        }
        private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT GroupId AS [Group No.], Title  FROM GroupProject JOIN Project ON GroupProject.ProjectId = Project.Id ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }
        private void clear()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        //DELETE BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You want to deleted this data", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(con);
                conn.Open();
                string query = "SELECT Id FROM Project WHERE Title = '" + comboBox2.Text + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int Projectid = (Int32)cmd.ExecuteScalar();
                cmd.CommandText = "DELETE FROM GroupProject WHERE ProjectId  ='" + Projectid + "' AND GroupId = '" + comboBox1.Text + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                MessageBox.Show("Data not deleted!", "Delete data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
            view();
            clear();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }
    }
}
