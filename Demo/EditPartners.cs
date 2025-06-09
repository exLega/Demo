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
    public partial class EditPartners : Form
    {
        SqlConnection sqlConnection;
        string conn = "Data Source=EXLEGAWORKER;Initial Catalog=MasterPol;Integrated Security=True;";

        int SelectPartId = 0;

        public EditPartners(int Partners_Id)
        {
            InitializeComponent();
            SelectPartId = Partners_Id;
            label1.Text = $"ID Выбранного партнера {SelectPartId}";
            sqlConnection = new SqlConnection(conn);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void EditPartners_Load(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"select * from Partners where Partners_Id = {SelectPartId};", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    string parnersType = reader["Partners_Type"].ToString();
                    string name = reader["Name"].ToString();
                    string director = reader["Director"].ToString();
                    string email = reader["email"].ToString();
                    string phone = reader["phone"].ToString();
                    string address = reader["legal_address"].ToString();
                    string inn = reader["inn"].ToString();
                    string raiting = reader["rating"].ToString();

                    comboBox1.Text = parnersType;
                    textBox1.Text = name;
                    textBox2.Text = director;
                    textBox3.Text = email;
                    maskedTextBox1.Text = phone;
                    textBox4.Text = address;
                    maskedTextBox2.Text = inn;
                    textBox5.Text = raiting;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                sqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"update Partners set Partners_Type = @Partners_Type, Name = @Name, Director = @Director, email = @email, phone = @phone, legal_address = @legal_address, inn = @inn, rating = @rating where Partners_Id = {SelectPartId}", sqlConnection);

                cmd.Parameters.AddWithValue("@Partners_Type", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                cmd.Parameters.AddWithValue("@Director", textBox2.Text);
                cmd.Parameters.AddWithValue("@email", textBox3.Text);
                cmd.Parameters.AddWithValue("@phone", maskedTextBox1.Text);
                cmd.Parameters.AddWithValue("@legal_address", textBox4.Text);
                cmd.Parameters.AddWithValue("@inn", maskedTextBox2.Text);
                cmd.Parameters.AddWithValue("@rating", textBox5.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Данные успешно обновлены", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) {
                MessageBox.Show($"Ошибка {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                sqlConnection.Close();
            }
        }
    }
}
