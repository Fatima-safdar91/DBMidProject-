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
    public partial class Update : Form
    {
        public Update()
        {
            InitializeComponent();
        }

        private void Update_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MainMenu main = new MainMenu();
            this.Close();
            main.Show(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Update Student set FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, Status = @Status where RegistrationNumber = @RegistrationNumber", con);
            cmd.Parameters.AddWithValue("@RegistrationNumber", textBox2.Text);
            cmd.Parameters.AddWithValue("@FirstName", textBox3.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox4.Text);
            cmd.Parameters.AddWithValue("@Contact", textBox5.Text);
            cmd.Parameters.AddWithValue("@Email", textBox6.Text);
            cmd.Parameters.AddWithValue("@Status", textBox7.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Updated");

        }
    }
}
