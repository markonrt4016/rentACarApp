using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat1TVP
{
    [Serializable]
    public class Rezervacija
    {
        int idAuta;
        int idKupca;
        DateTime datumOd;
        DateTime datumDo;
        double cena;

        public Rezervacija(int id_automobila, int id_kupca, DateTime datum_od, DateTime datum_do, double cena)
        {
            this.idAuta = id_automobila;
            this.idKupca = id_kupca;
            this.datumOd = datum_od;
            this.datumDo = datum_do;
            this.cena = cena;
        }

        public int IdAutomobila { get => idAuta; set => idAuta = value; }
        public int IdKupca { get => idKupca; set => idKupca = value; }
        public DateTime DatumOd { get => datumOd; set => datumOd = value; }
        public DateTime DatumDo { get => datumDo; set => datumDo = value; }
        public double Cena { get => cena; set => cena = value; }


        public static void Sortiraj(List<Rezervacija> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                for (int j = i + 1; j < lista.Count; j++)
                {
                    if (lista[i].idAuta > lista[j].idAuta || (lista[i].idAuta == lista[j].idAuta && lista[i].datumOd.Date > lista[j].datumOd.Date))
                    {
                        Rezervacija pom = lista[i];
                        lista[i] = lista[j];
                        lista[j] = pom;
                    }
                }
            }

        }



        public override string ToString()
        {
            return "ID Auta: " + idAuta + "  Datum od: " + datumOd.ToString("dd/MM/yyyy") + "  Datum do: " + datumDo.ToString("dd/MM/yyyy") + " Cena: " + cena;
        }
    }
}
