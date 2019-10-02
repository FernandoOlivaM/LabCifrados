using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab_3_1251518_1229918.Models
{
    public class CifradoZigZag
    {
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
        public string DecifrarMensaje(string ArchivoLeido, int niveles, int bufferLengt)
        {
            int n = niveles - 2;
            List<byte> BytesList = new List<byte>();
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
            var m =(BytesList.Count()+2*n+1)/(2+2*n);
            
            return ArchivoLeido;
        }
    }
}