using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoRSA
    {
        public void GenerarLlaves(int p, int q)
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
            EscribirArchivoClave(e, n, "public");
        }
        public void EscribirArchivoClave(int k, int n, string nombre)
        {
            var ByteBuffer = k + "," + n;
            var ruta = string.Empty;
            using (var writeStream = new FileStream(ruta + "\\..\\Files\\" + nombre + ".key", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Write(ByteBuffer);
                }
            }
        }
        //funcion para maximo comun divisor
        private int MaximoComunDivisor(int n1, int n2)
        {
            int temp;
            bool encontrado = false;
            while (!encontrado)
            {
                temp = n1 % n2;
                if(temp == 0)
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
    }
}
