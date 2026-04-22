using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.comm
{
    internal class rsautility
    {
        public static void keygen(out string PrivateKey, out string PublicKey, int length = 0x800)
        {
            RsaKeyPairGenerator __keygener = new RsaKeyPairGenerator();
            __keygener.Init(new KeyGenerationParameters(new SecureRandom(), length));
            AsymmetricCipherKeyPair __keys = __keygener.GenerateKeyPair();

            TextWriter __prvtextwriter = new StringWriter();
            PemWriter __prvpemwriter = new PemWriter(__prvtextwriter);
            __prvpemwriter.WriteObject(__keys.Private);
            __prvpemwriter.Writer.Flush();

            TextWriter __pubtextwriter = new StringWriter();
            PemWriter __pubpemwriter = new PemWriter(__pubtextwriter);
            __pubpemwriter.WriteObject(__keys.Public);
            __pubpemwriter.Writer.Flush();

            PrivateKey = __prvtextwriter.ToString();
            PublicKey = __pubtextwriter.ToString();
        }

        public static byte[] encrypt(byte[] proclaimedata, string publickey)
        {
            byte[] __result = null;

            using (TextReader __reader = new StringReader(publickey))
            {
                AsymmetricKeyParameter __key = new PemReader(__reader).ReadObject() as AsymmetricKeyParameter;
                Pkcs1Encoding __pkcs1 = new Pkcs1Encoding(new RsaEngine());
                __pkcs1.Init(true, __key);
                __result = __pkcs1.ProcessBlock(proclaimedata, 0x00, proclaimedata.Length);
            }

            return __result;
        }

        public static byte[] decrypt(byte[] cipherdata, string privatekey)
        {
            byte[] __result = null;

            using (TextReader __reader = new StringReader(privatekey))
            {
                dynamic __key = new PemReader(__reader).ReadObject();
                Pkcs1Encoding __pkcs1 = new Pkcs1Encoding(new RsaEngine());
                if (__key is AsymmetricKeyParameter)
                    __key = (AsymmetricKeyParameter)__key;
                else if (__key is AsymmetricCipherKeyPair)
                    __key = ((AsymmetricCipherKeyPair)__key).Private;
                __pkcs1.Init(false, __key);
                __result = __pkcs1.ProcessBlock(cipherdata, 0x00, cipherdata.Length);
            }

            return __result;
        }

        public static string sign(string data, string privatekey, string algorithm = "SHA256WithRSA")
        {
            RsaKeyParameters __prvkeyparams =
                PrivateKeyFactory.CreateKey(Convert.FromBase64String(privatekey))
                as RsaKeyParameters;
            ISigner __signer = SignerUtilities.GetSigner(algorithm);
            __signer.Init(true, __prvkeyparams);
            byte[] __databuff = Encoding.UTF8.GetBytes(data);
            __signer.BlockUpdate(__databuff, 0x00, __databuff.Length);

            return Convert.ToBase64String(__signer.GenerateSignature());
        }
        

        public static bool verify(string data, string sign, string publickey, string algorithm = "SHA256WithRSA")
        {
            RsaKeyParameters __pubkeyparams =
                PublicKeyFactory.CreateKey(Convert.FromBase64String(publickey))
                as RsaKeyParameters;
            ISigner __signer = SignerUtilities.GetSigner(algorithm);
            __signer.Init(false, __pubkeyparams);
            byte[] __databuff = Encoding.UTF8.GetBytes(data);
            __signer.BlockUpdate(__databuff, 0x00, __databuff.Length);

            return __signer.VerifySignature(Convert.FromBase64String(sign));
        }

    }
}
