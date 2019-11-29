using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RyhmaHauMauMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RyhmaHauMauMVC.Extensions
{
    public class FormDataHelper
    {
        public static List<Laji> HaeLajit()
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/Laji").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<List<Laji>>(json);//hakee listan lajeista
        }

        public static List<Elain> HaeKayttajanLemmikit(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/Kayttaja/Elain/{id}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<List<Elain>>(json);
        }

        public static Kayttaja HaeKayttajaIdlla(int id)
        {
            string paluu = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/kayttaja/{id}").Result;
                paluu = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<Kayttaja>(paluu);
        }

        public static Kayttaja HaeKayttajaSahkopostilla(string sahkoposti)
        {
            string paluu = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/kayttaja/sahkoposti/{sahkoposti}").Result;
                paluu = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<Kayttaja>(paluu);
        }

        public static List<Kayttaja> HaeKayttajat()
        {
            string paluu = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/Kayttaja").Result;
                paluu = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<List<Kayttaja>>(paluu);
        }

        public static List<string> HaePostinumerot()
        {
            string pnrot = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/KayttajaPnro").Result;
                pnrot = response.Content.ReadAsStringAsync().Result;
            }
            return JsonConvert.DeserializeObject<List<string>>(pnrot);
        }

        public static bool LisaaKayttaja(Kayttaja kayttaja)
        {
            string json = JsonConvert.SerializeObject(kayttaja);
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync("https://localhost:44328/api/Tietokanta/Kayttaja", content).Result;
            }
            return response.IsSuccessStatusCode;
        }

        public static bool PaivitaKayttaja(Kayttaja kayttaja, int id)
        {
            string json = JsonConvert.SerializeObject(kayttaja);
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PutAsync($"https://localhost:44328/api/Tietokanta/Kayttaja/{id}", content).Result;
            }
            return response.IsSuccessStatusCode;
        }

        public static List<Elain> HaeElaimet()
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta").Result; //tuo listan
                json = response.Content.ReadAsStringAsync().Result;
            }
            var lemmikit = JsonConvert.DeserializeObject<List<Elain>>(json);
            return lemmikit;
        }

        public static bool LisaaElain(Elain elain)
        {
            string json = JsonConvert.SerializeObject(elain);
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync("https://localhost:44328/api/Tietokanta/Elain", content).Result;
            }
            return response.IsSuccessStatusCode;
        }

        public static Elain HaeElainIdlla(int id)
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/Elain/hae/{id}").Result;
                json = response.Content.ReadAsStringAsync().Result;
            }
            var elain = JsonConvert.DeserializeObject<Elain>(json);
            return elain;
        }

        public static bool PaivitaElain(int id, Elain elain)
        {
            string json = JsonConvert.SerializeObject(elain);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PutAsync($"https://localhost:44328/api/Tietokanta/Elain/{id}", content).Result;

                return response.IsSuccessStatusCode;
            }
        }

        public static List<Yllapito> Muutokset()
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/YllapitoMuutokset").Result; //tuo listan
                json = response.Content.ReadAsStringAsync().Result;
            }
            var muutokset = JsonConvert.DeserializeObject<List<Yllapito>>(json);
            return muutokset;
        }

        public static List<Yllapito> Tulossa()
        {
            string json = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync($"https://localhost:44328/api/Tietokanta/YllapitoTulossa").Result; //tuo listan
                json = response.Content.ReadAsStringAsync().Result;
            }
            var tulossa = JsonConvert.DeserializeObject<List<Yllapito>>(json);
            return tulossa;
        }
    }
}
