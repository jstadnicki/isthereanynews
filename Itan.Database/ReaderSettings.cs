using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ReadersSettings")]
    internal class ReaderSettings
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }

        [MaxLength(25)]
        public string ShowUpdatedNews { get; set; }

        [MaxLength(25)]
        public string SquashNewsUpdates { get; set; }

        public virtual Person Person { get; set; }
    }
}