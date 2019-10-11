using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoSDES
    {
        //Generar Llaves
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
                    if (contador > 1)
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
            var resultanteP10 = Permutation10(Key, P10);
            resultanteLS1 = LeftShift1(resultanteP10);
            var K1 = Permutation8(resultanteLS1, P8);
            return K1;
        }
        public string GenerarK2(string resultanteLS1, string P8)
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
            var resultadoLS1 = string.Empty;
            for (var i = 1; i < LS1.Count(); i++)
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
            foreach (char caracter in P8)
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
        //Cifrar
        public List<string> LecturaArchivo(string ArchivoLeido,int bufferLengt)
        {
            var BytesList = new List<string>();
            using (var stream = new FileStream(ArchivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                           var binary = Convert.ToString(bit, 2);
                           binary = binary.PadLeft(8, '0');
                           BytesList.Add(binary);
                        }
                    }
                }
            }
            return BytesList;
        }
        public void Cifrar(string binary,string IP,string EP,string K1,string P4)
        {
            var resultanteIP1 = string.Empty;
            var resultanteIP2 = InitialPermutation(binary, IP,ref resultanteIP1);
            var resultanteEP = ExpandAndPermute(resultanteIP2,EP);
            var resultanteXOR = XOR(resultanteEP,K1);
            var S1 = resultanteXOR.Substring(0, 4);
            var S2 = resultanteXOR.Substring(4);
            var Sboxes = SBoxes(S1,S2);
            var resultanteP4 = Permutation4(Sboxes,P4);
            var resultanteXOR2 = XOR(resultanteP4,resultanteIP1);
            var fusion = resultanteXOR2 + resultanteIP2;
        }
        private string InitialPermutation(string binary,string IP,ref string resultanteIP1)
        {
            var chain = string.Empty;
            var resultanteIP2 = string.Empty;
            foreach(char caracter in IP)
            {
                chain += caracter;
                resultanteIP2 += binary[Convert.ToInt32(chain)];
                chain = string.Empty;
            }
            resultanteIP1 = resultanteIP2.Substring(0, 4);
            resultanteIP2 = resultanteIP2.Substring(4);
            return resultanteIP2;
        }
        private string ExpandAndPermute(string resultanteIP, string EP)
        {
            var chain = string.Empty;
            var resultanteEP = string.Empty;
            foreach (char caracter in EP)
            {
                chain += caracter;
                resultanteEP += resultanteIP[Convert.ToInt32(chain)];
                chain = string.Empty;
            }
            return resultanteEP;
        }
        private string XOR(string resultante, string clave)
        {
            var resultanteXOR = string.Empty;
            for(var i =0;i<resultante.Count();i++)
            {
                if (resultante[i] == clave[i])
                {
                    resultanteXOR += 0;
                }
                else
                {
                    resultanteXOR += 1;
                }
            }
            return resultanteXOR;
        }
        private string SBoxes(string S1, string S2)
        {
            string[,] matrizS0 = { { "01", "00","11","10" },{ "11", "10", "01", "00" },{ "00", "10", "01", "11" },{ "11", "01", "11", "10" } };
            string[,] matrizS1 = { { "00", "01", "10", "11" }, { "10", "00", "01", "11" }, { "11", "00", "01", "00" }, { "10", "01", "00", "11" } };
            //valores S1
            var FS0 = string.Empty;
            FS0 += S1[0];
            FS0 += S1[3];
            var F0 = Convert.ToInt32(FS0, 2);
            var CS0 = string.Empty; 
            CS0 += S1[1];
            CS0 += S1[2];
            var C0 = Convert.ToInt32(CS0, 2);
            //valores S2
            var FS1 = string.Empty;
            FS1 += S2[0];
            FS1 += S2[3];
            var F1 = Convert.ToInt32(FS1, 2);
            var CS1 = string.Empty;
            CS1 += S1[1];
            CS1 += S2[2];
            var C1 = Convert.ToInt32(CS1, 2);
            var Sboxes = matrizS0[F0,C0]+matrizS1[F1,C1];
            return Sboxes;
        }
        private string Permutation4(string Sboxes,string P4)
        {
            var chain = string.Empty;
            var resultanteP4 = string.Empty;
            foreach (char caracter in P4)
            {
                chain += caracter;
                resultanteP4 += Sboxes[Convert.ToInt32(chain)];
                chain = string.Empty;
            }
            return resultanteP4;
        }
    }
}