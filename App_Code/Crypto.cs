namespace OpenSocials.App_Code
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class Crypto
    {
        // key = chave secreta , iv = vetor de inicializacao
        private byte[] key;
        private byte[] iv;

        public Crypto()
        {
            InitializeKeys();
        }

        private void InitializeKeys()
        {
            string keysFilePath = GetKeysFilePath();

            if (File.Exists(keysFilePath))
            {
                // load key if exist
                ReadKeysFromFile();
            }
            else
            {
                // Create keys
                this.key = RandomKey(256); // Adjust key size as needed
                this.iv = RandomIV();

                SaveKeysToFile();
            }
        }

        ///<summary>Criptografa</summary>
        ///<param name="plainText">String a ser criptografado</param>
        ///<return>Retorna o texto criptografado em uma string</return>
        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.key;
                aesAlg.IV = this.iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        ///<summary>Descriptografa</summary>
        ///<param name="cipherText">String a ser descriptografado</param>
        ///<return>Retorna o texto descriptografado em uma string</return>
        public string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.key;
                aesAlg.IV = this.iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private byte[] RandomKey(int keySize)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[keySize / 8];
                rng.GetBytes(key);
                return key;
            }
        }

        private byte[] RandomIV()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] iv = new byte[16];
                rng.GetBytes(iv);
                return iv;
            }
        }

        private string GetKeysFilePath()
        {
            // Key folder
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Keys");

            // Create folder if not exist
            Directory.CreateDirectory(folderPath);

            // keyfilename
            return Path.Combine(folderPath, "keys.dat");
        }

        private void SaveKeysToFile()
        {
            // Save key iv same file
            string filePath = GetKeysFilePath();

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(this.key, 0, this.key.Length);
                fs.Write(this.iv, 0, this.iv.Length);
            }
        }

        private void ReadKeysFromFile()
		{
			
			string filePath = GetKeysFilePath();

			if (File.Exists(filePath))
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open))
				{
					long keySize = fs.Length / 2;
					long ivSize = fs.Length - keySize;

					this.key = new byte[keySize];
					this.iv = new byte[16];
					
					// read the key
					fs.Read(this.key, 0, (int)keySize);

					// read the iv last 16 bytes
					fs.Read(this.iv, 0, 16);
				}
			}
		}

    }
}
