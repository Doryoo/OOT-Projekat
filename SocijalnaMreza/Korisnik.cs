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
        private Image profilnaSlika;                            //treba li getter i setter za ovo
        private ObservableCollection<Post> objavljeniPostovi;
        private ObservableCollection<Korisnik> listaPrijatelja;
        



        public Korisnik(string id, string ime, string prezime, DateOnly datumRodjenja, Image profilnaSlika)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            this.profilnaSlika = profilnaSlika;
            objavljeniPostovi = new ObservableCollection<Post>();
            listaPrijatelja = new ObservableCollection<Korisnik>();
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
        public void dodajPost(Post novPost)
        {
            objavljeniPostovi.Add(novPost);
        }

        public void obrisiPost(Post novPost)
        {
            objavljeniPostovi.Remove(novPost);
        }

        public ObservableCollection<Post> getPosts() {
            return objavljeniPostovi;
        }



        


        public bool DodajPrijatelja(Korisnik k)
        {
            if (k == null || listaPrijatelja.Contains(k)) return false;
            listaPrijatelja.Add(k);
            return true;
        }

        public bool ukloniPrijatelja(Korisnik k)
        {
            return listaPrijatelja.Remove(k);
        }


        /*
         
        public bool DodajPrijatelja(List<Korisnik> k)
        {
            if (k == null || listaPrijatelja.Contains(k)) return false;
            listaPrijatelja.Add(k);
            return true;
        }

        public bool ukloniPrijatelja(List<Korisnik> k)
        {
            return listaPrijatelja.Remove(k);
        }



        //ovako bi izgledalo da zelimo listu sa stringovima (id-evima) prijatelja
        private ObservableCollection<string> listaPrijatelja;

        public bool DodajPrijatelja(string s)
        {
            if (s == null || s.Length == 0 || listaPrijatelja.Contains(s)) return false;
            listaPrijatelja.Add(s);
            return true;
        }

        public bool DodajPrijatelja(Korisnik k)
        {
            if (k == null || listaPrijatelja.Contains(k.Id)) return false;
            listaPrijatelja.Add(k.Id);
            return true;
        }


        public bool ukloniPrijatelja(string s)
        {
            return listaPrijatelja.Remove(s);
        }
        public bool ukloniPrijatelja(Korisnik k)
        {
            return listaPrijatelja.Remove(k.Id);
        }
        */





        public override string ToString()
        {
            return "Ime : " + ime + ", Prezime : " + prezime;
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }

        public string Prezime
        {
            get { return prezime; }
            set { prezime = value; }
        }

        public DateOnly DatumRodjenja
        {
            get { return datumRodjenja; }
            set { datumRodjenja = value; }
        }

    }
}
