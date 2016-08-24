using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Common.Units
{
    /// <summary>
    /// 加解密
    /// </summary>
    public class Encrypt
    {
        private string key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
        private SymmetricAlgorithm symmeAlgori = new RijndaelManaged();

        public string Decrypt(string Source)
        {
            byte[] buffer = Convert.FromBase64String(Source);
            MemoryStream stream = new MemoryStream(buffer, 0, buffer.Length);
            this.symmeAlgori.Key = this.GetLegalKey();
            this.symmeAlgori.IV = this.GetLegalIV();
            ICryptoTransform transform = this.symmeAlgori.CreateDecryptor();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }

        //public string Encrypt(string Source)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(Source);
        //    MemoryStream stream = new MemoryStream();
        //    this.symmeAlgori.Key = this.GetLegalKey();
        //    this.symmeAlgori.IV = this.GetLegalIV();
        //    ICryptoTransform transform = this.symmeAlgori.CreateEncryptor();
        //    CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
        //    stream2.Write(bytes, 0, bytes.Length);
        //    stream2.FlushFinalBlock();
        //    stream.Close();
        //    return Convert.ToBase64String(stream.ToArray());
        //}

        private byte[] GetLegalIV()
        {
            string s = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            this.symmeAlgori.GenerateIV();
            int length = this.symmeAlgori.IV.Length;
            if (s.Length > length)
            {
                s = s.Substring(0, length);
            }
            else if (s.Length < length)
            {
                s = s.PadRight(length, ' ');
            }
            return Encoding.ASCII.GetBytes(s);
        }

        private byte[] GetLegalKey()
        {
            string key = this.key;
            this.symmeAlgori.GenerateKey();
            int length = this.symmeAlgori.Key.Length;
            if (key.Length > length)
            {
                key = key.Substring(0, length);
            }
            else if (key.Length < length)
            {
                key = key.PadRight(length, ' ');
            }
            return Encoding.ASCII.GetBytes(key);
        }

        public static string SHA256(string oldstr)
        {
            byte[] bytes = Encoding.Default.GetBytes(oldstr);
            bytes = new SHA256CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytes)
            {
                builder.AppendFormat("{0:x2}", num);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Des 解密 GB2312
        /// </summary>
        /// <param name="str">Desc string</param>
        /// <param name="key">Key ,必须为8位 </param>
        /// <returns></returns>
        public static string Decode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < (str.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encode(string str, string key)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        }
    }
}
