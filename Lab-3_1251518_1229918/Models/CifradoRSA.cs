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
            while (e < phi)
            {
                if (MaximoComunDivisor(e, phi) == 1)
                    break;
                else
                    e++;
            }
        }
        //funcion para maximo comun divisor
        private int MaximoComunDivisor(int n1, int n2)
        {
            while (n1 != 0 && n2 != 0)
            {
                if (n1 > n2)
                {
                    n1 %= n2;
                }
                else
                    n2 %= n1;
            }

            return n1 == 0 ? n2 : n1;
        }
    }
}
