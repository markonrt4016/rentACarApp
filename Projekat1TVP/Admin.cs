using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{
    class Admin : Korisnik
    {
        string putanjaAuta, putanjaKupaca, putanjaPonuda, putanjaRezervacije;
        List<Automobil> listaAuta;
        List<Kupac> listaKupaca;
        List<Ponuda> listaPonuda;
        List<Rezervacija> listaRezervacija;
        public Admin() : base()
        {
            putanjaAuta = "auti.bin";
            putanjaKupaca = "kupci.bin";
            putanjaPonuda = "ponude.bin";
            listaPonuda = Datoteka.Citaj(putanjaPonuda) as List<Ponuda>;
            listaKupaca = Datoteka.Citaj(putanjaKupaca) as List<Kupac>;
            listaAuta = Datoteka.Citaj(putanjaAuta) as List<Automobil>;
            putanjaRezervacije = "rezervacije.bin";
            listaRezervacija = Datoteka.Citaj(putanjaRezervacije) as List<Rezervacija>;
        }

        public Admin(string korisnickoIme, string password) : base(korisnickoIme, password)
        {
            putanjaAuta = "auti.bin";
            putanjaKupaca = "kupci.bin";
            putanjaPonuda = "ponude.bin";
            listaPonuda = Datoteka.Citaj(putanjaPonuda) as List<Ponuda>;
            listaKupaca = Datoteka.Citaj(putanjaKupaca) as List<Kupac>;
            listaAuta = Datoteka.Citaj(putanjaAuta) as List<Automobil>;
            putanjaRezervacije = "rezervacije.bin";
            listaRezervacija = Datoteka.Citaj(putanjaRezervacije) as List<Rezervacija>;
        }

        public string PutanjaAuta { get => putanjaAuta; set => putanjaAuta = value; }
        public List<Automobil> ListaAuta { get => listaAuta; set => listaAuta = value; }
        public string PutanjaKupaca { get => putanjaKupaca; set => putanjaKupaca = value; }
        internal List<Kupac> ListaKupaca { get => listaKupaca; set => listaKupaca = value; }
        internal List<Ponuda> ListaPonuda { get => listaPonuda; set => listaPonuda = value; }
        public string PutanjaRezervacije { get => putanjaRezervacije; set => putanjaRezervacije = value; }
        public List<Rezervacija> ListaRezervacija { get => listaRezervacija; set => listaRezervacija = value; }

        public List<Automobil> IzmeniVozilo(Automobil a)
        {
            listaAuta = Datoteka.Citaj(putanjaAuta) as List<Automobil>;

            for (int i = 0; i < listaAuta.Count; i++)
                if (listaAuta[i].Id == a.Id)
                {
                    listaAuta[i] = a;
                    break;
                }
            Datoteka.Upisi(listaAuta, putanjaAuta);
            return listaAuta;
        }

        public List<Automobil> ObrisiVozilo(Automobil a)
        {
            listaAuta = Datoteka.Citaj(putanjaAuta) as List<Automobil>;

            foreach (Automobil n in listaAuta)
                if (n.Id == a.Id)
                {
                    if (listaPonuda != null)
                    {
                        foreach(Ponuda p in listaPonuda)
                        {
                            if(p.IdAuta == n.Id)
                            {
                                ObrisiPonudu(p);
                            }
                        }
                    }
                    Datoteka.Upisi(listaPonuda, putanjaPonuda);


                    listaAuta.Remove(n);
                    break;
                }
            Datoteka.Upisi(listaAuta, putanjaAuta);
            return listaAuta;

        }

        public bool DodajVozilo(Automobil a)
        {
            listaAuta = Datoteka.Citaj(putanjaAuta) as List<Automobil>;
            List<int> idjevi = new List<int>();
            if (listaAuta != null)
            {
                foreach (Automobil automobil in listaAuta)
                {
                    idjevi.Add(automobil.Id);
                }
 
                if (!idjevi.Contains(a.Id))
                {
                    listaAuta.Add(a);
                    Datoteka.Upisi(listaAuta, putanjaAuta);
                    return true;
                }
                else
                    return false;
            }
            else //za prvo pokretanje aplikacije, kad nema datoteke
            {
                listaAuta = new List<Automobil>();
                listaAuta.Add(a);
                Datoteka.Upisi(listaAuta, putanjaAuta);
                return true;
            }
        }

        public bool DodajKupca(Kupac k)
        {
            listaKupaca = Datoteka.Citaj(putanjaKupaca) as List<Kupac>;
            if (listaKupaca != null)
            {

                bool postoji = false;
                foreach(Kupac kup in listaKupaca)
                {
                    if (kup.KorisnickoIme == k.KorisnickoIme)
                        postoji = true;
                }

                if (!postoji)
                {
                    listaKupaca.Add(k);
                    Datoteka.Upisi(listaKupaca, putanjaKupaca);
                    return true;
                }
                else
                {
                    Kupac.BrojKupaca -= 1;
                    return false;
                }
                    
                
            }
            else //za prvo pokretanje aplikacije, kad nema datoteke
            {
                listaKupaca = new List<Kupac>();
                listaKupaca.Add(k);
                Datoteka.Upisi(listaKupaca, putanjaKupaca);
                return true;
            }
        }

        public List<Kupac> IzmeniKupca(Kupac k)
        {
            listaKupaca = Datoteka.Citaj(putanjaKupaca) as List<Kupac>;

            bool postoji = false;
            foreach (Kupac kup in listaKupaca)
            {
                if (kup.KorisnickoIme == k.KorisnickoIme && kup.Id != k.Id)
                    postoji = true;
            }
            if (!postoji)
            {
                for (int i = 0; i < listaKupaca.Count; i++)
                    if (listaKupaca[i].Id == k.Id)
                    {
                        listaKupaca[i] = k;
                        break;
                    }
                Datoteka.Upisi(listaKupaca, putanjaKupaca);
                return listaKupaca;
            }
            else
            {
                return null;
            }
            
        }

        public List<Kupac> ObrisiKupca(Kupac k)
        {
            listaKupaca = Datoteka.Citaj(putanjaKupaca) as List<Kupac>;
            foreach (Kupac n in listaKupaca)
                if (n.Id == k.Id)
                {
                    listaKupaca.Remove(n);
                    break;
                }
            Datoteka.Upisi(listaKupaca, putanjaKupaca);
            return listaKupaca;
        }


        public bool DodajPonudu(Ponuda p)
        {
            listaPonuda = Datoteka.Citaj(putanjaPonuda) as List<Ponuda>;
            if (listaPonuda != null)
            {
                foreach(Ponuda pon in listaPonuda)
                {
                    if (pon.IdAuta == p.IdAuta && ((p.Datum_od.Ticks >= pon.Datum_od.Ticks && p.Datum_do.Ticks <= pon.Datum_do.Ticks) || (p.Datum_do.Ticks >= pon.Datum_od.Ticks && p.Datum_do.Ticks <= pon.Datum_do.Ticks) || (p.Datum_od.Ticks <= pon.Datum_do.Ticks && p.Datum_od.Ticks >= pon.Datum_od.Ticks) || (p.Datum_od.Ticks <= pon.Datum_od.Ticks && p.Datum_do.Ticks >= pon.Datum_do.Ticks)))
                        return false;
                }
                listaPonuda.Add(p);
                Datoteka.Upisi(listaPonuda, putanjaPonuda);
                return true;
            }
            else //za prvo pokretanje aplikacije, kad nema datoteke
            {
                listaPonuda = new List<Ponuda>();
                listaPonuda.Add(p);
                Datoteka.Upisi(listaPonuda, putanjaPonuda);
                return true;
            }
        }

        public List<Ponuda> IzmeniPonudu(Ponuda p)
        {
            listaPonuda = Datoteka.Citaj(putanjaPonuda) as List<Ponuda>;

            for (int i = 0; i < listaPonuda.Count; i++)
                if (listaPonuda[i].IdAuta == p.IdAuta)
                {
                    listaPonuda[i] = p;
                    break;
                }
            Datoteka.Upisi(listaPonuda, putanjaPonuda);
            return listaPonuda;
        }

        public List<Ponuda> ObrisiPonudu(Ponuda p)
        {
            listaPonuda = Datoteka.Citaj(putanjaPonuda) as List<Ponuda>;

            if (listaPonuda == null)
                return null;
            else
            {
                foreach (Ponuda n in listaPonuda)
                    if (n.IdAuta == p.IdAuta && n.Datum_od.Date == p.Datum_od.Date && n.Datum_do == p.Datum_do && p.CenaDan == n.CenaDan)
                    {
                        listaPonuda.Remove(n);
                        break;
                    }
                Datoteka.Upisi(listaPonuda, putanjaPonuda);
                return listaPonuda;
            }
        }

        public List<Rezervacija> ObrisiRezervaciju(Rezervacija r)
        {
            listaRezervacija = Datoteka.Citaj(putanjaRezervacije) as List<Rezervacija>;

            if (listaRezervacija == null)
                return null;
            else
            {
                foreach (Rezervacija rez in listaRezervacija)
                    if (rez.ToString() == r.ToString())
                    {
                        listaRezervacija.Remove(rez);
                        break;
                    }
                
                Datoteka.Upisi(listaRezervacija, putanjaRezervacije);
                return listaRezervacija;
            }
        }

    }
}
