using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projekat1TVP
{
    [Serializable]
    public class Automobil
    {
        int id, godiste, kubikaza, brVrata;
        string marka, model, pogon, menjac, karoserija, gorivo;

        public Automobil()
        {
            Id = Godiste = Kubikaza = BrVrata = -1;
            Marka = Model = Pogon = Menjac = Karoserija = Gorivo = "nedefinisano!";
        }


        public Automobil(int id, int godiste, int kubikaza, int brVrata, string marka, string model, string pogon, string menjac, string karoserija, string gorivo)
        {
            this.Id = id;
            this.Godiste = godiste;
            this.Kubikaza = kubikaza;
            this.BrVrata = brVrata;
            this.Marka = marka;
            this.Model = model;
            this.Pogon = pogon;
            this.Menjac = menjac;
            this.Karoserija = karoserija;
            this.Gorivo = gorivo;
        }
        public int Id { get => id; set => id = value; }
        public int Godiste { get => godiste; set => godiste = value; }
        public int Kubikaza { get => kubikaza; set => kubikaza = value; }
        public int BrVrata { get => brVrata; set => brVrata = value; }
        public string Marka { get => marka; set => marka = value; }
        public string Model { get => model; set => model = value; }
        public string Pogon { get => pogon; set => pogon = value; }
        public string Menjac { get => menjac; set => menjac = value; }
        public string Karoserija { get => karoserija; set => karoserija = value; }
        public string Gorivo { get => gorivo; set => gorivo = value; }


        

        public override string ToString()
        {
            return "id: " + Id + " marka: " + Marka + " model: " + Model + " godiste: " + Godiste + " kubikaza: " + Kubikaza + " br. vrata: " +BrVrata + " pogon: " + Pogon + " menjac: " + Menjac + " karoserija: " + Karoserija + " gorivo: " + Gorivo + Environment.NewLine; 
        }
       
    }
}
