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
    public partial class ShowSalesHistory : Form
    {
        private int partId;
        string conn = "Data Source=EXLEGAWORKER;Initial Catalog=MasterPol;Integrated Security=True;";
        SqlConnection sqlConnection;
        public ShowSalesHistory(int Partners_Id)
        {
            InitializeComponent();
            partId = Partners_Id;
            label1.Text = $"История продаж партнера ID: {partId}";
            this.Text = label1.Text;
            sqlConnection = new SqlConnection(conn);
        }

        private void ShowSalesHistory_Load(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"select p.Product_Name, p.Product_Type_Id, p.Material_Type_Id, s.Quantity, s.Sale_Date from Sales s join Product p ON s.Product_Id = p.Product_Id where s.Partner_id = {partId} order by s.Sale_Date Desc", sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);

                sqlConnection.Close();
                foreach (DataRow dataRow in dt.Rows) {
                    int productTypeId = Convert.ToInt32(dataRow["Product_Type_Id"]);
                    int MaterialTypeId = Convert.ToInt32(dataRow["Material_Type_Id"]);
                    int quantity = Convert.ToInt32(dataRow["Quantity"]);
                }

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["Product_Name"].HeaderText = "Продукция";
                dataGridView1.Columns["Quantity"].HeaderText = "Количество";
                dataGridView1.Columns["Sale_Date"].HeaderText = "Дата продажи";

                dataGridView1.Columns["Product_Type_Id"].Visible = false;
                dataGridView1.Columns["Material_Type_Id"].Visible = false;


            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally {
                sqlConnection.Close();
            }
        }

        private int CalcMaterial(int ProductTypeId, int MaterialTypeId, int quantity, double param1, double param2) {
            if (param1 < 0 || param2 < 0 || quantity < 0)
                return -1;

            try {
                sqlConnection.Open();
                SqlCommand coefQuery = new SqlCommand($"select Coefficient from ProductType where Product_Type_Id = {ProductTypeId}", sqlConnection);
                double coef = (double)coefQuery.ExecuteScalar();

                SqlCommand deffQuery = new SqlCommand($"select deffect_percentage from MaterialType where Material_Type_Id = {MaterialTypeId}", sqlConnection);
                double deffect = (double)deffQuery.ExecuteScalar();
                sqlConnection.Close();

                double materialPerUnit = param1 * param2 * coef;
                double totalMaterial = (quantity * materialPerUnit) * (1 + (deffect / 100));

                return (int)Math.Ceiling(totalMaterial);
            } catch {
                return -1;
            } finally {
                sqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) {
                MessageBox.Show("Пожалуйста, выберите строку в таблице", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            try
            {
                int productTypeId = Convert.ToInt32(selectedRow.Cells["Product_Type_Id"].Value);
                int materialTypeId = Convert.ToInt32(selectedRow.Cells["Material_Type_Id"].Value);
                int quantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value);
                string productName = selectedRow.Cells["Product_Name"].Value.ToString();

                double param1 = Convert.ToDouble(textBox1.Text);
                double param2 = Convert.ToDouble(textBox2.Text);
                int requiredMaterial = CalcMaterial(productTypeId, materialTypeId, quantity, param1, param2);

                MessageBox.Show($"Выбрана продукция: {productName}\n" +
                              $"Количество: {quantity}\n" +
                              $"Требуемое количество материала: {requiredMaterial}",
                              "Подсчет требуемого количества материалов",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении параметров: {ex.Message}\nПроверьте написани вещественных параметров через запятую", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
