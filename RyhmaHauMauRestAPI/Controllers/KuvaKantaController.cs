using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tietokantakirjasto;

namespace RyhmaHauMauRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KuvaKantaController : ControllerBase
    {
        // GET: api/KuvaKanta
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/KuvaKanta/5
        [HttpGet("/Kuvat/{id}", Name = "GetKuvat")]
        public FileContentResult GetKuvat(int id)
        {
            using (HauMauPicsContext db = new HauMauPicsContext())
            {
                var valinta = db.Kuvakirjasto.Where(k => k.KuvaId == id).FirstOrDefault();
                if (valinta == null){ return null; }
                Kuvakirjasto kuva = valinta;
                return File(kuva.Kuva, "image/" + ".jpg");
            }
        }

        // POST: api/KuvaKanta
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
