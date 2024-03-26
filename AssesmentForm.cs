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

namespace DBProject
{
    public partial class AssesmentForm : Form
    {

        bool isUpdateChecked;
        int assessmentId, assessmentMarks, assessmentWeightage;
        string assessmentTitle;
        bool isTitleValid = false;
        bool isMarksValid = false;
        bool isWeightageValid = false;

        public AssesmentForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string existenceResult = CheckAssessmentExistence();
            DateTime selectedDateTime = dateTimePicker1.Value;
            string sqlDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (isTitleValid && isMarksValid && isWeightageValid)
            {
                if (!isUpdateChecked && existenceResult != "1")
                {
                    var connection = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Assessment VALUES (@title, @date, @Marks, @Weightage)", connection);
                    cmd.Parameters.AddWithValue("@title", textBox3.Text);
                    cmd.Parameters.AddWithValue("@date", sqlDateTime);
                    cmd.Parameters.AddWithValue("@Marks", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Weightage", textBox4.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox3.Text = String.Empty;
                    textBox5.Text = String.Empty;
                    textBox4.Text = String.Empty;

                /*    MessageBox.Show("ADD Assessment Components ");
                    AssesmentComponent newUserControl = new AssesmentComponent();
                    newUserControl.Dock = DockStyle.Fill;
                    this.Parent.Controls.Add(newUserControl);
                    newUserControl.BringToFront();
                    this.Hide();*/
                }
                else if (isUpdateChecked)
                {
                    var connection2 = Configuration.getInstance().getConnection();
                    SqlCommand cmd2 = new SqlCommand("UPDATE Assessment SET Title = @title, DateCreated = @date, TotalMarks = @Marks, TotalWeightage = @Weightage WHERE Id = @ID", connection2);
                    cmd2.Parameters.AddWithValue("@title", textBox3.Text);
                    cmd2.Parameters.AddWithValue("@date", sqlDateTime);
                    cmd2.Parameters.AddWithValue("@Marks", textBox5.Text);
                    cmd2.Parameters.AddWithValue("@Weightage", textBox4.Text);
                    cmd2.Parameters.AddWithValue("@ID", assessmentId);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Updated Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection2.Close();
                    connection2.Open();
                    isUpdateChecked = false;
                    textBox3.Text = String.Empty;
                    textBox5.Text = String.Empty;
                    textBox4.Text = String.Empty;
                }
                else
                {
                    if (existenceResult == "1") { MessageBox.Show("Already Exist"); }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = String.Empty;
            textBox5.Text = String.Empty;
            textBox4.Text = String.Empty;
        }

        private String CheckAssessmentExistence()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"IF (SELECT MAX(1) FROM Assessment WHERE Assessment.Title = '{textBox3.Text}') > 0 BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", connection);
            string result = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetString(0);
            }
            reader.Close();
            cmd.ExecuteNonQuery();
            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            ViewAssessment();
        }

        private void ViewAssessment()
        {
            var connection2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("SELECT * FROM Assessment", connection2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
        }


        private void AssesmentForm_Load(object sender, EventArgs e)
        {

        }
       /* private void label4_Click(object sender, EventArgs e)
        {
            AssessCompC newUserControl = new AssessCompC();
            newUserControl.Dock = DockStyle.Fill;
            this.Parent.Controls.Add(newUserControl);
            newUserControl.BringToFront();
            this.Hide();
        }*/

        private void datagridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                assessmentId = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                assessmentTitle = datagridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                assessmentMarks = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[4].Value.ToString());
                assessmentWeightage = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[5].Value.ToString());
            }
            catch (Exception exp) { }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                label1.Text = "Enter the name";
                isTitleValid = false;
            }
            else if (textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                label1.Text = "Allowed characters: a-z, A-Z";
                isTitleValid = false;
            }
            else
            {
                label1.Text = " ";
                isTitleValid = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                label4.Text = "Enter the marks";
                isMarksValid = false;
            }
            if (textBox5.Text.Any(ch => !char.IsDigit(ch)))
            {
                label4.Text = "Allowed characters: 1-9";
                isMarksValid = false;
            }
            else
            {
                label4.Text = " ";
                isMarksValid = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                label2.Text = "Enter the marks";
                isWeightageValid = false;
            }
            if (textBox4.Text.Any(ch => !char.IsDigit(ch)))
            {
                label2.Text = "Allowed characters: 1-9";
                isWeightageValid = false;
            }
            else
            {
                label2.Text = " ";
                isWeightageValid = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            AssessmentComp comp = new AssessmentComp();
            this.Hide();
            comp.Show();
           /* AssesmentComponent assessmentComponent = new AssesmentComponent();
            this.Hide();
            assessmentComponent.Show();*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show();
        }

        private void datagridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = datagridView.CurrentCell.ColumnIndex;

            if (index == 0)
            {
                textBox3.Text = assessmentTitle;
                textBox5.Text = assessmentMarks.ToString();
                textBox4.Text = assessmentWeightage.ToString();
                isUpdateChecked = true;
                dateTimePicker1.Visible = false;
                label3.Visible = false;
            }
        }

        private void AssessmentC_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2023, 12, 31);

            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.HeaderText = "Update";
            updateColumn.Text = "Update";
            updateColumn.UseColumnTextForButtonValue = true;
            datagridView.Columns.Add(updateColumn);

            ViewAssessment();
            isUpdateChecked = false;
        }
    }
}
