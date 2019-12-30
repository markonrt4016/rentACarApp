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
    public delegate void PrikaziRezervacije(List<Rezervacija> rezervacije);
    public partial class FrmKupac : Form
    {
        Kupac kupac;
        List<Rezervacija> listaRezervacija;
        FrmRezervacijeKupca frmRezKup;
        string putanjaRezervacija = "rezervacije.bin";
        public PrikaziRezervacije prikaziRezervacije;
        FrmLogovanje fl;
        public FrmKupac()
        {
            InitializeComponent();
        }

        public ListBox LstRezervacije { get => lstRezervacije;}

        public FrmKupac(Kupac k, FrmLogovanje f)
        {
            InitializeComponent();
            fl = f;
            kupac = k;
            listaRezervacija = Datoteka.Citaj(putanjaRezervacija) as List<Rezervacija>;
            prikaziRezervacije += prikaziTrenutneRezervacije;
        }

        public void prikaziTrenutneRezervacije(List<Rezervacija> lstRez)
        {
            lstRezervacije.Items.Clear();
            foreach(Rezervacija r in lstRez)
            {
                if (r.IdKupca == kupac.Id)
                    lstRezervacije.Items.Add(r.ToString());
            }
        }

        private void frmKupac_Load(object sender, EventArgs e)
        {
            if(listaRezervacija != null)
            {
                prikaziTrenutneRezervacije(listaRezervacija);
            }
            frmRezKup = new FrmRezervacijeKupca(prikaziRezervacije,kupac.Id, this);
        }


        private void izadjiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnRezervisi_Click(object sender, EventArgs e)
        {
             frmRezKup.ShowDialog(); 
        }
        
        private void btnUkiniRezervaciju_Click(object sender, EventArgs e)
        {
            if(lstRezervacije.SelectedItem != null)
            {
                MessageBox.Show(lstRezervacije.SelectedItem.ToString() + "Obrisano!");
                foreach(Rezervacija r in listaRezervacija)
                {
                    if (r.ToString().Equals(lstRezervacije.SelectedItem.ToString()))
                    {
                        listaRezervacija.Remove(r);
                        break;
                    }
                }
                lstRezervacije.Items.Clear();
                foreach (Rezervacija r in listaRezervacija)
                    lstRezervacije.Items.Add(r.ToString());
                Datoteka.Upisi(listaRezervacija, putanjaRezervacija);
            }
            else
            {
                MessageBox.Show("Morate izabrati neku rezervaciju!");
            }
        }

        private void odjaviSeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            fl.Show();
        }
    }
}
