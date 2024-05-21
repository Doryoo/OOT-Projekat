using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocijalnaMreza
{
    internal class Post
    {
        private string id;
        private string sadrzaj;
        private DateOnly datumObjave;
        private int brojLajkova;
        private string idAutora;

        public Post(string id, string sadrzaj, DateOnly datumObjave, int brojLajkova, string autorId)
        {
            this.id = id;
            this.sadrzaj = sadrzaj;
            this.datumObjave = datumObjave;
            this.brojLajkova = brojLajkova;
            this.idAutora = autorId;
        }
    }
}
