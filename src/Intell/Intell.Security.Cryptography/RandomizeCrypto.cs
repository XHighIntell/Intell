using System.Security.Cryptography;

namespace Intell.Security.Cryptography {
    ///<summary>Cryptographic Random Number Generator utility class.</summary>
    public static class RandomizeCrypto {
        public readonly static RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();

        ///<summary>Gets an array of bytes with a cryptographically strong sequence of random values.</summary>
        public static byte[] GetBytes(int count) {
            var randomBytes = new byte[count];
            
            // Fill the array with cryptographically secure random bytes.
            Random.GetBytes(randomBytes);

            return randomBytes;
        }


        ///<summary>Fills an array of bytes with a cryptographically strong sequence of random values.</summary>
        public static void FillBytes(byte[] data) { Random.GetBytes(data); }
        ///<summary>Fills the specified byte array with a cryptographically strong random sequence of values.</summary>
        public static void FillBytes(byte[] data, int offset, int count) { Random.GetBytes(data, offset, count); }

    }
}
