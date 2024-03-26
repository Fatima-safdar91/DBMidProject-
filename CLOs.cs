using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
/*using iTextSharp.tool.xml.html.head;*/

namespace DBProject
{
    public partial class CLOs : Form
    {
        bool isUpdate = false;
        string cloName;
        int cloId;

        public CLOs()
        {
            InitializeComponent();
        }

        private void CLOs_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2023, 12, 31);
            dateTimePicker2.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker2.MaxDate = new DateTime(2023, 12, 31);

            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.HeaderText = "Update";
            updateColumn.Text = "Update";
            updateColumn.UseColumnTextForButtonValue = true;

            datagridView.Columns.Add(updateColumn);

            view();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = String.Empty;
        }
        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from Clo", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime1 = dateTimePicker1.Value;
            string dateCreated = selectedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime selectedDateTime2 = dateTimePicker2.Value;
            string dateUpdated = selectedDateTime2.ToString("yyyy-MM-dd HH:mm:ss");

            bool checkDate = false;
            string dateFormat = "yyyy-MM-dd";
            bool validDate1 = DateTime.TryParseExact(dateCreated, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime1);
            bool validDate2 = DateTime.TryParseExact(dateUpdated, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDateTime2);

            if (validDate1 && validDate2)
            {
                if (selectedDateTime2 > selectedDateTime1 || selectedDateTime2 == selectedDateTime1)
                {
                    checkDate = true;
                }
            }

            if (!isUpdate)
            {
                var con = Configuration.getInstance().getConnection();
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Clo values (@title,@dateC,@dateUI)", con);
                cmd.Parameters.AddWithValue("@title", textBox3.Text);
                cmd.Parameters.AddWithValue("@dateC", dateCreated);
                cmd.Parameters.AddWithValue("@dateUI", dateCreated);
                cmd.ExecuteNonQuery();
                MessageBox.Show(" Added  Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
                textBox3.Text = String.Empty;
            }
            else if (isUpdate)
            {
                // if (checkDate)
                // {
                var con2 = Configuration.getInstance().getConnection();
                con2.Open();
                SqlCommand cmd2 = new SqlCommand("Update Clo Set Name=@title,DateCreated=@dateC,DateUpdated=@dateU where Id=@ID", con2);
                cmd2.Parameters.AddWithValue("@title", textBox3.Text);
                cmd2.Parameters.AddWithValue("@dateC", dateCreated);
                cmd2.Parameters.AddWithValue("@dateU", dateUpdated);
                cmd2.Parameters.AddWithValue("@ID", cloId);
                cmd2.ExecuteNonQuery();
                MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con2.Close();
                isUpdate = false;
                textBox3.Text = String.Empty;
                // }
                // else
                // {
                //     MessageBox.Show("Updated date must be greater than or equal to created date");
                // }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            view();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExportToPDF(datagridView);
        }
        private void ExportToPDF(DataGridView dgv)
        {
            try
            {
                Document document = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(document, new FileStream("Total CLO's.pdf", FileMode.Create));
                document.AddHeader("Header", "Report of CLo's list");
                document.AddHeader("Date", DateTime.Now.ToString());
                document.Open();

                PdfPTable table = new PdfPTable(dgv.Columns.Count);

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Index == datagridView.Rows.Count - 1)
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
                                   // MessageBox.Show("Fill all the columns of table (status) it can not be null");
                                }
                                else
                                {
                                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString()));
                                    table.AddCell(pdfCell);

                                    MessageBox.Show("Report Generated");
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                           // MessageBox.Show("Fill all the columns of table (status) it can not be null");
                        }
                    }
                }
                document.Add(table);
                document.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Fill all the columns of table (status) it can not be null");
            }
        }


        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = datagridView.CurrentCell.ColumnIndex;

            if (index == 0)
            {
                textBox3.Text = cloName;
                isUpdate = true;
            }
        }

        private void datagridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                cloId = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[2].Value.ToString());
                cloName = datagridView.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch (Exception exp) { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show();
        }
    }
}
