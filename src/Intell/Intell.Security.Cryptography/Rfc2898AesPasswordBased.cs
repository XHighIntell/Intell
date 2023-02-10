
#if NET6_0_OR_GREATER
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Intell.Security.Cryptography {
    ///<summary>Password based using Rfc2898 and Advanced Encryption Standard for transforming small data.</summary>
    public static class Rfc2898AesPasswordBased {

        /*
            byte    version     - 
            int     keySize     - 
            int     blockSize   -

            int     iterations  - default 1000
            string  HashAlgorithmName - with 1 byte for length

            int     salt_length -
            byte[]  salt_data   -

            int     data_length -
            byte[]  data        -

            
            
        */

        ///<summary>Encrypt the specified byte array.</summary>
        public static byte[] Encrypt(byte[] plainBytes, string password, int keySize, int blockSize, int iterations, HashAlgorithmName hashAlgorithmName) {
            byte version = 1;
            byte[] salt = RandomizeCrypto.GetBytes(32);
            byte[] key;
            byte[] iv;


            using (var generator = new Rfc2898DeriveBytes(password, salt, iterations, hashAlgorithmName)) {
                
                key = generator.GetBytes(keySize / 8);
                iv = generator.GetBytes(blockSize / 8);

            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream)) {

                writer.Write((byte)version);
                writer.Write((int)keySize);
                writer.Write((int)blockSize);
                writer.Write((int)iterations);
                writer.Write((byte)hashAlgorithmName.Name.Length); writer.Write((byte[])Encoding.ASCII.GetBytes(hashAlgorithmName.Name));
                writer.Write((int)salt.Length); stream.Write(salt, 0, salt.Length);


                var data = AesUtilities.Encrypt(plainBytes, key, iv);

                writer.Write((int)data.Length); stream.Write(data, 0, data.Length);

                return stream.ToArray();
            }
        }

        ///<summary>Decrypt the specified byte array.</summary>
        public static byte[] Decrypt(byte[] plainBytes, string password) {


            using (var stream = new MemoryStream(plainBytes))
            using (var reader = new BinaryReader(stream)) {

                var version = reader.ReadByte();
                var keySize = reader.ReadInt32();
                var blockSize = reader.ReadInt32();
                var iterations = reader.ReadInt32();

                var algorithmNameLength = reader.ReadByte();
                var algorithmName = Encoding.ASCII.GetString(reader.ReadBytes(algorithmNameLength));

                var salt_length = reader.ReadInt32();
                var salt = reader.ReadBytes(salt_length);

                var data_length = reader.ReadInt32();
                var data = reader.ReadBytes(data_length);

                byte[] key;
                byte[] iv;
                
                using (var s = new Rfc2898DeriveBytes(password, salt, iterations, new HashAlgorithmName(algorithmName))) {
                    key = s.GetBytes(keySize / 8);
                    iv = s.GetBytes(blockSize / 8);
                }


                return AesUtilities.Decrypt(data, key, iv);

            }
        }
    }
}
#endif