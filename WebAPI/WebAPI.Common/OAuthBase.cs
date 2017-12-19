using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace WebAPI.Common
{
    public class OAuthBase
    {
        public enum SignatureTypes
        {
            HMACSHA1,
            PLAINTEXT,
            RSASHA1
        }

        public string GenerateSignature(Uri url, string consumerPublicKey, string consumerSecretKey, string httpMethod, string timeStamp, string nonce, SignatureTypes signatureType, out string normalizedUrl, out string normalizedRequestParameters)
        {
            normalizedUrl = null;
            normalizedRequestParameters = null;

            switch (signatureType)
            {
                case SignatureTypes.PLAINTEXT:
                    return HttpUtility.UrlEncode(string.Format("{0}", consumerSecretKey));
                case SignatureTypes.HMACSHA1:
                    string signatureBase = consumerPublicKey.ToString() + timeStamp + nonce + consumerSecretKey;
                    signatureBase = signatureBase.ToLower();

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] keyByte = encoding.GetBytes(consumerSecretKey.ToString().ToLower());
                    HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
                    return GenerateSignatureUsingHash(hmacsha1, signatureBase);
                case SignatureTypes.RSASHA1:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }
        }

        private string GenerateSignatureUsingHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        protected byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // The salt bytes must be at least 8 bytes.
            string salt = ConfigurationManager.AppSettings["SaltKey"];
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.PKCS7;

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            string salt = ConfigurationManager.AppSettings["SaltKey"];
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public void EncryptFile(string inputFile, string outputFile)
        {
            string password = ConfigurationManager.AppSettings["EncryptionKey"];

            byte[] bytesToBeEncrypted = File.ReadAllBytes(inputFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            File.WriteAllBytes(outputFile, bytesEncrypted);
        }

        public string DecryptFile(string inputFile)
        {
            string password = ConfigurationManager.AppSettings["EncryptionKey"];

            byte[] bytesToBeDecrypted = File.ReadAllBytes(inputFile);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
    }
}
