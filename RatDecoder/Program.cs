using System;
using System.Security.Cryptography;
using System.Text;

internal class RatDecoder
{
    private static void Main(string[] args)
    {
        for (; ; )
        {
            Console.Write("Enter Mutex: ");
            string pass = Console.ReadLine();
            for (; ; )
            {
                Console.Write("Enter text to decrypt (!menu to reset Mutex): ");
                string text = Console.ReadLine();
                if (text == "!menu")
                {
                    break;
                }
                try
                {
                    string str = RatDecoder.DecryptBase(text, pass);
                    Console.WriteLine("Decrypted: " + str);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }

    public static string DecryptBase(string input, string pass)
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
        byte[] array = new byte[32];
        byte[] sourceArray = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(pass));
        Array.Copy(sourceArray, 0, array, 0, 16);
        Array.Copy(sourceArray, 0, array, 15, 16);
        rijndaelManaged.Key = array;
        rijndaelManaged.Mode = CipherMode.ECB;
        ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
        byte[] array2 = Convert.FromBase64String(input);
        return Encoding.UTF8.GetString(cryptoTransform.TransformFinalBlock(array2, 0, array2.Length));
    }

    private static string Mutex;
}
