using System;
using System.Collections.Generic;

namespace Tietokantakirjasto
{
    public partial class Laji
    {
        public Laji()
        {
            Elain = new HashSet<Elain>();
        }

        public int LajiId { get; set; }
        public string Nimi { get; set; }

        public virtual ICollection<Elain> Elain { get; set; }
    }
}
