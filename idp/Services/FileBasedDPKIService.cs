using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
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
            string privFilePath = Path.Combine(_keyPath, keyName);
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

        public Task<string> Sign(string key, string text)
        {
            // load private key
        }
    }
}
