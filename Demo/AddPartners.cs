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

namespace Demo
{
    public partial class AddPartners : Form
    {
        SqlConnection sqlConnection;
        string conn = "Data Source=EXLEGAWORKER;Initial Catalog=MasterPol;Integrated Security=True;";

        public AddPartners()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(conn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(maskedTextBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(maskedTextBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text)) 
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"INSERT INTO Partners (Partners_Type, Name, Director, email, phone, legal_address, inn, rating) VALUES" +
                    $"(@Partners_Type, @Name, @Director, @email, @phone, @legal_address, @inn, @rating)", sqlConnection);

                cmd.Parameters.AddWithValue("@Partners_Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Director", textBox2.Text);
                cmd.Parameters.AddWithValue("@email", textBox3.Text);
                cmd.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                cmd.Parameters.AddWithValue("@legal_address", textBox4.Text);
                cmd.Parameters.AddWithValue("@inn", maskedTextBox2.Text);
                cmd.Parameters.AddWithValue("@rating", textBox5.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Данные успешно добавлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) {
                MessageBox.Show($"Ошибка: {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                sqlConnection.Close();
            }
        }
    }
}
