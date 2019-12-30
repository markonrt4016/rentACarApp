using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{   [Serializable]
    public class Kupac : Korisnik
    {
        int id;
        string ime, prezime, jmbg, telefon;
        DateTime datumRod;
        static int brojKupaca;
        public Kupac() : base()
        {
            ime = "";
            prezime = "";
            jmbg = "";
            telefon = "";
            datumRod = new DateTime();
            brojKupaca++;
            id = brojKupaca;
        }

        public Kupac(string ime, string prezime, string jmbg, string telefon, DateTime datumRod, string korisnickoIme, string password):base(korisnickoIme, password)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.jmbg = jmbg;
            this.telefon = telefon;
            this.datumRod = datumRod;
            brojKupaca++;
            id = brojKupaca;
        }

        public Kupac(int id, string ime, string prezime, string jmbg, string telefon, DateTime datumRod, string korisnickoIme, string password) : base(korisnickoIme, password)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.jmbg = jmbg;
            this.telefon = telefon;
            this.datumRod = datumRod;
            this.id = id;
        }

        public int Id { get => id; set => id = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public string Jmbg { get => jmbg; set => jmbg = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public DateTime DatumRod { get => datumRod; set => datumRod = value; }
        public static int BrojKupaca { get => brojKupaca; set => brojKupaca = value; }

        public override string ToString()
        {
            return "Kupac: " + "id: " + id.ToString() + " Ime i prezime: " + ime + " " + prezime + " jmbg: " + jmbg + " telefon: " + telefon + " datum rodjenja: " + datumRod.Date.ToString("d/M/yyyy") + "Podaci za logovanje: " + KorisnickoIme + "...." + Lozinka;
        }

        public string RezervacijaString()
        {
            return "IDBR: " + Id + "Ime:" + Ime + "Prezime " + Prezime;
        }

    }
}
