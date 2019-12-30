using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{
    public partial class FrmLogovanje : Form
    {
        List<Kupac> kupci = Datoteka.Citaj("kupci.bin") as List<Kupac>;
        string admin = "admin",adminLoz = "admin";
        Form korisnickaForma;
        public FrmLogovanje()
        {
            InitializeComponent();
        }
        private void btnPrijava_Click(object sender, EventArgs e)
        {
            kupci = Datoteka.Citaj("kupci.bin") as List<Kupac>;
            bool postoji = false;
            if (txtKorisnickoIme.Text == admin && txtLozinka.Text == adminLoz)
            {
                korisnickaForma = new FrmAdmin(this);
                korisnickaForma.FormClosed += korisnickaForma_FormClosed;
                korisnickaForma.Show();
                this.Hide();
                txtKorisnickoIme.Text = "";
                txtLozinka.Text = "";
                postoji = true;
            }
            else
            {
                foreach(Kupac k in kupci)
                {
                    if(txtKorisnickoIme.Text.Equals(k.KorisnickoIme) && txtLozinka.Text.Equals(k.Lozinka))
                    {
                        korisnickaForma = new FrmKupac(k, this);
                        korisnickaForma.FormClosed += korisnickaForma_FormClosed;
                        korisnickaForma.Show();
                        this.Hide();
                        postoji = true;
                        break;
                    }
                }
                txtKorisnickoIme.Text = "";
                txtLozinka.Text = "";
            }
            if (!postoji)
            {
                MessageBox.Show("Korisnik sa datim podacima ne postoji!");
            }
        }

        private void korisnickaForma_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}
