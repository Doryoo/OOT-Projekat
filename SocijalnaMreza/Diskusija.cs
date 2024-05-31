using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
