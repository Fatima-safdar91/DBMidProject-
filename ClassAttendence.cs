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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBProject
{
    public partial class ClassAttendence : Form
    {
        bool check_date = false;
        bool check_update = false;
        int id;
        public ClassAttendence()
        {
            InitializeComponent();
        }

        private void ClassAttendence_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("\r\nselect Student.Id , Concat(Student.FirstName,Student.LastName)   as StudentName,RegistrationNumber from Student ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            DataBind();
        }
        private void ClassAttendence_Load_1(object sender, EventArgs e)
        {
           /* var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("\r\nselect Student.Id , Concat(Student.FirstName,Student.LastName)   as StudentName,RegistrationNumber from Student ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            DataBind();*/
        }
        private void DataBind()
        {


            DataGridViewComboBoxColumn Update = new DataGridViewComboBoxColumn();
            Update.HeaderText = "Status";
            Update.Items.Add("Present");
            Update.Items.Add("Absent");
            Update.Items.Add("Late");
            Update.Items.Add("Leave");

            datagridView.Columns.Add(Update);

        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = new DateTime(2023, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(2024, 12, 31);

            if (dateTimePicker1.Value.Year != 2024)
            {
                MessageBox.Show("Please select a date within the year 2024");
                check_date = false;
                return;
            }
            else
            {
                check_date = true;
            }
        }
        private void load1(string sqlDateTime)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Insert into ClassAttendance values (@date)", con);
            cmd.Parameters.AddWithValue("@date", (sqlDateTime));
            cmd.ExecuteNonQuery();
            // MessageBox.Show("Successfully Saved");
        }
        private void load2(string sqlDateTime)
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("select max(Id) from ClassAttendance where AttendanceDate=@date", con2);
            cmd2.Parameters.AddWithValue("@date", sqlDateTime);
            cmd2.ExecuteNonQuery();
            id = (Int32)cmd2.ExecuteScalar();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                DateTime selectedDateTime = dateTimePicker1.Value;
                string sqlDateTime = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                load1(sqlDateTime);
                load2(sqlDateTime);


                MessageBox.Show(id.ToString());
                if (datagridView.Rows.Count != 0)
                {
                    var connection = Configuration.getInstance().getConnection();



                    for (int i = 0; i < datagridView.Rows.Count - 1; i++)
                    {
                        if (datagridView.Rows[i].Cells[0].Value != null)
                        {
                            string SI = datagridView.Rows[i].Cells[0].Value.ToString();
                            string S = datagridView.Rows[i].Cells[3].Value.ToString();
                            int x = 1;

                            if (S == "Present") { x = 1; }
                            else if (S == "Late") { x = 4; }
                            else if (S == "Absent") { x = 2; }
                            else if (S == "Leave") { x = 3; }
                            string cmd3 = $"INSERT INTO StudentAttendance  VALUES ({id},{SI},{x})";
                            SqlCommand command = new SqlCommand(cmd3, connection);
                            /// MessageBox.Show(x.ToString());

                            command.ExecuteNonQuery();
                            MessageBox.Show("Saved Successfully");
                        }
                        else { MessageBox.Show("Mark the attendance first it!!!"); }
                    }


                };


            }
            catch (Exception exp) { MessageBox.Show(exp.Message.ToString()); }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (check_update == true)
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select   AttendanceId, AttendanceDate, Concat(FirstName, LastName) as StudentName, RegistrationNumber            from StudentAttendance AS S join Student as SA on S.StudentId = SA.Id    join ClassAttendance as C           ON C.Id = S.AttendanceId", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                datagridView.DataSource = dt;
                datagridView.DefaultCellStyle.ForeColor = Color.Black;
                DataBind();
                check_update = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show();
        }
       
        /*private void LoadUpdatedAttendanceData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT AttendanceId, AttendanceDate, CONCAT(FirstName, ' ', LastName) AS StudentName, RegistrationNumber FROM StudentAttendance AS S JOIN Student AS SA ON S.StudentId = SA.Id JOIN ClassAttendance AS C ON C.Id = S.AttendanceId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            BindComboBoxColumn();
        }
*/

        private void button1_Click_1(object sender, EventArgs e)
        {
            ViewAttendance newUserControl = new ViewAttendance();

            // Show the ViewAttendance form without specifying a parent
            MessageBox.Show("Select the date and click on result to generate Report");
            newUserControl.Dock = DockStyle.Fill;
            newUserControl.ShowDialog();
            this.Close();
        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
