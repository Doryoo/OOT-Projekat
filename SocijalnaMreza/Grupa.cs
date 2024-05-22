using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace SocijalnaMreza
{
    internal class Grupa
    {
        private string id;
        private string naziv;
        private string opis;
        List<Korisnik> listaClanova;

        public Grupa(string id, string naziv, string opis, List<Korisnik> listaClanova)
        {
            this.id = id;
            this.naziv = naziv;
            this.opis = opis;
            this.listaClanova = listaClanova;
        }

        // temp ig, posto treba da imas perms da bih mogao dodati/ukloniti clana iz grupe?!?
        public bool DodajClana(Korisnik k)
        {
            if(k == null || listaClanova.Contains(k)) return false;
            listaClanova.Add(k); 
            return true;
        }

        public bool DodajClana(string id, string ime, string prezime, DateOnly datumRodjenja, Image profilnaSlika)
        {
            Korisnik k = new Korisnik(id,ime,prezime,datumRodjenja,profilnaSlika);
            if (listaClanova.Contains(k)) return false;
            listaClanova.Add(k);
            return true;
        }

        public bool UkloniClana(Korisnik k) {
            return listaClanova.Remove(k);
        }
    }
}
