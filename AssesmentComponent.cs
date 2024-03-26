/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DBProject
{
    public partial class AssesmentComponent : Form
    {
        int RubricID;
        int AssessmentID;
        bool isDateValid = false;
        string dateCreated, dateUpdated;
        bool isMarksValid = false;
        bool isNameValid = false;
        bool isUpdateChecked = false;
        int componentId;
        string componentName;
        int componentMarks;

        public AssesmentComponent()
        {
            InitializeComponent();
        }

        private void LoadRubricData()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Details FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                NameRubric.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
        }
        private void l2()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Details FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                NameRubric.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void LoadRubricIds()
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("Select  id FROM Rubric", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                RubericIDS.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();
            con2.Close();
        }

        private void LoadAssessmentIds()
        {
            using (var connection = Configuration.getInstance().getConnection())
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open(); // Open the connection
                SqlCommand cmd = new SqlCommand("Select  Id FROM Assessment", connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        assessmentIds.Add(Convert.ToInt32(reader.GetInt32(0)));
                    }
                }
            }
        }


        private void LoadAssessmentData()
        {
            LoadRubricData();
            LoadRubricIds();
            comboBox2.DataSource = rubricNames;
        }

        private void LoadAssessmentIds()
        {
            using (var connection = Configuration.getInstance().getConnection())
            {
                SqlCommand cmd = new SqlCommand("Select  Id FROM Assessment", connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    assessments.Add(reader.GetString(0));
                }
                reader.Close();
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadAssessmentComboBoxData()
        {
            LoadAssessmentData();
            LoadAssessmentIds();
            comboBox1.DataSource = assessments;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void setDateTimePickerLimits()
        {
            dateTimePicker1.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2023, 12, 31);
            dateTimePicker2.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker2.MaxDate = new DateTime(2023, 12, 31);
        }

        private void AssesmentComponent_Load(object sender, EventArgs e)
        {
            LoadAssessmentComboBoxData();
            setDateTimePickerLimits();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = String.Empty;
            textBox1.Text = String.Empty;
        }
        private string CheckComponentName(string name)
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"IF (SELECT MAX(1) FROM AssessmentComponent WHERE AssessmentComponent.Name = '{name}' AND AssessmentId = {AssessmentID}) > 0 BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", connection);
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

        private string CheckMarks(int marks)
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"DECLARE @x AS INT = (SELECT SUM(AssessmentComponent.TotalMarks) FROM Assessment JOIN AssessmentComponent ON AssessmentId = Assessment.Id WHERE AssessmentId = {AssessmentID}) " +
                                            $"DECLARE @y AS INT = (SELECT DISTINCT (Assessment.TotalMarks) FROM Assessment JOIN AssessmentComponent ON AssessmentId = Assessment.Id WHERE AssessmentId = {AssessmentID}) " +
                                            $"IF @x + {marks} > @y BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", connection);

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


        private void button1_Click(object sender, EventArgs e)
        {
            setDateTimeOnBtnClick();

            if (!isDateValid && isMarksValid && isNameValid)
            {
                if (componentMarksTextBox.Text != String.Empty && componentNameTextBox.Text != String.Empty)
                {
                    string marksCheck = CheckMarks(Convert.ToInt32(componentMarksTextBox.Text));
                    string nameCheck = CheckComponentName(componentNameTextBox.Text);

                    if (!isUpdateChecked && nameCheck != "1" && marksCheck != "1")
                    {
                        var connection = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("Insert into AssessmentComponent values (@name, @RId, @marks, @dateC, @dateU, @AssID)", connection);
                        cmd.Parameters.AddWithValue("@marks", componentMarksTextBox.Text);
                        cmd.Parameters.AddWithValue("@name", componentNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@AssID", AssessmentID.ToString());
                        cmd.Parameters.AddWithValue("@RId", RubricID.ToString());
                        cmd.Parameters.AddWithValue("@dateC", dateCreated);
                        cmd.Parameters.AddWithValue("@dateU", dateUpdated);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        connection.Open();
                        componentMarksTextBox.Text = String.Empty;
                        componentNameTextBox.Text = String.Empty;
                    }
                    else if (isUpdateChecked)
                    {
                        label1.Visible = true;
                        dateTimePicker2.Visible = true;
                        var connection2 = Configuration.getInstance().getConnection();
                        SqlCommand cmd2 = new SqlCommand("Update AssessmentComponent Set Name = @name, RubricId = @RId, TotalMarks = @marks, DateUpdated = @dateU, AssessmentId = @AssID where Id = @ID", connection2);
                        cmd2.Parameters.AddWithValue("@marks", componentMarksTextBox.Text);
                        cmd2.Parameters.AddWithValue("@name", componentNameTextBox.Text);
                        cmd2.Parameters.AddWithValue("@AssID", AssessmentID.ToString());
                        cmd2.Parameters.AddWithValue("@RId", RubricID.ToString());
                        cmd2.Parameters.AddWithValue("@dateC", dateCreated);
                        cmd2.Parameters.AddWithValue("@dateU", dateUpdated);
                        cmd2.Parameters.AddWithValue("@ID", componentId);
                        cmd2.ExecuteNonQuery();
                        MessageBox.Show("Updated Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection2.Close();
                        componentMarksTextBox.Text = String.Empty;
                        componentNameTextBox.Text = String.Empty;
                        isUpdateChecked = false;
                    }

                    if (nameCheck == "1")
                    {
                        MessageBox.Show("Already Exist");
                    }
                }
                else
                {
                    MessageBox.Show("Fill the data First");
                }
            }
            else
            {
                MessageBox.Show("Invalid data. Please check your input.");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAssessmentId = assessmentIds[comboBox1.SelectedIndex];
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedRubricId = rubricIds[comboBox2.SelectedIndex];
            LoadRubricData();
            l2();
            LoadRubricIds();

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (componentMarksTextBox.Text == string.Empty)
            {
                nameLabel.Text = "Enter the name";
                isNameValid = false;
            }

            if (componentMarksTextBox.Text.Any(ch => !char.IsLetter(ch)))
            {
                nameLabel.Text = "Allowed characters: a-Z";
                isNameValid = false;
            }
            else
            {
                isNameValid = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = datagridView.CurrentCell.ColumnIndex;

            if (index == 0)
            {
                componentNameTextBox.Text = componentName;
                componentMarksTextBox.Text = componentMarks.ToString();
                isUpdateChecked = true;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            view();
        }

        private void setDateTimeOnBtnClick()
        {
            DateTime selectedDateTime1 = dateTimePicker1.Value;
            dateCreated = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            dateUpdated = selectedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");

            string dateFormat = "yyyy-MM-dd";
            bool validDate1 = DateTime.TryParseExact(dateCreated, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime1);
            bool validDate2 = DateTime.TryParseExact(dateUpdated, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime2);

            if (validDate1 && validDate2)
            {
                if (selectedDateTime2 > selectedDateTime1 || selectedDateTime2 == selectedDateTime1)
                {
                    isDateValid = true;
                }
            }
        }
    }
}
*/