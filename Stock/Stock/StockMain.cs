using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock
{
    public partial class StockMain : Form
    {

        public StockMain()
        {
            InitializeComponent();
        }

        bool close = true;
        private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {
                DialogResult dialogResult = MessageBox.Show("Çıkış yapmak istiyor musunuz?", "Çıkış", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    close = false;
                    Application.Exit();
                } 
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void ürünlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Ürünlere basıldığında ürünler menüsü açılması
            Product pro = new Product();
            pro.MdiParent = this;
            pro.StartPosition = FormStartPosition.CenterScreen;
            pro.Show();

        }

        private void stokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stock stk = new Stock();
            stk.MdiParent = this;
            stk.StartPosition = FormStartPosition.CenterScreen;
            stk.Show();
        }

        private void ürünListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporForm.Rapor prod = new RaporForm.Rapor();
            prod.MdiParent = this;
            prod.StartPosition = FormStartPosition.CenterScreen;
            prod.Show();



        }

        private void stokBilgisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RaporForm.StokRapor stok = new RaporForm.StokRapor();
            stok.MdiParent = this;
            stok.StartPosition = FormStartPosition.CenterScreen;
            stok.Show();

        }
    }
}
