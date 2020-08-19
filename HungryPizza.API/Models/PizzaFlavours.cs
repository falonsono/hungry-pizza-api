
namespace HungryPizza.API.Models
{
    public class PizzaFlavours
    {
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        public int FlavourId { get; set; }
        public Flavour Flavour { get; set; }
    }
}
