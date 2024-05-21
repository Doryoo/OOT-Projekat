using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocijalnaMreza
{
    internal class Grupa
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
        }
    }
}
