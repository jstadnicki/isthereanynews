using System;

namespace Itan.Core.GetFollowerActivity
{
    public class FollowerActivityViewModel
    {
        public Guid Id { get; set; }
        public Guid NewsId { get; set; }
        public Guid ChannelId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReadType { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public string Link { get; set; }
    }
}