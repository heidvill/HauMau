using System;
using System.Collections.Generic;
using System.Text;

namespace RyhmaHauMauMVC
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
