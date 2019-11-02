using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lab_3_1251518_1229918.Models;
using static System.Convert;

namespace Lab_3_1251518_1229918.Controllers
{
    public class CifradoRSAController : Controller
    {
        static int bufferLengt = 10000000;
        static string RutaArchivos = string.Empty;
        static string nombreArchivo = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LecturaCifrado(HttpPostedFileBase postedKey, HttpPostedFileBase postedFile)
        {
            var KeyFile = (postedKey == null) ? "" : postedKey.FileName;
            var CipherFile = (postedFile == null) ? "" : postedFile.FileName;
            if (KeyFile != "")
            {
                string rutaDirectorioUsuario = Server.MapPath(string.Empty);
                //se obtiene el nombre del archivo para utilizarlo en la generacion de nuevos
                nombreArchivo = Path.GetFileName(postedFile.FileName);
                nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("."));
                // se añade la extensión del archivo
                RutaArchivos = rutaDirectorioUsuario;
                return RedirectToAction("Cifrado", new { KeyFile, CipherFile });
            }
            else
            {
                return View();
            }
        }
        public ActionResult LecturaDeCifrado(HttpPostedFileBase postedKey, HttpPostedFileBase postedFile)
        {
            var KeyFile = (postedKey == null) ? "" : postedKey.FileName;
            var CipherFile = (postedFile == null) ? "" : postedFile.FileName;
            if (KeyFile != "")
            {
                string rutaDirectorioUsuario = Server.MapPath(string.Empty);
                //se obtiene el nombre del archivo para utilizarlo en la generacion de nuevos
                nombreArchivo = Path.GetFileName(postedFile.FileName);
                nombreArchivo = nombreArchivo.Substring(0, nombreArchivo.IndexOf("."));
                // se añade la extensión del archivo
                RutaArchivos = rutaDirectorioUsuario;
                return RedirectToAction("Decifrado", new { KeyFile, CipherFile });
            }
            else
            {
                return View();
            }
        }
        public ActionResult GenerarClaves()
        {
            return View();
        }
        public bool ValidarPrimo(int n)
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
                var N = p * q;
                var phi = (p - 1) * (q - 1);
                CifradoRSA RSA = new CifradoRSA();
                var e = RSA.GenerarLlavePublica(phi, p, q);
                //aun falta implementar funcion para privada
                var Ivalue = 1;
                var d = RSA.GenerarLlavePrivada(phi, phi, e, Ivalue, phi);
                //generar el archivo de texto
                var folder = Server.MapPath("~/Files/");
                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    var filePath = Path.Combine(folder, "PublicKey.key");
                    using (var writeStream = new FileStream(filePath, FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            writer.Write(e);
                            writer.Write(',');
                            writer.Write(N);
                        }
                    }
                    filePath = Path.Combine(folder, "PrivateKey.key");
                    using (var writeStream = new FileStream(filePath, FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            writer.Write(d);
                            writer.Write(',');
                            writer.Write(N);
                        }
                    }
                }
                else
                {
                    var filePath = Path.Combine(folder, "PublicKey.key");
                    using (var writeStream = new FileStream(filePath, FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            writer.Write(Convert.ToByte(e));
                            writer.Write(',');
                            writer.Write(Convert.ToByte(N));
                        }
                    }
                    filePath = Path.Combine(folder, "PrivateKey.key");
                    using (var writeStream = new FileStream(filePath, FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            writer.Write(Convert.ToByte(d));
                            writer.Write(',');
                            writer.Write(Convert.ToByte(N));
                        }
                    }
                }
                ViewBag.Primo = 1;
                return RedirectToAction("DownloadKey");

            }
            else
            {
                //0 representa que uno o los dos numeros no son primos
                //esto funcionara para activar un script en la vista
                ViewBag.Primo = 0;
                return View("GenerarClaves");

            }
        }
        public ActionResult DownloadKey()
        {
            string path = Server.MapPath("~/Files/");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles(".");
            List<string> lista = new List<string>(files.Length);
            foreach (var item in files)
            {
                if (item.Name.Contains(".key"))
                {
                    lista.Add(item.Name);

                }
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
            var diccionario = new Dictionary<char, int>();
            var ByteList=cifrado.LecuraCipherFile(CipherFile, bufferLengt, ref diccionario);
            var KeyList = cifrado.LecuraKeyFile(KeyFile, bufferLengt);
            var Key = KeyList.Substring(0, KeyList.IndexOf(","));
            var N = KeyList.Substring(Key.Length+1);
            var BinaryList = new List<string>();
            var ValorMax = 0;
            foreach (byte bit in ByteList)
            {
                string binario = cifrado.Cifrar(Convert.ToInt32(bit), Convert.ToInt32(Key), Convert.ToInt32(N), ref ValorMax);
                BinaryList.Add(binario);
            }
            if(ValorMax<8)
            {
                ValorMax = 8;
            }
            ByteList = new List<byte>();
            var Auxiliar = string.Empty;
            var binary = string.Empty;
            ByteList.Add(Convert.ToByte(ValorMax));
            foreach (string binario in BinaryList)
            {
                binary += binario.PadLeft(ValorMax,'0');
                foreach (char caracter in binary)
                {
                    Auxiliar += caracter;
                    if (Auxiliar.Count() == 8)
                    {
                        byte bit = Convert.ToByte(Convert.ToInt32(Auxiliar, 2));
                        ByteList.Add(bit);
                        Auxiliar = string.Empty;
                    }
                }
                binary = string.Empty;
            }
            cifrado.EscrituraArchivoCifrado(ByteList,RutaArchivos,nombreArchivo, diccionario);
            return View();
        }
        public ActionResult Decifrado(string KeyFile, string CipherFile)
        {
            CifradoRSA decifrado = new CifradoRSA();
            var ValorMax = 0;
            var diccionario = new Dictionary<int, char>();
            var ByteList = decifrado.LecuraCipherFileDecifrado(CipherFile, bufferLengt, ref ValorMax, ref diccionario, nombreArchivo, RutaArchivos);
            ByteList.Remove(ByteList[0]);
            var KeyList = decifrado.LecuraKeyFile(KeyFile, bufferLengt);
            var Key = KeyList.Substring(0, KeyList.IndexOf(","));
            var N = KeyList.Substring(Key.Length + 1);
            var BinaryList = new List<byte>();
            var binary = string.Empty;
            var Auxiliar = string.Empty;
            foreach (byte bit in ByteList)
            {
                binary += Convert.ToString(Convert.ToInt32(bit), 2);
                binary = binary.PadLeft(8, '0');
                foreach (char caracter in binary)
                {
                    Auxiliar += caracter;
                    if (Auxiliar.Count() == ValorMax)
                    {
                        var value = Convert.ToInt32(Auxiliar, 2);
                        var returnbyte = decifrado.Decifrar(value, Convert.ToInt32(Key), Convert.ToInt32(N), diccionario);
                        BinaryList.Add(returnbyte);
                        Auxiliar = string.Empty;
                    }
                }
                binary = string.Empty;
            }
            decifrado.EscrituraArchivoDecifrado(BinaryList, RutaArchivos, nombreArchivo);
            return View();
        }
    }
}
