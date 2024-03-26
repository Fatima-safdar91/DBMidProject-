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
    public partial class Form1 : Form
    {
        private string firstName;
        private string lastName;
        private string registrationNo;
        private string email;
        private string contact;
        private int statusId;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
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
            GatherFormData();
            if (ValidateInputFields())
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Insert into Student values (@FirstName, @LastName,  @Contact,  @Email,  @RegistrationNumber,  @Status)", con);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox3.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox4.Text);
                cmd.Parameters.AddWithValue("@Email", textBox5.Text);
                cmd.Parameters.AddWithValue("@RegistrationNumber", textBox6.Text);
                int id_check = 0;
                if (comboBox1.Text == "Active")
                {
                    id_check = 5;
                }
                else
                {
                    id_check = 6;
                }
                cmd.Parameters.AddWithValue("@Status", id_check);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully saved");
                ClearFormFields();
            }
            else
            {
                MessageBox.Show("Please ensure all fields are correctly filled.");
            }
        }

        private void ClearFormFields()
        {
            // Clear both form fields and class attributes
           
        }
        private void GatherFormData()
        {
            // Collect data from the form and store in class attributes
            this.firstName = textBox2.Text;
            this.lastName = textBox3.Text;
            this.registrationNo = textBox6.Text;
            this.email = textBox5.Text;
            this.contact = textBox4.Text;
            // Check if combobox selected item is not null before accessing ToString() method
            this.statusId = comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Active" ? 5 : 6;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox6.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox4.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;

            this.firstName = string.Empty;
            this.lastName = string.Empty;
            this.registrationNo = string.Empty;
            this.email = string.Empty;
            this.contact = string.Empty;
            this.statusId = 0;
            ClearErrorLabels();
          
        }
        private bool ValidateInputFields()
        {
            bool isValid = true;

            // Clear error messages initially
            ClearErrorLabels();

            if (string.IsNullOrWhiteSpace(this.firstName))
            {
                ShowErrorLabel(label1, "Please enter a valid first name.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(this.lastName))
            {
                ShowErrorLabel(label8, "Please enter a valid last name.");
                isValid = false;
            }

            /*if (string.IsNullOrWhiteSpace(this.registrationNo) || !IsValidRegistrationNumber(this.registrationNo))
            {
                ShowErrorLabel(label11, "Please enter a valid registration number (Format: ####-Letters-####).");
                isValid = false;
            }*/

            if (string.IsNullOrWhiteSpace(this.email) || !IsValidEmail(this.email))
            {
                ShowErrorLabel(label10, "Please enter a valid email address.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(this.contact) || !IsValidContact(this.contact))
            {
                ShowErrorLabel(label9, "Please enter a valid contact number.");
                isValid = false;
            }

            if (this.statusId != 5 && this.statusId != 6)
            {
                ShowErrorLabel(label12, "Please select a valid status.");
                isValid = false;
            }

            return isValid;
        }


        private void ClearErrorLabels()
        {
            label1.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
        }
      

        private void ShowErrorLabel(Label label, string errorMessage)
        {
            label.Text = errorMessage;
            label.Visible = true;
        }

        private bool IsValidRegistrationNumber(string registrationNumber)
        {
            // Implement your custom validation logic for registration number here
            // Example: Check if the string matches a specific format
            return registrationNumber.Length == 12 && registrationNumber[4] == '-' && registrationNumber[9] == '-';
        }

        private bool IsValidEmail(string email)
        {
            // Implement your custom validation logic for email here
            // Example: Check if the string contains an '@' character
            return email.Contains("@");
        }

        private bool IsValidContact(string contact)
        {
            // Implement your custom validation logic for contact number here
            // Example: Check if the string contains only digits and optional '+' or '-' characters
            return contact.All(char.IsDigit) || (contact.StartsWith("+") || contact.StartsWith("-"));
        }

        private bool ShowErrorAndSetValidity(Label label, string errorMessage)
        {
            ShowErrorLabel(label, errorMessage);
            return false;
        }


        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
    
}
