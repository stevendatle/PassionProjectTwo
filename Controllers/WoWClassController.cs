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

namespace PassionProjectTwo.Controllers
{
    public class WoWClassController : Controller
    {
        //connection to web api
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static WoWClassController()
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

        //get: comp/list
        public ActionResult List()
        {
            string url = "wowclassdata/getclasses";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<WoWClass> SelectedClasses = response.Content.ReadAsAsync<IEnumerable<WoWClass>>().Result;
                return View(SelectedClasses);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Class/Details/5
        public ActionResult Details(int id)
        {
            string url = "WoWClassData/FindClass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //catching status code
            if (response.IsSuccessStatusCode)
            {
                //add data
                WoWClass SelectedClass = response.Content.ReadAsAsync<WoWClass>().Result;
                Debug.WriteLine(SelectedClass.ClassName);
                //  ViewModel.Class = SelectedClass;
                url = "compdata/getclassesforcomp/" + id;
                return View(SelectedClass);
            }
            return RedirectToAction("List");
        }

        // GET: Class/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        public ActionResult Create(WoWClass ClassInfo)
        {
            Debug.WriteLine(ClassInfo.ClassName);
            string url = "WoWClassData/addClass";
            Debug.WriteLine(jss.Serialize(ClassInfo));
            HttpContent content = new StringContent(jss.Serialize(ClassInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int Classid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Classid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Class/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "WoWClassData/FindClass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into comp DTO
                WoWClass SelectedClass = response.Content.ReadAsAsync<WoWClass>().Result;
                return View(SelectedClass);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, WoWClass ClassInfo)
        {
            Debug.WriteLine(ClassInfo.ClassName);
            string url = "WoWClassdata/updateclass/" + id;
            Debug.WriteLine(jss.Serialize(ClassInfo));
            HttpContent content = new StringContent(jss.Serialize(ClassInfo));
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

        // GET: Class/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "classdata/findclass/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                //Put data into comp dto
                WoWClass SelectedClass = response.Content.ReadAsAsync<WoWClass>().Result;
                return View(SelectedClass);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Class/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "classdata/delete/" + id;
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
