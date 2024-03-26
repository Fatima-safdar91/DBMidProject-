﻿using System;
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
    public partial class RubericLevel : Form
    {
        bool check_update = false;
        string detail;
        int id, rubericID, Measurment;
        //List<string> NamesClo = new List<string>();
        //List<int> IDS = new List<int>();
        int RubericSID;
        bool check_c = false;

        public RubericLevel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_update == false)
                {
                    if (check_c && detailstxt.Text != String.Empty && textBox1.Text != String.Empty)
                    {
                        using (var con = Configuration.getInstance().getConnection())
                        {
                            con.Open();
                            using (SqlCommand cmd = new SqlCommand("Insert into RubricLevel values (@Id,@Detail,@measure)", con))
                            {
                                cmd.Parameters.AddWithValue("@Detail", detailstxt.Text);
                                cmd.Parameters.AddWithValue("@measure", textBox1.Text);
                                cmd.Parameters.AddWithValue("@Id", RubericSID.ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Added Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        detailstxt.Text = String.Empty;
                        textBox1.Text = String.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Fill the data First");
                    }
                }
                else if (check_update == true && check_c)
                {
                    using (var con2 = Configuration.getInstance().getConnection())
                    {
                        con2.Open();
                        using (SqlCommand cmd2 = new SqlCommand("Update RubricLevel Set Details=@Detail,RubricId=@CloID , MeasurementLevel=@measure where Id=@ID", con2))
                        {
                            cmd2.Parameters.AddWithValue("@Detail", detailstxt.Text);
                            cmd2.Parameters.AddWithValue("@CloID", RubericSID);
                            cmd2.Parameters.AddWithValue("@ID", id);
                            cmd2.Parameters.AddWithValue("@measure", textBox1.Text);
                            cmd2.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Updated Successfully", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    check_update = false;
                    detailstxt.Text = String.Empty;
                    textBox1.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            detailstxt.Text = String.Empty;
            comboBox1.Text = String.Empty;
        }

        private void datagridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = datagridView.CurrentCell.ColumnIndex;
            {

                if (index == 0)
                {
                    detailstxt.Text = detail;
                    textBox1.Text = Measurment.ToString();

                    check_update = true;


                }

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (textBox1.Text == string.Empty)
            {// check is empty
                label3.Text = "Enter the name";
                check_c = false;
            }
            if (textBox1.Text.Any(ch => !char.IsDigit(ch)))
            {//check isSpecialCharactor
                label3.Text = "Allowed characters: 1-9";
                check_c = false;
            }

            else

            {//ready for storage or action
                label3.Text = " ";
                check_c = true;
            }

        }

        private void RubericLevel_Load(object sender, EventArgs e)
        {
            load_combobox_assessment_data();
            DataGridViewButtonColumn Update = new DataGridViewButtonColumn();
            Update.HeaderText = "Update";
            Update.Text = "Update";
            Update.UseColumnTextForButtonValue = true;

            datagridView.Columns.Add(Update);
        }
        private void load_combobox_assessment_data()
        {

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            load1();
            load2();

        }
        private void load2()
        {
            var con2 = Configuration.getInstance().getConnection();

            SqlCommand cmd2 = new SqlCommand("Select  id FROM Rubric", con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                comboBox2.Items.Add(Convert.ToInt16(reader2.GetInt32(0)));
            }
            reader2.Close();

            cmd2.ExecuteNonQuery();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RubericSID = Convert.ToInt32(comboBox2.Items[comboBox1.SelectedIndex].ToString());
        }

        private void datagridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[1].Value.ToString());
                rubericID = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[2].Value.ToString());
                Measurment = Convert.ToInt16(datagridView.Rows[e.RowIndex].Cells[4].Value.ToString());
                detail = datagridView.Rows[e.RowIndex].Cells[3].Value.ToString();

            }
            catch (Exception exp) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            view();
        }
        private void view()
        {
            var con2 = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select * from RubricLevel", con2);
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            DataTable dt = new DataTable();
            da.Fill(dt);
            datagridView.DataSource = null;
            datagridView.DataSource = dt;
            datagridView.DefaultCellStyle.ForeColor = Color.Black;
            con2.Close();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ruberic ruberic = new Ruberic();
            ruberic.Show();
            this.Close();
        }

        private void load1()
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("Select  Details  FROM Rubric", con);
            //con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            con.Close();
            con.Open();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
