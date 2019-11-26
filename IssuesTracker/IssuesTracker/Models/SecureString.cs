using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IssuesTracker.Models
{
    public class SecureString
    {
        string EncrKey = "password";
        public string Encryptor(string strText)
        {
            byte[] byteKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byteKey = Encoding.UTF8.GetBytes(EncrKey);
            DESCryptoServiceProvider DEScrypto = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, DEScrypto.CreateEncryptor(byteKey, IV), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        public string Decryptor(string stringToDecrypt)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            byte[] byteKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byteKey = Encoding.UTF8.GetBytes(EncrKey);
            DESCryptoServiceProvider DEScrypto = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, DEScrypto.CreateDecryptor(byteKey, IV), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(memoryStream.ToArray());
        }
    }
}