using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoRSA
    {
        public int GenerarLlavePublica(int Phi, int p, int q)
        {
            //lave publica
            var e = 0;
            for (int i =2;i<Phi;i++)
            {
                var encontrado = MaximoComunDivisor(i, p*q,(p-1)*(q-1));
                if (encontrado)
                {
                    e = i;
                    break;
                }
            }
            return e;
        }
        public int GenerarLlavePrivada(int Itable1, int Itable2, int Ivalue1, int Ivalue2, int phi)
        {
            var resultante1 = Itable1 / Ivalue1;
            var rmultiplication1 = Ivalue1 * resultante1;
            var rmultiplication2 = Ivalue2 * resultante1;
            var rresta1 = Itable1 - rmultiplication1;
            var rresta2 = Itable2 - rmultiplication2;
            while(rresta1<1)
            {
                rresta1 = phi + rresta1;
            }
            while(rresta2<1)
            {
                rresta2 = phi + rresta2;
            }
            var d = rresta2;
            d = (rresta1 != 1) ? GenerarLlavePrivada(Ivalue1, Ivalue2, rresta1, rresta2, phi) : d;
            return d;
        }
        //funcion para maximo comun divisor
        private bool MaximoComunDivisor(int i,int phi,int N)
        {
            var encontrado = ((phi % i == 1) && (N % i != 0)&&(i%2!=0)) ? true : false;
            return encontrado;
        }
        public List<byte> LecuraCipherFile(string CipherFile, int bufferLengt ,ref Dictionary<char, int> diccionario)
        {
            var dictionary = new Dictionary<char, int>() { { 'A', 0 }, { 'B', 1 }, { 'C', 2 }, { 'D', 3 }, { 'E', 4 }, { 'F', 5 }, { 'G', 6 }, { 'H', 7 }, { 'I', 8 }, { 'J', 9 }, { 'K', 10 }, { 'L', 11 }, { 'M', 12 }, { 'N', 13 }, { 'O', 14 }, { 'P', 15 }, { 'Q', 16 }, { 'R', 17 }, { 'S', 18 }, { 'T', 19 }, { 'U', 20 }, { 'V', 21 }, { 'W', 22 }, { 'X', 23 }, { 'Y', 24 }, { 'Z', 25 }, { '.', 26 } };
            var BytesList = new List<byte>();
            var character = string.Empty;
            using (var stream = new FileStream(CipherFile, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                            character += (char)bit;
                            character = character.ToUpper();
                            if (dictionary.ContainsKey(character[0]))
                            {
                                BytesList.Add((byte)dictionary[character[0]]);
                            }
                            else
                            {
                                dictionary.Add(character[0], dictionary.Count());
                                BytesList.Add((byte)dictionary[character[0]]);
                            }
                            character = string.Empty;
                        }
                    }
                }
            }
            diccionario = dictionary;
            return BytesList;
        }
        public string LecuraKeyFile(string CipherFile, int bufferLengt)
        {
            var Privatekey = string.Empty;
            using (var stream = new FileStream(CipherFile, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                            if (bit == 44)
                            {
                                Privatekey += (char)bit;
                            }
                            else
                            {
                                Privatekey += bit;
                            }
                        }
                    }
                }
            }
            return Privatekey;
        }
        public string Cifrar(int bit, int Key, int N, ref int Cantidadmax)
        {
            var valorgenerado = 1;
            for (int i = 0; i < Key; i++)
            {
                valorgenerado = valorgenerado * bit % N;
            }
            var binario = Convert.ToString(Convert.ToInt32(valorgenerado), 2);
            Cantidadmax = (Cantidadmax < binario.Count()) ? binario.Count() : Cantidadmax;
            return binario;
        }
        public void EscrituraArchivoCifrado(List<byte> ByteList, string RutaArchivos, string ArchivoNombre, Dictionary<char, int> diccionario)
        {
            var ListaElementosDiccionario = new List<byte>();
            using (var writeStream = new FileStream(RutaArchivos + "\\..\\Files\\" + ArchivoNombre + ".dic", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    foreach (var elemento in diccionario)
                    {
                        ListaElementosDiccionario.Add(Convert.ToByte(elemento.Key));
                        ListaElementosDiccionario.Add(Convert.ToByte('|'));
                        ListaElementosDiccionario.Add(Convert.ToByte(elemento.Value));
                    }
                    byte[] byteBuffer = new byte[ListaElementosDiccionario.Count()];
                    for (int i = 0; i < ListaElementosDiccionario.Count(); i++)
                    {
                        byteBuffer[i] = ListaElementosDiccionario[i];
                    }
                    writer.Write(byteBuffer);
                }
            }
            using (var writeStream = new FileStream(RutaArchivos + "\\..\\Files\\" + ArchivoNombre+ ".rsacif", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    var ByteBuffer = new byte[ByteList.Count()];
                    for (var i = 0; i < ByteList.Count(); i++)
                    {
                        ByteBuffer[i] = ByteList[i];
                    }
                    writer.Write(ByteBuffer);
                }
            }
        }
        public List<byte> LecuraCipherFileDecifrado(string CipherFile, int bufferLengt, ref int MaxValue, ref Dictionary<int, char> diccionario, string NombreArchivo, string RutaArchivos)
        {
            using (var stream = new FileStream(RutaArchivos + "\\..\\Files\\" + NombreArchivo + ".dic", FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    byte[] byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        for (int i = 0; i < byteBuffer.Count(); i++)
                        {
                            if (byteBuffer[i] == 124)
                            {
                                diccionario.Add((int)byteBuffer[i + 1], (char)byteBuffer[i - 1]);
                            }
                        }
                    }
                }
            }
            var BytesList = new List<byte>();
            using (var stream = new FileStream(CipherFile, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                            MaxValue = (MaxValue == 0) ? Convert.ToInt32(bit) : MaxValue;
                            BytesList.Add(bit);
                        }
                    }
                }
            }
            return BytesList;
        }
        public byte Decifrar(int value, int Key, int N, Dictionary<int, char> diccionario)
        {
            var valorgenerado = 1;
            for (var i = 0; i < Key; i++)
            {
                valorgenerado = value * valorgenerado % N;
            }
            var Byte = Convert.ToByte(diccionario[valorgenerado]);
            return Byte;
        }
        public void EscrituraArchivoDecifrado(List<byte> ByteList, string RutaArchivos, string ArchivoNombre)
        {
            using (var writeStream = new FileStream(RutaArchivos + "\\..\\Files\\" + ArchivoNombre + ".txt", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    var ByteBuffer = new byte[ByteList.Count()];
                    for (var i = 0; i < ByteList.Count(); i++)
                    {
                        ByteBuffer[i] = ByteList[i];
                    }
                    writer.Write(ByteBuffer);
                }
            }
        }
    }
}
