using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RyhmaHauMauMVC.Models
{
    public partial class Elain
    {
        public int ElainId { get; set; }
        public int KayttajaId { get; set; }
        public int LajiId { get; set; }
        public string Nimi { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Syntymapaiva { get; set; }

        private string ikä;

        public string Ikä
        {
            get
            {
                if (Syntymapaiva != null)
                {
                    int nvuosi = int.Parse(DateTime.Now.Year.ToString());
                    int svuosi = int.Parse(Convert.ToDateTime(Syntymapaiva).Year.ToString());
                    int ikävuosina = nvuosi - svuosi;

                    int nkk = int.Parse(DateTime.Now.Month.ToString());
                    int skk = int.Parse(Convert.ToDateTime(Syntymapaiva).Month.ToString());
                    int ikäkuukaudet = nkk - skk;

                    if (nkk - skk < 0)
                    {
                        ikävuosina--;
                        ikäkuukaudet = 12 - skk;
                    }

                    ikä = $"{ikävuosina}v {ikäkuukaudet}kk";

                    if (ikävuosina < 1)
                    {
                        ikä = $"{ikäkuukaudet}kk";
                    }
                    else if (ikäkuukaudet == 0)
                    {
                        ikä = $"{ikävuosina}v";
                    }
                }

                else
                { ikä = ""; }

                return ikä;
            }
            set
            {

            }
        }

        public string Kuvaus { get; set; }
        public string Rotu { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual Laji Laji { get; set; }
    }
}
