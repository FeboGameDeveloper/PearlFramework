using System;
using System.IO;
using System.Security.Cryptography;

namespace Pearl
{
    public static class Encrypted
    {
        public static string Decrypt(string path, byte[] encryptedKey)
        {
            // Does the file exist
            if (File.Exists(path))
            {
                // Create FileStream for opening files.
                FileStream dataStream = new(path, FileMode.Open);

                // Create new AES instance.
                Aes oAes = Aes.Create();

                // Create an array of correct size based on AES IV.
                byte[] outputIV = new byte[oAes.IV.Length];

                // Read the IV from the file.
                dataStream.Read(outputIV, 0, outputIV.Length);

                // Create CryptoStream, wrapping FileStream
                CryptoStream oStream = new(
                       dataStream,
                       oAes.CreateDecryptor(encryptedKey, outputIV),
                       CryptoStreamMode.Read);

                // Create a StreamReader, wrapping CryptoStream
                StreamReader reader = new(oStream);

                string result = reader.ReadToEnd();

                // Close StreamWriter.
                reader.Close();

                // Close CryptoStream.
                oStream.Close();

                // Read the entire file into a String value.
                return result;
            }

            return null;
        }

        public static bool Encrypt(string path, byte[] encryptedKey, string jsonString, out string error)
        {
            // Create new AES instance.
            Aes iAes = Aes.Create();
            // Create a FileStream for creating files.
            FileStream dataStream;
            error = "";
            try
            {
                dataStream = new FileStream(path, FileMode.Create);
            }
            catch (Exception)
            {
                error = "storageerror";
                return false;
            }

            // Save the new generated IV.
            byte[] inputIV = iAes.IV;

            // Write the IV to the FileStream unencrypted.
            dataStream.Write(inputIV, 0, inputIV.Length);

            // Create CryptoStream, wrapping FileStream.
            CryptoStream iStream = new(
                    dataStream,
                    iAes.CreateEncryptor(encryptedKey, iAes.IV),
                    CryptoStreamMode.Write);

            // Create StreamWriter, wrapping CryptoStream.
            StreamWriter sWriter = new(iStream);

            // Write to the innermost stream (which will encrypt).
            sWriter.Write(jsonString);

            // Close StreamWriter.
            sWriter.Close();

            // Close CryptoStream.
            iStream.Close();

            // Close FileStream.
            dataStream.Close();

            return true;
        }
    }
}
