using System;
using System.Collections.Generic;
using System.Text;

namespace Tietokantakirjasto
{
    public partial class Kuvakirjasto
    {
        public int KuvaId { get; set; }
        public string KuvaNimi { get; set; }
        public byte[] Kuva { get; set; }
        public int ElainId { get; set; }
    }
}
