using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace student
{
    public partial class GroupStudent : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";
        public GroupStudent()
        {
            InitializeComponent();
            string query0 = "INSERT INTO [Group](Created_On) Values('" + DateTime.Now + "')";
            SqlConnection conn0 = new SqlConnection(con);
            conn0.Open();
            SqlCommand cmd0 = new SqlCommand(query0, conn0);
            cmd0.ExecuteNonQuery();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            SqlConnection con1 = new SqlConnection(con);
            conn.Open();
            string regno = comboBox2.Text;
            string Groupnum = comboBox1.Text;

            string query = "SELECT Id FROM Student WHERE RegistrationNo = '" + comboBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int studentid = (Int32)cmd.ExecuteScalar();

            string query1 = "SELECT StudentId FROM GroupStudent WHERE GroupId = '" + Convert.ToInt32(Groupnum) + "'";
            cmd.CommandText = query1;
            SqlDataReader reader = cmd.ExecuteReader();
            List<int> list = new List<int>();
            while (reader.Read())
            {
                list.Add(Convert.ToInt32(reader[0].ToString()));
            }
            SqlConnection conn0 = new SqlConnection(con);
            conn0.Open();
            SqlCommand cmd0 = new SqlCommand("", conn0);
            if (list.Contains(studentid))
            {
                

                string updatequery = "UPDATE GroupStudent SET Status = 3 WHERE GroupId = '" + Convert.ToInt32(Groupnum) + "' AND StudentId = '" + studentid + "'";
                cmd0.CommandText = updatequery;
                cmd0.ExecuteNonQuery();
                
            }
            else
            {
                /*con1.Open();
                string query4 = string.Format("SELECT Id from LookUp WHERE Value = '{0}' AND Category = 'STATUS'", comboBox2.Text);
                SqlCommand cmd4 = new SqlCommand(query, con1);
                int Id = (Int32)cmd.ExecuteScalar();*/

                cmd0.CommandText = "INSERT INTO GroupStudent(GroupId, StudentId, Status, AssignmentDate) Values('" + Convert.ToInt32(Groupnum) + "', '" + studentid + "', 4 , '" + DateTime.Now + "')";
                cmd0.ExecuteNonQuery();
                //con1.Close();
            }
            view();
            clear();
            conn0.Close();
            conn.Close();
        }

        private void GroupStudent_Load(object sender, EventArgs e)
        {
            

            SqlConnection conn = new SqlConnection(con);
            SqlConnection conn1 = new SqlConnection(con);
           // SqlConnection con1 = new SqlConnection(con);
            SqlConnection conn2 = new SqlConnection(con);
            //SqlConnection con2 = new SqlConnection(con);

            /*con1.Open();
            string queryy = string.Format("SELECT Id from LookUp WHERE Value = '{0}' AND Category = 'STATUS'", comboBox2.Text);
            SqlCommand cmd4 = new SqlCommand(queryy, con1);
            int Id = (Int32)cmd4.ExecuteScalar();*/

            conn.Open();
            string query = "SELECT RegistrationNo FROM Student WHERE Student.id NOT IN(SELECT Studentid FROM GroupStudent WHERE Status = 3) OR Student.Id IN (SELECT StudentId FROM GroupStudent WHERE Status = 4)";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader[0]);
            }
            
            conn2.Open();
            string query1 = "SELECT DISTINCT Id FROM [Group] WHERE Id NOT IN (SELECT GroupId FROM GroupStudent) OR Id IN (SELECT GroupId FROM GroupStudent Group By GroupId Having Count(*) < 5 OR Id IN (SELECT GroupId FROM GroupStudent Group By Groupid, Status Having Status =))";
            SqlCommand cmd2 = new SqlCommand(query1, conn2);
            //cmd0.CommandText = query1;
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                comboBox1.Items.Add(reader1[0]);
            }

            conn1.Close();
            conn2.Close();
            conn.Close();
           // con1.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            view();
            
        }

        private void view()
        {
            SqlConnection con1 = new SqlConnection(con);
            con1.Open();
            String query = "SELECT GroupId AS [Group No.], RegistrationNo, Value FROM GroupStudent JOIN Student ON GroupStudent.Studentid = Student.Id JOIN Lookup ON GroupStudent.Status = Lookup.Id";
            SqlDataAdapter SDA = new SqlDataAdapter(query, con1);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            con1.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            string query = "SELECT Id FROM Student WHERE RegistrationNo = '" + comboBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int studentid = (Int32)cmd.ExecuteScalar();
            cmd.CommandText = "UPDATE GroupStudent SET Status = 4 WHERE  GroupId = '" + comboBox1.Text + "' AND StudentId = '" + studentid + "'";
            cmd.ExecuteNonQuery();
            view();
            clear();
            conn.Close();
            
        }
        private void clear()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
        }
    }
}
