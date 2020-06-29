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
            var stringToUpload = File.ReadAllText("D:/20200627011910_19.xml");
            var bytes = Encoding.UTF8.GetBytes(stringToUpload);
            File.WriteAllText("D:/20200627011910_19_decompress.xml",Decompress(bytes));
        }
        
        public static byte[] Compress(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    gs.Write(bytes, 0, bytes.Length);
                }
                return mso.ToArray();
            }
        }
        
        public static string Decompress(byte[] data)
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
                    using (var gzip = new GZipStream(ms, CompressionLevel.NoCompression))
                    {
                        gzip.Read(buffer, 0, uncompressedSize);
                    }
                }
                return Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }            
        }

    }
}