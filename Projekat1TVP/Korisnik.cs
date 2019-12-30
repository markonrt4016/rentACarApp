using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekat1TVP
{
    [Serializable]
    public abstract class Korisnik
    {
        private string lozinka;
        private string korisnickoIme;

        public string KorisnickoIme { get => korisnickoIme; set => korisnickoIme = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }

        public Korisnik()
        {
            korisnickoIme = "";
            lozinka = "";
        }

        public Korisnik(string ime, string pas)
        {
            korisnickoIme = ime;
            lozinka = pas;
        }

    }
}
