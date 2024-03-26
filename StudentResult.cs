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
    public partial class StudentResult : Form
    {
        string name;
        int ID;

        public StudentResult()
        {
            InitializeComponent();
        }

        private void StudentResult_Load(object sender, EventArgs e)
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select *  from Assessment", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;

            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Evaluate";
            Update.Text = "Evaluate";
            Update.UseColumnTextForButtonValue = true;
            DataGridViewButtonColumn Delete = new DataGridViewButtonColumn();
            Delete.HeaderText = "Result";
            Delete.Text = "Result";
            Delete.UseColumnTextForButtonValue = true;
            datagridView.Columns.Add(Update);
            datagridView.Columns.Add(Delete);

            


        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int index = datagridView.CurrentCell.ColumnIndex;
            {

                if (index == 5)
                {
                   
                    Evaluation newUserControl = new Evaluation(name, ID);
                    newUserControl.Dock = DockStyle.Fill;
                    newUserControl.ShowDialog();
                    this.Close();


                }
                else if (index == 6)
                {
                    Result newUserControl = new Result(ID, name);
                    newUserControl.Dock = DockStyle.Fill;
                    newUserControl.ShowDialog();
                    this.Close();
                }
            }
        }

        private void datagridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                name = datagridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                ID = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[0].Value.ToString());



            }
            catch (Exception exp) { }
        }
    }
}
