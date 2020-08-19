using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungryPizza.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public string Address { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime Date { get; set; }

        [MaxLength(10, ErrorMessage = "O pedido deve ter no máximo 10 itens")]
        [MinLength(1, ErrorMessage = "O pedido deve ter no mínimo 1 item")]
        public List<Pizza> Items { get; set; }

        public decimal Total { get; set; }

    }
}
