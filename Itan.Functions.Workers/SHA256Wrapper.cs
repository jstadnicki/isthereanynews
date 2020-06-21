using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Itan.Functions.Workers
{
    public class SHA256Wrapper : IHashSum
    {
        public string GetHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytesToHash = Encoding.UTF8.GetBytes(input);
            var hashedBytes = sha256.ComputeHash(bytesToHash);
            var hash = hashedBytes.Aggregate(string.Empty, (current, b) => current + b.ToString("X"));
            return hash;
        }
    }
}