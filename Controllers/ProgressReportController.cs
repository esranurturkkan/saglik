using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_project.Data;
using school_project.Models;

namespace school_project.Controllers
{
    [Route("api/ProgressReport")]
    [ApiController]
    public class ProgressReportController : ControllerBase
    {
        private readonly schoolDbContext _context;

        public ProgressReportController(schoolDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgressReport>>> GetProgressReports()
        {
            var progressReports = await _context.ProgressReport
                .Include(pr => pr.User)  // İlişkili kullanıcıyı dahil et
                .ToListAsync();

            return Ok(progressReports);
        }
        // GET: api/progressreport
        [HttpGet]
     

        // GET: api/progressreport/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProgressReport>> GetProgressReport(int id)
        {
            var progressReport = await _context.ProgressReport.FindAsync(id);

            if (progressReport == null)
            {
                return NotFound();
            }

            return progressReport;
        }

        // POST: api/progressreport
        [HttpPost]
        public async Task<ActionResult<ProgressReport>> CreateProgressReport(ProgressReport progressReport)
        {
            // Kullanıcı ID'sinin var olduğundan emin olun
            var userExists = await _context.Users.AnyAsync(u => u.UserId == progressReport.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.ProgressReport.Add(progressReport);
            await _context.SaveChangesAsync();

            // Yeni oluşturulan ProgressReport'u döner
            return CreatedAtAction(nameof(GetProgressReport), new { id = progressReport.ProgressId }, progressReport);
        }

        // PUT: api/progressreport/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgressReport(int id, ProgressReport progressReport)
        {
            if (id != progressReport.ProgressId)
            {
                return BadRequest();
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == progressReport.UserId);
            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            _context.Entry(progressReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProgressReport.Any(p => p.ProgressId == id))
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

        // DELETE: api/progressreport/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgressReport(int id)
        {
            var progressReport = await _context.ProgressReport.FindAsync(id);
            if (progressReport == null)
            {
                return NotFound();
            }

            _context.ProgressReport.Remove(progressReport);
            await _context.SaveChangesAsync();

            return NoContent(); // Silinen veriyi geri döner
        }
    }
}

