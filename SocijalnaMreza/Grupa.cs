using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace SocijalnaMreza
{
    internal class Grupa : INotifyPropertyChanged
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
            brojClanova = listaClanova.Count;
        }
        
        

        private int brojClanova; // teoretski nepotrebno? moze se uvek izvuci iz listeClanova, ali teoretski ce trebati za prikazivanje kako bi se moglo navezati sa bindingom (dole su getteri i setteri isto)

        

        public Grupa(string id, string naziv, string opis)
        {
            this.id = id;
            this.naziv = naziv;
            this.opis = opis;
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


        // temp ig, posto treba da imas perms da bih mogao dodati/ukloniti clana iz grupe?!?
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

        public override string ToString()
        {
            return "ID : " + id + " \nNaziv : " + naziv; 
        }
    }
}
