using System;
using System.Collections.Generic;
using System.Text;

namespace Tietokantakirjasto
{
    public partial class Kuva
    {
        public int KuvaId { get; set; }
        public byte[] ThumbnailKuva { get; set; }
        public string ThumbnailKuvaFileName { get; set; }
        public byte[] IsoKuva { get; set; }
        public string IsoKuvaFileName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
