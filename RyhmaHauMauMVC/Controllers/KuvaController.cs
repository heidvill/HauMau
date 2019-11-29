using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tietokantakirjasto;

namespace RyhmaHauMauMVC.Controllers
{
    public class KuvaController : Controller
    {
        public FileContentResult GetKuvat(int id)
        {
            using (HauMauPicsContext db = new HauMauPicsContext())
            {
                var valinta = db.Kuvakirjasto.Where(k => k.ElainId == id).FirstOrDefault();
                if (valinta == null) 
                { 
                    var eikuvaa = db.Kuvakirjasto.Where(k => k.ElainId == 404).FirstOrDefault();
                    return File(eikuvaa.Kuva, "image/" + ".jpg");
                }
                else
                {
                Kuvakirjasto kuva = valinta;
                return File(kuva.Kuva, "image/" + ".jpg");
                }
                
            }
        }

    }
}