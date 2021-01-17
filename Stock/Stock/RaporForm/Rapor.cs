using CrystalDecisions.CrystalReports.Engine;
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

namespace Stock.RaporForm
{
    public partial class Rapor : Form
    {
        ReportDocument cryrpt = new ReportDocument(); 
        public Rapor()
        {
            InitializeComponent();
        }

        private void Rapor_Load(object sender, EventArgs e)
        {

            cryrpt.Load(@"D:\Oyunlar\CodeBase\16011131\Stokprogram\Stock\Stock\Stock\Rapor\Ürünler.rpt");
            SqlConnection conn = Connection.getConnection();
            conn.Open();
            DataSet dst = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Ürünler]", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            cryrpt.SetDataSource(dt);
            crystalReportViewer1.ReportSource = cryrpt;





        }
    }
}
