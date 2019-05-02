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
    public partial class Form8 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";

        public Form8()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form8_Load(object sender, EventArgs e)
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
            string query1 = "SELECT FirstName + ' ' + LastName AS Name FROM Person JOIN Advisor ON Person.Id = Advisor.Id";
            SqlCommand cmd2 = new SqlCommand(query1, conn2);
            //cmd0.CommandText = query1;
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                comboBox1.Items.Add(reader1[0]);
            }

            //conn3.Open();
            SqlConnection conn3 = new SqlConnection(con);
            conn3.Open();
            string query2 = "SELECT Value FROM LookUP  WHERE Category = 'Advisor_Role'";
            SqlCommand cmd3 = new SqlCommand(query2, conn3);
            //cmd0.CommandText = query1;
            SqlDataReader reader3 = cmd3.ExecuteReader();
            while (reader3.Read())
            {
                comboBox3.Items.Add(reader3[0]);
            }

            conn1.Close();
            conn3.Close();
            conn.Close();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            SqlConnection conn1 = new SqlConnection(con);
            SqlConnection conn2 = new SqlConnection(con);
            conn.Open();
            conn1.Open();
            conn2.Open();
            string Ptitle = comboBox2.Text;
            string AdvisorName = comboBox1.Text;
            //string AdvisorRole = comboBox3.Text;


            string query = "SELECT Id FROM Project WHERE Title = '" + Ptitle + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int Projectid = (Int32)cmd.ExecuteScalar();

            string query1 = "SELECT Advisor.Id FROM Advisor JOIN Person ON Advisor.Id = Person.Id WHERE FirstName + ' ' + LastName = '" +comboBox1.Text+ "'";
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            int Advisorid = (Int32)cmd1.ExecuteScalar();

            string query2 = string.Format("SELECT Id FROM LookUp WHERE Value = '{0}' AND Category = 'ADVISOR_ROLE'", comboBox3.Text);
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            int id = (Int32)cmd2.ExecuteScalar();


            cmd.CommandText = "INSERT INTO ProjectAdvisor( AdvisorId,ProjectId, AdvisorRole, AssignmentDate) Values(  '" + Advisorid + "','" + Projectid + "', '"+ id + "', '" + DateTime.Now + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            conn1.Close();
            conn2.Close();

            view();
            clear();
        }



        private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT FirstName + ' '  + LastName AS Advisor, Title AS Project, AdvisorRole, AssignmentDate FROM ProjectAdvisor JOIN Advisor ON ProjectAdvisor.AdvisorId = Advisor.Id JOIN Person ON Advisor.Id = Person.Id JOIN Project ON ProjectAdvisor.ProjectId = Project.Id JOIN Lookup ON ProjectAdvisor.AdvisorRole = LookUp.Id";
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
            comboBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            view();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You want to deleted this data", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(con);
                SqlConnection conn1 = new SqlConnection(con);
                conn.Open();
                string query = "SELECT Id FROM Project WHERE Title = '" + comboBox2.Text + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int Projectid = (Int32)cmd.ExecuteScalar();
                string query1 = "SELECT Id FROM Advisor Join Person ON Advisor.Id = Person.Id WHERE FirstName+ ' ' + LastName = '" + comboBox2.Text + "'";
                
                SqlCommand cmd1 = new SqlCommand(query1, conn1);
                int Advisorid = (Int32)cmd.ExecuteScalar();

                cmd.CommandText = "DELETE FROM ProjectAdvisor WHERE ProjectId  ='" + Projectid + "' AND AdvisorId = '" + Advisorid + "'";
                cmd.ExecuteNonQuery();
                conn.Close();
                conn1.Close();
            }
            else
            {
                MessageBox.Show("Data not deleted!", "Delete data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            view();
            clear();
        }
    }
}
