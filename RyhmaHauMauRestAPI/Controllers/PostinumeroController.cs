using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RyhmaHauMauRestAPI.Data;

namespace RyhmaHauMauRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostinumeroController : ControllerBase
    {
        // GET: api/Postinumero
        [HttpGet]
        public IEnumerable<Paikka> Get()
        {
            List<Paikka> postinumerot = LuePostinumerot();
            return postinumerot;
        }

        // GET: api/Postinumero/5
        [HttpGet("{id}", Name = "Get")]
        public Paikka Get(string id)
        {
            List<Paikka> postinumerot = LuePostinumerot();
            Paikka p = postinumerot.Where(nro => nro.Postinumero == id).FirstOrDefault();
            return p;
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
    }
}
