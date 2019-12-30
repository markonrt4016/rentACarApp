using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{
    
    public partial class FrmAdmin : Form
    {
        Admin admin;
        const int Podrazumevano = 0, Izmena = 1, Brisanje = 2;
        Ponuda uzorakPonuda;
        bool zabraniCrtanje;
        FrmLogovanje formaLogovanje;
        public FrmAdmin()
        {
            InitializeComponent();
            admin = new Admin("admin", "admin");
        }

        public FrmAdmin(FrmLogovanje f)
        {
            InitializeComponent();
            admin = new Admin("admin", "admin");
            formaLogovanje = f;
        }

        private bool proveriKontrole(ref int id, ref int kub)
        {
            bool status = true;
            foreach(Control c in panelAutomobil.Controls)
            {
                if (c.Name.StartsWith("cmbOdb") && (c as ComboBox).SelectedItem == null)
                    status = false;
                else if(c.Name.StartsWith("txt") && !String.IsNullOrEmpty((c as TextBox).Text))
                {
                    if (!Int32.TryParse(txtID.Text, out id) || !Int32.TryParse(txtKubikaza.Text, out kub))
                        status = false;
                }
            }
            return status;
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            int id = 0, kub = 0;
            if(proveriKontrole(ref id, ref kub))
            {
                int vrata = Int32.Parse(cmbOdbVrata.SelectedItem.ToString());
                int god = Int32.Parse(cmbOdbGod.SelectedItem.ToString());
                admin.IzmeniVozilo(new Automobil(id, god, kub, vrata, txtMarka.Text, txtModel.Text, cmbOdbPogon.SelectedItem.ToString(), cmbOdbMenjac.SelectedItem.ToString(), cmbOdbKaroserija.SelectedItem.ToString(), cmbOdbGorivo.SelectedItem.ToString()));
                NapuniCmb(admin.ListaAuta, cmbPretragaMarka);
                MessageBox.Show("Uspesno izmenjeni podaci!");
            }
            else
                MessageBox.Show("Uneti podaci nisu odgovarajuci!");
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            int id = 0, kub = 0;
            if (proveriKontrole(ref id, ref kub))
            {
                int vrata = Int32.Parse(cmbOdbVrata.SelectedItem.ToString());
                int god = Int32.Parse(cmbOdbGod.SelectedItem.ToString());
                
                if (admin.DodajVozilo(new Automobil(id, god, kub, vrata, txtMarka.Text, txtModel.Text, cmbOdbPogon.SelectedItem.ToString(), cmbOdbMenjac.SelectedItem.ToString(), cmbOdbKaroserija.SelectedItem.ToString(), cmbOdbGorivo.SelectedItem.ToString())))
                    MessageBox.Show("Uspesno dodat automobil!");
                else
                    MessageBox.Show("Vec postoji dati automobil!");

                if (admin.ListaAuta != null)
                    cmbOdabirOpcije.Enabled = true;
            }
            else
            {
                MessageBox.Show("Neispravno popunjena polja!");
            }
        }
        private void btnObrisi_Click(object sender, EventArgs e)
        {
            int id = 0, kub = 0;
            if (proveriKontrole(ref id, ref kub))
            {
                int vrata = Int32.Parse(cmbOdbVrata.SelectedItem.ToString());
                int god = Int32.Parse(cmbOdbGod.SelectedItem.ToString());
                admin.ObrisiVozilo(new Automobil(id, god, kub, vrata, txtMarka.Text, txtModel.Text, cmbOdbPogon.SelectedItem.ToString(), cmbOdbMenjac.SelectedItem.ToString(), cmbOdbKaroserija.SelectedItem.ToString(), cmbOdbGorivo.SelectedItem.ToString()));
                MessageBox.Show("Uspesno obrisan automobil!");
                NapuniCmb(admin.ListaAuta, cmbPretragaMarka);
            }
            else
            {
                MessageBox.Show("Odaberite automobil!");
            }
            
        }
        private void cmbOdabirOpcije_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            btnDodajVozilo.Click -= btnIzmeni_Click;
            btnDodajVozilo.Click -= btnDodaj_Click;
            btnDodajVozilo.Click -= btnObrisi_Click;
            foreach (Control c in panelAutomobil.Controls)
                if (c is TextBox)
                    c.Text = "";
                else if (c.Name.StartsWith("cmbPretraga"))
                    (c as ComboBox).Items.Clear();
                else if (c.Name.Contains("Odb"))
                    (c as ComboBox).SelectedIndex = -1;

            switch (cmbOdabirOpcije.SelectedIndex)
            {
                case Podrazumevano:
                    panelPretraga.Hide();
                    foreach (Control c in panelAutomobil.Controls)
                        c.Enabled = true;
                    txtID.ReadOnly = false;
                    btnDodajVozilo.Text = "Dodaj vozilo";
                    btnDodajVozilo.Click += btnDodaj_Click;

                    break;
                case Izmena:
                    panelPretraga.Show();
                    txtID.ReadOnly = true;
                    btnDodajVozilo.Text = "Izmeni podatke";
                    btnDodajVozilo.Click += btnIzmeni_Click;
                    foreach (Control c in panelAutomobil.Controls)
                        if (c != panelPretraga)
                            c.Enabled = false;
                    NapuniCmb(admin.ListaAuta, cmbPretragaMarka);
                    break;
                case Brisanje:
                    panelPretraga.Show();
                    btnDodajVozilo.Text = "Obriši vozilo";
                    btnDodajVozilo.Click += btnObrisi_Click;
                    foreach (Control c in panelAutomobil.Controls)
                        if (c != panelPretraga)
                            c.Enabled = false;

                    NapuniCmb(admin.ListaAuta, cmbPretragaMarka);
                    break;
            }
        }
   
        private void NapuniCmb(List<Automobil> listaPunjenje, ComboBox cmb)
        {
            foreach (Control c in panelAutomobil.Controls)
                if (c != panelPretraga)
                    c.Enabled = false;

            if (cmb == cmbPretragaMarka)
            {
                cmbPretragaModel.Items.Clear();
                cmbPretragaGodiste.Items.Clear();
                cmbPretragaID.Items.Clear();
                foreach (Automobil a in listaPunjenje)
                    if (cmb.SelectedItem != null && a.Marka.Equals(cmb.SelectedItem.ToString()) && !cmbPretragaModel.Items.Contains(a.Model))
                            cmbPretragaModel.Items.Add(a.Model);
                    else if (!cmb.Items.Contains(a.Marka))
                        cmb.Items.Add(a.Marka);
            }
            else if(cmb == cmbPretragaModel)
            {
                cmbPretragaGodiste.Items.Clear();
                foreach (Automobil a in listaPunjenje)
                    if (cmb.SelectedItem != null && a.Model.Equals(cmb.SelectedItem.ToString()) && !cmbPretragaGodiste.Items.Contains(a.Godiste))
                            cmbPretragaGodiste.Items.Add(a.Godiste);
            }
            else
            {
                cmbPretragaID.Items.Clear();
                foreach (Automobil a in listaPunjenje)
                    if (cmb.SelectedItem != null && a.Godiste.ToString().Equals(cmb.SelectedItem.ToString()) && !cmbPretragaID.Items.Contains(a.Id))
                        cmbPretragaID.Items.Add(a.Id);
            }
        }

        private void cmbID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((sender as ComboBox).SelectedItem != null)
            {
                foreach (Control c in panelAutomobil.Controls)
                    if (cmbOdabirOpcije.SelectedIndex != Brisanje || c is Button)
                        c.Enabled = true;
                int id;
                int.TryParse((sender as ComboBox).SelectedItem.ToString(), out id);
                foreach (Automobil a in admin.ListaAuta)
                {
                    if (a.Id == id)
                    {
                        txtID.Text = a.Id.ToString();
                        txtMarka.Text = a.Marka;
                        txtModel.Text = a.Model;
                        cmbOdbGod.SelectedItem = a.Godiste;
                        txtKubikaza.Text = a.Kubikaza.ToString();
                        cmbOdbPogon.SelectedItem = a.Pogon;
                        cmbOdbMenjac.SelectedItem = a.Menjac;
                        cmbOdbKaroserija.SelectedItem = a.Karoserija;
                        cmbOdbGorivo.SelectedItem = a.Gorivo;
                        cmbOdbVrata.SelectedItem = a.BrVrata.ToString();
                        break;
                    }
                }
            }    
        }


        private void cmbPretragaPromenjenIndex(object sender, EventArgs e)
        {
            NapuniCmb(admin.ListaAuta, sender as ComboBox);
        }

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            if (admin.ListaAuta == null)
                cmbOdabirOpcije.Enabled = false;

            cmbOdabirOpcije.SelectedIndex = Podrazumevano;
            panelAutomobil.Height  = tabOpcijaAzuriranja.Height;
            panelAutomobil.Width  = tabOpcijaAzuriranja.Width;
            panelAutomobil.Top  = lblObjasnjenje.Top + lblObjasnjenje.Height;
            panelAutomobil.Left  = lblObjasnjenje.Left;
            for (int i = DateTime.Today.Year; i >= 1900 ; i--)
                cmbOdbGod.Items.Add(i);

            radioDodaj.CheckedChanged += RadioPromenaOpcije;
            radioBrisanje.CheckedChanged += RadioPromenaOpcije;
            radioIzmeni.CheckedChanged += RadioPromenaOpcije;
            radioDodaj.Checked = true;

            if (admin.ListaKupaca == null) //kad nema datoteke kupci
            {
                radioIzmeni.Enabled = radioBrisanje.Enabled = false;
                btnAzuriranjeKupca.Click += btnDodajKupca_Click;
                txtIdKupca.Text = "1";
                txtIdKupca.Text = "1";
            }

            radioDodajPonudu.CheckedChanged += RadioPromenaOpcijePonude;
            radioIzmeniPonudu.CheckedChanged += RadioPromenaOpcijePonude;
            radioBrisiPonudu.CheckedChanged += RadioPromenaOpcijePonude;

            radioDodajPonudu.Checked = true;


            zabraniCrtanje = true;

        }

        private void RadioPromenaOpcije(object sender, EventArgs e)
        {
            foreach (Control c in tabKupac.Controls)
                if (c is TextBox)
                    c.Text = "";
            btnAzuriranjeKupca.Click -= btnDodajKupca_Click;
            btnAzuriranjeKupca.Click -= btnIzmeniKupca_Click;
            btnAzuriranjeKupca.Click -= btnBrisiKupca_Click;
            if (radioDodaj.Checked && admin.ListaKupaca != null)
            {
                foreach (Control c in tabKupac.Controls)
                    if (c != txtIdKupca)
                        c.Enabled = true;

                Kupac.BrojKupaca = admin.ListaKupaca.Count();
                lstKupci.Items.Clear();
                foreach (Kupac k in admin.ListaKupaca)
                    lstKupci.Items.Add(k.ToString());

                btnAzuriranjeKupca.Text = "Dodaj kupca";
                btnAzuriranjeKupca.Click += btnDodajKupca_Click;
                txtIdKupca.Text = (Kupac.BrojKupaca +1).ToString();
            }
            else if(radioIzmeni.Checked)
            {
                foreach (Control c in tabKupac.Controls)
                    if (c != txtIdKupca)
                        c.Enabled = true;
                btnAzuriranjeKupca.Text = "Izmeni kupca";
                btnAzuriranjeKupca.Click += btnIzmeniKupca_Click;
            }
            else if(radioBrisanje.Checked)
            {
                foreach (Control c in tabKupac.Controls)
                    if (c is TextBox || c is DateTimePicker)
                        c.Enabled = false;
                btnAzuriranjeKupca.Text = "Briši kupca";
                btnAzuriranjeKupca.Click += btnBrisiKupca_Click;
            }
            
        }

        private void lstKupci_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((radioIzmeni.Checked || radioBrisanje.Checked) && lstKupci.SelectedItem != null)
            {
                int idIzmene = -1;
                for (int i = admin.ListaKupaca.Count.ToString().Length; i > 0; i--)
                    if (int.TryParse(lstKupci.SelectedItem.ToString().Substring(10, 2 + i), out idIzmene))
                        break;
                Kupac kupacIzmena = null;
                foreach (Kupac k in admin.ListaKupaca)
                    if (k.Id == idIzmene)
                    {
                        kupacIzmena = k;
                        break;
                    }
                foreach (Control c in this.Controls)
                    if (c is TextBox)
                        c.Text = "";

                txtIdKupca.Text = kupacIzmena.Id.ToString();
                txtIme.Text = kupacIzmena.Ime;
                txtPrezime.Text = kupacIzmena.Prezime;
                dtDatumRod.Value = kupacIzmena.DatumRod;
                txtKorisnickoIme.Text = kupacIzmena.KorisnickoIme;
                txtLozinka.Text = kupacIzmena.Lozinka;
                txtJMBG.Text = kupacIzmena.Jmbg;
                txtTelefon.Text = kupacIzmena.Telefon;
            }
        }

        private void btnDodajKupca_Click(object sender, EventArgs e)
        {
            foreach (Control c in tabKupac.Controls)
            {
                if (c is TextBox && c.Text == "")
                {
                    MessageBox.Show("Popunite sva polja!");
                    return;
                } 
            }
            Regex regJmbg = new Regex("^[0-9]{13}$");
            Regex regBroj = new Regex("^[0-9]{3}-[0-9]{3,4}-[0-9]{3}$");
            if (!regJmbg.IsMatch(txtJMBG.Text) || !regBroj.IsMatch(txtTelefon.Text))
            {
                MessageBox.Show("Neispravan jmbg ili telefon!");
                return;
            }

            foreach (Control c in tabKupac.Controls)
                if (c is TextBox && String.IsNullOrEmpty((c as TextBox).Text))
                {
                    MessageBox.Show("Popunite sva polja!");
                    return;
                }
                    
            Kupac k = new Kupac(txtIme.Text, txtPrezime.Text, txtJMBG.Text, txtTelefon.Text, dtDatumRod.Value, txtKorisnickoIme.Text, txtLozinka.Text);
            if (admin.DodajKupca(k))
            {
                MessageBox.Show("Uspesno dodat kupac!");
                lstKupci.Items.Add(k.ToString());

                foreach (Control c in tabKupac.Controls)
                    if (c is TextBox)
                        c.Text = "";
                    else if (c is DateTimePicker)
                        (c as DateTimePicker).Value = DateTime.Now;
                txtIdKupca.Text = (Kupac.BrojKupaca + 1).ToString();
                radioDodaj.Enabled = radioIzmeni.Enabled = true;

            }
            else
                MessageBox.Show("Greska, vec postoji kupac sa datim korisnickim imenom!");

            

        }

        private void btnIzmeniKupca_Click(object sender, EventArgs e)
        {
            foreach (Control c in tabKupac.Controls)
            {
                if (c is TextBox && c.Text == "")
                {
                    MessageBox.Show("Popunite sva polja!");
                    return;
                }
            }
            Regex regJmbg = new Regex("^[0-9]{13}$");
            Regex regBroj = new Regex("^[0-9]{3}-[0-9]{3,4}-[0-9]{3}$");
            if (!regJmbg.IsMatch(txtJMBG.Text) || !regBroj.IsMatch(txtTelefon.Text))
            {
                MessageBox.Show("Neispravan jmbg ili telefon!");
                return;
            }

            foreach (Control c in tabKupac.Controls)
                if (c is TextBox && String.IsNullOrEmpty((c as TextBox).Text))
                {
                    MessageBox.Show("Popunite sva polja!");
                    return;
                }

            int izmenjenId = int.Parse(txtIdKupca.Text);
            Kupac k = new Kupac(izmenjenId, txtIme.Text, txtPrezime.Text, txtJMBG.Text, txtTelefon.Text, dtDatumRod.Value, txtKorisnickoIme.Text, txtLozinka.Text);


            if(admin.IzmeniKupca(k) != null)
            {
                MessageBox.Show("Uspesno izmenjeno!");

                lstKupci.Items.Clear();
                foreach (Kupac kup in admin.ListaKupaca)
                    lstKupci.Items.Add(kup.ToString());

                dtDatumRod.Value = DateTime.Now;
                foreach (Control c in tabKupac.Controls)
                    if (c is TextBox)
                        c.Text = "";
            }
            else
            {
                MessageBox.Show("Postoji korisnik sa datim korisnickim imenom!");
            }

            //admin.ListaKupaca = admin.IzmeniKupca(k);
        }

        private void btnBrisiKupca_Click(object sender, EventArgs e)
        {
            foreach(Control c in tabKupac.Controls)
                if(c is TextBox && String.IsNullOrEmpty((c as TextBox).Text))
                {
                    MessageBox.Show("Niste izabrali nijednog kupca!");
                    return;
                }
            admin.ListaKupaca = admin.ObrisiKupca(new Kupac(int.Parse(txtIdKupca.Text),txtIme.Text, txtPrezime.Text, txtJMBG.Text, txtTelefon.Text, dtDatumRod.Value, txtKorisnickoIme.Text, txtLozinka.Text));
            foreach (Control c in tabKupac.Controls)
                if (c is TextBox)
                    c.Text = "";
            lstKupci.Items.Clear();
            foreach (Kupac kup in admin.ListaKupaca)
                lstKupci.Items.Add(kup.ToString());

            MessageBox.Show("Uspesno obrisano!");

        }


        private void tabOpcijaAzuriranja_SelectedIndexChanged(object sender, EventArgs e)
        {
            zabraniCrtanje = true;
            //admin.ListaAuta = Datoteka.Citaj("auti.bin") as List<Automobil>;
            if (tabOpcijaAzuriranja.SelectedIndex == tabPonuda.TabIndex)
            {

                if (admin.ListaAuta != null)
                {
                    cmbIdAutaPonuda.Enabled = true;
                    cmbIdAutaPonuda.Items.Clear();
                    foreach (Automobil a in admin.ListaAuta)
                        cmbIdAutaPonuda.Items.Add(a.Id);
                    cmbIdAutaPonuda.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Nemate nijedan auto trenutno!");
                    cmbIdAutaPonuda.Enabled = false;
                }

                if (!radioDodajPonudu.Checked)
                {
                    cmbIdAutaPonuda.Enabled = false;
                    foreach (Control c in tabPonuda.Controls)
                        if (c is TextBox)
                            c.Enabled = false;
                }
                    
                    
                if (admin.ListaPonuda != null)
                {
                    lstPonude.Items.Clear();
                    foreach (Ponuda p in admin.ListaPonuda)
                        lstPonude.Items.Add(p.ToString());
                    radioIzmeniPonudu.Enabled = radioBrisiPonudu.Enabled = true;
                }
            }

            if (tabOpcijaAzuriranja.SelectedIndex == tabRezervacije.TabIndex)
            {
                cmbRezervacijeKupac.Items.Clear();
                if (admin.ListaKupaca != null)
                {
                    foreach (Kupac k in admin.ListaKupaca)
                    {
                        cmbRezervacijeKupac.Items.Add(k.RezervacijaString());
                    }
                }
            }

            lstRezervacije.Items.Clear();

            dtDatumOd.MinDate = DateTime.Now;
            dtDatumDo.Value = dtDatumOd.Value;
            //statistika
            lblLegenda.Hide();
            panelLegenda.Controls.Clear();

            cmbStatGodina.Items.Clear();

            NapuniGodineStatistike();

        }

        private void RadioPromenaOpcijePonude(object sender, EventArgs e)
        {
            btnAzuriranjePonude.Click -= btnDodajPonudu;
            btnAzuriranjePonude.Click -= btnIzmeniPonudu;
            btnAzuriranjePonude.Click -= btnObrisiPonudu;
            cmbIdAutaPonuda.Enabled = dtDatumOd.Enabled = dtDatumDo.Enabled = txtCenaDan.Enabled = true;
            if (radioDodajPonudu.Checked)
            {
                btnAzuriranjePonude.Text = "Dodaj ponudu";
                btnAzuriranjePonude.Click += btnDodajPonudu;
            }
            else if (radioIzmeniPonudu.Checked)
            {
                btnAzuriranjePonude.Text = "Izmeni ponudu";
                btnAzuriranjePonude.Click += btnIzmeniPonudu;
                cmbIdAutaPonuda.Enabled = false;
            }
            else if (radioBrisiPonudu.Checked)
            {
                btnAzuriranjePonude.Text = "Obriši ponudu";
                btnAzuriranjePonude.Click += btnObrisiPonudu;
                cmbIdAutaPonuda.Enabled = dtDatumOd.Enabled = dtDatumDo.Enabled = txtCenaDan.Enabled = false;
            }
        }

        private void btnIzmeniPonudu(object sender, EventArgs e)
        {
            double cena;
            if (lstPonude.SelectedItem == null)
            {
                MessageBox.Show("Niste odabrali ponudu!");
            }
            else if(!double.TryParse(txtCenaDan.Text, out cena))
            {
                MessageBox.Show("Neispravan unos cene!");
            }
            else
            {
                Ponuda p = new Ponuda(int.Parse(cmbIdAutaPonuda.SelectedItem.ToString()), dtDatumOd.Value, dtDatumDo.Value, cena);
                admin.ObrisiPonudu(uzorakPonuda);

                if (!admin.DodajPonudu(p))
                {
                    MessageBox.Show("Preklopljen datum!");
                }
                else
                {
                    admin.DodajPonudu(uzorakPonuda);
                    lstPonude.Items.Clear();
                    foreach (Ponuda pon in admin.ListaPonuda)
                        lstPonude.Items.Add(pon.ToString());
                    MessageBox.Show("Uspesno izmenjen datum!");
                }
            }
        }

        private void btnDodajPonudu(object sender, EventArgs e)
        {
            double cenaDan;
            DateTime dOd = dtDatumOd.Value;
            DateTime dDo = dtDatumDo.Value;

            if (admin.ListaAuta == null)
            {
                MessageBox.Show("Dodajte makar 1 auto da bi ste napravili ponudu!");
                return;
            }

            int idAuta = int.Parse(cmbIdAutaPonuda.SelectedItem.ToString());
            if (!double.TryParse(txtCenaDan.Text, out cenaDan) || (dOd.Ticks > dDo.Ticks))
            {
                MessageBox.Show("Nepravilan unos cene ili datuma!");
                return;
            }
            if (admin.DodajPonudu(new Ponuda(idAuta, dOd, dDo, cenaDan)))
            {
                MessageBox.Show("Uspesno dodata ponuda!");
                lstPonude.Items.Clear();
                foreach (Ponuda p in admin.ListaPonuda)
                    lstPonude.Items.Add(p.ToString());
            }
            else
                MessageBox.Show("Preklapa se!");
        }

        private void lstPonude_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!radioDodajPonudu.Checked && lstPonude.SelectedItem != null && admin.ListaAuta != null)
            {
                string procitanaPonuda = lstPonude.SelectedItem.ToString();
                DateTime dOdProcitan = DateTime.Now, dDoProcitan = DateTime.Now;
            
                int maxId = admin.ListaAuta[0].Id;
                foreach(Automobil a in admin.ListaAuta)
                    if (a.Id > maxId)
                        maxId = a.Id;

                Ponuda trazena = null;
                foreach (Ponuda p in admin.ListaPonuda)
                {
                    if (p.ToString() == procitanaPonuda)
                    {
                        trazena = p;
                    }
                }
                
                uzorakPonuda = trazena;

                cmbIdAutaPonuda.SelectedItem = trazena.IdAuta;
                dtDatumOd.Value = trazena.Datum_od;
                dtDatumDo.Value = trazena.Datum_do;
                txtCenaDan.Text = trazena.CenaDan.ToString();
                if(radioBrisiPonudu.Checked)
                    txtCenaDan.Enabled = false;
                
            }
        }

        private void lstRezervacije_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstRezervacije.SelectedItem != null)
            {
                foreach(Rezervacija r in admin.ListaRezervacija)
                {
                    if(r.ToString() == lstRezervacije.SelectedItem.ToString())
                    {
                        txtRezIdAuto.Text = r.IdAutomobila.ToString();
                        txtRezIdKupca.Text = r.IdKupca.ToString();
                        txtRezCena.Text = r.Cena.ToString();
                        dtRezDatumDo.Value = r.DatumDo;
                        dtRezDatumOd.Value = r.DatumOd;
                        break;
                    }
                }
            }
        }

        private void btnObrisiRez_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtRezIdAuto.Text))
            {
                Rezervacija r = new Rezervacija(int.Parse(txtRezIdAuto.Text), int.Parse(txtRezIdKupca.Text), dtRezDatumOd.Value, dtRezDatumDo.Value, double.Parse(txtRezCena.Text));
                if (admin.ObrisiRezervaciju(r) != null)
                {
                    MessageBox.Show("Uspesno obrisana rezervacija!");
                    lstRezervacije.Items.Clear();

                    foreach (Rezervacija rez in admin.ListaRezervacija)
                    {
                        if(int.Parse(txtRezIdKupca.Text) == rez.IdKupca)
                            lstRezervacije.Items.Add(rez.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Greska!");
                }
            }
            else
            {
                MessageBox.Show("Morate odabrati neku rezervaciju!");
            }


            txtRezCena.Text = txtRezIdAuto.Text = txtRezIdKupca.Text = "";

        }

        private void btnObrisiPonudu(object sender, EventArgs e)
        {
            if (lstPonude.SelectedItem == null)
                MessageBox.Show("Morate izabrati odredjenu ponudu!");
            else
            {
                Ponuda p = new Ponuda(int.Parse(cmbIdAutaPonuda.SelectedItem.ToString()), dtDatumOd.Value, dtDatumDo.Value, int.Parse(txtCenaDan.Text));
                admin.ObrisiPonudu(p);
                MessageBox.Show("Uspesno obrisano!");
                lstPonude.Items.Clear();
                foreach (Ponuda pon in admin.ListaPonuda)
                    lstPonude.Items.Add(pon.ToString());
                txtCenaDan.Text = "";
            }
        }


        public int UkupnoDanaMesec(int mesec, int godina)
        {
            int ukupnoDana = 0;
            foreach(Automobil a in admin.ListaAuta)
            {
                ukupnoDana += RacunajDaneMesec(a.Id, admin.ListaRezervacija, mesec, godina);
            }
            return ukupnoDana;
        }



        public int RacunajDaneMesec(int autoIdbr, List<Rezervacija> rezervacije, int mesec, int godina)
        {
            int ukupnoAuto = 0;
            DateTime pocetak = new DateTime(godina, mesec, 1).Date;
            DateTime kraj = pocetak.AddMonths(1).AddDays(-1).Date;
            Rezervacija.Sortiraj(rezervacije);
            for (int i = 0; i < rezervacije.Count; i++)
            {
                if (autoIdbr == rezervacije[i].IdAutomobila)
                {
                    if (rezervacije[i].DatumOd.Date >= pocetak && rezervacije[i].DatumOd <= kraj)
                    {
                        if (rezervacije[i].DatumDo.Date <= kraj)
                        {
                            ukupnoAuto += (int)(rezervacije[i].DatumDo.Date - rezervacije[i].DatumOd.Date).TotalDays + 1;
                        }
                        else
                        {
                            ukupnoAuto += (int)(kraj - rezervacije[i].DatumOd.Date).TotalDays + 1;
                        }
                    }
                    else if (rezervacije[i].DatumDo >= pocetak && pocetak >= rezervacije[i].DatumOd)
                    {
                        if (rezervacije[i].DatumDo <= kraj)
                        {
                            ukupnoAuto += (int)(rezervacije[i].DatumDo - pocetak).TotalDays + 1;
                        }
                        else if (rezervacije[i].DatumDo > kraj)
                        {
                            ukupnoAuto += (int)(kraj - pocetak).TotalDays + 1;
                        }
                    }
                }
            }
            return ukupnoAuto;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            zabraniCrtanje = false;
            if(cmbStatGodina.SelectedItem != null && cmbStatMesec.SelectedItem != null)
            {
                pnlStatistika.Invalidate();
                lblLegenda.Show();
            }
            else
            {
                MessageBox.Show("Odaberite godinu i mesec!");
                lblLegenda.Hide();
            }
        }

        private void cmbRezervacijeKupac_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstRezervacije.Items.Clear();
            if (admin.ListaRezervacija != null)
            {
                int idKupca = -1;
                foreach (Kupac k in admin.ListaKupaca)
                {
                    if (k.RezervacijaString() == cmbRezervacijeKupac.SelectedItem.ToString())
                    {
                        idKupca = k.Id;
                        break;
                    }
                }
                foreach (Rezervacija r in admin.ListaRezervacija)
                {
                    if(r.IdKupca == idKupca)
                        lstRezervacije.Items.Add(r.ToString());
                }
            }
        }

        private void Crtaj(Graphics g)
        {
            panelLegenda.Controls.Clear();
            float pocetak = 0;
            Random rnd = new Random();
            int ukupnoDana = UkupnoDanaMesec(int.Parse(cmbStatMesec.SelectedItem.ToString()), int.Parse(cmbStatGodina.SelectedItem.ToString()));
            //int ukupnoDana = UkupnoDanaMesec(cmbStatMesec.SelectedIndex + 1, int.Parse(cmbStatGodina.SelectedItem.ToString()));
            int brojac = 0;
            foreach (Automobil a in admin.ListaAuta)
            {
                //int dani = RacunajDaneMesec(a.Id, admin.ListaRezervacija, cmbStatMesec.SelectedIndex + 1, int.Parse(cmbStatGodina.SelectedItem.ToString()));
                int dani = RacunajDaneMesec(a.Id, admin.ListaRezervacija, int.Parse(cmbStatMesec.SelectedItem.ToString()), int.Parse(cmbStatGodina.SelectedItem.ToString()));
                if (dani != 0)
                {
                    Rectangle r = new Rectangle(20,20,pnlStatistika.Width / 2 - 70, pnlStatistika.Height -40);
                    Color c = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                    Brush b = new SolidBrush(c);
                    g.FillPie(b,r,pocetak, (float)(360.0/ukupnoDana * dani));
                    pocetak += (float)(360.0 / ukupnoDana * dani);
                    Label nova = new Label();
                    nova.Font = button1.Font;
                    nova.Width = panelLegenda.Width - 20;
                    nova.Top = 20 * ++brojac;
                    nova.Left = 20;
                    nova.Text = a.Id + ", " + a.Marka + ", " + a.Model + ", "  + Math.Round((100.0/ukupnoDana*dani),2) + "%";
                    nova.BackColor = c;
                    nova.ForeColor = Color.FromArgb(c.ToArgb() ^ 0xffffff);
                    panelLegenda.Controls.Add(nova);
                }
            }

            if(brojac == 0)
            {
                lblLegenda.Hide();
                MessageBox.Show("Nema auta za date kriterijume!");
            }

            zabraniCrtanje = true;
        }

        private void odjaviSeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formaLogovanje.Show();
            this.Hide();
        }

        private void txtCenaDan_TextChanged(object sender, EventArgs e)
        {
            double provera;
            if(double.TryParse((sender as TextBox).Text, out provera) && provera <  0)
            {
                MessageBox.Show("Ne sme biti negativna cena!");
                (sender as TextBox).Text = "";
            }
        }

        private void txtKubikaza_TextChanged(object sender, EventArgs e)
        {
            double provera;
            if (double.TryParse((sender as TextBox).Text, out provera) && provera < 0)
            {
                MessageBox.Show("Ne sme biti negativna kubikaza!");
                (sender as TextBox).Text = "";
            }
        }

        private void dtDatumOd_ValueChanged(object sender, EventArgs e)
        {
            dtDatumDo.MinDate = dtDatumOd.Value;
        }

        private void pnlStatistika_Paint(object sender, PaintEventArgs e)
        {
            if (zabraniCrtanje == false)
                Crtaj(e.Graphics);
        }

        private void cmbStatGodina_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbStatMesec.Items.Clear();
            List<int> meseci = new List<int>();

            if (admin.ListaRezervacija != null)
                foreach (Rezervacija r in admin.ListaRezervacija)
                {
                    if (!meseci.Contains(r.DatumOd.Date.Month) && r.DatumOd.Date.Year == int.Parse(cmbStatGodina.SelectedItem.ToString()))
                        meseci.Add(r.DatumOd.Date.Month);
                    if (!meseci.Contains(r.DatumDo.Date.Month) && r.DatumDo.Date.Year == int.Parse(cmbStatGodina.SelectedItem.ToString()))
                        meseci.Add(r.DatumDo.Date.Month);
                }

            for (int i = 0; i < meseci.Count - 1; i++)
            {
                for (int j = i + 1; j < meseci.Count; j++)
                {
                    if (meseci[i] > meseci[j])
                    {
                        int pom = meseci[i];
                        meseci[i] = meseci[j];
                        meseci[j] = pom;
                    }
                }
            }

            foreach (int m in meseci)
            {
                cmbStatMesec.Items.Add(m);
            }      

        }

        private void cmbStatMesec_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NapuniGodineStatistike()
        {
            List<int> godine = new List<int>();
            if (admin.ListaRezervacija != null)
                foreach (Rezervacija r in admin.ListaRezervacija)
                {
                    if (!godine.Contains(r.DatumOd.Date.Year)){
                        godine.Add(r.DatumOd.Date.Year);
                    }
                    if (!godine.Contains(r.DatumDo.Date.Year))
                        godine.Add(r.DatumDo.Date.Year);
                }

            for (int i = 0; i < godine.Count - 1; i++)
            {
                for (int j = i + 1; j < godine.Count; j++)
                {
                    if (godine[i] > godine[j])
                    {
                        int pom = godine[i];
                        godine[i] = godine[j];
                        godine[j] = pom;
                    }
                }
            }

            foreach (int g in godine)
                cmbStatGodina.Items.Add(g);
        }

    }
}
