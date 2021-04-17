using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("PersonsPersons")]
    internal class PersonPerson
    {
        public Guid Id { get; set; }
        public Guid TargetPersonId { get; set; }
        public Guid FollowerPersonId { get; set; }
    
        [ForeignKey(nameof(TargetPersonId))]
        public Person Target { get; set; }
        
        [ForeignKey(nameof(FollowerPersonId))]
        public Person Follower { get; set; }
    
        public DateTime CreatedOn { get; set; }
    }
}