using System;
using System.Security.Cryptography;
using System.Text;

internal class RatDecoder
{
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Enter Mutex: ");
            string mutex = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(mutex))
            {
                Console.WriteLine("Mutex cannot be empty.");
                continue;
            }

            RunDecoder(mutex);
        }
    }

    private static void RunDecoder(string mutex)
    {
        while (true)
        {
            Console.Write("Enter text to decrypt (!menu to reset Mutex): ");
            string input = Console.ReadLine();

            if (input == null || input.Equals("!menu", StringComparison.OrdinalIgnoreCase))
                return;

            try
            {
                string result = DecryptBase(input, mutex);
                Console.WriteLine("Decrypted: " + result);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid Base64 input.");
            }
            catch (CryptographicException)
            {
                Console.WriteLine("Error: Decryption failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    public static string DecryptBase(string input, string pass)
    {
        using (var algorithm = new RijndaelManaged())
        using (var hash = MD5.Create())
        {
            byte[] key = new byte[32];
            byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(pass));

            Array.Copy(hashBytes, 0, key, 0, 16);
            Array.Copy(hashBytes, 0, key, 15, 16);

            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;

            using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
            {
                byte[] encrypted = Convert.FromBase64String(input);
                byte[] decrypted = decryptor.TransformFinalBlock(
                    encrypted, 0, encrypted.Length);

                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}
