using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PassionProjectTwo.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Passionproject.Controllers
{
    public class CompController : Controller
    {

        //using HTTP Client to connect to web api
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CompController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("http://localhost:52117/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }



        // GET: Comp/List
        public ActionResult List()
        {
            string url = "compdata/getcomps";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<WoWComp> SelectedComps = response.Content.ReadAsAsync<IEnumerable<WoWComp>>().Result;
                return View(SelectedComps);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Comp/Details/5
        public ActionResult Details(int id)
        {
            // ShowComp ViewModel = new ShowComp();
            string url = "compdata/findcomp/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            //Catch the status code
            if (response.IsSuccessStatusCode)
            {
                //add data into comp data transfer object
                WoWComp SelectedComp = response.Content.ReadAsAsync<WoWComp>().Result;
                Debug.WriteLine(SelectedComp.CompName);
                //  ViewModel.comp = SelectedComp;
                url = "teamdata/getclassesforcomp/" + id;
                return View(SelectedComp);
            }
            return RedirectToAction("List");
        }

        // GET: Comp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comp/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(WoWComp CompInfo)
        {
            Debug.WriteLine(CompInfo.CompName);
            string url = "Compdata/addComp";
            Debug.WriteLine(jss.Serialize(CompInfo));
            HttpContent content = new StringContent(jss.Serialize(CompInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int Compid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Compid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Comp/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string url = "compdata/findcomp/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into comp DTO
                WoWComp SelectedComp = response.Content.ReadAsAsync<WoWComp>().Result;
                return View(SelectedComp);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Comp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, WoWComp CompInfo)
        {
            Debug.WriteLine(CompInfo.CompName);
            string url = "compdata/updatecomp/" + id;
            Debug.WriteLine(jss.Serialize(CompInfo));
            HttpContent content = new StringContent(jss.Serialize(CompInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Comp/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "compdata/findcomp/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into comp dto
                WoWComp SelectedComp = response.Content.ReadAsAsync<WoWComp>().Result;
                return View(SelectedComp);
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // POST: Comp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "compdata/delete/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}