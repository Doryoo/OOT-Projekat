using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SocijalnaMreza 
{


    internal class Korisnik : INotifyPropertyChanged
    {
        private string id;
        private string ime;
        private string prezime;
        private DateOnly datumRodjenja;
        private string profilnaSlikaPath;                            //treba li getter i setter za ovo
        private ObservableCollection<Post> objavljeniPostovi;

        private ObservableCollection<Korisnik> listaPrijatelja;
        private ObservableCollection<Korisnik> listaPrijateljaSelektovana;



        public Korisnik(string id, string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            if (profilnaSlikaPath != null)
                this.profilnaSlikaPath = profilnaSlikaPath;
            else
                this.profilnaSlikaPath = "images/profileImage.png";
            objavljeniPostovi = new ObservableCollection<Post>();
            listaPrijatelja = new ObservableCollection<Korisnik>();
            listaPrijateljaSelektovana = new ObservableCollection<Korisnik>();
        }
        public Korisnik(string id, string ime, string prezime, string profilnaSlikaPath)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = DateOnly.FromDateTime(DateTime.Now);
            if (profilnaSlikaPath != null)
                this.profilnaSlikaPath = profilnaSlikaPath;
            else
                this.profilnaSlikaPath = "images/profileImage.png";
            objavljeniPostovi = new ObservableCollection<Post>();
            listaPrijatelja = new ObservableCollection<Korisnik>();
            listaPrijateljaSelektovana = new ObservableCollection<Korisnik>();
        }
        public Korisnik(string id, string ime, string prezime)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = DateOnly.FromDateTime(DateTime.Now);
            this.profilnaSlikaPath = "images/profileImage.png";
            objavljeniPostovi = new ObservableCollection<Post>();
            listaPrijatelja = new ObservableCollection<Korisnik>();
            listaPrijateljaSelektovana = new ObservableCollection<Korisnik>();
        }

        public Korisnik(string id)
        {
            this.id = id;
            this.ime = "bata";
            this.prezime = "posao";
            this.datumRodjenja = DateOnly.FromDateTime(DateTime.Now);
            this.profilnaSlikaPath = null;
            objavljeniPostovi = new ObservableCollection<Post>();
            listaPrijatelja = new ObservableCollection<Korisnik>();
            listaPrijateljaSelektovana = new ObservableCollection<Korisnik>();
        }



        public ObservableCollection<Korisnik> ListaPrijatelja
        {
            get { return listaPrijatelja; }
            set
            {
                if (listaPrijatelja != value)
                {
                    listaPrijatelja = value;
                    OnPropertyChanged(nameof(ListaPrijatelja));
                }
            }
        }

        public ObservableCollection<Korisnik> ListaPrijateljaSelektovana
        {
            get { return listaPrijateljaSelektovana; }
            set
            {
                if (listaPrijateljaSelektovana != value)
                {
                    listaPrijateljaSelektovana = value;
                    OnPropertyChanged(nameof(ListaPrijateljaSelektovana));
                }
            }
        }

        public void initPrijatelji()
        {
            listaPrijateljaSelektovana.Clear();
            foreach (var item in listaPrijatelja) 
            {
                ListaPrijateljaSelektovana.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SearchFor(string s)
        {
            listaPrijateljaSelektovana.Clear();
            foreach (var item in ListaPrijatelja)
            {
                foreach (var item2 in item.getPosts())
                {
                    if (item2.Sadrzaj.Contains(s) && !listaPrijateljaSelektovana.Contains(item))
                    {
                        ListaPrijateljaSelektovana.Add(item);
                        //MessageBox.Show("test");
                    }
                }

                if (s == "" || item.ime.Contains(s) || item.prezime.Contains(s) || item.DatumRodjenja.ToString().Contains(s)) 
                {
                    if (!listaPrijateljaSelektovana.Contains(item)) { 
                        listaPrijateljaSelektovana.Add(item);
                        //MessageBox.Show("test");
                    }
                    //dodati jos za search po post-ovima
                }
               
            }
        }

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

        public void editujPost(Post post, string novSadrzaj)
        {
            int index = objavljeniPostovi.IndexOf(post);
            post.Sadrzaj= novSadrzaj;
            objavljeniPostovi.RemoveAt(index);
            objavljeniPostovi.Insert(index, post);
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

        public bool DodajPrijatelja(string s)
        {
            if (s == null || s.Length == 0) return false;
            bool postoji = true;
            Korisnik k;
            for(int i = 0; i < listaPrijatelja.Count; i++)
            {
                if (listaPrijatelja[i].id == s)
                {
                    postoji = false;
                }
            }
            if (postoji) { 
                listaPrijatelja.Add(new Korisnik(s));
            }
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
            set { 
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Ime
        {
            get { return ime; }
            set { 
                ime = value;
                OnPropertyChanged(nameof(Ime));
            }
        }

        public string Prezime
        {
            get { return prezime; }
            set { 
                prezime = value;
                OnPropertyChanged(nameof(Prezime));
            }
        }

        public DateOnly DatumRodjenja
        {
            get { return datumRodjenja; }
            set { 
                datumRodjenja = value;
                OnPropertyChanged(nameof(DatumRodjenja));
            }
        }
        public string ProfilnaSlikaPath
        {
            get { return profilnaSlikaPath; }
            set {
                profilnaSlikaPath = value;
                OnPropertyChanged(nameof(ProfilnaSlikaPath));
            }
        }
    }
}
