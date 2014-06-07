using Ramses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ramses.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        private readonly List<Client> clients = new List<Client>()
        { 
            new Client { Id = 1, Name = "Julio Avellaneda", Email = "julito_gtu@hotmail.com" },
            new Client { Id = 2, Name = "Juan Torres", Email = "jtorres@hotmail.com" },
            new Client { Id = 3, Name = "Oscar Camacho", Email = "oscar@hotmail.com" },
            new Client { Id = 4, Name = "Gina Urrego", Email = "ginna@hotmail.com" },
            new Client { Id = 5, Name = "Nathalia Ramirez", Email = "natha@hotmail.com" },
            new Client { Id = 6, Name = "Raul Rodriguez", Email = "rodriguez.raul@hotmail.com" },
            new Client { Id = 7, Name = "Johana Espitia", Email = "johana_espitia@hotmail.com" }
        };

        /// <summary>
        /// Renderiza uma grid
        /// </summary>
        /// <returns>Pacial contendo a codificação da grid</returns>
        public ActionResult GriClients()
        {
            return PartialView("_GridClients", clients);
        }
	}
}