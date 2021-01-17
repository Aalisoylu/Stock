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

namespace Stock
{
    public partial class Stock : Form
    {
        public Stock()
        {
            InitializeComponent();
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            this.ActiveControl = dateTimePicker1;
            comboBox1.SelectedIndex = 0;
            LoadData();
            Search();


        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgview.Rows.Count > 0)
                {
                    textBox1.Text = dgview.SelectedRows[0].Cells[0].Value.ToString();
                    textBox2.Text = dgview.SelectedRows[0].Cells[1].Value.ToString();
                    this.dgview.Visible = false;
                    textBox3.Focus();
                }
                else
                {
                    this.dgview.Visible = false;
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text.Length > 0)
                {
                    textBox3.Focus();
                }
                else
                {
                    textBox2.Focus();
                }
            }
        }


        bool change = true;

        private void proCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (change)
            {
                change = false;
                textBox1.Text = dgview.SelectedRows[0].Cells[0].Value.ToString();
                textBox2.Text = dgview.SelectedRows[0].Cells[1].Value.ToString();
                this.dgview.Visible = false;
                textBox3.Focus();
                change = true;

            }


        }





        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox3.Text.Length > 0)
                {
                    comboBox1.Focus();
                }
                else
                {
                    textBox3.Focus();
                }
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    button1.Focus();
                }
                else
                {
                    comboBox1.Focus();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void ResetRecords()
        {
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            button1.Text = "Ekle";
            dateTimePicker1.Focus();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetRecords();
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox1, "Ürün kodu gereklidir.");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Ürün ismi gereklidir.");
            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox3, "Miktar gereklidir.");
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Durum seçiniz");

            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }


            return result;
        }

        private bool IfProductExists(SqlConnection conn, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 from [Stok] WHERE [Ürünkodu]='" + productCode + "'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                //sql baglantısı
                SqlConnection conn = Connection.getConnection();
                conn.Open();
                //Ekleme islemi

                bool status;
                if (comboBox1.SelectedIndex == 0) { status = true; }
                else { status = false; }

                string sqlcommand;
                if (IfProductExists(conn, textBox1.Text))
                {
                    sqlcommand = @"UPDATE [Stok] SET [Ürünismi] = '" + textBox2.Text + "' ,[Miktar] = '" + textBox3.Text + "',[Durum] = '" + status + "' WHERE [Ürünkodu] = '" + textBox1.Text + "'";
                }
                else
                {
                    sqlcommand = @"INSERT INTO Stok (Ürünkodu, Ürünismi, Tarih, Miktar, Durum)
                                 VALUES  ('" + textBox1.Text + "','" + textBox2.Text + "','" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "','" + textBox3.Text + "','" + status + "')";
                }


                SqlCommand cmd = new SqlCommand(sqlcommand, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Ürün kaydı başarılı");
                ResetRecords();
            }
            LoadData();
        }

        public void LoadData()
        {

            SqlConnection con = Connection.getConnection();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Stok]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["dgSno"].Value = n + 1;
                dataGridView1.Rows[n].Cells["dgÜrünkodu"].Value = item["Ürünkodu"].ToString();
                dataGridView1.Rows[n].Cells["dgÜrünismi"].Value = item["Ürünismi"].ToString();
                dataGridView1.Rows[n].Cells["dgMiktar"].Value = float.Parse(item["Miktar"].ToString());
                dataGridView1.Rows[n].Cells["dgTarih"].Value = Convert.ToDateTime(item["Tarih"].ToString()).ToString("dd/MM/yyyy");

                if (item["Durum"].ToString() == "True")
                {
                    dataGridView1.Rows[n].Cells["dgDurum"].Value = "Aktif";
                }
                else
                {
                    dataGridView1.Rows[n].Cells["dgDurum"].Value = "Pasif";
                }
            }

            if (dataGridView1.Rows.Count > 0)
            {
                label8.Text = dataGridView1.Rows.Count.ToString();
                float totQty = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    totQty += float.Parse(dataGridView1.Rows[i].Cells["dgMiktar"].Value.ToString());
                    label9.Text = totQty.ToString();
                }

            }
            else
            {
                label8.Text = "0";
                label9.Text = "0";
            }






        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //butona basildigi zaman ki hatayi düzelt
            button1.Text = "Güncelle";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["dgÜrünkodu"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells["dgÜrünismi"].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells["dgMiktar"].Value.ToString();
            dateTimePicker1.Text = DateTime.Parse(dataGridView1.SelectedRows[0].Cells["dgTarih"].Value.ToString()).ToString("dd/MM/yyyy");
            if (dataGridView1.SelectedRows[0].Cells["dgDurum"].Value.ToString() == "Aktif")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Ürünü silmek istiyor musunuz", "Ürün silme işlemi", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (Validation())
                {
                    SqlConnection conn = Connection.getConnection();
                    if (IfProductExists(conn, textBox1.Text))
                    {
                        conn.Open();
                        string sqlcommand = "DELETE FROM [dbo].[Stok] WHERE [Ürünkodu] = '" + textBox1.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlcommand, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Ürün başarı ile silindi");
                    }
                    else
                    {
                        MessageBox.Show("Ürün Bulunamadı");

                    }

                    LoadData();
                    ResetRecords();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (textBox1.Text.Length > 0)
            {
                this.dgview.Visible = true;
                dgview.BringToFront();
                Search(150, 105, 430, 200, "Ürünkodu,Ürünismi", "100,0");
                this.dgview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.proCode_MouseDoubleClick);
                SqlConnection conn = Connection.getConnection();
                SqlDataAdapter sda = new SqlDataAdapter("Select Top(10) Ürünkodu,Ürünismi From [Ürünler] Where [Ürünkodu] Like '" + textBox1.Text + "%'", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgview.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int n = dgview.Rows.Add();
                    dgview.Rows[n].Cells[0].Value = row["Ürünkodu"].ToString();
                    dgview.Rows[n].Cells[1].Value = row["Ürünismi"].ToString();

                }


            }

            else
            {
                dgview.Visible = false;
            }



        }

        private DataGridView dgview;
        private DataGridViewTextBoxColumn dgviewcol1;
        private DataGridViewTextBoxColumn dgviewcol2;

        void Search()
        {
            dgview = new DataGridView();
            dgviewcol1 = new DataGridViewTextBoxColumn();
            dgviewcol2 = new DataGridViewTextBoxColumn();
            this.dgview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.dgviewcol1, this.dgviewcol2 });
            this.dgview.Name = "dgview";
            dgview.Visible = false;
            this.dgviewcol1.Visible = false;
            this.dgviewcol2.Visible = false;
            this.dgview.AllowUserToAddRows = false;
            this.dgview.RowHeadersVisible = false;
            this.dgview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //this.dgview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgview_KeyDown);

            this.Controls.Add(dgview);
            this.dgview.ReadOnly = true;
            dgview.BringToFront();

        }

        void Search(int LX, int LY, int DW, int DH, string ColName, String ColSize)
        {
            this.dgview.Location = new System.Drawing.Point(LX, LY);
            this.dgview.Size = new System.Drawing.Size(DW, DH);

            string[] ClSize = ColSize.Split(',');
            //Size
            for (int i = 0; i < ClSize.Length; i++)
            {
                if (int.Parse(ClSize[i]) != 0)
                {
                    dgview.Columns[i].Width = int.Parse(ClSize[i]);
                }

                else
                {
                    dgview.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                }

            }


            string[] ClName = ColName.Split(',');

            for (int i = 0; i < ClName.Length; i++)
            {
                this.dgview.Columns[i].HeaderText = ClName[i];
                this.dgview.Columns[i].Visible = true;
            }









        }



    }
}
