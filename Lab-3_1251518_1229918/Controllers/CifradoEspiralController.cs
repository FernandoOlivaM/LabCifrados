using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;

namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoEspiralController : Controller
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
            int m = 2;
            bool direccion = false;
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
                m = Convert.ToInt32(Request.Form["m"].ToString());
                string validarDireccion = Request.Form["direccion"];
                if(validarDireccion == "1")
                {
                    direccion = true;
                }
            }
            return RedirectToAction("Cifrado", new { ArchivoLeido, m, direccion });
        }
        public ActionResult Cifrado(string archivoLeido, int m, bool direccion)
        {
            CifradoEspiral cifradoEspiral = new CifradoEspiral();
            cifradoEspiral.CifrarMensaje(RutaArchivos, archivoLeido, m, direccion);
            return View();
        }
        public ActionResult LecturaDecifrado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LecturaDecifrado(HttpPostedFileBase postedFile)
        {
            int m = 2;
            bool direccion = false;
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
                m = Convert.ToInt32(Request.Form["m"].ToString());
                //se valida la direccion para el cifrado, mas detalles en el modelo
                string validarDireccion = Request.Form["direccion"];
                if (validarDireccion == "1")
                {
                    direccion = true;
                }
            }
            return RedirectToAction("Decifrado", new { ArchivoLeido, m, direccion });
        }
        public ActionResult Decifrado(string archivoLeido, int m, bool direccion)
        {
            CifradoEspiral cifradoEspiral = new CifradoEspiral();
            cifradoEspiral.DecifrarMensaje(RutaArchivos, archivoLeido, m, direccion);
            return View();
        }
    }
}
