using System;

namespace Itan.Core
{
    public class ChannelViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Guid Id { get; set; }
        public int NewsCount { get; set; }
    }
}