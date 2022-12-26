using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database;

[Table("NewsTags")]
internal class NewsTag
{
    public Guid TagId { get; set; }
    public Guid NewsId { get; set; }
    
    [ForeignKey(nameof(NewsTag.NewsId))]
    virtual public News News { get; set; }
    
    [ForeignKey(nameof(NewsTag.TagId))]
    virtual public Tag Tag { get; set; }
}