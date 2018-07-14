using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace idp.Services
{
    public class FileBasedDPKIServicec : IDPKIService
    {
        private IConfigurationService _config;
        private string _keyPath;

        public FileBasedDPKIServicec(IConfigurationService config)
        {
            _config = config;
            _keyPath = _config.GetKeyPath();
        }

        public async Task GenNewKey(string keyName)
        {
            RsaKeyPairGenerator rsaGenerator = new RsaKeyPairGenerator();
            rsaGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaGenerator.GenerateKeyPair();
            // write private key to pem file
            string privFilePath = Path.Combine(_keyPath, keyName + ".asc");
            using (TextWriter privWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(privWriter);
                pemWriter.WriteObject(keyPair.Private);
                await pemWriter.Writer.FlushAsync();
                await File.WriteAllTextAsync(privFilePath, privWriter.ToString());
            }
            // write public key to pem file
            string pubFilePath = Path.Combine(_keyPath, keyName + ".pub");
            using (TextWriter pubWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(pubWriter);
                pemWriter.WriteObject(keyPair.Public);
                await pemWriter.Writer.FlushAsync();
                await File.WriteAllTextAsync(pubFilePath, pubWriter.ToString());
            }
        }

        public async Task<string> GetPubKey(string keyName)
        {
            string fileName = Path.Combine(_keyPath, keyName + ".pub");
            return await File.ReadAllTextAsync(fileName, Encoding.UTF8);
        }

        public void UpdateKey(string oldKeyName, string newKeyName)
        {
            string old_pub = Path.Combine(_keyPath, oldKeyName + ".pub");
            string new_pub = Path.Combine(_keyPath, newKeyName + ".pub");
            File.Copy(old_pub, new_pub);
            string old_priv = Path.Combine(_keyPath, oldKeyName + ".asc");
            string new_priv = Path.Combine(_keyPath, newKeyName + ".asc");
            File.Copy(old_priv, new_priv);
            File.Delete(old_pub);
            File.Delete(old_priv);
        }

        public Task<string> Sign(string key, string text)
        {
            string fileName = Path.Combine(_keyPath, key + ".asc");
            using (TextReader privReader = new StringReader(File.ReadAllText(fileName)))
            {
                AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair) new PemReader(privReader).ReadObject();
                RsaPrivateCrtKeyParameters privParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
                RSAParameters rsaParams = _ConvertRSAParameters(privParams);
                string signedResult;
                using(RSA rsa = RSA.Create())
                {
                    rsa.ImportParameters(rsaParams);
                    byte[] signedByte = rsa.SignHash(Convert.FromBase64String(text) , HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    signedResult = Convert.ToBase64String(signedByte);
                }
                return Task.FromResult(signedResult);
            }
        }

        private static RSAParameters _ConvertRSAParameters(RsaPrivateCrtKeyParameters privKey)
        {
            RSAParameters rp = new RSAParameters();
            rp.Modulus = privKey.Modulus.ToByteArrayUnsigned();
            rp.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
            rp.P = privKey.P.ToByteArrayUnsigned();
            rp.Q = privKey.Q.ToByteArrayUnsigned();
            rp.D = _ConvertRSAParametersField(privKey.Exponent, rp.Modulus.Length);
            rp.DP = _ConvertRSAParametersField(privKey.DP, rp.P.Length);
            rp.DQ = _ConvertRSAParametersField(privKey.DQ, rp.Q.Length);
            rp.InverseQ = _ConvertRSAParametersField(privKey.QInv, rp.Q.Length);
            return rp;
        }

        private static byte[] _ConvertRSAParametersField(BigInteger n, int size)
        {
            byte[] bs = n.ToByteArrayUnsigned();

            if (bs.Length == size)
                return bs;

            if (bs.Length > size)
                throw new ArgumentException("Specified size too small", "size");

            byte[] padded = new byte[size];
            Array.Copy(bs, 0, padded, size - bs.Length, bs.Length);
            return padded;
        }
    }
}
