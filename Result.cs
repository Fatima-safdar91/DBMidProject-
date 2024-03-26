using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBProject
{
    public partial class Result : Form
    {
        int id; string name;
        string x;
        int am;
        int acm;
        public Result()
        {
            InitializeComponent();
        }
        public Result(int id, string name)
        {
            InitializeComponent();
            this.id = id;
            this.name = name;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StudentResult studentResult = new StudentResult();
            this.Hide();
            studentResult.Show();
        }
        private void load_student_result()
        {

            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand($"  select  s.FirstName as StudentName, s.RegistrationNumber,Assessment.Title,ac.Name as AssComp,  ac.TotalMarks as totalmarks,  rl.MeasurementLevel as O_Level,  max(rl2.MeasurementLevel)  as MaxLevel ,  Cast (CAST(rl.MeasurementLevel AS FLOAT)/ max( CAST(rl2.MeasurementLevel AS FLOAT) )* ac.TotalMarks as float) as Obtainedarks    from StudentResult as st    join Student as s    on st.StudentId=s.Id    join AssessmentComponent as ac    on ac.Id=st.AssessmentComponentId    join Rubric as r    on r.Id=ac.RubricId    join RubricLevel as rl    on rl.Id=st.RubricMeasurementId    join RubricLevel as rl2    on rl2.RubricId=r.Id    join Assessment    on Assessment.Id=ac.AssessmentId     where Assessment.Title='{LBLX.Text}'    and s.RegistrationNumber='{x}'       group by ac.Name,ac.TotalMarks,rl.MeasurementLevel,s.FirstName,s.RegistrationNumber,Assessment.Title  \r\n", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.DarkBlue;


        }
        private void load_basic_data()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand($"   select \r\ns.FirstName as StudentName, s.RegistrationNumber,Assessment.Title,\r\nac.Name as AssComp,\r\n  ac.TotalMarks as totalmarks,\r\n  rl.MeasurementLevel as O_Level,\r\n  max(rl2.MeasurementLevel)  as MaxLevel ,\r\n  Cast (CAST(rl.MeasurementLevel AS FLOAT)/ max( CAST(rl2.MeasurementLevel AS FLOAT) )* ac.TotalMarks as float) as Obtainedarks\r\n\r\n  from StudentResult as st\r\n  join Student as s\r\n  on st.StudentId=s.Id\r\n  join AssessmentComponent as ac\r\n  on ac.Id=st.AssessmentComponentId\r\n  join Rubric as r\r\n  on r.Id=ac.RubricId\r\n  join RubricLevel as rl\r\n  on rl.Id=st.RubricMeasurementId\r\n  join RubricLevel as rl2\r\n  on rl2.RubricId=r.Id\r\n  join Assessment\r\n  on Assessment.Id=ac.AssessmentId\r\n    where Assessment.Id={id}   group by ac.Name,ac.TotalMarks,rl.MeasurementLevel,s.FirstName,s.RegistrationNumber,Assessment.Title", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.DarkBlue;
        }
        private void Result_Load(object sender, EventArgs e)
        {

            load_basic_data();
            load_student_result();

          // LBLX.Text = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            label1.Visible = true;
            comboBox2.Visible = true;
            loads_box();
        }
        private void loads_box()
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("Select  student.RegistrationNumber FROM student", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(reader2.GetString(0));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();



        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            x = comboBox2.Items[comboBox2.SelectedIndex].ToString();
           load_student_result();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            label1.Visible = false;
            comboBox2.Visible = false;
            load_basic_data();
        }
        private void ExportToPDF(DataGridView dgv, string name, string l, string marks, string o)
        {
            try
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(document, new FileStream(name + ".pdf", FileMode.Create));
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



                iTextSharp.text.Font headingFont3 = FontFactory.GetFont("Times New Roman", 14, iTextSharp.text.Font.BOLD);
                Paragraph heading3 = new Paragraph(o, headingFont3);
                heading3.Alignment = Element.ALIGN_LEFT;
                heading3.SpacingBefore = 10f;
                heading3.SpacingAfter = 10f;

                document.Add(heading3);

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
                    if (row.Index == datagridView.Rows.Count)
                    {
                        continue;

                    }
                    else
                    {
                        try
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {

                                //if (cell.Value == null)
                                //{
                                //    continue;
                                //    MessageBox.Show("Fill all the columns of table (status) it can not be null");
                                //}
                                //else
                                //{
                                PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
                                table.AddCell(pdfCell);
                                //}
                            }
                        }
                        catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }

                    }


                }
                document.Add(table);
                document.Close();
            }
            catch (Exception exp) { MessageBox.Show("Fill all the columns of table (status) it can not be null"); }
            // Close the document
        }
        private void load_mark_ass()
        {

            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand($"select TotalMarks from Assessment where Assessment.Id={id}", con2);


            am = (int)cmd2.ExecuteScalar();


            cmd2.ExecuteNonQuery();




        }
        private void load_mark_ass_obtained()
        {

            try
            {
                var con2 = Configuration.getInstance().getConnection();

                SqlCommand cmd2 = new SqlCommand($"select sum (AssessmentComponent.TotalMarks) from AssessmentComponent join StudentResult on AssessmentComponent.Id=StudentResult.AssessmentComponentId join Assessment on Assessment.Id=AssessmentComponent.AssessmentId join student on StudentResult.StudentId=Student.Id  where AssessmentId={id} and Student.RegistrationNumber='{comboBox2.Text}'\r\n", con2);

                acm = (int)cmd2.ExecuteScalar();

                cmd2.ExecuteNonQuery();

            }
            catch (Exception exp) { }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (datagridView.DataSource != null && comboBox2.Text != String.Empty)
            {
                acm = 0;
                am = 0;
                load_mark_ass();
                load_mark_ass_obtained();
                string namex = "Total Result of a " + comboBox2.Text + " in " + LBLX.Text;
                string linex = "Result of " + comboBox2.Text + " in " + LBLX.Text + " according to Assessment Component";
                string m = "Total Marks : " + am.ToString();
                string mm = "Obtained Marks: " + acm.ToString();
                ExportToPDF(datagridView, namex, linex, m, mm);
                MessageBox.Show("Report Generated");
            }
            else { MessageBox.Show("Select the record first to generate report"); }
        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            load_basic_data();
            //load_student_result();
            
        }
    }
}
