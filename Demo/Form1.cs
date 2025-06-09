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

    public partial class Form1 : Form
    {

        string conn = "Data Source=EXLEGAWORKER;Initial Catalog=MasterPol;Integrated Security=True;";
        SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(conn);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("select p.Partners_Id, p.Name, p.Partners_Type, p.Director, p.phone, p.rating, sum(s.Quantity) as total_sum from Partners p left join Sales s ON p.Partners_Id = s.Partner_id group by p.Partners_Id, p.Director, p.phone, p.rating, p.Partners_Type, p.Name", sqlConnection);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Panel panel = new Panel()
                {
                    BackColor = Color.Gray,
                    Height = 100,
                    Width = 740,
                    Tag = sqlDataReader["Partners_Id"].ToString(),
                    Cursor = Cursors.Hand
                };

                string tooltipstr = "Нажмите чтобы открыть редактирование";

                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(panel, tooltipstr);

                string parnersType = sqlDataReader["Partners_Type"].ToString();
                string name = sqlDataReader["Name"].ToString();
                string director = sqlDataReader["Director"].ToString();
                string phone = sqlDataReader["phone"].ToString();
                string raiting = sqlDataReader["rating"].ToString();

                int skid = 0;
                if (!string.IsNullOrWhiteSpace(sqlDataReader["total_sum"].ToString()))
                {
                    skid = Convert.ToInt32(sqlDataReader["total_sum"].ToString());
                }
                else {
                    skid = 0;
                }

                if (skid < 10000)
                {
                    skid = 0;
                }
                else if (skid > 10000 && skid < 50000)
                {
                    skid = 5;
                }
                else if (skid > 50000 && skid < 300000)
                {
                    skid = 10;
                }
                else if (skid > 300000)
                {
                    skid = 15;
                }

                Label label = new Label()
                {
                    Text = $"{parnersType} | {name}",
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft YaHei", 9.75f),
                    Location = new Point(10, 10),
                    Width = 500,
                };

                Label labelDir = new Label()
                {
                    Text = $"{director}",
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft YaHei", 9.75f),
                    Location = new Point(10, 30),
                    Width = this.Width,
                };

                Label labelPhone = new Label()
                {
                    Text = $"{phone}",
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft YaHei", 9.75f),
                    Location = new Point(10, 50),
                    Width = this.Width,
                };

                Label labelRaiting = new Label()
                {
                    Text = $"Рейтинг: {raiting}",
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft YaHei", 9.75f),
                    Location = new Point(10, 70),
                    Width = 500,
                };

                Label labelSkid = new Label()
                {
                    Text = $"{skid}%",
                    ForeColor = Color.Black,
                    Font = new Font("Microsoft YaHei", 9.75f),
                    Location = new Point(690, 10),
                    Width = this.Width,
                };

                Button button = new Button()
                {
                    Text = $"История продаж",
                    Location = new Point(600, 70),
                    Size = new Size(120, 25),
                    Tag = panel.Tag,
                    Cursor = Cursors.Hand
                };

                panel.Controls.Add(label);
                panel.Controls.Add(labelDir);
                panel.Controls.Add(labelPhone);
                panel.Controls.Add(labelRaiting);
                panel.Controls.Add(labelSkid);
                panel.Controls.Add(button);

                toolTip.SetToolTip(label, tooltipstr);
                toolTip.SetToolTip(labelDir, tooltipstr);
                toolTip.SetToolTip(labelPhone, tooltipstr);
                toolTip.SetToolTip(labelRaiting, tooltipstr);
                toolTip.SetToolTip(labelSkid, tooltipstr);

                panel.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));
                label.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));
                labelDir.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));
                labelPhone.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));
                labelRaiting.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));
                labelSkid.Click += (s1, e1) => EditPartner(Convert.ToInt32(panel.Tag.ToString()));

                button.Click += (s1, e1) => ShowSalesHistory(Convert.ToInt32(panel.Tag.ToString()));

                flowLayoutPanel1.Controls.Add(panel);

            }
        }

        public void EditPartner(int Partners_Id) {
            EditPartners editPartners = new EditPartners(Partners_Id);
            editPartners.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPartners addPartners = new AddPartners();
            addPartners.Show(this);
            this.Hide();
        }

        private void ShowSalesHistory(int Partners_Id) {
            ShowSalesHistory showSalesHistory = new ShowSalesHistory(Partners_Id);
            showSalesHistory.Show(this);
            this.Hide();
        }

    }
}
