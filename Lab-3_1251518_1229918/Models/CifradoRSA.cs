using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoRSA
    {
        public int GenerarLlavePublica(int p, int q)
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
            return e;
        }
        public int GenerarLlavePrivada()
        {
            var d = 0;
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
