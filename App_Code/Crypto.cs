using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections;

public class Crypto
{
    // key = chave secreta , iv = vetor de inicializacao
    private byte[] key;
    private byte[] iv; 
     
	
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

    private void SaveKeyToFile(byte[] key, string filePath)
    {
        System.IO.File.WriteAllBytes(filePath, this.key);
        System.IO.File.WriteAllBytes(filePath, this.iv);
    }

    private List<byte[]> ReadKeyFromFile(string filePath)
    {
        List<byte[]> keys = new List<byte[]>();

        keys.Add(System.IO.File.ReadAllBytes(filePath));
		keys.Add(System.IO.File.ReadAllBytes(filePath));

        return keys;
    }
}
