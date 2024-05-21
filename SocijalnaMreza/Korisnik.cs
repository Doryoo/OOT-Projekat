using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SocijalnaMreza
{
    internal class Korisnik
    {
        private string id;
        private string ime;
        private string prezime;
        private DateOnly datumRodjenja;
        private Image profilnaSlika;

        public Korisnik(string id, string ime, string prezime, DateOnly datumRodjenja, Image profilnaSlika)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            this.profilnaSlika = profilnaSlika;
        }
    }
}
