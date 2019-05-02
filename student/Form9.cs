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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Rectangle = System.Drawing.Rectangle;

namespace student
{
    public partial class Form9 : Form
    {
        string con = "Data Source=FAIQA-PC\\SQLSERVER;Initial Catalog=ProjectA;Persist Security Info=True;User ID=sa;Password= 8200551 ";

        public Form9()
        {
            SqlConnection conn = new SqlConnection(con);
            conn.Open();
            InitializeComponent();
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            
            String cmd = "Select e.Name ,e.TotalMarks, pr.Title , s.RegistrationNo FROM [dbo].Evaluation e JOIN GroupEvaluation ge ON e.Id = ge.EvaluationId JOIN [dbo].[Group] g ON ge.GroupId = g.Id JOIN GroupProject gp ON g.Id =gp.GroupId JOIN Project pr ON gp.ProjectId = pr.Id JOIN GroupStudent gs ON g.Id = gs.GroupId JOIN Student  s ON gs.StudentId=s.Id";
            SqlCommand command = new SqlCommand(cmd, conn);
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbadataset = new DataTable();
                sda.Fill(dbadataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbadataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbadataset);
                this.dataGridView1.AllowUserToAddRows = false;
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exportgridtopdf(dataGridView1, "MarksSheet");
        }




        private void button6_Click(object sender, EventArgs e)
        {

        }
        

        private void Form9_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("List of projects along with advisory board and list of students");
            comboBox1.Items.Add("Marks sheet of projects");
            comboBox1.Items.Add("Group Evaluations remaining");
            comboBox1.Items.Add("Projects with all advisors roles not assigned");
            comboBox1.Items.Add("Advisors not involved in supervision");
            comboBox1.Items.Add("Projects supervised");
            comboBox1.Items.Add("Advisors and their projects count");
            comboBox1.Items.Add("Group evaluations");


        }
            
    
  

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            form.Show();
            this.Close();
        }

        public void exportgridtopdf(DataGridView dqw, string filename)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdftable = new PdfPTable(dqw.Columns.Count);
            pdftable.DefaultCell.Padding = 3;
            pdftable.WidthPercentage = 100;
            pdftable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdftable.DefaultCell.BorderWidth = 1;

            iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
            //add header
            foreach (DataGridViewColumn column in dqw.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdftable.AddCell(cell);
            }

            //add datarow

            foreach (DataGridViewRow row in dqw.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdftable.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }
            var savefiledialoge = new SaveFileDialog();
            savefiledialoge.FileName = filename;
            savefiledialoge.DefaultExt = ".pdf";
            if (savefiledialoge.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefiledialoge.FileName, FileMode.Create))
                {
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    pdfdoc.Add(pdftable);
                    pdfdoc.Close();
                    stream.Close();
                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(con);
            connection.Open();
            string text = comboBox1.SelectedItem.ToString();
            string text1 = "List of projects along with advisory board and list of students";
            string text2 = "Marks sheet of projects";
            string text3 = "Group Evaluations remaining";
            string text4 = "Projects with all advisors roles not assigned";
            string text5 = "Advisors not involved in supervision";
            string text6 = "Projects supervised";
            string text7 = "Advisors and their projects count";
            string text8 = "Group evaluations";
            if (text == text1)
            {
                string display = "SELECT RegistrationNo, CONCAT(S.FirstName, ' ', S.LastName) AS [Student Name], Groupstudent.GroupId AS [Group Number], Title AS [Project], CONCAT(A.FirstName, ' ', A.LastName) AS [Advisor], Value AS [Advisor Role] " +
                    "FROM Person AS S JOIN Student ON S.Id = Student.Id " +
                    "JOIN GroupStudent ON GroupStudent.StudentId = Student.Id " +
                    "JOIN GroupProject ON GroupProject.GroupId = GroupStudent.GroupId " +
                    "JOIN Project ON GroupProject.ProjectId = Project.Id " +
                    "JOIN ProjectAdvisor ON ProjectAdvisor.ProjectId = Project.Id " +
                    "JOIN Advisor ON Advisor.Id = ProjectAdvisor.AdvisorId " +
                    "JOIN Person AS A ON Advisor.Id = A.Id " +
                    "JOIN LookUp ON ProjectAdvisor.AdvisorRole = LookUp.Id";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }

            if (text == text2)
            {
                string display = "SELECT RegistrationNo, CONCAT(S.FirstName, ' ', S.LastName) AS [Student Name], Groupstudent.GroupId AS [Group Number], Title AS [Project], " +
                    "ObtainedMarks , Totalmarks FROM Person AS S JOIN Student ON S.Id = Student.Id " +
                    "JOIN GroupStudent ON GroupStudent.StudentId = Student.Id " +
                    "JOIN GroupProject ON GroupProject.GroupId = GroupStudent.GroupId " +
                    "JOIN Project ON GroupProject.ProjectId = Project.Id " +
                    "JOIN GroupEvaluation ON GroupEvaluation.GroupId = GroupStudent.GroupId " +
                    "JOIN Evaluation ON Evaluation.Id = GroupEvaluation.EvaluationId";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            if (text == text3)
            {
                string display = "SELECT DISTINCT GroupId AS [Group Number], Title AS [Project Title] FROM GroupProject JOIN Project ON GroupProject.ProjectId = Project.Id " +
                    "WHERE GroupId NOT IN(SELECT GroupId FROM GroupEvaluation)";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            if (text == text4)
            {
                string display = "SELECT Title AS [Project Title], Description FROM Project WHERE Id IN (SELECT ProjectId FROM ProjectAdvisor GROUP BY ProjectId HAVING COUNT(*) < 3 )";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            if (text == text5)
            {
                string display = "SELECT * FROM ListOfAdvisors";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;

            }
            if (text == text6)
            {
                string display = "SELECT * FROM ListOfProjectbyEachAdvisors";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
            if (text == text7)
            {
                string display = "SELECT * FROM ListOfStudentsSupervisedbyEachAdvisor";
                SqlCommand cmd = new SqlCommand(display, connection);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            exportgridtopdf(dataGridView1, "MarksSheet");
        }
    }
    }
    

