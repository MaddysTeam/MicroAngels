using MicroAngels.Core.Plugins;
using System.Text;
using Xunit;

namespace MicroAngels.Core.Test.Plugins
{

	public class RSACryptoTest
	{

		[Fact]
		public void CreatPrivateKeyTest()
		{
			var keys = RSACryptor.CreateKeys(2048, KeyFormat.XML);
			Assert.NotNull(keys);
			Assert.True(keys.Count == 2);

			var publicKey = keys[0];
			var privateKey = keys[1];
			Assert.NotNull(publicKey);
			Assert.NotNull(privateKey);
			Assert.True(!publicKey.IsBase64()); // xml content is not base64 string

			keys = RSACryptor.CreateKeys(2048, KeyFormat.Pkcs1);
			publicKey = keys[0];
			privateKey = keys[1];
			Assert.True(publicKey.IsBase64() && privateKey.IsBase64()); // is base64

			keys = RSACryptor.CreateKeys(2048, KeyFormat.Pkcs8);
			publicKey = keys[0];
			privateKey = keys[1];
			Assert.True(publicKey.IsBase64() && privateKey.IsBase64()); // is base64
		}

		[Fact]
		public void EncrptorTest()
		{
			var encryptor = CreateCryptor();
			Assert.NotNull(encryptor);
		}

		[Fact]
		public void EncryptAndDecryptTest()
		{
			var cryptor = CreateCryptor();
			var content = "secret content";
			var encryptContent = cryptor.Encrypt(content);
			var decryptContent = cryptor.Decrypt(encryptContent);

			Assert.Equal(content, decryptContent);
		}

		private RSACryptor CreateCryptor()
		{
			var keys = RSACryptor.CreateKeys(2048, KeyFormat.Pkcs1);
			return new RSACryptor(RSAType.RSA2, Encoding.UTF8, keys[0], keys[1]);
		}

	}

}
