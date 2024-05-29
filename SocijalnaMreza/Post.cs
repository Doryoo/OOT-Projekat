using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SocijalnaMreza
{
    public class Post : INotifyPropertyChanged
    {
        private string id;
        private string sadrzaj;
        private DateOnly datumObjave;
        private int brojLajkova;
        private string idAutora;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public Post(string id, string sadrzaj, DateOnly datumObjave, int brojLajkova, string autorId)
        {
            this.id = id;
            this.sadrzaj = sadrzaj;
            this.datumObjave = datumObjave;
            this.brojLajkova = brojLajkova;
            this.idAutora = autorId;
        }

        //nepotrebni su, ali eto, ako ustreba tu su
        public string Id
        {
            get { return id; }
            set { 
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Sadrzaj
        {
            get { return sadrzaj; }
            set { 
                sadrzaj = value;
                OnPropertyChanged("Sadrzaj");
            }
        }
        public DateOnly DatumObjave
        {
            get { return datumObjave; }
            set { 
                datumObjave = value;
                OnPropertyChanged("DatumObjave");
            }
        }
        public int BrojLajkova
        {
            get { return brojLajkova; }
            set { 
                brojLajkova = value;
                OnPropertyChanged("BrojLajkova");
            }
        }
        public string IdAutora
        {
            get { return idAutora; }
            set { 
                idAutora = value;
                OnPropertyChanged("IdAutora");
            }
        }
        
        public override string ToString()
        {
            return sadrzaj + " " + datumObjave;
        }
    }
}
