using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tietokantakirjasto
{
    public partial class Elain
    {
        public int ElainId { get; set; }
        public int KayttajaId { get; set; }
        public int LajiId { get; set; }
        public string Nimi { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Syntymapaiva { get; set; }
        public string Kuvaus { get; set; }
        public string Rotu { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual Laji Laji { get; set; }
    }
}
