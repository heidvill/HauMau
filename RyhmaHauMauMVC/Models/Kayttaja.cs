using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RyhmaHauMauMVC.Models
{
    public partial class Kayttaja
    {
        public Kayttaja()
        {
            Elain = new HashSet<Elain>();
        }

        public int KayttajaId { get; set; }
        [EmailAddress]
        public string Sahkoposti { get; set; }
        public string Postinumero { get; set; }

        public virtual ICollection<Elain> Elain { get; set; }
    }
}
