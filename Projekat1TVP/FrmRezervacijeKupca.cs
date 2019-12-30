using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{
    public partial class FrmRezervacijeKupca : Form
    {
        List<Automobil> listaAuta;
        List<Ponuda> listaPonuda;
        List<Rezervacija> listaRezervacija;
        PrikaziRezervacije prikaziRezervacije;

        List<Ponuda> odabranePonude, izabranePonude;
        List<Rezervacija> odabraneRezervacije;

        string putanjaRezervacija = "rezervacije.bin";
        int idKupca;
        FrmKupac fk;
        double cena = 0;
        public FrmRezervacijeKupca()
        {
            InitializeComponent();
        }

        public FrmRezervacijeKupca(PrikaziRezervacije prikaziRezervacije,int idKupca, FrmKupac fk)
        {
            InitializeComponent();
            this.fk = fk;
            this.idKupca = idKupca;
            listaAuta = Datoteka.Citaj("auti.bin") as List<Automobil>;
            listaPonuda = Datoteka.Citaj("ponude.bin") as List<Ponuda>;
            listaRezervacija = Datoteka.Citaj(putanjaRezervacija) as List<Rezervacija>;
            if (listaRezervacija == null)
                listaRezervacija = new List<Rezervacija>();
            this.prikaziRezervacije = prikaziRezervacije;
            dtDatumOd.MinDate = DateTime.Now;
        }


        private void FrmRezervacijeKupca_Load(object sender, EventArgs e)
        {
            foreach(Automobil a in listaAuta)
                if(!cmbMarka.Items.Contains(a.Marka))
                    cmbMarka.Items.Add(a.Marka);
        }

        private void cmbMarka_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbModel.Items.Clear();
            foreach(Automobil a in listaAuta)
                if(a.Marka.Equals((sender as ComboBox).SelectedItem.ToString()) && !cmbModel.Items.Contains(a.Model))
                        cmbModel.Items.Add(a.Model);

        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbGodiste.Items.Clear();
            foreach (Automobil a in listaAuta)
                if (a.Model.Equals((sender as ComboBox).SelectedItem.ToString()) && !cmbGodiste.Items.Contains(a.Godiste))
                {
                    cmbGodiste.Items.Add(a.Godiste);
                }
        }

        private void cmbGodiste_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbGorivo.Items.Clear();
            cmbKaroserija.Items.Clear();
            cmbKubikaza.Items.Clear();
            cmbMenjac.Items.Clear();
            cmbPogon.Items.Clear();
            cmbVrata.Items.Clear();
            foreach (Automobil a in listaAuta)
                if (cmbMarka.SelectedItem.ToString().Equals(a.Marka) && cmbModel.SelectedItem.ToString().Equals(a.Model) && cmbGodiste.SelectedItem.ToString().Equals(a.Godiste.ToString()))
                {
                    if(!cmbGorivo.Items.Contains(a.Gorivo))
                        cmbGorivo.Items.Add(a.Gorivo);
                    if (!cmbKaroserija.Items.Contains(a.Karoserija))
                        cmbKaroserija.Items.Add(a.Karoserija);
                    if (!cmbKubikaza.Items.Contains(a.Kubikaza))
                        cmbKubikaza.Items.Add(a.Kubikaza);
                    if (!cmbMenjac.Items.Contains(a.Menjac))
                        cmbMenjac.Items.Add(a.Menjac);
                    if (!cmbPogon.Items.Contains(a.Pogon))
                        cmbPogon.Items.Add(a.Pogon);
                    if (!cmbVrata.Items.Contains(a.BrVrata))
                        cmbVrata.Items.Add(a.BrVrata);
                    cmbGorivo.SelectedIndex = cmbKaroserija.SelectedIndex = cmbKubikaza.SelectedIndex = cmbMenjac.SelectedIndex = cmbPogon.SelectedIndex = cmbVrata.SelectedIndex = 0;
                }


        }

        private void btnPrikaziTermine_Click(object sender, EventArgs e)
        {
            bool ima = false;
            odabranePonude = new List<Ponuda>();
            odabraneRezervacije = new List<Rezervacija>();
            if (cmbMarka.SelectedItem != null && cmbModel.SelectedItem != null && cmbGodiste.SelectedItem != null)
            {
                foreach (Automobil a in listaAuta)
                {
                    if (a.Marka == cmbMarka.SelectedItem.ToString() && a.Model == cmbModel.SelectedItem.ToString() && a.Godiste.ToString() == cmbGodiste.SelectedItem.ToString() && a.Pogon.ToString() == cmbPogon.SelectedItem.ToString() && a.Kubikaza.ToString() == cmbKubikaza.SelectedItem.ToString() && a.Karoserija.ToString() == cmbKaroserija.SelectedItem.ToString() && a.BrVrata.ToString() == cmbVrata.SelectedItem.ToString() && a.Gorivo.ToString() == cmbGorivo.SelectedItem.ToString() && a.Menjac.ToString() == cmbMenjac.SelectedItem.ToString())
                    {
                        foreach (Ponuda p in listaPonuda)
                        {
                            if (p.IdAuta == a.Id)
                            {
                                odabranePonude.Add(p);
                                ima = true;
                            }
                        }
                        foreach(Rezervacija r in listaRezervacija)
                        {
                            if(r.IdAutomobila == a.Id)
                            {
                                odabraneRezervacije.Add(r);
                            }
                        }
                    } 
                }
            }
            
            if (!ima)
            {
                MessageBox.Show("Nema termina za izabrane kriterijume auta!");
                lstTermini.Items.Clear();
            }
            else
            {
                Ponuda.Sortiraj(odabranePonude);
                Rezervacija.Sortiraj(odabraneRezervacije);
                RacunajPonude();
                IspisiPonude();
            }
        }

        private void RacunajPonude()
        {
            izabranePonude = new List<Ponuda>();
            DateTime ponPocetak, ponKraj, rezPocetak, rezKraj;

            for(int i=0; i < odabranePonude.Count; i++)
            {
                bool provera = true;
                bool cela = true;
                for(int j=0; j< odabraneRezervacije.Count; j++)
                {
                    if(odabranePonude[i].IdAuta == odabraneRezervacije[j].IdAutomobila)
                    {
                        provera = false;
                        ponPocetak = odabranePonude[i].Datum_od.Date;
                        ponKraj = odabranePonude[i].Datum_do.Date;

                        rezPocetak = odabraneRezervacije[j].DatumOd.Date;
                        rezKraj = odabraneRezervacije[j].DatumDo.Date;

                        if(ponPocetak >= rezPocetak && ponKraj <= rezKraj)
                        {
                            cela = false;
                            break;
                        }
                        else if(ponPocetak >= rezPocetak && ponPocetak <= rezKraj && ponKraj > rezKraj)
                        {
                            cela = false;
                            odabranePonude[i] = new Ponuda(odabranePonude[i].IdAuta, rezKraj.AddDays(1).Date, ponKraj, odabranePonude[i].CenaDan);

                            if(j == odabraneRezervacije.Count - 1)
                            {
                                izabranePonude.Add(odabranePonude[i]);
                                break;
                            }
                            else if(ponKraj < odabraneRezervacije[j+i].DatumOd.Date || odabraneRezervacije[j+1].IdAutomobila != odabranePonude[i].IdAuta)
                            {
                                izabranePonude.Add(odabranePonude[i]);
                                break;
                            }

                        }
                        else if(ponPocetak <= rezPocetak && ponKraj >= rezKraj)
                        {
                            cela = false;
                            odabranePonude[i] = new Ponuda(odabranePonude[i].IdAuta, rezKraj.AddDays(1).Date, ponKraj, odabranePonude[i].CenaDan);
                            izabranePonude.Add(new Ponuda(odabranePonude[i].IdAuta, ponPocetak.Date, rezPocetak.AddDays(-1).Date, odabranePonude[i].CenaDan));

                            if(j == odabraneRezervacije.Count - 1)
                            {
                                izabranePonude.Add(odabranePonude[i]);
                                break;
                            }
                            else if(ponKraj < odabraneRezervacije[j+1].DatumOd.Date || odabraneRezervacije[j+1].IdAutomobila != odabranePonude[i].IdAuta)
                            {
                                izabranePonude.Add(odabranePonude[i]);
                                break;
                            }
                        }
                        else if (ponPocetak <= rezPocetak && ponKraj <= rezKraj && ponKraj > rezPocetak)
                        {
                            cela = false;
                            izabranePonude.Add(new Ponuda(odabranePonude[i].IdAuta, ponPocetak, rezPocetak.AddDays(-1).Date, odabranePonude[i].CenaDan));
                            break;
                        }

                    }
                }
                if(provera || cela)
                {
                    izabranePonude.Add(odabranePonude[i]);
                }

            }
        }

        private void IspisiPonude()
        {
            for(int i = 0; i<izabranePonude.Count; i++)
            {
                if (izabranePonude[i].Datum_od > izabranePonude[i].Datum_do)
                    izabranePonude.RemoveAt(i--);
            }
            lstTermini.Items.Clear();
            foreach(Ponuda p in izabranePonude)
            {
                lstTermini.Items.Add(p.Termin());
            }

        }


        private void btnRezervisi_Click(object sender, EventArgs e)
        {
            bool nadjen = true;
            if (izabranePonude != null)
            {
                foreach (Ponuda p in izabranePonude)
                {
                    if (dtDatumOd.Value.Date >= p.Datum_od.Date && dtDatumDo.Value.Date <= p.Datum_do.Date)
                    {

                        cena = (double)((int)(dtDatumDo.Value.Date - dtDatumOd.Value.Date).TotalDays * (int)p.CenaDan) + p.CenaDan;
                        listaRezervacija.Add(new Rezervacija(p.IdAuta, idKupca, dtDatumOd.Value.Date, dtDatumDo.Value.Date, cena));
                        prikaziRezervacije(listaRezervacija);
                        Datoteka.Upisi(listaRezervacija, putanjaRezervacija);
                        btnPrikaziTermine_Click(sender, e);
                        nadjen = false;
                        MessageBox.Show("Uspesno rezervisano!");
                        this.Close();
                        break;
                    }
                }
                if (nadjen)
                {
                    MessageBox.Show("Ponuda nije dostupna u izabranom terminu");
                }
            }
            else
                MessageBox.Show("Morate prvo videti mogucnosti izbora termina!");
        }

        private void dtDatumOd_ValueChanged(object sender, EventArgs e)
        {
            dtDatumDo.MinDate = dtDatumOd.Value;
            PrikaziCenu();
        }

        private void dtDatumDo_ValueChanged(object sender, EventArgs e)
        {
            PrikaziCenu();
        }

        private void PrikaziCenu()
        {
            double cenaPrikaz = 0;
            if (lstTermini.Items.Count != 0)
            {
                foreach (Ponuda p in izabranePonude)
                {
                    if (dtDatumOd.Value.Date >= p.Datum_od && dtDatumDo.Value.Date <= p.Datum_do.Date)
                    {
                        cenaPrikaz = p.CenaDan;
                        break;
                    }
                }
                if (cenaPrikaz != 0)
                    txtCena.Text = (((dtDatumDo.Value.Date - dtDatumOd.Value.Date).TotalDays + 1) * cenaPrikaz).ToString();
                else
                    txtCena.Text = "nepoznato";
            }
        }

    }
}
