using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoZigZag
    {
        //métodos y funciones para el cifrado
        public List<byte> LecturaCifrado(string ArchivoLeido, int bufferLengt)
        {
            var BytesList = new List<byte>();
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
                            BytesList.Add(bit);
                        }
                    }
                }
            }
            return BytesList;
        }
        public byte[,] MatrixCreation(int conteo, int niveles, ref int CantidadCaracterExtra)
        {
            CantidadCaracterExtra = conteo;
            var CharacterArray = new byte[niveles, conteo];
            var retornar = false;
            var i = 0;
            for (int j = 0; j < conteo; j++)
            {
                if (!retornar)
                {
                    CharacterArray[i, j] = Convert.ToByte('_');
                    i++;
                    if (i == niveles)
                    {
                        retornar = true;
                        i -= 2;
                    }
                }
                else
                {
                    CharacterArray[i, j] = Convert.ToByte('_');
                    i--;
                    if (i < 0)
                    {
                        retornar = false;
                        i += 2;
                    }
                }
            }
            if (CharacterArray[0,conteo-1] != Convert.ToByte('_'))
            {
                CharacterArray = MatrixCreation(conteo+1, niveles, ref CantidadCaracterExtra);
            }
            else
            {
                return CharacterArray;
            }
            return CharacterArray;
        }
        public List<byte> AgregarCaracterExtra(List<byte> BytesList, int conteo, ref byte CaracterExtra)
        {
            bool encontrado = false;
            var i = 1;
            while (!encontrado)
            {
                if (!BytesList.Contains(Convert.ToByte(i)))
                {
                    encontrado = true;
                }
                else
                {
                    i++;
                }
            }
            CaracterExtra = Convert.ToByte(i);
            while (BytesList.Count()!=conteo)
            {
                BytesList.Add(Convert.ToByte(i));
            }
            return BytesList;
        }
        public void CifrarMensaje(byte[,] CharacterArray, int niveles, string RutaArchivos, List<byte> BytesList, byte CaracterExtra)
        {
            //proceso para introducir los bytes a la matiz
            var listposition = 0;
            var retornar = false;
            var i = 0;
            for (int j = 0; j < BytesList.Count(); j++)
            {
                if (!retornar)
                {
                    CharacterArray[i, j] = BytesList[listposition];
                    listposition++;
                    i++;
                    if (i == niveles)
                    {
                        retornar = true;
                        i -= 2;
                    }
                }
                else
                {
                    CharacterArray[i, j] = BytesList[listposition];
                    listposition++;
                    i--;
                    if (i < 0)
                    {
                        retornar = false;
                        i += 2;
                    }
                }
            }
            //proceso para obtener los bytes cifrados
            var ByteBuffer = new byte[BytesList.Count()];
            var bufferposition = 0;
            for (i = 0; i < niveles; i++)
            {
                for (int j = 0; j < BytesList.Count(); j++)
                {
                    if (CharacterArray[i, j] != 0)
                    {
                        ByteBuffer[bufferposition] = CharacterArray[i, j];
                        bufferposition++;
                    }
                }
            }
            using (var writeStream = new FileStream(RutaArchivos + "\\..\\Files\\ArchivoCifradoZigZag.cif", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Write(CaracterExtra);
                    writer.Seek(0,SeekOrigin.End);
                    writer.Write(ByteBuffer);
                }
            }
        }
        //métodos y funciones para el descifrado
        public List<byte> LecturaDescifrado(string ArchivoLeido, int bufferLengt, ref byte CaracterExtra)
        {
            var BytesList = new List<byte>();
            using (var stream = new FileStream(ArchivoLeido, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var contador = 0;
                    var byteBuffer = new byte[bufferLengt];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLengt);
                        foreach (byte bit in byteBuffer)
                        {
                            byteBuffer = reader.ReadBytes(bufferLengt);
                            if (contador == 0)
                            {
                                CaracterExtra = bit;
                                contador++;
                            }
                            else
                            {
                                BytesList.Add(bit);
                            }
                        }
                    }
                }
            }
            return BytesList;
        }
        public byte[,] MatrixCreationDecryption(int conteo, int niveles, ref int CantidadCaracterExtra)
        {
            CantidadCaracterExtra = conteo;
            var CharacterArray = new byte[niveles, conteo];
            var retornar = false;
            var i = 0;
            for (int j = 0; j < conteo; j++)
            {
                if (!retornar)
                {
                    CharacterArray[i, j] = Convert.ToByte('_');
                    i++;
                    if (i == niveles)
                    {
                        retornar = true;
                        i -= 2;
                    }
                }
                else
                {
                    CharacterArray[i, j] = Convert.ToByte('_');
                    i--;
                    if (i < 0)
                    {
                        retornar = false;
                        i += 2;
                    }
                }
            }
            if (CharacterArray[0, conteo - 1] != Convert.ToByte('_'))
            {
                CharacterArray = MatrixCreationDecryption(conteo + 1, niveles, ref CantidadCaracterExtra);
            }
            else
            {
                return CharacterArray;
            }
            return CharacterArray;
        }
        public void DecifrarMensaje(string RutaArchivos, int niveles, List<byte> BytesList, byte[,] Matrix, byte CaracterExtra)
        {
            var n = niveles - 2;
            var m = (BytesList.Count() + 2 * n + 1) / (2 + 2 * n);
            var CaracteresInferiores = m - 1;
            var CaracteresCentrales = 2 * (m - 1);
            var Position = 0;
            //Introducir los caracteres superiores
            for(int i=0;i<BytesList.Count();i++)
            { 
                if (Matrix[0, i] == Convert.ToByte('_'))
                {
                    Matrix[0, i] = BytesList[Position];
                    Position++;
                }
            }
            //Introducir los caracteres inferiores
            var CaracteresFinales = string.Empty;
            var j = 0;
            var Contador = BytesList.Count() - 1;
            while (j != CaracteresInferiores)
            {
                CaracteresFinales = (char)BytesList[Contador]+CaracteresFinales;
                Contador--;
                j++;
            }
            var Pozition=0;
            for(int i = 0; i < BytesList.Count(); i++)
            {
                if (Matrix[niveles - 1, i] == Convert.ToByte('_'))
                {
                    Matrix[niveles - 1, i] = (byte)CaracteresFinales[Pozition];
                    Pozition++;
                }
            }
            //Ingresar los caracteres centrales
            for(int i = 1; i < niveles - 1; i++)
            {
                for(int x = 0;x< BytesList.Count(); x++)
                {
                    if ((Matrix[i, x] == Convert.ToByte('_')))
                    {
                        Matrix[i, x] = BytesList[Position];
                        Position++;
                    }
                }
            }
            //Recorrer la matríz para obtener los caracteress
            var retornar = false;
            var q = 0;
            //var texto = string.Empty;
            byte[] buffer = new byte[BytesList.Count()];
            var Colocación = 0;
            for (int y = 0; y < BytesList.Count(); y++)
            {
                if (!retornar)
                {
                    if (Matrix[q, y] != CaracterExtra)
                    {
                        buffer[Colocación] = Matrix[q, y];
                        Colocación++;
                    }
                    q++;
                    if (q == niveles)
                    {
                        retornar = true;
                        q -= 2;
                    }
                }
                else
                {
                    if (Matrix[q, y] != CaracterExtra)
                    {
                        buffer[Colocación] =Matrix[q, y];
                        Colocación++;
                    }
                    q--;
                    if (q < 0)
                    {
                        retornar = false;
                        q += 2;
                    }
                }
            }
            using (var writeStream = new FileStream(RutaArchivos + "\\..\\Files\\ArchivoDescifradoZigZag.cif", FileMode.Create))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    writer.Write(buffer);
                }
            }
        }
    }
}