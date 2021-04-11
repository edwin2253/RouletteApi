using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteApi.Models;
using RouletteApi.Models.ModelContext;

namespace RouletteApi.Controllers
{
    [Route("api/Roulettes")]
    [ApiController]
    public class RoulettesController : ControllerBase
    {
        private readonly RouletteContext _context;
        public RoulettesController(RouletteContext context)
        {
            _context = context;
        }        
        [HttpGet] // GET: api/Roulettes
        public async Task<ActionResult<IEnumerable<Roulette>>> GetRoulettes()
        {
            return await _context.Roulettes.ToListAsync();
        }        
        [HttpGet("{id}")] // GET: api/Roulettes
        public async Task<ActionResult<Roulette>> GetRoulette(long id)
        {
            var roulette = await _context.Roulettes.FindAsync(id);
            if (roulette == null)
            {
                return NotFound();
            }

            return roulette;
        }        
        [HttpPut("{id}")] // PUT: api/Roulettes
        public async Task<IActionResult> PutRoulette(long id, Roulette roulette)
        {
            if (id != roulette.Id)
            {
                return BadRequest();
            }
            _context.Entry(roulette).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouletteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }        
        [HttpPost] // POST: api/Roulettes
        public async Task<ActionResult<Roulette>> PostRoulette(Roulette roulette)
        {
            _context.Roulettes.Add(roulette);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoulette", new { id = roulette.Id }, roulette);
        }        
        [HttpDelete("{id}")] // DELETE: api/Roulettes/5
        public async Task<ActionResult<Roulette>> DeleteRoulette(long id)
        {
            var roulette = await _context.Roulettes.FindAsync(id);
            if (roulette == null)
            {
                return NotFound();
            }
            _context.Roulettes.Remove(roulette);
            await _context.SaveChangesAsync();

            return roulette;
        }
        private bool RouletteExists(long id)
        {
            return _context.Roulettes.Any(e => e.Id == id);
        }
        [HttpGet("Open/{id}")] // GET: api/Roulettes/Open
        public async Task<IActionResult> OpenRoulette(long id, Roulette roulette)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            var rouletteSelected = await _context.Roulettes.FindAsync(id);
            if (rouletteSelected.IsOpen)
            {
                return Ok("Denied");
            }
            else
            {
                rouletteSelected.IsOpen = true;
                roulette.Game++;
                _context.Entry(rouletteSelected).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouletteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Successful");
            }
        }
    }
}
