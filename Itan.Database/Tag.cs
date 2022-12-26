using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database;

[Table("Tags")]
internal class Tag
{
    public Guid Id { get; set; }
    
    [MaxLength(150)]
    public string Text { get; set; }
    
    public virtual IEnumerable<NewsTag> NewsTags { get; set; }
}