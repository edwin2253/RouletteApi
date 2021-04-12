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
        private readonly BetContext _contextBet;
        private readonly UserContext _contextUser;
        public RoulettesController(RouletteContext context, BetContext contextBet, UserContext contextUser)
        {
            _contextUser = contextUser;
            _contextBet = contextBet;
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
        public async Task<IActionResult> OpenRoulette(long id)
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
                rouletteSelected.Game++;
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
        [HttpGet("Close/{id}")] // GET: api/Roulettes/Close
        public async Task<IActionResult> CloseRoulette(long id)
        {
            if (!(id >= 1))
            {
                return BadRequest();
            }
            var rouletteSelected = await _context.Roulettes.FindAsync(id);
            if (rouletteSelected.IsOpen)
            {
                var bets = _contextBet.Bets;
                Result betsResult = CalculateRouletteBetsResult(bets, rouletteSelected);
                foreach (var userResult in betsResult.Users)
                {
                    var user = await _contextUser.Users.FindAsync(userResult.Id);
                    user.Money += userResult.Money;
                    _contextUser.Entry(user).State = EntityState.Modified;
                    try
                    {
                        await _contextUser.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return NotFound();
                    }
                }
                rouletteSelected.IsOpen = false;
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
                return Ok(betsResult);
            }
            else
            {
                return Ok("Denied");
            }
        }
        private readonly Random _random = new Random();     
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        private Result CalculateRouletteBetsResult(DbSet<Bet> bets, Roulette rouletteSelected)
        {
            int number = RandomNumber(0, 36);
            string color;
            if (number % 2 == 0) color = "r"; else color = "b";
            List<Bet> selectedBets = new List<Bet>();
            foreach (var betItem in bets)
            {
                if (betItem.Game == rouletteSelected.Game && betItem.IdRoullete == rouletteSelected.Id)
                {
                    if (betItem.Number == number)
                    {
                        betItem.Value *= 5;
                        selectedBets.Add(betItem);
                    }
                    else if (betItem.Color == color)
                    {
                        betItem.Value *= 1.8;
                        selectedBets.Add(betItem);
                    }
                }
            }

            return new Result()
            {
                IdRoulette = rouletteSelected.Id,
                Number = number,
                Color = color,
                Users = BetsToUsers(selectedBets)
            };
        }
        private List<User> BetsToUsers(List<Bet> bets)
        {
            List<User> result = new List<User>();
            foreach (var bet in bets)
            {
                var user = result.Find(x => x.Id == bet.IdUser);
                if (user == null)
                {
                    result.Add(new User() { Id = bet.IdUser, Money = bet.Value });
                }
                else
                {
                    user.Money += bet.Value;
                }
            }
            return result;
        }
    }
}
