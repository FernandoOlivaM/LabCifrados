using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;
namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoZigZagController : Controller
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
            int niveles = 2;
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
                niveles = Convert.ToInt32(Request.Form["niveles"].ToString());
            }
            return RedirectToAction("Cifrado", new { ArchivoLeido, niveles });
        }
        public ActionResult Cifrado(string archivoLeido, int niveles)
        {
            CifradoZigZag cifradoZigZag = new CifradoZigZag();
            cifradoZigZag.CifrarMensaje(archivoLeido, niveles);
            return View();
        }
        public ActionResult LecturaDecifrado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LecturaDecifrado(HttpPostedFileBase postedFile)
        {
            int niveles = 2;
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
                niveles = Convert.ToInt32(Request.Form["niveles"].ToString());
            }
            return RedirectToAction("Cifrado", new { ArchivoLeido, niveles });
        }
        public ActionResult Decifrado(string archivoLeido, int niveles)
        {
            CifradoZigZag cifradoZigZag = new CifradoZigZag();
            cifradoZigZag.DecifrarMensaje(archivoLeido, niveles);
            return View();
        }
    }
}
