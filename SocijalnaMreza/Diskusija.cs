using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
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

        public bool exportToCsv()
        {
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
                    if (s.Length == 3)
                    {
                        sw.WriteLine(ExportString());
                    }
                    else
                    {
                        sw.WriteLine(id + "," + naziv + "," + datumPoslednjePoruke.ToString() + "," + idGrupe + "," + brojClanovaGrupe);

                    }
                }
                else
                {
                    throw new Exception("Folder cannot be accessed");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Fajl " + id + ".csv");
            }
            finally
            {
                if (sw != null) { 

                    try { sw.Close();} catch (Exception ex) { MessageBox.Show(ex.Message); Console.WriteLine(ex.StackTrace); Console.WriteLine(ex.Message);}
                    MessageBox.Show("Uspesan export!");
                }
            }
            return false;
        }

        public string ExportString()
        {
            string s = "";
            string[] s2 = datumPoslednjePoruke.ToString().Split('/');
            s += id + "," + naziv + "," + s2[0] + "." + s2[1] + "." + s2[2] + "." + "," + idGrupe + "," + brojClanovaGrupe;
            return s;
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



    }
}
