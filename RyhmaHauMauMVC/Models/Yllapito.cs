using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmaHauMauMVC.Models
{
    public partial class Yllapito
    {
        public int ViestiId { get; set; }
        public DateTime? Pvm { get; set; }
        public string Otsikko { get; set; }
        public string Viesti { get; set; }
    }
}
