using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
        private static List<string> listaPrijateljskihIDs = new List<string>();

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
            this.ime = "defaultIme";
            this.prezime = "defaultPrezime";
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
        public void initPrijatelji()
        {
            listaPrijateljaSelektovana.Clear();
            foreach (var item in listaPrijatelja)
            {
                ListaPrijateljaSelektovana.Add(item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
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
            return true;
        }

        
        //Funkcija koja se poziva za dodavanje prijatelja unutar novog prozora
        public bool DodajPrijateljaV2(string[] s, List<Korisnik> svi)
        {
            if (s == null || s.Length == 0) return false;
            foreach (var item in svi)
            {
                if (!listaPrijatelja.Contains(item) && item.Id != "964964") { 
                    if (s[0].Contains(item.Ime) && s[0] != "" || item.Ime.Contains(s[0]) && s[0] != "")
                    {
                        listaPrijatelja.Add(item);
                        listaPrijateljaSelektovana.Add(item);
                        Korisnik.SaveUser(this);
                        //MessageBox.Show(" nadjen korisnik : " + item.ToString() + " trazen korisnik (po imenu)" + s[0]);
                        return true;
                    }
                    else if (s[1].Contains(item.Prezime) && s[1] != "" || item.prezime.Contains(s[1]) && s[1] != "")
                    {
                        listaPrijatelja.Add(item);
                        listaPrijateljaSelektovana.Add(item);
                        //MessageBox.Show(" nadjen korisnik : " + item.ToString() + " trazen korisnik (po prezimenu)" + s[1]);
                        Korisnik.SaveUser(this);
                        return true;
                    }
                    else if (s[2] == item.Id && s[2] != "")
                    {
                        listaPrijatelja.Add(item);
                        listaPrijateljaSelektovana.Add(item);
                        Korisnik.SaveUser(this);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ukloniPrijatelja(Korisnik k)
        {
            listaPrijateljaSelektovana.Remove(k);
            return listaPrijatelja.Remove(k);
        }

        static public List<Korisnik> LoadAllUsers()
        {
            List<Korisnik> ucitaniKorisnici = new List<Korisnik>();

            string folderPath = "users/";

            if (Directory.Exists(folderPath))
            {
                var txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

                for (int i = 0; i < txtFiles.Length; i++)
                {
                    Korisnik k = LoadUser(txtFiles[i]);
                    ucitaniKorisnici.Add(k);
                }
            }
            else
            {
                MessageBox.Show("The specified folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ucitaniKorisnici;
        }

        static public Korisnik LoadUser(string file)
        {
            Korisnik newKorisnik = new Korisnik("temp");
            StreamReader sr = null;
            string? linija;
            int numberOfPosts = 0;
            try
            {
                sr = new StreamReader(file);

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
                if (linija != null)
                {
                    List<string> friendIDs = linija.Split('¬').ToList<string>();
                    newKorisnik.ListaPrijateljskihIDs = friendIDs;
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
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return newKorisnik;
        }

        static public void SaveUser(Korisnik k)
        {
            StreamWriter sw = null;
            
            int numberOfPosts = k.getPosts().Count();
            try
            {
                if (!Directory.Exists("users/"))
                {

                    Directory.CreateDirectory("users/");

                }

                sw = new StreamWriter(System.IO.Path.Combine("users/", k.Id + ".txt"));

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
                    if (i != k.ListaPrijateljskihIDs.Count() - 1)
                        friendIDs += friendIDArr[i] + "¬";
                    else
                        friendIDs += friendIDArr[i];
                }


                if (friendIDs.Length > 0)
                {
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


