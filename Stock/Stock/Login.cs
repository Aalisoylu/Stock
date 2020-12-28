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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text ="";
            textBox2.Clear();
            textBox1.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Check- lo gin id and password
            // SqlConnection con = new SqlConnection();
            SqlConnection conn = Connection.getConnection();

            string sqlada = "SELECT* FROM[dbo].[Login] Where Kullanıcı = '"+ textBox1.Text + "' and Şifre = '"+textBox2.Text+"'";
            SqlDataAdapter sda = new SqlDataAdapter(@sqlada,conn);
             
            DataTable dt = new DataTable();
            sda.Fill(dt); 
            if(dt.Rows.Count == 1)
                {
                    this.Hide();
                    StockMain main = new StockMain();
                    main.Show(); 
                }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı veya Şifre","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }
    }
}
