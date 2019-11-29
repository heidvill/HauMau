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
using RyhmaHauMauMVC.Extensions.Alerts;
using RyhmaHauMauMVC.Models;

namespace RyhmaHauMauMVC.Controllers
{
    public class KayttajaController : Controller
    {
        // GET: Kayttaja
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kayttaja/Tiedot/5
        public ActionResult Tiedot(int id)
        {
            Kayttaja k = FormDataHelper.HaeKayttajaIdlla(id);
            ViewBag.KayttajanLemmikit = FormDataHelper.HaeKayttajanLemmikit(id);
            return View(k);
        }

        // GET: Kayttaja/Uusi
        public ActionResult Uusi()
        {
            return View();
        }

        // POST: Kayttaja/Uusi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Uusi(Kayttaja kayttaja)
        {
            bool isValid = true;
            if (FormDataHelper.HaeKayttajaSahkopostilla(kayttaja.Sahkoposti) != null)
            {
                isValid = false;
                ModelState.AddModelError("Sahkoposti", "Sähköposti on jo käytössä");
            }
            if (!ValidatePostinro(kayttaja.Postinumero))
            {
                isValid = false;
                ModelState.AddModelError("Postinumero", "Anna suomalainen postinumero");
            }

            if (!isValid || !ModelState.IsValid)
            {
                return View(kayttaja).WithWarning("Korjaa", "tiedot");
            }

            bool succee = FormDataHelper.LisaaKayttaja(kayttaja);
            if (succee)
            {
                Kayttaja uusi = FormDataHelper.HaeKayttajaSahkopostilla(kayttaja.Sahkoposti);
                HttpContext.Session.SetInt32("ID", uusi.KayttajaId);
                return RedirectToAction("Tiedot", new { id = uusi.KayttajaId }).WithSuccess("Onnistui!", "Uusi käyttäjä luotu. Olet nyt kirjautunut sisään.");
            }
            else
            {
                return View(kayttaja).WithWarning("Hups!", "Jokin meni vikaan.");
            }
        }

        // GET: Kayttaja/Edit/5
        public ActionResult Muokkaa(int id)
        {
            Kayttaja k = FormDataHelper.HaeKayttajaIdlla(id);
            return View(k);
        }

        // POST: Kayttaja/Paivita/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Muokkaa(int id, Kayttaja kayttaja)
        {
            kayttaja.KayttajaId = HttpContext.Session.GetInt32("ID").GetValueOrDefault();

            bool isValid = true;
            if (!ValidatePostinro(kayttaja.Postinumero))
            {
                isValid = false;
                ModelState.AddModelError("Postinumero", "Anna suomalainen postinumero");
            }
            if (!isValid || !ModelState.IsValid)
            {
                return View(kayttaja).WithWarning("Korjaa", "tiedot");
            }

            bool success = FormDataHelper.PaivitaKayttaja(kayttaja, id);
            if (success)
            {
                Kayttaja uusi = FormDataHelper.HaeKayttajaSahkopostilla(kayttaja.Sahkoposti);
                return RedirectToAction("Tiedot", new { id = uusi.KayttajaId }).WithSuccess("Onnistui!", "Tiedot päivitetty!");
            }
            else
            {
                return View(kayttaja).WithWarning("Hups!", "Jokin meni vikaan.");
            }
        }


        // GET: Kayttaja/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //// POST: Kayttaja/Delete/5
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string sahkoposti)
        {
            if (string.IsNullOrEmpty(sahkoposti))
            {
                return View().WithDanger("Anna", "sähköposti kirjautuaksesi.");
            }
            Kayttaja k = FormDataHelper.HaeKayttajaSahkopostilla(sahkoposti);
            if (k == null)
            {
                return View().WithDanger("Ei", "löytynyt käyttäjää antamallasi sähköpostilla.");
            }

            HttpContext.Session.SetInt32("ID", k.KayttajaId);
            return RedirectToAction("Tiedot", new { id = k.KayttajaId }).WithInfo("Kirjauduit", "sisään.");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ID");
            return RedirectToAction("Login").WithInfo("Olet", "kirjautunut ulos.");
        }

        private bool ValidatePostinro(string postinro)
        {
            if (string.IsNullOrEmpty(postinro)) return false;
            if (postinro.Length != 5) return false;
            if (!int.TryParse(postinro, out int luku)) return false;
            if (luku < 0) return false;

            return true;
        }

    }
}