﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoRSA
    {
        public int GenerarLlavePublica(int p, int q, ref int Phi)
        {
            //lave publica
            var n = p * q;
            var phi = (p - 1) * (q - 1);
            var e = 2;
            bool encontrado = false;
            while (e < phi && !encontrado)
            {
                if (MaximoComunDivisor(e, phi) == 1 && e < phi && e != p)
                {
                    encontrado = true;
                }
                else
                    e++;
            }
            Phi = phi;
            return e;
        }
        public int GenerarLlavePrivada(int Itable1, int Itable2, int Ivalue1, int Ivalue2, int phi)
        {
            var resultante1 = Itable1 / Ivalue1;
            var rmultiplication1 = Ivalue1 * resultante1;
            var rmultiplication2 = Ivalue2 * resultante1;
            var rresta1 = Itable1 - rmultiplication1;
            var rresta2 = Itable2 - rmultiplication2;
            rresta1 = (rresta1 < 0) ? phi + rresta1 : rresta1;
            rresta2 = (rresta2 < 0) ? phi + rresta2 : rresta2;
            var d = rresta2;
            d = (rresta1 != 1) ? GenerarLlavePrivada(Ivalue1, Ivalue2, rresta1, rresta2, phi) : d;
            return d;
        }
        //funcion para maximo comun divisor
        private int MaximoComunDivisor(int n1, int n2)
        {
            int temp;
            bool encontrado = false;
            while (!encontrado)
            {
                temp = n1 % n2;
                if (temp == 0)
                {
                    encontrado = true;
                }
                if (!encontrado)
                {
                    n1 = n2;
                    n2 = temp;
                }

            }
            return n2;
        }

        public List<byte> LecuraCipherFile(string CipherFile, int bufferLengt)
        {
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
                           BytesList.Add(bit);
                        }
                    }
                }
            }
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
                            Privatekey += (char)bit;
                        }
                    }
                }
            }
            return Privatekey;
        }
        public string Cifrar(int bit, int Key, int phi, int Cantidadmax)
        {
            var valorgenerado = Math.Pow(bit, Key) % phi;
            var binario = Convert.ToString(Convert.ToInt32(valorgenerado), 2);
            binario = binario.PadLeft(Cantidadmax, '0');
            return binario;
        }
    }
}
