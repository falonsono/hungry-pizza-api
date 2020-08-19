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
    [Route("v1/flavours")]
    public class FlavourController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Flavour>>> Get([FromServices] DataContext context)
        {
            var flavours = await context.Flavours
                .AsNoTracking()
                .ToListAsync();
            return Ok(flavours);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Flavour>> GetById([FromServices] DataContext context, int id)
        {
            var flavour = await context.Flavours.FindAsync(id);
            if (flavour == null)
            {
                return NotFound();
            }

            return Ok(flavour);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Flavour>> Post([FromServices] DataContext context, [FromBody] Flavour model)
        {
            if (ModelState.IsValid)
            {
                context.Flavours.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromServices] DataContext context, int id,[FromBody] Flavour model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var flavour = await context.Flavours.FindAsync(id);
            if (flavour == null)
            {
                return NotFound();
            }

            flavour.Name = model.Name;
            flavour.Price = model.Price;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!FlavourExists(context, id))
            {
                return NotFound();
            }

            return Ok(flavour);
        }

        private bool FlavourExists(DataContext context, int id) =>
                context.Flavours.Any(e => e.Id == id);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Flavour>> Delete([FromServices] DataContext context, int id)
        {
            var flavour = await context.Flavours.FindAsync(id);
            if (flavour == null)
            {
                return NotFound();
            }

            context.Flavours.Remove(flavour);
            await context.SaveChangesAsync();

            return Ok(flavour);
        }
    }
}
