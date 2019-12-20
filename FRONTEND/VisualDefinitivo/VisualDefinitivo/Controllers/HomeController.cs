﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Visual.Models;

namespace PaginaVisual.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            IEnumerable<RecetasViewModel> recetas = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://apicocina.azurewebsites.net/api/");
                //HTTP GET
                var responseTask = client.GetAsync("recetas/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RecetasViewModel>>();
                    readTask.Wait();

                    recetas = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    recetas = Enumerable.Empty<RecetasViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(recetas);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult blog()
        {
            ViewBag.Message = "Your blog page.";

            return View();
        }

        public ActionResult gallery()
        {
            ViewBag.Message = "Your gallery page.";

            return View();
        }

        public ActionResult Recetas()
        {
            IEnumerable<RecetasViewModel> recetas = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44311/api/");
                //HTTP GET
                var responseTask = client.GetAsync("recetas/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RecetasViewModel>>();
                    readTask.Wait();

                    recetas = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    recetas = Enumerable.Empty<RecetasViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(recetas);
        }

        public ActionResult Productos()
        {
            ViewBag.Message = "Your single page.";

            return View();
        }


        public ActionResult loguearse()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult loguearse(ClientesViewModel cliente)
        {

            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://apicocina.azurewebsites.net/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ClientesViewModel>("operations/login", cliente);
                var lavuelta = postTask.Result.Content.ReadAsStringAsync();

                JObject jObject = JObject.Parse(lavuelta.Result);
                
                
                cliente.id_Cliente= Int32.Parse((string)jObject["id_Cliente"]);
                cliente.iEdad = Int32.Parse((string)jObject["iEdad"]);
                cliente.id_direccion = Int32.Parse((string)jObject["id_direccion"]);
                cliente.vNombre = (string)jObject["vNombre"];
                cliente.vApellido= (string)jObject["vApelldo"];
                cliente.vApelldo2= (string)jObject["vApelldo2"];
                cliente.vROL= (string)jObject["vROL"];

                AutoMapper.Mapper.Initialize(cfg=> {
                    cfg.CreateMap<ClientesViewModel, VisualDefinitivo.Models.Cliente>();
                    cfg.CreateMap< VisualDefinitivo.Models.Cliente,ClientesViewModel > ();
                });

                VisualDefinitivo.Models.Cliente.INSTANCIA = AutoMapper.Mapper.Map<ClientesViewModel, VisualDefinitivo.Models.Cliente>(cliente);

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    if (cliente.vNombre!="")
                    {
                        Session["login"] = "si";
                        return RedirectToAction("Index");
                    }
                    


                    
                }
                else { 
                    Session["login"] = "no";


                   // return RedirectToAction("Index");
                }



            }

            //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(cliente);
        }



        public ActionResult logout() {


            Session["login"] = "no";
            VisualDefinitivo.Models.Cliente.INSTANCIA = null;

            return RedirectToAction("Index");
        }
    }
}