using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace MD5CS
{
    public class MD5Sharp
    {
        string message { get; set; }
        byte[] buffer { get; set; }
        UInt64 size { get; set; }
        UInt32 A { get; set; }
        UInt32 B { get; set; }
        UInt32 C { get; set; }
        UInt32 D { get; set; }

        UInt32[] T = new UInt32[64]; //64-элементная таблица данных (констант).

        // Функции для каждого раунда 
        uint F(uint X, uint Y, uint Z) { return ((X & Y) | ((~X) & Z)); }
        uint G(uint X, uint Y, uint Z) { return (X & Z) | (Y & (~Z)); }
        uint H(uint X, uint Y, uint Z) { return X ^ Y ^ Z; }
        uint I(uint X, uint Y, uint Z) { return Y ^ (X | (~Z)); }
        uint RotateLeft(uint value, int shift) { return value << shift | value >> (32 - shift); }

        public MD5Sharp(string str)
        {
            message = str;
            buffer = new byte[str.Length]; 
            buffer = Encoding.ASCII.GetBytes(str);
            size = (UInt64)buffer.Length * 8;            
        }

        public string calcMD5()
        {
            ApeendPaddingBytes();
            AppendLength();
            InitMDbuf();
            ProcessMsgInt16WordBlocks(ByteToIntArray(buffer));

            return rawMD5ToHEX(A) + rawMD5ToHEX(B) + rawMD5ToHEX(C) + rawMD5ToHEX(D);
        }

        private void ApeendPaddingBytes() // Выравнивание потока
        {
            buffer = buffer.Append<byte>(128).ToArray();
            while (buffer.Length % 64 != 56)
            {
                buffer = buffer.Append<byte>(0).ToArray();
            }
        }

        private void AppendLength() // Добавление длины сообщения
        {
            byte[] buf = new byte[buffer.Length + 8];
            for(int i = 0; i < buffer.Length; i++)
            {
                buf [i]= buffer[i];
            }
            for (int i = 0; i < 8; i++) //последние 8 байт
            {
                buf[buf.Length - 8 + i] = (byte)(size >> i * 8); //заполняем 64-битным представлением длины данных до выравнивания
            }
            buffer = buf;
        }            

        // Инициализация буфера
        private void InitMDbuf() 
        {
            A = 0x67452301;
            B = 0xefcdab89;
            C = 0x98badcfe;
            D = 0x10325476;

            // Вычисление таблицы констант
            for (int i = 0; i < 64; i++)
                T[i] = (uint)(Math.Pow(2, 32) * Math.Abs(Math.Sin(i + 1)));
        }

        private UInt32[] ByteToIntArray(byte[] buf)
        {
            UInt32[] res = new UInt32[buf.Length/4];
            for (int i = 0; i < buf.Length; i += 4)
            {
                res[i / 4] += (UInt32)(buf[i + 0]) << 0;
                res[i / 4] += (UInt32)(buf[i + 1]) << 8;
                res[i / 4] += (UInt32)(buf[i + 2]) << 16;
                res[i / 4] += (UInt32)(buf[i + 3]) << 24;
            }
            return res;
        }

        // Перевод в 16ю СС
        private string rawMD5ToHEX(UInt32 value)
        {
            string res = "";
            for (int i = 0; i < 4; i++)
            {
                res += Convert.ToString(value % 256, 16);
                value /= 256;
            }
            return res;
        }     
       

        // Вычисления в цикле
        private void ProcessMsgInt16WordBlocks(UInt32[] buf)
        {
            for (int n = 0; n < buf.Length; n += 16)
            {
                UInt32 AA = A;
                UInt32 BB = B;
                UInt32 CC = C;
                UInt32 DD = D;
                // Round 1
                A = B + RotateLeft((A + F(B, C, D) + buf[n + 0] + T[0]), 7);
                D = A + RotateLeft((D + F(A, B, C) + buf[n + 1] + T[1]), 12);
                C = D + RotateLeft((C + F(D, A, B) + buf[n + 2] + T[2]), 17);
                B = C + RotateLeft((B + F(C, D, A) + buf[n + 3] + T[3]), 22);


                A = B + RotateLeft((A + F(B, C, D) + buf[n + 4] + T[4]), 7);
                D = A + RotateLeft((D + F(A, B, C) + buf[n + 5] + T[5]), 12);
                C = D + RotateLeft((C + F(D, A, B) + buf[n + 6] + T[6]), 17);
                B = C + RotateLeft((B + F(C, D, A) + buf[n + 7] + T[7]), 22);


                A = B + RotateLeft((A + F(B, C, D) + buf[n + 8] + T[8]), 7);
                D = A + RotateLeft((D + F(A, B, C) + buf[n + 9] + T[9]), 12);
                C = D + RotateLeft((C + F(D, A, B) + buf[n + 10] + T[10]), 17);
                B = C + RotateLeft((B + F(C, D, A) + buf[n + 11] + T[11]), 22);


                A = B + RotateLeft((A + F(B, C, D) + buf[n + 12] + T[12]), 7);
                D = A + RotateLeft((D + F(A, B, C) + buf[n + 13] + T[13]), 12);
                C = D + RotateLeft((C + F(D, A, B) + buf[n + 14] + T[14]), 17);
                B = C + RotateLeft((B + F(C, D, A) + buf[n + 15] + T[15]), 22);

                // Round 2
                A = B + RotateLeft((A + G(B, C, D) + buf[n + 1] + T[16]), 5);
                D = A + RotateLeft((D + G(A, B, C) + buf[n + 6] + T[17]), 9);
                C = D + RotateLeft((C + G(D, A, B) + buf[n + 11] + T[18]), 14);
                B = C + RotateLeft((B + G(C, D, A) + buf[n + 0] + T[19]), 20);

                A = B + RotateLeft((A + G(B, C, D) + buf[n + 5] + T[20]), 5);
                D = A + RotateLeft((D + G(A, B, C) + buf[n + 10] + T[21]), 9);
                C = D + RotateLeft((C + G(D, A, B) + buf[n + 15] + T[22]), 14);
                B = C + RotateLeft((B + G(C, D, A) + buf[n + 4] + T[23]), 20);

                A = B + RotateLeft((A + G(B, C, D) + buf[n + 9] + T[24]), 5);
                D = A + RotateLeft((D + G(A, B, C) + buf[n + 14] + T[25]), 9);
                C = D + RotateLeft((C + G(D, A, B) + buf[n + 3] + T[26]), 14);
                B = C + RotateLeft((B + G(C, D, A) + buf[n + 8] + T[27]), 20);

                A = B + RotateLeft((A + G(B, C, D) + buf[n + 13] + T[28]), 5);
                D = A + RotateLeft((D + G(A, B, C) + buf[n + 2] + T[29]), 9);
                C = D + RotateLeft((C + G(D, A, B) + buf[n + 7] + T[30]), 14);
                B = C + RotateLeft((B + G(C, D, A) + buf[n + 12] + T[31]), 20);

                // Round 3

                A = B + RotateLeft((A + H(B, C, D) + buf[n + 5] + T[32]), 4);
                D = A + RotateLeft((D + H(A, B, C) + buf[n + 8] + T[33]), 11);
                C = D + RotateLeft((C + H(D, A, B) + buf[n + 11] + T[34]), 16);
                B = C + RotateLeft((B + H(C, D, A) + buf[n + 14] + T[35]), 23);

                A = B + RotateLeft((A + H(B, C, D) + buf[n + 1] + T[36]), 4);
                D = A + RotateLeft((D + H(A, B, C) + buf[n + 4] + T[37]), 11);
                C = D + RotateLeft((C + H(D, A, B) + buf[n + 7] + T[38]), 16);
                B = C + RotateLeft((B + H(C, D, A) + buf[n + 10] + T[39]), 23);

                A = B + RotateLeft((A + H(B, C, D) + buf[n + 13] + T[40]), 4);
                D = A + RotateLeft((D + H(A, B, C) + buf[n + 0] + T[41]), 11);
                C = D + RotateLeft((C + H(D, A, B) + buf[n + 3] + T[42]), 16);
                B = C + RotateLeft((B + H(C, D, A) + buf[n + 6] + T[43]), 23);

                A = B + RotateLeft((A + H(B, C, D) + buf[n + 9] + T[44]), 4);
                D = A + RotateLeft((D + H(A, B, C) + buf[n + 12] + T[45]), 11);
                C = D + RotateLeft((C + H(D, A, B) + buf[n + 15] + T[46]), 16);
                B = C + RotateLeft((B + H(C, D, A) + buf[n + 2] + T[47]), 23);

                // Round 4

                A = B + RotateLeft((A + I(B, C, D) + buf[n + 0] + T[48]), 6);
                D = A + RotateLeft((D + I(A, B, C) + buf[n + 7] + T[49]), 10);
                C = D + RotateLeft((C + I(D, A, B) + buf[n + 14] + T[50]), 15);
                B = C + RotateLeft((B + I(C, D, A) + buf[n + 5] + T[51]), 21);


                A = B + RotateLeft((A + I(B, C, D) + buf[n + 12] + T[52]), 6);
                D = A + RotateLeft((D + I(A, B, C) + buf[n + 3] + T[53]), 10);
                C = D + RotateLeft((C + I(D, A, B) + buf[n + 10] + T[54]), 15);
                B = C + RotateLeft((B + I(C, D, A) + buf[n + 1] + T[55]), 21);

                A = B + RotateLeft((A + I(B, C, D) + buf[n + 8] + T[56]), 6);
                D = A + RotateLeft((D + I(A, B, C) + buf[n + 15] + T[57]), 10);
                C = D + RotateLeft((C + I(D, A, B) + buf[n + 6] + T[58]), 15);
                B = C + RotateLeft((B + I(C, D, A) + buf[n + 13] + T[59]), 21);

                A = B + RotateLeft((A + I(B, C, D) + buf[n + 4] + T[60]), 6);
                D = A + RotateLeft((D + I(A, B, C) + buf[n + 11] + T[61]), 10);
                C = D + RotateLeft((C + I(D, A, B) + buf[n + 2] + T[62]), 15);
                B = C + RotateLeft((B + I(C, D, A) + buf[n + 9] + T[63]), 21);

                A += AA;
                B += BB;
                C += CC;
                D += DD;
            }
        }
    }   
}
