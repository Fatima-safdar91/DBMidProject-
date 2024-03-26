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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace DBProject
{
    public partial class Ruberic : Form
    {
        bool check_update = false;
        int rubricId;
        string rubricDetails;
        int cloId;
        public Ruberic()
        {
            InitializeComponent();
        }

        private void Ruberic_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.HeaderText = "Update";
            updateColumn.Text = "Update";
            updateColumn.UseColumnTextForButtonValue = true;

            datagridView.Columns.Add(updateColumn);

            ViewRubrics();
            bool check_update = false;
            LoadComboBoxes();
        }
        private void ViewRubrics()
        {
            var connection = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM Rubric", connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            datagridView.DataSource = null;
            datagridView.DataSource = dataTable;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            connection.Close();
        }
        private string check_q(string x)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand($"        IF(select max(1) from Rubric where Details='{x}' )>0 BEGIN SELECT '1' END ELSE BEGIN SELECT '2' END", con);
            string z = "";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                z = (reader.GetString(0));
            }
            reader.Close();

            // X=cmd.ExecuteReader().GetString(0);
            cmd.ExecuteNonQuery();
            return z;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = check_q(detailstxt.Text);

            if ((check_update == false && result != "1"))
            {
                if (detailstxt.Text != String.Empty)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 200);

                    var connection = Configuration.getInstance().getConnection();
                    SqlCommand command = new SqlCommand($"INSERT INTO Rubric VALUES ({randomNumber}, '{detailstxt.Text}', {cloId})", connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detailstxt.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("Fill in the data first");
                }
            }
            else if (check_update == true)
            {
                var connection = Configuration.getInstance().getConnection();
                SqlCommand command = new SqlCommand("UPDATE Rubric SET Details=@Detail, CloId=@CloID WHERE Id=@ID", connection);
                command.Parameters.AddWithValue("@Detail", detailstxt.Text);
                command.Parameters.AddWithValue("@CloID", cloId);
                command.Parameters.AddWithValue("@ID", rubricId);
                command.ExecuteNonQuery();
                MessageBox.Show("UPDATED Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                check_update = false;
                detailstxt.Text = String.Empty;
            }
            else if (result == "1")
            {
                MessageBox.Show("Already Exists");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            detailstxt.Text = String.Empty;
            comboBox1.Text = String.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ViewRubrics();
        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = datagridView.CurrentCell.ColumnIndex;

            if (index == 0)
            {
                detailstxt.Text = rubricDetails;
                check_update = true;
            }
        }
        private void LoadComboBoxes()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            LoadComboBox1();
            LoadComboBox2();
        }
        private void LoadComboBox2()
        {
            var connection = Configuration.getInstance().getConnection();

            SqlCommand command = new SqlCommand("SELECT Id FROM Clo", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader.GetInt32(0)));
            }

            reader.Close();
            command.ExecuteNonQuery();
        }

        private void LoadComboBox1()
        {
            var connection = Configuration.getInstance().getConnection();

            SqlCommand command = new SqlCommand("SELECT Name FROM Clo", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }

            reader.Close();
            command.ExecuteNonQuery();
            connection.Close();
            connection.Open();
        }

        private void datagridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                rubricId = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                rubricDetails = datagridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            catch (Exception exp) { }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cloId = Convert.ToInt32(comboBox2.Items[comboBox1.SelectedIndex].ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RubericLevel newUserControl = new RubericLevel();
            newUserControl.Dock = DockStyle.Fill;
            newUserControl.ShowDialog();
            this.Close();
        }
    }
}
