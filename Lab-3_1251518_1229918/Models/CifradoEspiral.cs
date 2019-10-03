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

        public void CifrarMensaje(string rutaAchivos, string archivoLeido, int m, bool direccion)
        {
                       RutaUsuario = rutaAchivos;
            LeerArchivo(archivoLeido);
            GenerarMatrizCifrado(m, direccion);


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
      
        public void GenerarMatrizCifrado(int valorM, bool direccion)
        {
            var valorN = texto.Length / valorM;
            int contadorTexto = 0;
            if (texto.Length > (valorM * valorN))
            {
                valorN++;
            }
            if(texto.Length % valorM != 0 )
            {
                valorM++;
            }
            
            //direccion = 0: vertical, direccion = 1: horizontal
            string[,] matriz = new string[valorM, valorN];
            if (direccion)
            {
                //llenando matriz horizontalmente
                for (int i = 0; i < valorM; i++)
                {
                    for (int j = 0; j < valorN; j++)
                    {
                        if (contadorTexto != texto.Length)
                        {
                            matriz[i, j] = Convert.ToString(texto[contadorTexto]);
                            contadorTexto++;
                        }
                        else
                        {
                            matriz[i, j] = "$";
                        }
                    }
                }
                //obteniendo texto verticalmente
                string cifrado = string.Empty;
                for (int i = 0; i < valorM; i++)
                {
                    for (int j = 0; j < valorN; j++)
                    {
                        cifrado += matriz[i, j];
                    }
                }
            }
            else
            {
                //llenando matriz verticalmente
                for (int i = 0; i < valorN; i++)
                {
                    for (int j = 0; j < valorM; j++)
                    {
                        if (contadorTexto != texto.Length)
                        {
                            matriz[j,i] = Convert.ToString(texto[contadorTexto]);
                            contadorTexto++;
                        }
                        else
                        {
                            matriz[j, i] = "$";
                        }
                    }
                }
            }
        }
        public void DecifrarMensaje(string RutaAchivos, string ArchivoLeido, int m, bool direccion)
        {

           // GenerarMatriz(m, direccion);
        }
    }
}