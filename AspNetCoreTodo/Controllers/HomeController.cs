using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace AspNetCoreTodo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            throw new Exception("Error en Accion Privacy");
            //return View();
        }

        [ResponseCache(Duration = 0, 
        Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = 
                HttpContext.Features
                .Get<IExceptionHandlerPathFeature>();

            logger.LogError($"La ruta {exceptionHandlerPathFeature.Path} " +
            $"lanzo una excepcion {exceptionHandlerPathFeature.Error} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}");

            return View(new ErrorViewModel { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Path = exceptionHandlerPathFeature.Path
            });
        }
    }
}
