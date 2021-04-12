using System;

namespace Itan.Core.GetReader
{
    public class ReaderSubscribedChannel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? Click { get; set; }
        public int? Read { get; set; }
        public int? Skip { get; set; }
        public int TotalNewsCount { get; set; }
    }
}