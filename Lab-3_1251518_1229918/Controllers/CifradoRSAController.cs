using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;
using static System.Convert;

namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoRSAController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LecturaCifrado()
        {
            return View();
        }

        public ActionResult LecturaDeCifrado()
        {
            return View();
        }
        public ActionResult GenerarClaves()
        {
            return View();
        }
        public ActionResult GenerarLlaves()
        {
            var p = Convert.ToInt32(Request.Form["p"]);
            var q = Convert.ToInt32(Request.Form["q"]);
            //se comprueba que los numeros ingresados sean primos
            var a = 0;
            var b = 0;
            bool pPrimo=true; 
            bool qPrimo=true;
            for (int i = 1; i < (p + 1); i++)
            {
                if (p % i == 0)
                {
                    a++;
                }
            }
            if (a != 2)
            {
                pPrimo =false;
            }
            for (int i = 1; i < (q + 1); i++)
            {
                if (q % i == 0)
                {
                    b++;
                }
            }
            if (b != 2)
            {
                qPrimo = false;
            }
            //se valida que ambos sean primos
            if (pPrimo && qPrimo)
            {
                CifradoRSA RSA = new CifradoRSA();
                RSA.GenerarLlaves(p, q);
                return RedirectToAction("Download");
            }
            else
            {
                //0 representa que uno o los dos numeros no son primos
                //esto funcionara para activar un script en la vista
                ViewBag.Primo = 0;
                return View("GenerarClaves");
            }
        }
        public ActionResult Download()
        {
            string path = Server.MapPath("~/Files/");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles(".");
            List<string> lista = new List<string>(files.Length);
            foreach (var item in files)
            {
                lista.Add(item.Name);
            }   
            return View(lista);
        }
        public ActionResult DownloadFile(string filename)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files/"), filename);
            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
    }
}
