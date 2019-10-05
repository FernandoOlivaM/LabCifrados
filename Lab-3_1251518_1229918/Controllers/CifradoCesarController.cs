using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;

namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoCesarController : Controller
    {
        static string RutaArchivos = string.Empty;

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LecturaCifrado()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LecturaCifrado(HttpPostedFileBase postedFile)
        {
            string clave = string.Empty;
            string ArchivoLeido = string.Empty;
            //le permite corroborar si la carpeta Files ya existe en la solución
            bool Exists;
            string Paths = Server.MapPath("~/Files/");
            Exists = Directory.Exists(Paths);
            if (!Exists)
            {
                Directory.CreateDirectory(Paths);
            }
            //el siguiente if permite seleccionar un archivo en específico
            if (postedFile != null)
            {
                string rutaDirectorioUsuario = Server.MapPath(string.Empty);
                //se toma la ruta y nombre del archivo
                ArchivoLeido = rutaDirectorioUsuario + Path.GetFileName(postedFile.FileName);
                // se añade la extensión del archivo
                RutaArchivos = rutaDirectorioUsuario;
                postedFile.SaveAs(ArchivoLeido);
                clave = Request.Form["clave"].ToString();
            }
            return RedirectToAction("Cifrado", new { ArchivoLeido, clave});
        }
        public ActionResult Cifrado(string archivoLeido, string clave)
        {
            CifradoCesar cifradoCesar = new CifradoCesar();
            cifradoCesar.CifrarMensaje(RutaArchivos, archivoLeido, clave);
            return RedirectToAction("Download");
        }
        public ActionResult LecturaDecifrado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LecturaDecifrado(HttpPostedFileBase postedFile)
        {
            string clave = string.Empty;
            string ArchivoLeido = string.Empty;
            //le permite corroborar si la carpeta Files ya existe en la solución
            bool Exists;
            string Paths = Server.MapPath("~/Files/");
            Exists = Directory.Exists(Paths);
            if (!Exists)
            {
                Directory.CreateDirectory(Paths);
            }
            //el siguiente if permite seleccionar un archivo en específico
            if (postedFile != null)
            {
                string rutaDirectorioUsuario = Server.MapPath(string.Empty);
                //se toma la ruta y nombre del archivo
                ArchivoLeido = rutaDirectorioUsuario + Path.GetFileName(postedFile.FileName);
                // se añade la extensión del archivo
                RutaArchivos = rutaDirectorioUsuario;
                postedFile.SaveAs(ArchivoLeido);
                clave = Request.Form["clave"].ToString();
            }
            return RedirectToAction("Decifrado", new { ArchivoLeido, clave });
        }
        public ActionResult Decifrado(string archivoLeido, string clave)
        {
            CifradoCesar cifradoCesar = new CifradoCesar();
            cifradoCesar.DecifrarMensaje(RutaArchivos ,archivoLeido, clave);
            return RedirectToAction("Download");
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
