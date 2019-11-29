using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RyhmaHauMauMVC.Extensions;
using RyhmaHauMauMVC.Models;
using RyhmaHauMauMVC.Extensions.Alerts;

namespace RyhmaHauMauMVC.Controllers
{
    public class ElainController : Controller
    {
        // GET: Elain
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HaeLemmikit()
        {
            ViewBag.Lajit = FormDataHelper.HaeLajit();
            ViewBag.Pnrot = FormDataHelper.HaePostinumerot();
            ViewBag.Kayttajat = FormDataHelper.HaeKayttajat();
            return View();
        }

        public ActionResult Lemmikit()
        {
            return RedirectToAction("LemmikkiHaku");
        }

        public IActionResult LemmikkiHaku(string laji = null, string postinumero = null, int sivunro = 0, string jarjestys = null)
        {
            List<Kayttaja> kayttajat = FormDataHelper.HaeKayttajat();
            ViewBag.Kayttajat = kayttajat;
            ViewBag.Lajit = FormDataHelper.HaeLajit();

            ViewBag.Laji = laji;
            ViewBag.PNro = postinumero;
            ViewBag.Jarjestys = jarjestys;

            List<Elain> lemmikit = FormDataHelper.HaeElaimet();
            if (laji != "kaikki" && laji != null)
            {
                lemmikit = lemmikit.Where(l => l.Laji.Nimi == laji).ToList();
            }

            if (postinumero != "eivalittu" && postinumero != null)
            {
                var pnrollarajattu = kayttajat.Where(k => k.Postinumero == postinumero);
                foreach (var kayttaja in pnrollarajattu)
                {
                    lemmikit = lemmikit.Where(l => l.KayttajaId == kayttaja.KayttajaId).ToList();
                }
            }

            if (jarjestys != null && jarjestys != "eivalittu")
            {
                lemmikit = JarjestaHakuTulokset(lemmikit, jarjestys);
            }

            lemmikit = Sivuta(lemmikit, sivunro);
            return View("Lemmikit", lemmikit);
        }

        private List<Elain> Sivuta(List<Elain> lemmikit, int sivunro = 0)
        {
            int tuloksiasivulla = 5;
            int sivumäärä = lemmikit.Count() / tuloksiasivulla;

            Math.DivRem(lemmikit.Count, tuloksiasivulla, out int jakojäännös);
            if (jakojäännös > 0) sivumäärä++;

            int skippaa = sivunro * tuloksiasivulla;
            if (skippaa >= lemmikit.Count - tuloksiasivulla)
            {
                skippaa = lemmikit.Count - tuloksiasivulla;
            }

            if (sivunro < 0) sivunro = 1;

            ViewBag.Sivunro = sivunro;
            ViewBag.Sivumäärä = sivumäärä;

            if (sivunro == sivumäärä-1)
            {
                skippaa += tuloksiasivulla - jakojäännös;
                tuloksiasivulla = jakojäännös;
            }

            lemmikit = lemmikit.Skip(skippaa).Take<Elain>(tuloksiasivulla).ToList();
            return lemmikit;
        }

        private List<Elain> JarjestaHakuTulokset(List<Elain> lemmikit, string jarjestys)
        {
            switch (jarjestys)
            {
                case "Nimi":
                    lemmikit = (from l in lemmikit
                                orderby l.Nimi ascending
                                select l).ToList();
                    break;
                case "Rotu":
                    lemmikit = (from l in lemmikit
                                orderby l.Rotu ascending
                                select l).ToList();
                    break;
                case "Laji":
                    lemmikit = (from l in lemmikit
                                orderby l.Laji.Nimi ascending
                                select l).ToList();
                    break;
                case "Ikä":
                    lemmikit = (from l in lemmikit
                                orderby l.Syntymapaiva descending
                                select l).ToList();
                    break;
                default:
                    break;
            }

            return lemmikit;
        }

        // GET: Elain/Tiedot/5
        public ActionResult Tiedot(int id)
        {
            List<Kayttaja> kayttajat = FormDataHelper.HaeKayttajat();
            ViewBag.Kayttajat = kayttajat;
            ViewBag.Lajit = FormDataHelper.HaeLajit();
            Elain elain = FormDataHelper.HaeElainIdlla(id);
            return View(elain);
        }

        // GET: Elain/Luo
        public ActionResult Luo()
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return RedirectToAction("Login", "Kayttaja");
            }
            ViewBag.Lajit = FormDataHelper.HaeLajit();
            return View();
        }

        // POST: Elain/Luo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Luo(Elain elain)
        {
            int kayttajaId = HttpContext.Session.GetInt32("ID").GetValueOrDefault();
            elain.KayttajaId = kayttajaId;
            bool success = FormDataHelper.LisaaElain(elain);
            if (success)
            {
                return RedirectToAction("Tiedot", "Kayttaja", new { id = kayttajaId }).WithSuccess("Lemmikki", "lisätty onnistuneesti");
            }
            else
            {
                return View(elain).WithDanger("Jokin", "meni vikaan...");
            }
        }

        // GET: Elain/Paivita/5
        public ActionResult Paivita(int id)
        {
            var kayttajaId = HttpContext.Session.GetInt32("ID").GetValueOrDefault();

            ViewBag.Lajit = FormDataHelper.HaeLajit();
            ViewBag.Kayttaja = FormDataHelper.HaeKayttajaIdlla(kayttajaId);
            Elain elain = FormDataHelper.HaeElainIdlla(id);
            return View(elain);
        }

        // POST: Elain/Paivita/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Paivita(int id, Elain elain)
        {
            int kayttajaId = HttpContext.Session.GetInt32("ID").GetValueOrDefault();
            elain.ElainId = id;
            elain.KayttajaId = kayttajaId;
            bool success = FormDataHelper.PaivitaElain(id, elain);
            if (success)
            {
                return RedirectToAction("Tiedot", "Kayttaja", new { id = kayttajaId }).WithSuccess("Tiedot", "päivitetty onnistuneesti");
            }
            else
            {
                return View().WithDanger("Jokin", "meni vikaan...");
            }
        }

        // GET: Elain/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //// POST: Elain/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
