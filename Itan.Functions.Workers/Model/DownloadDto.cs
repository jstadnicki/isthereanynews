﻿using System;

namespace Itan.Functions.Workers.Model
{
    public class DownloadDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChannelId { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string Sha { get; set; }
    }
}