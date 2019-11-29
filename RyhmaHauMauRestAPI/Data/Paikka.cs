using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RyhmaHauMauRestAPI.Data
{
    public class Paikka
    {
        public string Postinumero { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }
            Paikka toinen = (Paikka)obj;

            return Postinumero == toinen.Postinumero;
        }
    }
}
