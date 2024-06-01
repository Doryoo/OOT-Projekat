using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SocijalnaMreza
{
    public class Diskusija
    {
        private string id;
        private string naziv;
        private DateOnly datumPoslednjePoruke;
        private string idGrupe;

        private int brojClanovaGrupe;

        public int BrojClanovaGrupe
        {
            get { return brojClanovaGrupe; }
            set { brojClanovaGrupe = value; }
        }

        public Diskusija(string id, string naziv, DateOnly datumPoslednjePoruke, string idGrupe)
        {
            this.id = id;
            this.naziv = naziv;
            this.datumPoslednjePoruke = datumPoslednjePoruke;
            this.idGrupe = idGrupe;
        }

        public Diskusija(string id, string idGrupe)
        {
            this.id = id;
            this.naziv = "DiskusijaDefault";
            this.datumPoslednjePoruke = DateOnly.FromDateTime(DateTime.Now);
            this.idGrupe = idGrupe;
        }
        public Diskusija()
        {
            this.id = "";
            this.naziv = "";
            this.datumPoslednjePoruke = DateOnly.FromDateTime(DateTime.Now);
            this.idGrupe = "";
        }
        static public ObservableCollection<Diskusija> LoadAllDiscussions()
        {
            ObservableCollection<Diskusija> ucitaneDiskusije = new ObservableCollection<Diskusija>();

            string folderPath = "discussions/";

            if (Directory.Exists(folderPath))
            {
                var txtFiles = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);

                foreach (var txtFile in txtFiles)
                {
                    Diskusija k = LoadDiscussion(txtFile);
                    ucitaneDiskusije.Add(k);
                }
            }
            else
            {
                MessageBox.Show("The specified folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ucitaneDiskusije;
        }
        public bool exportToCsv()
        {
            bool uspeo = false;
            StreamWriter? sw = null;
            try
            {
                if (!Directory.Exists("export/"))
                {
                    Directory.CreateDirectory("export/");

                }
                sw = new StreamWriter("export/" + id + ".csv");
                if (sw != null)
                {
                    sw.WriteLine("Id,Naziv,DatumPoslednjePoruke,IdGrupe,BrojClanovaGrupe");
                    string[] s = datumPoslednjePoruke.ToString().Split('/');
                    if (s != null)
                    {
                        sw.WriteLine(ExportString());
                        uspeo = true;
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


        static public Diskusija LoadDiscussion(string file)
        {
            Diskusija newDiscussion = new Diskusija();
            StreamReader sr = null;
            string linija;

            try
            {
                sr = new StreamReader(file);

                linija = sr.ReadLine();

                string[] lineParts = linija.Split('¬');
                newDiscussion.Id = lineParts[0];
                newDiscussion.Naziv = lineParts[1];
                newDiscussion.DatumPoslednjePoruke = DateOnly.Parse(lineParts[2], CultureInfo.InvariantCulture);
                newDiscussion.IdGrupe = lineParts[3];

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
            return newDiscussion;
        }

        static public void SaveDiscussion(Diskusija g)
        {
            StreamWriter sw = null;

            try
            {
                if (!Directory.Exists("discussions/"))
                {

                    Directory.CreateDirectory("discussions/");

                }

                sw = new StreamWriter(System.IO.Path.Combine("discussions/", g.Id +".txt"));

                sw.WriteLine(g.Id + "¬" + g.Naziv + "¬" + g.DatumPoslednjePoruke.ToString() + "¬" + g.idGrupe);


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

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        public DateOnly DatumPoslednjePoruke
        {
            get { return datumPoslednjePoruke; }
            set { datumPoslednjePoruke = value; }
        }

        public string IdGrupe
        {
            get { return idGrupe; }
            set { idGrupe = value; }
        }

        public string ExportString()
        {
            string s = "";
            string[] s2 = datumPoslednjePoruke.ToString().Split('/');
            s += id + "," + naziv + "," + s2[0] + "." + s2[1] + "." + s2[2] + "." + "," + idGrupe + "," + brojClanovaGrupe;
            return s;
        }

    }
}
