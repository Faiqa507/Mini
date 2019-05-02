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
    public partial class Form6 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";

        public Form6()
        {
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            SqlConnection conn2 = new SqlConnection(con);

            conn.Open();
            string query = "SELECT Id FROM [Group] WHERE Id NOT IN (SELECT GroupId FROM GroupEvaluation)";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
            }

            conn2.Open();
            string query1 = "SELECT Id FROM Evaluation";
            SqlCommand cmd2 = new SqlCommand(query1, conn2);
            //cmd0.CommandText = query1;
            SqlDataReader reader1 = cmd2.ExecuteReader();
            while (reader1.Read())
            {
                comboBox2.Items.Add(reader1[0]);
            }

           
            conn2.Close();
            conn.Close();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            
            string Groupnum = comboBox1.Text;

            string query = "SELECT Id FROM Evaluation";
            SqlCommand cmd = new SqlCommand(query, conn);
            int EvalId = (Int32)cmd.ExecuteScalar();



            cmd.CommandText = "INSERT INTO GroupEvaluation(GroupId, EvaluationId, ObtainedMarks, EvaluationDate) Values('" + Convert.ToInt32(Groupnum) + "', '" + EvalId + "', '"+ObMarkstxt.Text+"','" + DateTime.Now + "')";
            cmd.ExecuteNonQuery();

            view();
            clear();
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
            String query = "SELECT GroupId AS [Group No.], EvaluationId, ObtainedMarks FROM GroupEvaluation";
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
            ObMarkstxt.Text = "";

        }
        // Delete button
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You want to deleted this data", "Delete data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(con);
                conn.Open();
                
                string query = string.Format("DELETE FROM GroupEvaluation WHERE GroupId  ='" + comboBox1.Text + "' AND EvaluationId = '" + comboBox2.Text + "'");
                SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
                SqlCommand com1 = new SqlCommand(query, conn);
                SDA.SelectCommand.ExecuteNonQuery();
                MessageBox.Show("Data deleted!");
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
            ObMarkstxt.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        //UPDATE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(con);
            conn.Open();

            string query = string.Format("UPDATE GroupEvaluation SET ObtainedMarks = '"+ObMarkstxt.Text+"' WHERE GroupId  ='" + comboBox1.Text + "' AND EvaluationId = '" + comboBox2.Text + "'");
            SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
            SqlCommand com1 = new SqlCommand(query, conn);
            SDA.SelectCommand.ExecuteNonQuery();
            MessageBox.Show("Data Updated!");
            conn.Close();
            view();
            clear();
        }

        private void ObMarkstxt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
