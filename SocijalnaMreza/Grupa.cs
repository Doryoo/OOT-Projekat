using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace SocijalnaMreza
{
    public class Grupa : INotifyPropertyChanged
    {
        private string id;
        private string naziv;
        private string opis;
        List<Korisnik> listaClanova;
        List<string> listaClanovaIDs;
        private int brojClanova; // teoretski nepotrebno? moze se uvek izvuci iz listeClanova, ali teoretski ce trebati za prikazivanje kako bi se moglo navezati sa bindingom (dole su getteri i setteri isto)

        public Grupa(string id, string naziv, string opis, List<Korisnik> listaClanova)
        {
            this.id = id;
            this.naziv = naziv;
            this.opis = opis;
            this.listaClanova = listaClanova;
            brojClanova = listaClanova.Count;
        }

        public Grupa(string id, string naziv, string opis)
        {
            this.id = id;
            this.naziv = naziv;
            this.opis = opis;
            this.listaClanova = new List<Korisnik>();
            brojClanova = listaClanova.Count;
        }

        public Grupa()
        {
            this.id = "";
            this.naziv = "";
            this.opis = "";
            this.listaClanova = new List<Korisnik>();
            brojClanova = listaClanova.Count;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool DodajClana(Korisnik k)
        {
            if(k == null || listaClanova.Contains(k)) return false;
                listaClanova.Add(k);
            brojClanova++;
            OnPropertyChanged(nameof(listaClanova));
            return true;
        }

        public bool DodajClana(string id, string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath)
        {
            Korisnik k = new Korisnik(id,ime,prezime,datumRodjenja, profilnaSlikaPath);
            if (listaClanova.Contains(k)) return false;
                listaClanova.Add(k);
            brojClanova++;
            OnPropertyChanged(nameof(listaClanova));
            return true;
        }

        public bool UkloniClana(Korisnik k) {
            OnPropertyChanged(nameof(listaClanova));
            brojClanova--;
            return listaClanova.Remove(k);
        }

        static public Grupa LoadGroup(string file)
        {
            Grupa newGroup = new Grupa();
            StreamReader sr = null;
            string linija;

            try
            {
                sr = new StreamReader(System.IO.Path.Combine("groups/", file));

                // petlja za kreiranje stavki (u fajlu je jedan red - jedna stavka)
                linija = sr.ReadLine();

                string[] lineParts = linija.Split('¬');
                newGroup.Id = lineParts[0];
                newGroup.Naziv = lineParts[1];
                newGroup.Opis = lineParts[2];

                linija = sr.ReadLine();
                newGroup.ListaClanovaIDs = linija.Split('¬').ToList<string>();

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
            return newGroup;
        }

        static public void SaveGroup(Grupa g)
        {
            StreamWriter sw = null;

            try
            {
                if (!Directory.Exists("groups/"))
                {

                    Directory.CreateDirectory("groups/");

                }

                sw = new StreamWriter(System.IO.Path.Combine("groups/", g.Id + ".txt"));

                sw.WriteLine(g.Id + "¬" + g.Naziv + "¬" + g.Opis);

                

                string groupUserIDs = "";
                Korisnik[] groupUserIDsArr = g.ListaClanova.ToArray();

                for (int i = 0; i < g.ListaClanova.Count(); i++)
                {
                    if (i != g.ListaClanova.Count() - 1)
                        groupUserIDs += groupUserIDsArr[i].Id + "¬";
                    else
                        groupUserIDs += groupUserIDsArr[i].Id;
                }


                if (groupUserIDs.Length > 0)
                {
                    sw.WriteLine(groupUserIDs);
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
        public bool exportToCsv(ObservableCollection<Diskusija> d)
        {
            bool uspeo = false;
            StreamWriter? sw = null;
            try
            {
                if (!Directory.Exists("export/"))
                {
                    Directory.CreateDirectory("export/");

                }
                sw = new StreamWriter("export/GRUPA" + id + ".csv");
                if (sw != null)
                {
                    sw.WriteLine("Id,Naziv,Opis,ListaClanova");
                    string line = id + "," + naziv + "," + opis + ",";
                    foreach(var tmp in listaClanova)
                    {
                       line += tmp.Id;
                       if(tmp != listaClanova.Last())
                            line += "¬";
                    }
                    
                    sw.WriteLine(line);
                    sw.WriteLine();
                    sw.WriteLine("Id,Naziv,DatumPoslednjePoruke,IdGrupe,BrojClanovaGrupe");
                    foreach(var tmp in d)
                    {
                        if(tmp.IdGrupe == id)
                            sw.WriteLine(tmp.ExportString());
                    }
                }
                else
                {
                    throw new Exception("Folder cannot be accessed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Fajl " + id + ".csv");
            }
            finally
            {
                if (sw != null)
                {
                    try { sw.Close(); } catch (Exception ex) { MessageBox.Show(ex.Message); Console.WriteLine(ex.StackTrace); Console.WriteLine(ex.Message); }
                    MessageBox.Show("Uspesan export!");
                    uspeo = true;
                }
            }
            return uspeo;
        }

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public int BrojClanova
        {
            get { return brojClanova; }
            set { brojClanova = value; }
        }
        public string Opis
        {
            get { return opis; }
            set { opis = value; }
        }
        public List<Korisnik> ListaClanova
        {
            get { return listaClanova; }
            set { listaClanova = value; OnPropertyChanged(nameof(listaClanova)); }
        }

        public List<string> ListaClanovaIDs
        {
            get { return listaClanovaIDs; }
            set { listaClanovaIDs = value; OnPropertyChanged(nameof(listaClanovaIDs)); }
        }
        public override string ToString()
        {
            return "ID : " + id + " \nNaziv : " + naziv; 
        }
    }
}
