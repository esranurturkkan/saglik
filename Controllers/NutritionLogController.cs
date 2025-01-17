using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_project.Data;
using school_project.Models;

namespace school_project.Controllers
{
    [Route("api/NutritionLog")]
    [ApiController]
    public class NutritionLogController : ControllerBase
    {
        private readonly schoolDbContext _context;

        public NutritionLogController(schoolDbContext context)
        {
            _context = context;
        }

        // GET: api/nutritionlog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NutritionLog>>> GetNutritionLogs()
        {
            var nutritionLogs = await _context.NutritionLog
                .Include(pr => pr.User)  // İlişkili kullanıcıyı dahil et
                .ToListAsync();

            return Ok(nutritionLogs);
        }

        // GET: api/nutritionlog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NutritionLog>> GetNutritionLog(int id)
        {
            var nutritionLog = await _context.NutritionLog.FindAsync(id);

            if (nutritionLog == null)
            {
                return NotFound();
            }

            return nutritionLog;
        }

        // POST: api/nutritionlog
        [HttpPost]
        public async Task<ActionResult<NutritionLog>> CreateNutritionLog(NutritionLog nutritionLog)
        {
            // Kullanıcı ID'sinin var olduğundan emin olun
            var userExists = await _context.Users.AnyAsync(u => u.UserId == nutritionLog.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.NutritionLog.Add(nutritionLog);
            await _context.SaveChangesAsync();

            // Yeni oluşturulan NutritionLog'u döner
            return CreatedAtAction(nameof(GetNutritionLog), new { id = nutritionLog.LogId }, nutritionLog);
        }

        // PUT: api/nutritionlog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNutritionLog(int id, NutritionLog nutritionLog)
        {
            if (id != nutritionLog.LogId)
            {
                return BadRequest();
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == nutritionLog.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.Entry(nutritionLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NutritionLog.Any(n => n.LogId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Güncellenen veri ile No Content döner
        }

        // DELETE: api/nutritionlog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNutritionLog(int id)
        {
            var nutritionLog = await _context.NutritionLog.FindAsync(id);
            if (nutritionLog == null)
            {
                return NotFound();
            }

            _context.NutritionLog.Remove(nutritionLog);
            await _context.SaveChangesAsync();

            return NoContent(); // Silinen veriyi geri döner
        }
    }
}

