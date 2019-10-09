using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoSDES
    {
        public bool GenerarPermutaciones(int bufferLengt, ref string P10, ref string P8, ref string P4, ref string EP, ref string IP)
        {
            var BytesList = new List<byte>();
            using (var stream = new FileStream("C:\\Users\\mache\\Documents\\Segundo año\\Lab Esctructuras II\\Lab 2\\Segunda Fase\\Base\\LabCifrados\\Lab-3_1251518_1229918\\Files\\Permutaciones.txt", FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                            if (Convert.ToInt32((char)bit) > -1)
                            {
                                BytesList.Add(bit);
                            }
                        }
                    }
                }
            }
            var NoEsValido = false;
            var receptor = string.Empty;
            if (BytesList.Count() > 37)
            {
                for (var i = 0; i < BytesList.Count(); i++)
                {
                    receptor += (char)BytesList[i];
                    if (i == 9)
                    {
                        P10 = receptor;
                        NoEsValido = EncontrarRepetidos(P10);
                        if (NoEsValido == true)
                        {
                            break;
                        }
                        receptor = string.Empty;
                    }
                    if (i == 17)
                    {
                        P8 = receptor;
                        NoEsValido = EncontrarRepetidos(P8);
                        if (NoEsValido == true)
                        {
                            break;
                        }
                        receptor = string.Empty;
                    }
                    if (i == 21)
                    {
                        P4 = receptor;
                        NoEsValido = EncontrarRepetidos(P4);
                        if (NoEsValido == true)
                        {
                            break;
                        }
                        receptor = string.Empty;
                    }
                    if (i == 29)
                    {
                        EP = receptor;
                        NoEsValido = EncontrarRepetidos(EP.Substring(0, 4));
                        if (NoEsValido == true)
                        {
                            break;
                        }
                        else
                        {
                            NoEsValido = EncontrarRepetidos(EP.Substring(4));
                            if (NoEsValido == true)
                            {
                                break;
                            }
                            else
                            {
                                receptor = string.Empty;
                            }
                        }
                        receptor = string.Empty;
                    }
                    if (i == 37)
                    {
                        IP = receptor;
                        NoEsValido = EncontrarRepetidos(IP);
                        if (NoEsValido == true)
                        {
                            break;
                        }
                        receptor = string.Empty;
                        break;
                    }
                }
            }
            else
            {
                NoEsValido = true;
            }
            return NoEsValido;
        }
        private bool EncontrarRepetidos(string permutacion)
        {
            bool Repetido = false;
            var caracter = ' ';
            for (var i = 0; i < permutacion.Count(); i++)
            {
                var contador = 0;
                caracter = permutacion[i];
                for (var j = 0; j < permutacion.Count(); j++)
                {
                    if (caracter == permutacion[j])
                    {
                        contador++;
                    }
                    if(contador>1)
                    {
                        Repetido = true;
                        break;
                    }
                }
            }
            return Repetido;
        }
        public string GenerarK1(string Key, string P10, string P8, ref string resultanteLS1)
        {
            var resultanteP10 = Permutation10(Key,P10);
            resultanteLS1 = LeftShift1(resultanteP10);
            var K1 = Permutation8(resultanteLS1, P8);
            return K1;
        }
        public string GenerarK2(string resultanteLS1,string P8)
        {
            var resultanteLS2 = LeftShif2(resultanteLS1);
            var K2 = Permutation8(resultanteLS2, P8);
            return K2;
        }
        private string Permutation10(string Key, string P10)
        {
            var chain = string.Empty;
            var resultanteP10 = string.Empty;
            foreach (char caracter in P10)
            {
                chain += caracter;
                resultanteP10 += Key[Convert.ToInt32(chain)];
                chain = string.Empty;
            }
            return resultanteP10;
        }
        private string LeftShift1(string resultanteP10)
        {
            var resultanteLS1 = string.Empty;
            var LS1 = resultanteP10.Substring(0, 5);
            var LS2 = resultanteP10.Substring(5);
            //LS al primer substring
            var resultadoLS1=string.Empty;
            for(var i = 1; i < LS1.Count(); i++)
            {
                resultadoLS1 += LS1[i];
            }
            resultadoLS1 += LS1[0];
            //LS al segundo substring
            var resultadoLS2 = string.Empty;
            for (var i = 1; i < LS2.Count(); i++)
            {
                resultadoLS2 += LS2[i];
            }
            resultadoLS2 += LS2[0];
            resultanteLS1 = resultadoLS1 + resultadoLS2;
            return resultanteLS1;
        }
        private string Permutation8(string resultanteLS1, string P8)
        {
            var chain = string.Empty;
            var resultanteP8 = string.Empty;
            foreach(char caracter in P8)
            {
                chain += caracter;
                resultanteP8 += resultanteLS1[Convert.ToInt32(chain)];
                chain = string.Empty;
            }
            return resultanteP8;
        }
        private string LeftShif2(string resultanteLS1)
        {
            var LS1 = resultanteLS1.Substring(0, 5);
            var LS2 = resultanteLS1.Substring(5);
            //LS al primer substring
            var resultadoLS1 = string.Empty;
            for (var i = 2; i < LS1.Count(); i++)
            {
                resultadoLS1 += LS1[i];
            }
            for (var i = 0; i < 2; i++)
            {
                resultadoLS1 += LS1[i];
            }
            //LS al segundo substring
            var resultadoLS2 = string.Empty;
            for (var i = 2; i < LS2.Count(); i++)
            {
                resultadoLS2 += LS2[i];
            }
            for (var i = 0; i < 2; i++)
            {
                resultadoLS2 += LS2[i];
            }
            var resultanteLS2 = resultadoLS1 + resultadoLS2;
            return resultanteLS2;
        }
    }
}