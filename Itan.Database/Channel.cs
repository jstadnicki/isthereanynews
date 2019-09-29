using System;

namespace Itan.Database
{
    internal class Channel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
