using System;
using System.Text;

namespace Core
{
	public static class EncryptionController
	{
		#region Key

		private const int Key = 64;

		#endregion

		public static string EncryptXor(string textToEncrypt)
		{
			var inSb = new StringBuilder(textToEncrypt);
			var outSb = new StringBuilder(textToEncrypt.Length);

			for (var i = 0; i < textToEncrypt.Length; i++)
			{
				var c = inSb[i];
				c = (char) (c ^ Key);

				outSb.Append(c);
			}

			return outSb.ToString();
		}

		//Use it to encrypt and decrypt
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

			return Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}
