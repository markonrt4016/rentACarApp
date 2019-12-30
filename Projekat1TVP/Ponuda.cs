using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat1TVP
{
    [Serializable]
    class Ponuda
    {
        int idAuta;
        DateTime datumOd, datumDo;
        double cenaDan;

        public Ponuda(int idAuta, DateTime datumOd, DateTime datumDo, double cenaDan)
        {
            this.idAuta = idAuta;
            this.datumOd = datumOd;
            this.datumDo = datumDo;
            this.cenaDan = cenaDan;
        }

        public int IdAuta { get => idAuta; set => idAuta = value; }
        public DateTime Datum_od { get => datumOd; set => datumOd = value; }
        public DateTime Datum_do { get => datumDo; set => datumDo = value; }
        public double CenaDan { get => cenaDan; set => cenaDan = value; }

        public string Termin()
        {
            return datumOd.ToString("d.M.yyyy.") + " - " + datumDo.ToString("d.M.yyyy.") + " Cena: " + cenaDan + " din po danu";
        }

        public static void Sortiraj(List<Ponuda> lista)
        {
            for(int i=0; i<lista.Count -1; i++)
            {
                for(int j=i+1; j< lista.Count; j++)
                {
                    if(lista[i].idAuta > lista[j].idAuta || (lista[i].idAuta == lista[j].idAuta && lista[i].datumOd.Date > lista[j].datumOd.Date))
                    {
                        Ponuda pom = lista[i];
                        lista[i] = lista[j];
                        lista[j] = pom;
                    }
                }
            }

        }

        public override string ToString()
        {
            return datumOd.ToString("d/M/yyyy") + " - " + datumDo.ToString("d/M/yyyy") + "ID automobila: " + idAuta + " Ponuda važi: " + " Cena: " + cenaDan + " RSD/dan";
        }
    }
}
