using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 加密安全帮助类
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// MD5加密字符串 (不可逆)
        /// </summary>
        /// <param name="text">需要加密的内容</param>
        /// <param name="key">加密密钥，防止简单密码被破解</param>
        /// <param name="binaryStyle">加密后结果内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>加密结果</returns>
        public static string EncryptByMD5(string text, string key = default(string), bool binaryStyle = true)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(text + key));
            return binaryStyle ? BitConverter.ToString(bytes) : Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// DES加密字符串 (可逆)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <param name="key">DES加密的私钥，必须是字节长度8位的字符串，否则会补0。</param>
        /// <param name="iv">DES加密偏移量，必须是>=8位长的字符串，否则会补0。</param>
        /// <param name="binaryStyle">加密后结果内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptByDES(string text, string key = "", string iv = "", bool binaryStyle = true)
        {
            List<byte> bKey = Encoding.UTF8.GetBytes(key).ToList();
            for (int i = bKey.Count; i < 8; i++)
            {
                bKey.Add((byte)i);
            }
            bKey = bKey.GetRange(0, 8);
            List<byte> bIV = Encoding.UTF8.GetBytes(iv).ToList();
            for (int i = bIV.Count; i < 8; i++)
            {
                bIV.Add((byte)i);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.UTF8.GetBytes(text);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bKey.ToArray(), bIV.ToArray()), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }
                return binaryStyle ? BitConverter.ToString(ms.ToArray()) : Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="text">待解密的字符串</param>
        /// <param name="key">DES加密的私钥，必须是字节长度8位的字符串，否则会补0。</param>
        /// <param name="iv">DES加密偏移量，必须是>=8位长的字符串，否则会补0。</param>
        /// <param name="binaryStyle">解密内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptByDES(string text, string key = "", string iv = "", bool binaryStyle = true)
        {
            List<byte> bKey = Encoding.UTF8.GetBytes(key).ToList();
            for (int i = bKey.Count; i < 8; i++)
            {
                bKey.Add((byte)i);
            }
            bKey = bKey.GetRange(0, 8);
            List<byte> bIV = Encoding.UTF8.GetBytes(iv).ToList();
            for (int i = bIV.Count; i < 8; i++)
            {
                bIV.Add((byte)i);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = binaryStyle ? text.GetBytesFromBitConverter() : Convert.FromBase64String(text);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey.ToArray(), bIV.ToArray()), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// TDES加密字符串 (可逆)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <param name="key">TDES加密的私钥，必须是字节长度24位的字符串，否则会自动补位。</param>
        /// <param name="iv">TDES加密偏移量，必须是字节长度>=8位的字符串，否则会自动补位。</param>
        /// <param name="binaryStyle">加密后结果内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptByTDES(string text, string key = "", string iv = "", bool binaryStyle = true)
        {
            List<byte> bKey = Encoding.UTF8.GetBytes(key).ToList();
            for (int i = bKey.Count; i < 24; i++)
            {
                bKey.Add((byte)i);
            }
            bKey = bKey.GetRange(0, 24);
            List<byte> bIV = Encoding.UTF8.GetBytes(iv).ToList();
            for (int i = bIV.Count; i < 8; i++)
            {
                bIV.Add((byte)i);
            }
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.UTF8.GetBytes(text);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bKey.ToArray(), bIV.ToArray()), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }
                return binaryStyle ? BitConverter.ToString(ms.ToArray()) : Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// TDES解密字符串
        /// </summary>
        /// <param name="text">待解密的字符串</param>
        /// <param name="key">TDES加密的私钥，必须是字节长度24位的字符串，否则会自动补位。</param>
        /// <param name="iv">TDES加密偏移量，必须是字节长度>=8位的字符串，否则会自动补位。</param>
        /// <param name="binaryStyle">解密内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptByTDES(string text, string key = "", string iv = "", bool binaryStyle = true)
        {
            List<byte> bKey = Encoding.UTF8.GetBytes(key).ToList();
            for (int i = bKey.Count; i < 24; i++)
            {
                bKey.Add((byte)i);
            }
            bKey = bKey.GetRange(0, 24);
            List<byte> bIV = Encoding.UTF8.GetBytes(iv).ToList();
            for (int i = bIV.Count; i < 8; i++)
            {
                bIV.Add((byte)i);
            }
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = binaryStyle ? text.GetBytesFromBitConverter() : Convert.FromBase64String(text);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey.ToArray(), bIV.ToArray()), CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 获取RSA加密公钥和私钥
        /// </summary>
        /// <returns>key:公钥 value:公钥和私钥</returns>
        public static KeyValuePair<string, string> GeneralRSAKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                return new KeyValuePair<string, string>(rsa.ToXmlString(false), rsa.ToXmlString(true));
            }
        }

        /// <summary>
        /// RSA加密字符串 (可逆)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <param name="name">密钥容器的名称。</param>
        /// <param name="binaryStyle">加密后结果内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptByRSA(string text, string name, bool binaryStyle = true)
        {
            CspParameters param = new CspParameters() { KeyContainerName = name };
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.UTF8.GetBytes(text);
                byte[] encryptdata = rsa.Encrypt(plaindata, false);
                return binaryStyle ? BitConverter.ToString(encryptdata) : Convert.ToBase64String(encryptdata);
            }
        }

        /// <summary>
        /// RSA解密字符串
        /// </summary>
        /// <param name="text">待解密的字符串</param>
        /// <param name="name">密钥容器的名称。</param>
        /// <param name="binaryStyle">解密内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptByRSA(string text, string name, bool binaryStyle = true)
        {
            CspParameters param = new CspParameters() { KeyContainerName = name };
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = binaryStyle ? text.GetBytesFromBitConverter() : Convert.FromBase64String(text);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.UTF8.GetString(decryptdata);
            }
        }

        /// <summary>
        /// RSA加密字符串 (可逆)
        /// </summary>
        /// <param name="text">待加密的字符串</param>
        /// <param name="xml">含有密钥信息的xml字符串。</param>
        /// <param name="binaryStyle">加密后结果内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptByRSAFromXmlString(string text, string xml, bool binaryStyle = true)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xml);
                byte[] plaindata = Encoding.UTF8.GetBytes(text);
                byte[] encryptdata = rsa.Encrypt(plaindata, false);
                return binaryStyle ? BitConverter.ToString(encryptdata) : Convert.ToBase64String(encryptdata);
            }
        }

        /// <summary>
        /// RSA解密字符串
        /// </summary>
        /// <param name="text">待解密的字符串</param>
        /// <param name="xml">含有密钥信息的xml字符串。</param>
        /// <param name="binaryStyle">解密内容样式。true:HEX格式加密结果，含“-”;Base64格式加密结果。</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptByRSAFromXmlString(string text, string xml, bool binaryStyle = true)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xml);
                byte[] encryptdata = binaryStyle ? text.GetBytesFromBitConverter() : Convert.FromBase64String(text);
                byte[] decryptdata = rsa.Decrypt(encryptdata, false);
                return Encoding.UTF8.GetString(decryptdata);
            }
        }

        /// <summary>
        /// 用时间简单混淆
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="timestampLength">混淆用时间戳长度</param>
        /// <returns>混淆后文本</returns>
        public static string MixUp(string text,int timestampLength = 36)
        {
            var timestamp = Guid.NewGuid().ToString();
            var count = text.Length + timestampLength;
            var sbd = new StringBuilder(count);
            int j = 0;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                if (j < timestampLength && k < text.Length)
                {
                    if (i % 2 == 0)
                    {
                        sbd.Append(text[k]);
                        k++;
                    }
                    else
                    {
                        sbd.Append(timestamp[j]);
                        j++;
                    }
                }
                else if (j >= timestampLength)
                {
                    sbd.Append(text[k]);
                    k++;
                }
                else if (k >= text.Length)
                {
                    break;
                }
            }

            return sbd.ToString();
        }

        /// <summary>
        /// 简单反混淆
        /// </summary>
        /// <param name="text">需要执行反混淆的文本</param>
        /// <param name="timestampLength">混淆用时间戳长度</param>
        /// <returns>原始文本</returns>
        public static string ClearUp(string text, int timestampLength = 36)
        {
            var sbd = new StringBuilder();
            int j = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (i % 2 == 0)
                {
                    sbd.Append(text[i]);
                }
                else
                {
                    j++;
                }

                if (j > timestampLength)
                {
                    sbd.Append(text.Substring(i));
                    break;
                }
            }

            return sbd.ToString();
        }
    }
}
