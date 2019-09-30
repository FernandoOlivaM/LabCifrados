﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoCesar
    {
        static Dictionary<string, int> diccionarioOriginal = new Dictionary<string, int>();
        static Dictionary<string, int> diccionarioCifrado = new Dictionary<string, int>();
        string RutaUsuario = string.Empty;
        static bool diccionarioOriginalVacio = true;

        public void CifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoOriginal(ArchivoLeido);
            diccionarioCifrado.Clear();
           
        }
        public void DecifrarMensaje(string RutaAchivos, string ArchivoLeido, string clave)
        {
            RutaUsuario = RutaAchivos;
            generarDiccionarioOriginal();
            generarDiccionarioCifrado(clave);
            ObtenerTextoArchivoDecifrado(ArchivoLeido);
            diccionarioCifrado.Clear();
        }
        //se considera que en el Cifrado Cesar unicamente se tienen las letras del alfabeto, mayúsculas y minúsculas, y que no se toman en cuenta las tildes
        public void generarDiccionarioOriginal()
        {
            if (diccionarioOriginalVacio)
            {
                int valorMayusculas = 65;
                int ValorMinusculas = 97;
                int contadorDiccionario = 1;
                //llenando mayusculas
                for (int i = 0; i < 26; i++)
                {
                    diccionarioOriginal.Add(Convert.ToString((char)valorMayusculas), contadorDiccionario);
                    contadorDiccionario++;
                    valorMayusculas++;
                }
                //lenando minusculas
                for (int i = 0; i < 26; i++)
                {
                    diccionarioOriginal.Add(Convert.ToString((char)ValorMinusculas), contadorDiccionario);
                    contadorDiccionario++;
                    ValorMinusculas++;
                }
                //agregando Ñ y ñ
                diccionarioOriginal.Add(Convert.ToString((char)241), contadorDiccionario);
                diccionarioOriginal.Add(Convert.ToString((char)209), contadorDiccionario + 1);
                //se agregaran caracteres que permitan manipular parrafos en el archivo de texto
                //agregando espacio, punto, coma y salto de linea,signos de interrogacion y admiracion
                diccionarioOriginal.Add(Convert.ToString((char)32), contadorDiccionario + 2);
                diccionarioOriginal.Add(Convert.ToString((char)44), contadorDiccionario + 3);
                diccionarioOriginal.Add(Convert.ToString((char)46), contadorDiccionario + 4);
                diccionarioOriginal.Add(Convert.ToString((char)10), contadorDiccionario + 5);
                diccionarioOriginal.Add(Convert.ToString((char)63), contadorDiccionario + 6);
                diccionarioOriginal.Add(Convert.ToString((char)191), contadorDiccionario + 7);
                diccionarioOriginal.Add(Convert.ToString((char)33), contadorDiccionario + 8);
                diccionarioOriginal.Add(Convert.ToString((char)161), contadorDiccionario + 9);
                diccionarioOriginalVacio = false;
            }
            //el alfabeto en mayusculas abarca del codigo ascci 65 al 90, el de minusculas del 97 al 122, la Ñ y ñ son 164 y 165
            
            
        }
        public void generarDiccionarioCifrado(string clave)
        {
            int contadorDiccionario = 1;
            //se añade la clave al diccionario
            if(clave != null)
            {
                foreach (char letra in clave)
                {
                    diccionarioCifrado.Add(Convert.ToString(letra), contadorDiccionario);
                    contadorDiccionario++;
                }
            }
            //se realiza la compracion de las letras del diccionario original que entraran en diferente orden al diccionario cifrado
            foreach (var item in diccionarioOriginal.Keys)
            {
                if (!diccionarioCifrado.ContainsKey(item))
                {
                    diccionarioCifrado.Add(item, contadorDiccionario);
                    contadorDiccionario++;
                }
            }
        }
        public void ObtenerTextoArchivoOriginal(string archivoLeido)
        {
            int bufferLength = 1024;
            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];
                    //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                        CifrarTexto(byteBuffer);
                    }
                }
            }
        }
        public void ObtenerTextoArchivoDecifrado(string archivoLeido)
        {
            int bufferLength = 10000;
            string texto = string.Empty;
            using (var stream = new FileStream(archivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];
                    //el buffer de lectura de archivo se utiliza indirectamente para la escritura del nuevo archivo tambien
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                        
                        DecifrarTexto(byteBuffer);
                    }
                }
            }
        }
        public void CifrarTexto(byte[] byteBuffer)
        {
            var texto = string.Empty;
            foreach (char letra in byteBuffer)
            {
                //se realiza la conversión de los caracteres 
                var receptorValorOriginal = diccionarioOriginal.LastOrDefault(x => x.Key == Convert.ToString(letra)).Value;
                var receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Value == receptorValorOriginal).Key;
                texto += receptorValorCifrado;
            }
            using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoCifradoCesar.cif", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Seek(0, SeekOrigin.End);
                    writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
                }
            }
        }
        private void DecifrarTexto(byte[] byteBuffer)
        {
            var texto = string.Empty;
            foreach (char letra in byteBuffer)
            {
                //se realiza la conversión de los caracteres 
                var receptorValorCifrado = diccionarioCifrado.LastOrDefault(x => x.Key == Convert.ToString(letra)).Value;
                var receptorValorDecifrado = diccionarioOriginal.LastOrDefault(x => x.Value == receptorValorCifrado).Key;
                texto += receptorValorDecifrado;
            }
            using (var writeStream = new FileStream(RutaUsuario + "\\..\\Files\\archivoDecifradoCesar.txt", FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Write(System.Text.Encoding.Unicode.GetBytes(texto));
                }
            }
        }
    }
}