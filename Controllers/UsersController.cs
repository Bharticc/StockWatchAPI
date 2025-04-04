using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockWatchAPI.data;
using StockWatchAPI.Models;
using Microsoft.EntityFrameworkCore;
using StockWatchAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace StockWatchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly StockWatchDbContext _context;
        private readonly PasswordService _passwordService;

        public UsersController(StockWatchDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            return await _context.Stocks.ToListAsync();
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Get user ID from JWT
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            return Ok(user);
        }

        // 🔵 GET: api/stock/{id} (Retrieve stock by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
                return NotFound("Stock not found");

            return stock;
        }
        // 🟡 PUT: api/stock/{id} (Update stock details)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] Stock stock)
        {
            if (id != stock.Id)
                return BadRequest("Stock ID mismatch");

            _context.Entry(stock).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Stock updated successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            bool isPasswordValid = _passwordService.VerifyPassword(model.PasswordHash, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok("Login successful!");
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
                return BadRequest("User already exists.");

            model.PasswordHash = _passwordService.HashPassword(model.Password);// Hash password before storing
            model.Password = null;


            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

        private string HashPassword(string password)
        {
            // Use a secure hashing algorithm (e.g., BCrypt or SHA256)
            return password; // Just a placeholder for now; don't store plain text passwords!
        }
    }
}
