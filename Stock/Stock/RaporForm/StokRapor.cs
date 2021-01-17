using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class StokRapor : Form
    {

        ReportDocument cryrpt = new ReportDocument();
        public StokRapor()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        DataSet dst = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            cryrpt.Load(@"D:\Oyunlar\CodeBase\16011131\Stokprogram\Stock\Stock\Stock\Rapor\Stok.rpt");
            SqlConnection conn = Connection.getConnection();
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stok] where Cast( Tarih as Date) between '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' and '" + dateTimePicker2.Value.ToString("MM/dd/yyyy") + "'", conn);
            sda.Fill(dst, "Stok");
            cryrpt.SetDataSource(dst);
            cryrpt.SetParameterValue("@BaşlangıçTarihi", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            cryrpt.SetParameterValue("@BitişTarihi", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            crystalReportViewer1.ReportSource = cryrpt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExportOptions exportOption;
            DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf Files|*.pdf";
            //sfd.Filter = "Excel|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                diskFileDestinationOptions.DiskFileName = sfd.FileName;
            }
            exportOption = cryrpt.ExportOptions;
            {
                exportOption.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                //exportOption.ExportFormatType = ExportFormatType.Excel;
                exportOption.ExportDestinationOptions = diskFileDestinationOptions;
                exportOption.ExportFormatOptions = new PdfRtfWordFormatOptions();
                //exportOption.ExportFormatOptions = new ExcelFormatOptions();
            }
            cryrpt.Export();
        }
    }
}
