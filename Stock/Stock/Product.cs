using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Stock
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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
                if (ProductExists(conn, idtext.Text))
                {
                    sqlcommand = "UPDATE [dbo].[Ürünler] SET" +
                        "[Ürünismi] = '" + isimtext.Text + "'" +
                        ",[Durum] ='" + status + "'" +
                        "WHERE [Ürünkodu] = '" + idtext.Text + "'";
                }
                else
                {
                    sqlcommand = "INSERT INTO[dbo].[Ürünler] ([Ürünkodu],[Ürünismi],[Durum])" +
                   " VALUES ('" + idtext.Text + "','" + isimtext.Text + "','" + status + "')";

                }


                SqlCommand cmd = new SqlCommand(sqlcommand, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
                ResetRecords();
            }




        }


        private bool ProductExists(SqlConnection conn, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("Select 1 from [Ürünler] WHERE [Ürünkodu]='" + productCode + "'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0) { return true; }
            else { return false; }

        }

        public void LoadData()
        {


            SqlConnection conn = Connection.getConnection();

            //Reading Data
            SqlDataAdapter sda = new SqlDataAdapter("Select * from [Stock].[dbo].[Ürünler]", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["Ürünkodu"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["Ürünismi"].ToString();

                if ((bool)item["Durum"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Aktif";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Pasif";
                }

            }

        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();
        }



        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1.Text = "Güncelle";
            idtext.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            isimtext.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Aktif")
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
                    if (ProductExists(conn, idtext.Text))
                    {
                        conn.Open();
                        string sqlcommand = "DELETE FROM [dbo].[Ürünler] WHERE [Ürünkodu] = '" + idtext.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlcommand, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
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

        private void ResetRecords()
        {
            idtext.Clear();
            isimtext.Clear();
            comboBox1.SelectedIndex = -1;
            button1.Text = "Ekle";
            idtext.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetRecords();
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(idtext.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(idtext, "Ürün kodu gereklidir.");
            }
            else if (string.IsNullOrEmpty(isimtext.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(isimtext, "Ürün ismi gereklidir.");
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

    }
}
