using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HungryPizza.API.Data;
using HungryPizza.API.Models;
using System.Linq;

namespace HungryPizza.API.Controllers
{
    [ApiController]
    [Route("v1/clients")]
    public class ClientController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Client>>> Get([FromServices] DataContext context)
        {
            var clients = await context.Clients
                .AsNoTracking()
                .ToListAsync();
            return Ok(clients);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Client>> GetById([FromServices] DataContext context, int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Client>> Post([FromServices] DataContext context, [FromBody] Client model)
        {
            if (ModelState.IsValid)
            {
                context.Clients.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] DataContext context, int id, [FromBody] Client model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            client.Name = model.Name;
            client.CEP = model.CEP;
            client.Address = model.Address;
            client.PhoneNumber = model.PhoneNumber;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ClientExists(context, id))
            {
                return NotFound();
            }

            return Ok(client);
        }

        private bool ClientExists(DataContext context, int id) =>
                context.Clients.Any(e => e.Id == id);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> Delete([FromServices] DataContext context, int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            await context.Orders
                .Include(x => x.Items)
                .ForEachAsync(x =>
            {
                if (x.ClientId == id)
                {
                    x.Items.ForEach(pizza => context.Pizzas.Remove(pizza));
                }
            });

            context.Clients.Remove(client);
            await context.SaveChangesAsync();

            return Ok(client);
        }
    }
}
