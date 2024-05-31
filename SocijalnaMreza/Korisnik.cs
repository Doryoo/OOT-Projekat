using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Printing;
using System.Windows;

namespace SocijalnaMreza
{


    public class Korisnik : INotifyPropertyChanged
    {
        private string id;
        private string ime;
        private string prezime;
        private DateOnly datumRodjenja;
        private string profilnaSlikaPath;
        private ObservableCollection<Post> objavljeniPostovi;

        private ObservableCollection<Korisnik> listaPrijatelja;
        private ObservableCollection<Korisnik> listaPrijateljaSelektovana;

        //private ObservableCollection<Grupa> listaGrupa;
        private List <string> listaPrijateljskihIDs = new List <string> ();

        public Korisnik(string id, string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath)
        {
            this.id = id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            if (profilnaSlikaPath != null)
                this.profilnaSlikaPath = profilnaSlikaPath;
            else
                this.profilnaSlikaPath = "images/defaultImage.jpg";
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
                this.profilnaSlikaPath = "images/defaultImage.jpg";
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
            this.profilnaSlikaPath = "images/defaultImage.jpg";
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
            this.profilnaSlikaPath = "images/defaultImage.jpg";
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

        /*
         // nepotrebno teoretski
        public ObservableCollection<Grupa> ListaGrupa
        {
            get { return listaGrupa; }
            set
            {
                if (listaGrupa != value)
                {
                    listaGrupa = value;
                    OnPropertyChanged(nameof(ListaGrupa));
                }
            }
        }

        public void updateGroups(ObservableCollection<Grupa> grupe)
        {
            listaGrupa.Clear();
            foreach(var item in grupe)
            {
                if (item.ListaClanova.Contains(this))
                {
                    ListaGrupa.Add(item);
                }
            }
        }*/

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
                    if (!listaPrijateljaSelektovana.Contains(item))
                    {
                        ListaPrijateljaSelektovana.Add(item);
                        //MessageBox.Show("test");
                    }
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
            post.Sadrzaj = novSadrzaj;
            objavljeniPostovi.RemoveAt(index);
            objavljeniPostovi.Insert(index, post);
        }

        public ObservableCollection<Post> getPosts()
        {
            return objavljeniPostovi;
        }

        public bool DodajPrijatelja(Korisnik k)
        {
            if (k == null || listaPrijatelja.Contains(k)) return false;
            ListaPrijatelja.Add(k);
            listaPrijateljaSelektovana.Add(k);
            listaPrijateljskihIDs.Add(k.Id);
            return true;
        }

        public bool DodajPrijatelja(string s, List<Korisnik> svi)
        {
            if (s == null || s.Length == 0) return false;
            for (int i = 0; i < listaPrijatelja.Count; i++)
            {
                if (listaPrijatelja[i].id == s)
                {
                    return false;
                }
            }


            foreach (var item in svi)
            {
                if (item.Id == s)
                {
                    listaPrijatelja.Add(item);
                    listaPrijateljaSelektovana.Add(item);
                    return true;
                }
            }
            Korisnik t = new Korisnik(s);
            listaPrijatelja.Add(t);
            listaPrijateljaSelektovana.Add(t);
            listaPrijateljskihIDs.Add(t.Id);
            return true;
        }

        public bool ukloniPrijatelja(Korisnik k)
        {
            listaPrijateljaSelektovana.Remove(k);
            listaPrijateljskihIDs.Remove(k.Id);
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



        static public Korisnik LoadUser(string file)
        {
            Korisnik newKorisnik = new Korisnik("temp");
            StreamReader sr = null;
            string naziv;
            double cena;
            string linija;
            bool first = true;
            int numberOfPosts = 0;
            try
            {
                sr = new StreamReader(System.IO.Path.Combine("users/", file));

                // petlja za kreiranje stavki (u fajlu je jedan red - jedna stavka)
                linija = sr.ReadLine();

                string[] lineParts = linija.Split('¬');
                newKorisnik.Id = lineParts[0];
                newKorisnik.Ime = lineParts[1];
                newKorisnik.Prezime = lineParts[2];
                newKorisnik.DatumRodjenja = DateOnly.Parse(lineParts[3], CultureInfo.InvariantCulture);
                newKorisnik.profilnaSlikaPath = lineParts[4];
                numberOfPosts = int.Parse(lineParts[5]);

                for (int i = 0; i < numberOfPosts; i++)
                {
                    linija = sr.ReadLine();
                    lineParts = linija.Split('¬');
                    newKorisnik.dodajPost(new Post(lineParts[0], lineParts[1], DateOnly.Parse(lineParts[2], CultureInfo.InvariantCulture), int.Parse(lineParts[3]), lineParts[4]));
                }

                linija = sr.ReadLine();
                List<string> friendIDs = linija.Split('¬').ToList<string>();
                newKorisnik.ListaPrijateljskihIDs = friendIDs;
                
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return newKorisnik;
        }

        static public void SaveUser(string file, Korisnik k)
        {
            StreamWriter sw = null;
        
            int numberOfPosts = k.getPosts().Count();
            try
            {
                if (!Directory.Exists("users/"))
                {
                    
                    Directory.CreateDirectory("users/");
                       
                }

                sw = new StreamWriter(System.IO.Path.Combine("users/",file));

                sw.WriteLine(k.Id + "¬" + k.Ime + "¬" + k.Prezime + "¬" + k.DatumRodjenja + "¬" + k.ProfilnaSlikaPath + "¬" + numberOfPosts);

                Post[] posts = k.getPosts().ToArray();
                for (int i = 0; i < numberOfPosts; i++)
                {
                    sw.WriteLine(posts[i].Id + "¬" + posts[i].Sadrzaj + "¬" + posts[i].DatumObjave + "¬" + posts[i].BrojLajkova + "¬" + posts[i].IdAutora);
                }

                string friendIDs = "";
                string[] friendIDArr = k.ListaPrijateljskihIDs.ToArray();
                    
                for (int i = 0; i < k.ListaPrijateljskihIDs.Count(); i++)
                {
                    friendIDs += friendIDArr[i] + "¬";
                }

                
                if (friendIDs.Length > 0) {
                    sw.WriteLine(friendIDs);
                }
                


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        public override string ToString()
        {
            return "Ime : " + ime + ", Prezime : " + prezime;
        }


        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged(nameof(Ime));
            }
        }

        public string Prezime
        {
            get { return prezime; }
            set
            {
                prezime = value;
                OnPropertyChanged(nameof(Prezime));
            }
        }

        public DateOnly DatumRodjenja
        {
            get { return datumRodjenja; }
            set
            {
                datumRodjenja = value;
                OnPropertyChanged(nameof(DatumRodjenja));
            }
        }
        public string ProfilnaSlikaPath
        {
            get { return profilnaSlikaPath; }
            set
            {
                profilnaSlikaPath = value;
                OnPropertyChanged(nameof(ProfilnaSlikaPath));
            }
        }
    
        public List<string> ListaPrijateljskihIDs
        {
            get { return listaPrijateljskihIDs; }
            set
            {
                listaPrijateljskihIDs = value;
                OnPropertyChanged(nameof(listaPrijateljskihIDs));
            }
        }
    }
}
