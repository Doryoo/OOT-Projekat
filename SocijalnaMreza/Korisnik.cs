using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Post> objavljeniPostovi;

        public Korisnik(string id, string ime, string prezime, DateOnly datumRodjenja, Image profilnaSlika)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            this.profilnaSlika = profilnaSlika;
            objavljeniPostovi = new ObservableCollection<Post>();
        }

        /*
         private string id;
        private string sadrzaj;
        private DateOnly datumObjave;
        private int brojLajkova;
        private string idAutora;
        */
        public void dodajPost(string sadrzaj)
        {
            Random idGen = new Random();
            objavljeniPostovi.Add(new Post(idGen.Next().ToString(), sadrzaj, DateOnly.FromDateTime(DateTime.Now), 0, id));
        }

        public ObservableCollection<Post> getPosts() {
            return objavljeniPostovi;
        }
    }
}
