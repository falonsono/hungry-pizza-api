using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HungryPizza.API.Data;
using HungryPizza.API.Models;
using System.Linq;
using System;

namespace HungryPizza.API.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Order>>> Get([FromServices] DataContext context)
        {
            var orders = await context.Orders
                .Include(x => x.Client)
                .Include(x => x.Items)
                .ThenInclude(x => x.PizzaFlavours)
                .ThenInclude(x => x.Flavour)
                .AsNoTracking()
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Order>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Orders
                .Include(x => x.Client)
                .Include(x => x.Items)
                .ThenInclude(x => x.PizzaFlavours)
                .ThenInclude(x => x.Flavour)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpGet]
        [Route("clients/{id:int}")]
        public async Task<ActionResult<List<Order>>> GetByClient([FromServices] DataContext context, int id)
        {
            var products = await context.Orders
                .Include(x => x.Client)
                .Include(x => x.Items)
                .ThenInclude(x => x.PizzaFlavours)
                .ThenInclude(x => x.Flavour)
                .AsNoTracking()
                .Where(x => x.ClientId == id)
                .ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Order>> Post([FromServices] DataContext context, [FromBody] Order model)
        {
            if (ModelState.IsValid)
            {
                if ((context.Clients.FirstOrDefault(x => x.Id == model.ClientId) == null) &&
                     string.IsNullOrEmpty(model.Address))
                {
                    return BadRequest("Cliente inexistente ou endereço inválido!");
                }

                model.Items.ForEach(x => {
                    x.PizzaFlavours.ForEach(y =>
                    {
                        Flavour flavour = context.Flavours.FirstOrDefault(z => z.Id == y.FlavourId);
                        x.Price += flavour.Price / x.PizzaFlavours.Count;
                    });
                });

                model.Total = model.Items.Sum(x => x.Price);
                model.Date = DateTime.Now;

                context.Orders.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
