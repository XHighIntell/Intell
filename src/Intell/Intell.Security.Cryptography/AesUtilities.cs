using System.Security.Cryptography;

namespace Intell.Security.Cryptography {
    ///<summary>Advanced Encryption Standard utility class for transforming small data.</summary>
    public static class AesUtilities {

        ///<summary>Encrypt the specified byte array.</summary>
        public static byte[] Encrypt(byte[] buffer, byte[] key, byte[] iv) { return Encrypt(buffer, 0, buffer.Length, key, iv, CipherMode.CBC, PaddingMode.PKCS7); }
        ///<summary>Encrypt the specified byte array.</summary>
        public static byte[] Encrypt(byte[] buffer, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding) { return Encrypt(buffer, 0, buffer.Length, key, iv, mode, padding); }
        ///<summary>Encrypt the specified region of the specified byte array.</summary>
        ///<param name="buffer">The input for which to compute the transform.</param>
        ///<param name="offset">The offset into the byte array from which to begin using data.</param>
        ///<param name="count">The number of bytes in the byte array to use as data.</param>
        ///<param name="key">The secret key to use for the symmetric algorithm.</param>
        ///<param name="iv">The initialization vector.</param>
        ///<param name="mode">The mode for operation of the symmetric algorithm.</param>
        ///<param name="padding">The padding mode used in the symmetric algorithm.</param>
        public static byte[] Encrypt(byte[] buffer, int offset, int count, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding) {
            using (var aes = Aes.Create()) {
                aes.KeySize = key.Length * 8;
                aes.BlockSize = iv.Length * 8;
                aes.Mode = mode;
                aes.Padding = padding;

                using (var encryptor = aes.CreateEncryptor(key, iv)) {
                    return encryptor.TransformFinalBlock(buffer, offset, count);
                }
            }
        }

        ///<summary>Decrypt the specified byte array.</summary>
        public static byte[] Decrypt(byte[] buffer, byte[] key, byte[] iv) { return Decrypt(buffer, 0, buffer.Length, key, iv, CipherMode.CBC, PaddingMode.PKCS7); }
        ///<summary>Decrypt the specified byte array.</summary>
        public static byte[] Decrypt(byte[] buffer, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding) { return Decrypt(buffer, 0, buffer.Length, key, iv, mode, padding); }
        ///<summary>Decrypt the specified region of the specified byte array.</summary>
        public static byte[] Decrypt(byte[] buffer, int offset, int count, byte[] key, byte[] iv, CipherMode mode, PaddingMode padding) {

            using (var aes = Aes.Create()) {
                aes.KeySize = key.Length * 8;
                aes.BlockSize = iv.Length * 8;
                aes.Mode = mode;
                aes.Padding = padding;

                using (var decryptor = aes.CreateDecryptor(key, iv)) {
                    return decryptor.TransformFinalBlock(buffer, offset, count);
                }
            }
        }
    }
}
