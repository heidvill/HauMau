using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RyhmaHauMauRestAPI.Data;
using Tietokantakirjasto;

namespace RyhmaHauMauRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TietokantaController : ControllerBase
    {
        // GET: api/Tietokanta
        [HttpGet]
        public IEnumerable<Elain> Get()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                List<Elain> elaimet = db.Elain.ToList();
                foreach (Elain elain in elaimet)
                {
                    db.Entry(elain).Reference(e => e.Laji).Load();
                }
                return elaimet;
            }
        }

        // GET: api/Tietokanta/Laji
        [HttpGet("Laji", Name = "GetLajit")]
        public IEnumerable<Laji> GetLajit()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Laji.ToList();
            }
        }

        // GET: api/Tietokanta/Kayttaja
        [HttpGet("Kayttaja", Name = "GetKayttaja")]
        public IEnumerable<Kayttaja> GetKayttaja()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Kayttaja.ToList();
            }
        }

        // GET: api/Tietokanta/KayttajaPnro
        [HttpGet("KayttajaPnro", Name = "GetKayttajaPnro")]
        public List<string> GetKayttajaPnro()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                var Pnrot = (from k in db.Kayttaja
                             select k.Postinumero).Distinct().ToList();

                return Pnrot;
            }
        }

        // GET: api/Tietokanta/Kayttaja/1
        [HttpGet("Kayttaja/{id}", Name = "GetKayttajaIDlla")]
        public Kayttaja GetKayttajaIDlla(int id)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Kayttaja.Where(k => k.KayttajaId.Equals(id)).FirstOrDefault();
            }
        }

        // GET: api/Tietokanta/Kayttaja/esimerkki@email.com
        [HttpGet("Kayttaja/sahkoposti/{sahkoposti}", Name = "GetKayttajaMeililla")]
        public Kayttaja GetKayttajaMeililla(string sahkoposti)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Kayttaja.Where(k => k.Sahkoposti.Equals(sahkoposti)).FirstOrDefault();
            }
        }

        // GET: api/Tietokanta/Kayttaja/Elain/1
        [HttpGet("Kayttaja/Elain/{id}", Name = "GetElainKayttajanIDlla")]
        public IEnumerable<Elain> GetElainKayttajanIDlla(int id)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Elain.Where(k => k.KayttajaId.Equals(id)).ToList();
            }
        }

        // GET: api/Tietokanta/Elain/Kissa
        [HttpGet("Elain/{laji}", Name = "GetElainLajilla")]
        public IEnumerable<Elain> GetElainLajilla(string laji)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Elain.Where(k => k.Laji.Nimi.Contains(laji)).ToList();
            }
        }

        // GET: api/Tietokanta/Kayttaja/Postinumero
        [HttpGet("Kayttaja/Postinumero", Name = "GetPostinumerot")]
        public IEnumerable<Paikka> GetKayttajienPostinumerot()
        {
            List<Paikka> suomenPostinrot = LuePostinumerot();
            List<Paikka> palautettavat = new List<Paikka>();
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                var q = db.Kayttaja.Select(k => k.Postinumero).Distinct();
                foreach (var postinro in q)
                {
                    var q2 = suomenPostinrot.Where(p => p.Postinumero == postinro).FirstOrDefault();
                    palautettavat.Add(q2);
                }
            }
            return palautettavat;
        }

        private List<Paikka> LuePostinumerot()
        {
            string[] rivit = System.IO.File.ReadAllLines(@".\Data\postinumerot.txt");

            List<Paikka> postinumerot = new List<Paikka>();
            foreach (string rivi in rivit)
            {
                string[] osat = rivi.Split(' ');
                Paikka p = new Paikka() { Postinumero = osat[1], Latitude = double.Parse(osat[2]), Longitude = double.Parse(osat[3]) };
                postinumerot.Add(p);
            }
            return postinumerot;
        }

        // GET: api/Tietokanta/Elain/1
        [HttpGet("Elain/hae/{id}", Name = "GetElainIdlla")]
        public Elain GetElainIdlla(int id)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Elain.Where(k => k.ElainId.Equals(id)).FirstOrDefault();
            }
        }

        // GET: api/Tietokanta/YllapitoMuutokset
        [HttpGet("YllapitoMuutokset", Name = "GetYllapitoMuutokset")]
        public IEnumerable<Yllapito> GetYllapitoMuutokset()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Yllapito.Where(y => y.Pvm.HasValue).OrderByDescending(y => y.Pvm).ToList();
            }
        }

        // GET: api/Tietokanta/YllapitoTulossa
        [HttpGet("YllapitoTulossa", Name = "GetYllapitoTulossa")]
        public IEnumerable<Yllapito> GetYllapitoTulossa()
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                return db.Yllapito.Where(y => !y.Pvm.HasValue).ToList();
            }
        }

        // POST: api/Tietokanta/Elain
        [HttpPost("Elain", Name = "PostElain")]
        public void PostElain([FromBody] Elain elain)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Add(elain);
                db.SaveChanges();
            }
        }

        // POST: api/Tietokanta/Kayttaja
        [HttpPost("Kayttaja", Name = "PostKayttaja")]
        public void PostKayttaja([FromBody] Kayttaja kayttaja)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Add(kayttaja);
                db.SaveChanges();
            }
        }

        // POST: api/Tietokanta/Yllapito
        [HttpPost("Yllapito", Name = "PostYllapito")]
        public void PostYllapito([FromBody] Yllapito yllapito)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Add(yllapito);
                db.SaveChanges();
            }
        }

        // PUT: api/Tietokanta/Kayttaja
        [HttpPut("Kayttaja/{id}", Name = "PutKayttaja")]
        public void PutKayttaja([FromBody] Kayttaja kayttaja)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Update(kayttaja);
                db.SaveChanges();
            }
        }

        // PUT: api/Tietokanta/Elain/5
        [HttpPut("Elain/{id}")]
        public void Put(int id, [FromBody] Elain elain)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Update(elain);
                db.SaveChanges();
            }
        }

        // PUT: api/Tietokanta/Yllapito/5
        [HttpPut("Yllapito/{id}")]
        public void PutYllapito(int id, [FromBody] Yllapito yllapito)
        {
            using (RyhmahaumauContext db = new RyhmahaumauContext())
            {
                db.Update(yllapito);
                db.SaveChanges();
            }
        }
    }
}
