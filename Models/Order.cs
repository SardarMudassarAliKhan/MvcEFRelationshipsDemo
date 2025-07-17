using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcEFRelationshipsDemo.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string Product { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
