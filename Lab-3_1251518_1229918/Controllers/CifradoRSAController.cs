using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;
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
        public ActionResult Download()
        {
            string path = Server.MapPath("~/Files/");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles(".");
            List<string> lista = new List<string>(files.Length);
            foreach (var item in files)
            {
                if (!item.Name.Contains(".per"))
                {
                    lista.Add(item.Name);

                }
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
