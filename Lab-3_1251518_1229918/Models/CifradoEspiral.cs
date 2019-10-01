using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoEspiral
    {
        string RutaUsuario = string.Empty;
        string texto = string.Empty;

        public void CifrarMensaje(string rutaAchivos, string archivoLeido, int m, int n, bool direccion)
        {
            RutaUsuario = rutaAchivos;
            LeerArchivo(archivoLeido);
            GenerarMatriz(m, n, direccion);


        }
        public void LeerArchivo(string archivoLeido)
        {
            int bufferLength = 1024;
            var byteBuffer = new byte[bufferLength];

            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                    }
                }
            }
            foreach (char letra in byteBuffer)
            {
                texto += letra;
            }
        }
        public void DecifrarMensaje(string RutaAchivos, string ArchivoLeido, int m, int n, bool direccion)
        {
            GenerarMatriz(m, n, direccion);
        }
        public void GenerarMatriz(int m, int n, bool direccion)
        {
            //direccion = 0: derecha, direccion = 1: izquierda
            string[,] matriz = new string[m, n];
            if (!direccion)
            {

            }
        }
    }
}