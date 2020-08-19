using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HungryPizza.API.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2, ErrorMessage = "A pizza deve ter no máximo 2 sabores")]
        [MinLength(1, ErrorMessage = "A pizza deve ter no mínimo 1 sabor")]
        public List<PizzaFlavours> PizzaFlavours { get; set; }

        public decimal Price { get; set; }
    }
}
