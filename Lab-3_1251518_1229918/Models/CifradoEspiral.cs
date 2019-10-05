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
        string textoMatriz = string.Empty;
        

        public void CifrarMensaje(string rutaAchivos, string archivoLeido, int m, bool direccion)
        {
            RutaUsuario = rutaAchivos;
            LeerArchivo(archivoLeido);
            GenerarMatrizCifrado(m, direccion);
            EscribirEnArchivoCifrado(textoMatriz);
        }
     
        public void LeerArchivo(string archivoLeido)
        {
            int bufferLength = 100000;
            var byteBuffer = new byte[bufferLength];

            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                        foreach (char letra in byteBuffer)
                        {
                            if (letra != 0)
                            {
                                texto += letra;
                            }
                        }
                    }
                }
            }
           
        }

        public void GenerarMatrizCifrado(int valorM, bool direccion)
        {
            var valorN = this.texto.Length / valorM;
            int contadorTexto = 0;
            if (this.texto.Length % valorM != 0)
            {
                valorN++;
            }

            //direccion = 0: vertical, direccion = 1: horizontal
            string[,] matriz = new string[valorM, valorN];
            if (direccion)
            {
                //llenando matriz horizontalmente
                for (int p = 0; p < valorM; p++)
                {
                    for (int j =0; j < valorN; j++)
                    {
                        if (contadorTexto != texto.Length)
                        {
                            matriz[p, j] = Convert.ToString(texto[contadorTexto]);
                            contadorTexto++;
                        }
                        else
                        {
                            matriz[p, j] = "$";
                        }
                    }
                }

                recorrerHaciaAbajo(valorM, valorN, matriz);
            }
            else
            {
                //llenando matriz verticalmente
                for (int p = 0; p < valorN; p++)
                {
                    for (int j = 0; j < valorM; j++)
                    {
                        if (contadorTexto != texto.Length)
                        {
                            matriz[j, p] = Convert.ToString(texto[contadorTexto]);
                            contadorTexto++;
                        }
                        else
                        {
                            matriz[j, p] = "$";
                        }
                    }
                }
                recorrerHaciaDerecha(valorM, valorN, matriz);
            }
            
           
        }
        public void recorrerHaciaAbajo(int valorM, int valorN , string[,] matriz)
        {
            //recorriendo matriz en esprial
            int i, auxiliarM = 0, auxiliarN = 0;
            while (auxiliarM < valorM && auxiliarN < valorN)
            {
                for (i = auxiliarM; i < valorM; ++i)
                {
                    textoMatriz += matriz[i, auxiliarN];
                }
                auxiliarN++;
                for (i = auxiliarN; i < valorN; ++i)
                {
                    textoMatriz += matriz[valorM - 1, i];
                }
                valorM--;
                if (auxiliarN < valorN)
                {
                    for (i = valorM - 1; i >= auxiliarM; --i)
                    {
                        textoMatriz += matriz[i, valorN - 1];
                    }
                    valorN--;
                }
                if (auxiliarM < valorM)
                {
                    for (i = valorN - 1; i >= auxiliarN; --i)
                    {
                        textoMatriz += matriz[auxiliarM, i];
                    }
                    auxiliarM++;
                }
            }
            EscribirEnArchivoCifrado(textoMatriz);
        }
        public void recorrerHaciaDerecha(int valorM, int valorN, string[,] matriz)
        {
            //recorriendo matriz en esprial
            int i, auxiliarM = 0, aulixiarN = 0;
            while (auxiliarM < valorM && aulixiarN < valorN)
            {
                for (i = aulixiarN; i < valorN; ++i)
                {
                    textoMatriz += matriz[auxiliarM, i];
                }
                auxiliarM++;
                for (i = auxiliarM; i < valorM; ++i)
                {
                    textoMatriz += matriz[i, valorN - 1];
                }
                valorN--;
                if (auxiliarM < valorM)
                {
                    for (i = valorN - 1; i >= aulixiarN; --i)
                    {
                        textoMatriz += matriz[valorM - 1, i];
                    }
                    valorM--;
                }
                if (aulixiarN < valorN)
                {
                    for (i = valorM - 1; i >= auxiliarM; --i)
                    {
                        textoMatriz += matriz[i, aulixiarN];
                    }
                    aulixiarN++;
                }

            }
            EscribirEnArchivoCifrado(textoMatriz);

        }
        public void EscribirEnArchivoCifrado(string texto)
        {
           //se escribe el texto cifrado en el archivo
            using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoCifradoEspiral.cif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
                }
            }
            textoMatriz = string.Empty;
        }
        public void EscribirEnArchivoDecifrado(string texto)
        {
            //se escribe el texto cifrado en el archivo
            using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoDecifradoEspiral.txt", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
                }
            }
        }
        public void DecifrarMensaje(string rutaAchivos, string archivoLeido, int m, bool direccion)
        {
            RutaUsuario = rutaAchivos;
            LeerArchivo(archivoLeido);
            GenerarMatrizDecifrado(m, direccion);
        }
        public void GenerarMatrizDecifrado(int valorM, bool direccion)
        {           
            var valorN = this.texto.Length / valorM;
            int contadorTexto = 0;
            if (this.texto.Length % valorM != 0)
            {
                valorN++;
            }
            var x = valorM;
            var y = valorN;
       
            char[,] matriz = new char[valorM, valorN];
            if (direccion)
            {
                int i, auxiliarM = 0, auxiliarN = 0;
                while (auxiliarM < valorM && auxiliarN < valorN)
                {
                    for (i = auxiliarM; i < valorM; ++i)
                    {
                        matriz[i, auxiliarN] = texto[contadorTexto];
                        contadorTexto++;
                    }
                    auxiliarN++;
                    for (i = auxiliarN; i < valorN; ++i)
                    {
                        matriz[valorM - 1, i] = texto[contadorTexto];
                        contadorTexto++;
                    }
                    valorM--;
                    if (auxiliarN < valorN)
                    {
                        for (i = valorM - 1; i >= auxiliarM; --i)
                        {
                            matriz[i, valorN - 1] = texto[contadorTexto];
                            contadorTexto++;
                        }
                        valorN--;
                    }
                    if (auxiliarM < valorM)
                    {
                        for (i = valorN - 1; i >= auxiliarN; --i)
                        {
                            matriz[auxiliarM, i] = texto[contadorTexto];
                            contadorTexto++;
                        }
                        auxiliarM++;
                    }
                }
                var textoDecifrado = string.Empty;
                for (int p = 0; p < x; p++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        if(matriz[p,j] != 36)
                        {
                            textoDecifrado += matriz[p, j];

                        }
                    }
                }
                EscribirEnArchivoDecifrado(textoDecifrado);
            }
            else
            {
                //recorriendo matriz en esprial
                int i, auxiliarM = 0, aulixiarN = 0;
                while (auxiliarM < valorM && aulixiarN < valorN)
                {
                    for (i = aulixiarN; i < valorN; ++i)
                    {
                        matriz[auxiliarM, i] = texto[contadorTexto];
                        contadorTexto++;
                    }
                    auxiliarM++;
                    for (i = auxiliarM; i < valorM; ++i)
                    {
                        matriz[i, valorN - 1] = texto[contadorTexto];
                        contadorTexto++; ;
                    }
                    valorN--;
                    if (auxiliarM < valorM)
                    {
                        for (i = valorN - 1; i >= aulixiarN; --i)
                        {
                            matriz[valorM - 1, i] = texto[contadorTexto];
                            contadorTexto++; ;
                        }
                        valorM--;
                    }
                    if (aulixiarN < valorN)
                    {
                        for (i = valorM - 1; i >= auxiliarM; --i)
                        {
                            matriz[i, aulixiarN] = texto[contadorTexto];
                            contadorTexto++;
                        }
                        aulixiarN++;
                    }

                }
                var textoDecifrado = string.Empty;
                for (int p = 0; p < y; p++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        if (matriz[j,p] != 36)
                        {
                            textoDecifrado += matriz[j, p];

                        }
                    }
                }
                EscribirEnArchivoDecifrado(textoDecifrado);

            }

        }
    }
}