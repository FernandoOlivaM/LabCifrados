using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;
using static System.Convert;

namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoRSAController : Controller
    {
        int bufferLengt = 10000000;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LecturaCifrado(HttpPostedFileBase postedKey, HttpPostedFileBase postedFile)
        {
            var KeyFile = (postedKey==null)?"":postedKey.FileName;
            var CipherFile = (postedFile == null) ? "" : postedFile.FileName;
            if (KeyFile != "")
            {
                return RedirectToAction("Cifrado", new { KeyFile, CipherFile });
            }
            else
            {
                return View();
            }
        }
        public ActionResult LecturaDeCifrado()
        {
            return View();
        }
        public ActionResult GenerarClaves()
        {
            return View();
        }
        public bool ValidarPrimo (int n)
        {
            var a = 0;
            for (int i = 1; i < (n + 1); i++)
            {
                if (n % i == 0)
                {
                    a++;
                }
            }
            if (a != 2)
            {
               return false;
            }
            else
            {
                return true;
            }
        }
        public ActionResult GenerarLlaves()
        {
            var p = Convert.ToInt32(Request.Form["p"]);
            var q = Convert.ToInt32(Request.Form["q"]);
            //se comprueba que los numeros ingresados sean primos
            bool pPrimo = ValidarPrimo(p); 
            bool qPrimo = ValidarPrimo(q);
            //se valida que ambos sean primos
            if (pPrimo && qPrimo)
            {
                var phi = 0;
                CifradoRSA RSA = new CifradoRSA();
                var e = RSA.GenerarLlavePublica(p, q, ref phi);
                //aun falta implementar funcion para privada
                var Ivalue = 1;
                var d = RSA.GenerarLlavePrivada(phi,phi, e,Ivalue, phi);
                //se envia el valor a la vista para generar el archivo de texto
                ViewBag.Primo = 1;
                ViewBag.eValue = e;
                ViewBag.dValue = d;
                ViewBag.nValue = p * q;
            }
            else
            {
                //0 representa que uno o los dos numeros no son primos
                //esto funcionara para activar un script en la vista
                ViewBag.Primo = 0;
            }
            return View("GenerarClaves");
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
        public ActionResult Cifrado(string KeyFile, string CipherFile)
        {
            CifradoRSA cifrado = new CifradoRSA();
            var ByteList=cifrado.LecuraCipherFile(CipherFile, bufferLengt);
            var KeyList = cifrado.LecuraKeyFile(KeyFile, bufferLengt);
            var Key = KeyList.Substring(0, KeyList.IndexOf(" "));
            var phi = KeyList.Substring(Key.Length+3);
            var BinaryList = new List<byte>();
            var Auxiliar = string.Empty;
            foreach (byte bit in ByteList)
            {
                string binario = cifrado.Cifrar(Convert.ToInt32(bit), Convert.ToInt32(Key), Convert.ToInt32(phi), Convert.ToInt32(phi));
                if(binario.Count()>8)
                {
                    foreach(char caracter in binario)
                    {
                        Auxiliar += caracter;
                        if(Auxiliar.Count()==8)
                        {
                            var Byte = Convert.ToByte(Auxiliar,2);
                            Auxiliar = string.Empty;
                            BinaryList.Add(Byte);
                        }
                    }
                }
            }
            return View();
        }
    }
}
