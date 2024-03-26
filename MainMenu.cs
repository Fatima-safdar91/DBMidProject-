using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBProject
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Hide();
            Update mai = new Update();
            mai.Show();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Search main = new Search();
            main.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 main = new Form1();
            main.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Delete main = new Delete();
            main.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClassAttendence classAttendence = new ClassAttendence();
            this.Hide();
            classAttendence.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AssesmentForm assesmentForm = new AssesmentForm();
            this.Hide();
            assesmentForm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CLOs cLOs = new CLOs(); 
            this.Hide();
            cLOs.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Ruberic ruberic = new Ruberic();    
            this.Hide();
            ruberic.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            StudentResult studentResult = new StudentResult();      
            this.Hide(); studentResult.Show();
        }
    }
}
