using System.ComponentModel.DataAnnotations;

namespace MvcEFRelationshipsDemo.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
