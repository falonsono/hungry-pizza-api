using System.ComponentModel.DataAnnotations;

namespace HungryPizza.API.Models
{
    public class Flavour
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
