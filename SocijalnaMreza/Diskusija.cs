using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocijalnaMreza
{
    internal class Diskusija
    {
        private string id;
        private string naziv;
        private DateOnly datumPoslednjePoruke;
        private string idGrupe;

        public Diskusija(string id, string naziv, DateOnly datumPoslednjePoruke, string idGrupe)
        {
            this.id = id;
            this.naziv = naziv;
            this.datumPoslednjePoruke = datumPoslednjePoruke;
            this.idGrupe = idGrupe;
        }
    }
}
