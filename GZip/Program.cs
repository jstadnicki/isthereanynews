using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace GZip
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var stringToUpload = File.ReadAllBytes("D:/msgraph-training-angularspa-master.zip");
            var compresed = Compress(stringToUpload);
            var decompresed = Decompress(compresed);
            
            File.WriteAllBytes("d:/zip.zip",decompresed);
        }
        
        public static byte[] Compress(byte[] bytes)
        {
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    gs.Write(bytes, 0, bytes.Length);
                }
                return mso.ToArray();
            }
        }
        
        public static byte[] Decompress(byte[] data)
        {
            // Read the last 4 bytes to get the length
            try
            {
                byte[] lengthBuffer = new byte[4];
                Array.Copy(data, data.Length - 4, lengthBuffer, 0, 4);
                int uncompressedSize = BitConverter.ToInt32(lengthBuffer, 0);

                var buffer = new byte[uncompressedSize];
                using (var ms = new MemoryStream(data))
                {
                    using (var gzip = new GZipStream(ms,CompressionMode.Decompress))
                    {
                        gzip.Read(buffer, 0, uncompressedSize);
                    }
                }

                return buffer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

    }
}