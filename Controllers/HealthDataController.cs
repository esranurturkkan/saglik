using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_project.Data;
using school_project.Models;

namespace school_project.Controllers
{
    [Route("api/HealthData")]
    [ApiController]
    public class HealthDataController : ControllerBase
    {
        private readonly schoolDbContext _context;

        public HealthDataController(schoolDbContext context)
        {
            _context = context;
        }

        // GET: api/healthdata
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthData>>> GetHealthDatas()
        {
            var healthData = await _context.HealthData
                .Include(h => h.User)  
                .ToListAsync();

            return Ok(healthData);
        }


        // GET: api/healthdata/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthData>> GetHealthData(int id)
        {
            var healthData = await _context.HealthData.FindAsync(id);

            if (healthData == null)
            {
                return NotFound();
            }

            return healthData;
        }

        // POST: api/healthdata
        [HttpPost]
        public async Task<ActionResult<HealthData>> CreateHealthData(HealthData healthData)
        {
            // Kullanıcı ID'sinin var olduğundan emin olun
            var userExists = await _context.Users.AnyAsync(u => u.UserId == healthData.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.HealthData.Add(healthData);
            await _context.SaveChangesAsync();

            // Yeni oluşturulan HealthData'yı döner
            return CreatedAtAction(nameof(GetHealthData), new { id = healthData.DataId }, healthData);
        }

        // PUT: api/healthdata/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHealthData(int id, HealthData healthData)
        {
            if (id != healthData.DataId)
            {
                return BadRequest();
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == healthData.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.Entry(healthData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HealthData.Any(h => h.DataId == id))
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

        // DELETE: api/healthdata/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthData(int id)
        {
            var healthData = await _context.HealthData.FindAsync(id);
            if (healthData == null)
            {
                return NotFound();
            }

            _context.HealthData.Remove(healthData);
            await _context.SaveChangesAsync();

            return NoContent(); // Silinen veriyi geri döner
        }
    }
}
