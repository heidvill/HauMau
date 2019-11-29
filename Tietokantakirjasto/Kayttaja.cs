using System;
using System.Collections.Generic;

namespace Tietokantakirjasto
{
    public partial class Kayttaja
    {
        public Kayttaja()
        {
            Elain = new HashSet<Elain>();
        }

        public int KayttajaId { get; set; }
        public string Sahkoposti { get; set; }
        public string Postinumero { get; set; }

        public virtual ICollection<Elain> Elain { get; set; }
    }
}
