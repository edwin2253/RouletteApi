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
    [Route("api/Bets")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly BetContext _context;
        public BetsController(BetContext context)
        {
            _context = context;
        }        
        [HttpGet] // GET: api/Bets
        public async Task<ActionResult<IEnumerable<Bet>>> GetBets()
        {
            return await _context.Bets.ToListAsync();
        }        
        [HttpGet("{id}")] // GET: api/Bets
        public async Task<ActionResult<Bet>> GetBet(long id)
        {
            var bet = await _context.Bets.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }

            return bet;
        }
        [HttpPut("{id}")] // PUT: api/Bets
        public async Task<IActionResult> PutBet(long id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest();
            }
            _context.Entry(bet).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BetExists(id))
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
        [HttpPost] // POST: api/Bets
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBet", new { id = bet.Id }, bet);
        }
        [HttpDelete("{id}")] // DELETE: api/Bets
        public async Task<ActionResult<Bet>> DeleteBet(long id)
        {
            var bet = await _context.Bets.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }
            _context.Bets.Remove(bet);
            await _context.SaveChangesAsync();

            return bet;
        }

        private bool BetExists(long id)
        {
            return _context.Bets.Any(e => e.Id == id);
        }
    }
}
