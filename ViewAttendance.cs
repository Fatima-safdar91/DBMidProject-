﻿using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBProject
{
    public partial class ViewAttendance : Form
    {
        string selectedDate;
        public ViewAttendance()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ViewAttendance_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            LoadStudentAttendance();
            LoadComboBoxItems();
        }
        private void LoadComboBoxItems()
        {
            var connection = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("SELECT DISTINCT AttendanceDate FROM ClassAttendance JOIN StudentAttendance ON StudentAttendance.AttendanceId = ClassAttendance.Id", connection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetSqlDateTime(0).ToString());
            }

            reader.Close();
        }

        private void LoadStudentAttendance()
        {
            var connection = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand($"SELECT CONCAT(FirstName, LastName) AS NAME, RegistrationNumber, Lookup.Name AS STATUS, AttendanceDate FROM ClassAttendance JOIN StudentAttendance ON StudentAttendance.AttendanceId = ClassAttendance.Id JOIN Student ON StudentAttendance.StudentId = Student.Id JOIN Lookup ON LookupId = StudentAttendance.AttendanceStatus WHERE ClassAttendance.AttendanceDate = '{selectedDate}'", connection);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.DefaultCellStyle.ForeColor = Color.DarkBlue;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDate = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            LoadStudentAttendance();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ExportToPDF(DataGridView dgv, string name, string l, string marks)
        {
            try
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(document, new FileStream(name + ".pdf", FileMode.CreateNew));
                document.Open();
                iTextSharp.text.Font headingFont = FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD);
                Paragraph heading = new Paragraph(name, headingFont);
                heading.Alignment = Element.ALIGN_CENTER;
                heading.SpacingBefore = 10f;
                heading.SpacingAfter = 10f;

                document.Add(heading);

                LineSeparator line = new LineSeparator();
                document.Add(line);

                iTextSharp.text.Font headingFont2 = FontFactory.GetFont("Times New Roman", 14, iTextSharp.text.Font.BOLD);
                Paragraph heading2 = new Paragraph(marks, headingFont2);
                heading2.Alignment = Element.ALIGN_LEFT;
                heading2.SpacingBefore = 10f;
                heading2.SpacingAfter = 10f;

                document.Add(heading2);



                iTextSharp.text.Font courseFont = FontFactory.GetFont("Times New Roman", 12);
                Paragraph course = new Paragraph(l, courseFont);

                course.Alignment = Element.ALIGN_CENTER;
                course.IndentationLeft = 55f;
                course.SpacingAfter = 20f;
                document.Add(course);

                LineSeparator line2 = new LineSeparator();
                document.Add(line2);



                PdfPTable table = new PdfPTable(dgv.Columns.Count);
                table.WidthPercentage = 100;
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Index == dataGridView1.Rows.Count)
                    {
                        continue;

                    }
                    else
                    {
                        try
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {

                                if (cell.Value == null)
                                {
                                    continue;
                                    MessageBox.Show("Fill all the columns of table (status) it can not be null");
                                }
                                else
                                {
                                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
                                    table.AddCell(pdfCell);
                                }
                            }
                        }
                        catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

                    }


                }
                document.Add(table);
                document.Close();
            }
            catch (Exception exp) { }//MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null && comboBox1.Text != String.Empty)
            {
                string dateString = comboBox1.Text;
                DateTime dateonly;
                DateTime.TryParse(dateString, out dateonly);

                string datestring2 = dateonly.ToString("yyyy-MM-dd");
                // DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", CultureInfo.InvariantCulture); string namex = "Total Result of a " + comboBox2.Text + " in " + LBLX.Text;
                string namx = " Student Attendance Report (" + datestring2 + ")";
                string linex = "Attendance Report of Students on" + comboBox1.Text;
                string date2 = "Attendance Date " + datestring2;

                ExportToPDF(dataGridView1, namx, linex, date2);
                MessageBox.Show("Report Generated");
            }
            else { MessageBox.Show("Select the record first to generate report"); }

        }
    }
}
