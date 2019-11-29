using System;
using System.Collections.Generic;
using System.Text;

namespace Tietokantakirjasto
{
    public partial class Yllapito
    {
        public int ViestiId { get; set; }
        public DateTime? Pvm { get; set; }
        public string Otsikko { get; set; }
        public string Viesti { get; set; }
    }
}
