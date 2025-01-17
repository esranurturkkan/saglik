using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using school_project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace school_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly string _jwtKey = "your-very-long-and-secure-secret-key-12345"; // Secret Key burada belirlenmiştir

        public UserController(SchoolDbContext context)
        {
            _context = context;
        }

        // Kayıt Ol
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Geçersiz veri.");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                return BadRequest("Bu e-posta adresi zaten kayıtlı.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayıt başarılı.");
        }

        // Giriş Yap
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _context.Users.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        // Kullanıcı Profilini Al
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulanamadı.");

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(new
            {
                user.Email,
                user.Password,
                user.Age,
                user.Height,
                user.Weight,
                user.StepGoal,
                user.CalorieGoal,
                user.Steps,
                user.Calories,
                user.ActiveMinutes,
                user.Protein,
                user.Fat,
                user.Carbs,
                user.WaterIntake,
                user.BloodPressure,
                user.HeartRate,
                user.Sleep
            });
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null)
                {
                    return BadRequest("Gönderilen veri eksik veya hatalı.");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized("Kullanıcı doğrulanamadı.");
                }

                var user = await _context.Users.FindAsync(int.Parse(userId));
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                // Sadece gönderilen (null olmayan) alanları güncelle
                if (!string.IsNullOrWhiteSpace(updatedUser.Email)) user.Email = updatedUser.Email;

                if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                {
                    // İsterseniz burada şifreyi hashleyebilirsiniz
                    user.Password = updatedUser.Password;
                }

                if (updatedUser.Age.HasValue) user.Age = updatedUser.Age.Value;
                if (updatedUser.Height.HasValue) user.Height = updatedUser.Height.Value;
                if (updatedUser.Weight.HasValue) user.Weight = updatedUser.Weight.Value;
                if (updatedUser.StepGoal.HasValue) user.StepGoal = updatedUser.StepGoal.Value;
                if (updatedUser.CalorieGoal.HasValue) user.CalorieGoal = updatedUser.CalorieGoal.Value;
                if (updatedUser.Steps.HasValue) user.Steps = updatedUser.Steps.Value;
                if (updatedUser.Calories.HasValue) user.Calories = updatedUser.Calories.Value;
                if (updatedUser.ActiveMinutes.HasValue) user.ActiveMinutes = updatedUser.ActiveMinutes.Value;
                if (updatedUser.Protein.HasValue) user.Protein = updatedUser.Protein.Value;
                if (updatedUser.Carbs.HasValue) user.Carbs = updatedUser.Carbs.Value;
                if (updatedUser.Fat.HasValue) user.Fat = updatedUser.Fat.Value;
                if (updatedUser.WaterIntake.HasValue) user.WaterIntake = updatedUser.WaterIntake.Value;
                if (updatedUser.HeartRate.HasValue) user.HeartRate = updatedUser.HeartRate.Value;
                if (!string.IsNullOrEmpty(updatedUser.BloodPressure)) user.BloodPressure = updatedUser.BloodPressure;
                if (!string.IsNullOrEmpty(updatedUser.Sleep)) user.Sleep = updatedUser.Sleep;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok("Profil başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }


    }
}
